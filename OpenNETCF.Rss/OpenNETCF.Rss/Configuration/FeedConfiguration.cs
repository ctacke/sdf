using System;
using System.Collections;
using System.Text;

//#if CF
	using OpenNETCF.Configuration;
//#else
//    using System.Configuration;
//#endif

using System.Xml;

namespace OpenNETCF.Rss.Configuration
{
	/// <summary>
	/// Uses to process a configuration file. 
	/// </summary>
	public class FeedConfiguration : IConfigurationSectionHandler
	{
		#region fields

		private static FeedConfiguration configuration;
		private TransportConfiguration transportConfiguration;
		private StorageConfiguration storageConfiguration;
		private static object configLock; 
		
		#endregion

		#region contructors
		
		static FeedConfiguration()
		{
			FeedConfiguration.configuration = null;
			FeedConfiguration.configLock = new object();
		}

		/// <summary>
		/// Initializes a new instance of the FeedConfiguration class.
		/// </summary>
		public FeedConfiguration()
		{
			transportConfiguration = new TransportConfiguration();
			storageConfiguration = new StorageConfiguration();
		}
		
		#endregion

		/// <summary>
		/// Gets the instance of the TransportConfiguration class.
		/// </summary>
		public static TransportConfiguration TransportConfiguration
		{
			get
			{
				if (configuration == null)
				{
					FeedConfiguration.Initialize();
				}
				return FeedConfiguration.configuration.transportConfiguration;
			}
		}

		public static StorageConfiguration StorageConfiguration
		{
			get
			{
				if (configuration == null)
				{
					FeedConfiguration.Initialize();
				}
				return FeedConfiguration.configuration.storageConfiguration;
			}
		}

		private static void Initialize()
		{
			if (FeedConfiguration.configuration != null)
			{
				return;
			}
			lock (FeedConfiguration.configLock)
            {
                configuration = ConfigurationSettings.GetConfig("feed.net") as FeedConfiguration;
            }
		}

		private void Clear()
		{
			this.transportConfiguration = new TransportConfiguration();
		}

		#region IConfigurationSectionHandler Members

		
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			this.Clear();
			FeedConfiguration.configuration = this;

			try
			{
				if (section.HasChildNodes)
				{
					foreach (XmlNode node in section.ChildNodes)
					{
						string text = node.LocalName;
						text = string.IsInterned(text);
						if (text == "communication")
						{
							this.transportConfiguration.Load(node);
							continue;
						}

						if (text == "storage")
						{
							this.storageConfiguration.Load(node);
							continue;
						}

					}
				}
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                FeedConfiguration.configuration = null;
				throw;
			}

			return this;


		}

		#endregion
}
}
