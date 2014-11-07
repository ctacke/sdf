using System;
using System.Xml;
using System.Reflection;
using System.Collections.Specialized;

namespace OpenNETCF.Rss.Configuration
{
	/// <summary>
	/// Summary description for StorageConfiguration.
	/// </summary>
	public class StorageConfiguration
	{
		
		private static IFeedStorage provider;

		static StorageConfiguration()
		{
			//StorageConfiguration.providers = new HybridDictionary();
			provider = null;
		}

		/// <summary>
		/// Initializes a new instance of the TransportConfiguration class.
		/// </summary>
		public StorageConfiguration()
		{
			
		} 

		/// <summary>
		/// Loads storage setings from config file.
		/// </summary>
		/// <param name="section">The XmlNode section from the config file.</param>
		public void Load(XmlNode section)
		{
			string text = "";

			foreach (XmlNode node in section.ChildNodes)
			{
				XmlElement element = node as XmlElement;
				if (element == null)
				{
					continue;
				}

				if ((text = element.LocalName) == null)
				{
					throw new ArgumentException("Error in the configuration");
				}

				text = string.IsInterned(text);

				if (text == "provider")
				{
					LoadProviders(element);
				}
			}

		}

		private void LoadProviders(XmlElement element)
		{
			
				IFeedStorage storage;
				string name = element.GetAttribute("name");
				string typeName = element.GetAttribute("type");
				bool enabled = Boolean.Parse(element.GetAttribute("enabled"));

				if (enabled)
				{
					if (name.Length == 0)
					{
						throw new ArgumentException("missing name");
					}
					if (element.HasChildNodes)
					{
						storage = this.LoadProvider(typeName, element);
					}
					else
					{
						storage = this.LoadProvider(typeName, (XmlNode)null);
					}
					StorageConfiguration.provider = storage;
				}
			

		}

		private IFeedStorage LoadProvider(string typeName, XmlNode configData)
		{
			Type type = Type.GetType(typeName, false);
			if (type != null)
			{
				return this.LoadProvider(type, configData);
			}
			return null;
		}

		private IFeedStorage LoadProvider(Type type, XmlNode configData)
		{
			//object[] objArray;
			ConstructorInfo info = null;
			//ISoapTransport transport = null;
			if (Utility.ImplementsInterface(type, typeof(IFeedStorage)))
			{
				if (configData != null)
				{
					info = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
					if (info == null)
					{
						throw new ArgumentException("No constructor");
					}
					IFeedStorage storage = info.Invoke(null) as IFeedStorage;
					if (storage != null)
					{	
						// Call the Init
						storage.Init(configData);
					}
					return storage;
				}

			}
			return null;
		}

		/// <summary>
		/// Returns the storage provider by the given scheme.
		/// </summary>		
		/// <returns></returns>
		public IFeedStorage GetProvider()
		{
			return (StorageConfiguration.provider as IFeedStorage);
		}

	}
}
