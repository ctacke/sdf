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
