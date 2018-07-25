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
