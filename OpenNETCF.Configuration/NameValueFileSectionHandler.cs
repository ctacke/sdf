
using System;
using System.IO;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class NameValueFileSectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			object result = parent;
			XmlNode fileAttribute = section.Attributes.RemoveNamedItem("file");
			result = NameValueSectionHandler.CreateStatic(result, section);
			if (fileAttribute != null && fileAttribute.Value.Length != 0)
			{
				string sectionName = fileAttribute.Value;
				IConfigXmlNode configXmlNode = fileAttribute as IConfigXmlNode;
				if (configXmlNode == null)
				{
					return null;
				}
				string sourceFileFullPath = Path.Combine(Path.GetDirectoryName(configXmlNode.Filename), sectionName);
				if (File.Exists(sourceFileFullPath))
				{
					ConfigXmlDocument configXmlDocument = new ConfigXmlDocument();
					try
					{
						configXmlDocument.Load(sourceFileFullPath);
					}
					catch (XmlException e)
					{
						throw new ConfigurationException(e.Message, e, sourceFileFullPath, e.LineNumber);
					}
					if (section.Name != configXmlDocument.DocumentElement.Name)
					{
						throw new ConfigurationException("Config NameValueFile Section: Invalid root", configXmlDocument.DocumentElement);
					}
					result = NameValueSectionHandler.CreateStatic(result, configXmlDocument.DocumentElement);
				}
			}
			return result;
		}
	}

}
