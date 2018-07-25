using System;
using System.Collections;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using OpenNETCF.Configuration;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Summary description for DiagnosticsConfigurationHandler.
	/// </summary>
	public class DiagnosticsConfigurationHandler : IConfigurationSectionHandler	
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			Hashtable parentConfig = (Hashtable)parent;
			Hashtable config;
			if (parentConfig == null)
				config = new Hashtable();
			else
				config = (Hashtable)parentConfig.Clone();

			foreach(XmlNode child in section.ChildNodes)
			{
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
					continue;

				switch(child.Name)
				{
					case "trace":
						HandleTrace(config, child, configContext);
						break;
					case "assert":
						HandleAssert(config, child, configContext);
						break;
                    case "switches":
                        HandleSwitches(config, child, configContext);
                        break;
					default:
						HandlerBase.ThrowUnrecognizedElement(child);
						break;
				}

				HandlerBase.CheckForUnrecognizedAttributes(child);
			}

			return config;
		}

		private static void HandleTrace(Hashtable config, XmlNode traceNode, object context)
		{
			bool autoFlush = false;
			if (HandlerBase.GetAndRemoveBooleanAttribute(traceNode, "autoflush", ref autoFlush) != null)
				config["autoflush"] = autoFlush;
                                       
			int indentSize = 0;
			if (HandlerBase.GetAndRemoveIntegerAttribute(traceNode, "indentsize", ref indentSize) != null)
				config["indentsize"] = indentSize;

			foreach (XmlNode traceChild in traceNode.ChildNodes) 
			{
				if (HandlerBase.IsIgnorableAlsoCheckForNonElement(traceChild))
					continue;
                
				if (traceChild.Name == "listeners") 
				{
					HandleListeners(config, traceChild, context);
				}
				else 
				{
					HandlerBase.ThrowUnrecognizedElement(traceChild);
				}
			}
		}

		private static void HandleListeners(Hashtable config, XmlNode listenersNode, object context) 
		{
			foreach(XmlNode listenersChild in listenersNode.ChildNodes) 
			{
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(listenersChild))
					continue;
                
				string name = null;
				string className = null;
				string initializeData = null;
				string operation = listenersChild.Name;
                
				switch(operation) 
				{
					case "add":
					case "remove":
					case "clear":
						break;
					default:
						HandlerBase.ThrowUnrecognizedElement(listenersChild);
						break;
				}

				HandlerBase.GetAndRemoveStringAttribute(listenersChild, "name", ref name);
				HandlerBase.GetAndRemoveStringAttribute(listenersChild, "type", ref className);
				HandlerBase.GetAndRemoveStringAttribute(listenersChild, "initializeData", ref initializeData);
				HandlerBase.CheckForUnrecognizedAttributes(listenersChild);

				TraceListener newListener = null;
				if(className != null) 
				{  
					Type t = Type.GetType(className);

					if(t == null)
						throw new ConfigurationException(string.Format("Could not find type {0}", className));

					// Create a listener with parameterless ctor 
					if (initializeData == null) 
					{
						ConstructorInfo ctorInfo = t.GetConstructor(new Type[] {});
						if (ctorInfo == null)
							throw new ConfigurationException(string.Format("Could not get constructor for {0}", className));
						newListener = (TraceListener)(ctorInfo.Invoke(new object[] {}));
					}						
					else // Create a listener with a one-string ctor 
					{
						ConstructorInfo ctorInfo = t.GetConstructor(new Type[] { typeof(string) });
						if (ctorInfo == null)
							throw new ConfigurationException(string.Format("Could not get constructor for {0}", className));
						newListener = (TraceListener)(ctorInfo.Invoke(new object[] { initializeData }));
					}
					if(name != null) 
					{
						newListener.Name = name;
					}
				}

				switch(operation) 
				{
					case "add":
						if (newListener == null)
							throw new ConfigurationException("Could not create listener");  
    
						Trace2.Listeners.Add(newListener);
    
						break;
					case "remove":
						if (newListener == null) 
						{
							if (name == null)
								throw new ConfigurationException("Cannot remove with null name");
    
							// No type has been specified, so remove by name
							Trace2.Listeners.Remove(name);
						}
						else 
						{
							// Remove by listener
							Trace2.Listeners.Remove(newListener.Name);
						}
						break;
					case "clear":
						Trace2.Listeners.Clear();
						break;
					default:
						HandlerBase.ThrowUnrecognizedElement(listenersChild);
						break;
				}
			}
		}

		private static void HandleAssert(Hashtable config, XmlNode assertNode, object context) 
		{
			bool assertUiEnabled = false;
			if(HandlerBase.GetAndRemoveBooleanAttribute(assertNode, "assertuienabled", ref assertUiEnabled) != null)
				config["assertuienabled"] = assertUiEnabled;

			string logFile = null;
			if(HandlerBase.GetAndRemoveStringAttribute(assertNode, "logfilename", ref logFile) != null)
				config["logfilename"] = logFile;

			HandlerBase.CheckForChildNodes(assertNode);
		}

        private static void HandleSwitches(Hashtable config, XmlNode switchesNode, object context)
        {
            Hashtable switches = (Hashtable)new SwitchesDictionarySectionHandler().Create(config["switches"], context, switchesNode);

            foreach(DictionaryEntry traceSwitch in switches)
            {
                try
                {
                    int.Parse((string)traceSwitch.Value);
                    continue;
                }
                catch
                {
                    throw new ConfigurationException("Value must be numeric");
                }
            }

            config["switches"] = switches;
        }


	}
}
