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
