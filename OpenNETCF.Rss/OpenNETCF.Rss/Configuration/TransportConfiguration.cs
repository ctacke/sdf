#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Reflection;

using OpenNETCF.Rss;

namespace OpenNETCF.Rss.Configuration
{
	/// <summary>
	/// Uses to process the transport section in an configuration file.
	/// </summary>
	public sealed class TransportConfiguration
	{
		static TransportConfiguration()
		{
			Transports = new HybridDictionary();
		}

		/// <summary>
		/// Initializes a new instance of the TransportConfiguration class.
		/// </summary>
		public TransportConfiguration()
		{
			RefreshInterval = 60000;
		} 
		
		/// <summary>
		/// Gets the refresh interval.
		/// </summary>
		public int RefreshInterval { get; private set; }

        private static HybridDictionary Transports { get; set; }

		#region public methods
		/// <summary>
		/// Loads the transports section from a config file.
		/// </summary>
		/// <param name="section"></param>
		public void Load(XmlNode section)
		{
			string text = "";

			foreach (XmlNode node in section.ChildNodes)
			{
				XmlElement element = node as XmlElement;
				if (element == null) continue;

				if ((text = element.LocalName) == null)
				{
					throw new ArgumentException("Error in the configuration");
				}

				text = string.IsInterned(text);

				if (text == "transports")
				{
					LoadTransports(element);
				}

				if (text == "refreshInterval")
				{
					RefreshInterval = Convert.ToInt32(node.Attributes["value"].Value);
				}
			}

		}

		/// <summary>
		/// Adds transport to the collection.
		/// </summary>
		/// <param name="scheme"></param>
		/// <param name="transport"></param>
		public void AddTransport(string scheme, IFeedTransport transport)
		{
			if ((scheme == null) || (scheme.Length == 0))
			{
				throw new ArgumentNullException("scheme");
			}
			if (transport == null)
			{
				throw new ArgumentNullException("transport");
			}
			Transports[scheme] = transport;
		}

		/// <summary>
		/// Returns the transport by the given scheme.
		/// </summary>
		/// <param name="scheme"></param>
		/// <returns></returns>
		public IFeedTransport GetTransport(string scheme)
		{
			if ((scheme == null) || (scheme.Length == 0))
			{
				throw new ArgumentNullException("scheme");
			}
			return (Transports[scheme] as IFeedTransport);
		}
		
		#endregion


		#region helper methods
		private void LoadTransports(XmlElement element)
		{
			for (int num = 0; num < element.ChildNodes.Count; num++)
			{
				string scheme;
				string name;
				XmlElement currentElement = element.ChildNodes[num] as XmlElement;

				if ((currentElement != null) && ((name = currentElement.LocalName) != null))
				{
					name = string.IsInterned(name);
					if (name == "add")
					{
						IFeedTransport transport;
						scheme = currentElement.GetAttribute("scheme");
						string typeName = currentElement.GetAttribute("type");
						if (scheme.Length == 0)
						{
							throw new ArgumentException("missing scheme");
						}
						if (typeName.Length == 0)
						{
							Type type = null;

							if (FeedHttpTransport.UriScheme == scheme)
							{
								type = typeof(FeedHttpTransport);
							}
							else
							{
								throw new ArgumentException("failed to create FeedHttpTransport");
							}
							if (element.HasChildNodes)
							{
								transport = this.LoadTransport(type, currentElement.ChildNodes);
							}
							else
							{
								transport = this.LoadTransport(type, (XmlNodeList)null);
							}

							this.AddTransport(scheme, transport);
						}
						else if (element.HasChildNodes)
						{
							transport = this.LoadTransport(typeName, element.ChildNodes);
						}
						else
						{
							transport = this.LoadTransport(typeName, (XmlNodeList)null);
						}
						Transports[typeName] = transport;
					}
				}
			}

		}


		private IFeedTransport LoadTransport(string typeName, XmlNodeList configData)
		{
			Type type = Type.GetType(typeName, false);
			if (type != null)
			{
				return this.LoadTransport(type, configData);
			}
			return null;
		}

		private IFeedTransport LoadTransport(Type type, XmlNodeList configData)
		{
			object[] objArray;
			ConstructorInfo info = null;
			//ISoapTransport transport = null;
			if (Utility.ImplementsInterface(type, typeof(IFeedTransport)))
			{
				if (configData == null)
				{
					info = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
					if (info == null)
					{
						throw new ArgumentException("No constructor");
					}
					return (info.Invoke(null) as IFeedTransport);
				}
				Type[] typeArray = new Type[1] { typeof(XmlNodeList) };
				info = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, typeArray, null);
				if (info == null)
				{
					throw new ArgumentException("No constructor");
				}
				objArray = new object[1] { configData };
				return (info.Invoke(objArray) as IFeedTransport);
			}
			return null;
		}
		
		#endregion
	}
}
