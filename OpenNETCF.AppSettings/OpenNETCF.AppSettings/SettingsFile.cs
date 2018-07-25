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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace OpenNETCF.AppSettings
{
    public class SettingsFile
    {
        private string m_settingsFilePath;
        private SettingGroups m_groups = new SettingGroups();

        public SettingsFile(string settingsFilePath) : this(settingsFilePath, true) { }

        public SettingsFile(string settingsFilePath, bool createIfMissing)
        {
            if (settingsFilePath == null) throw new ArgumentNullException("settingsFilePath");
            if (settingsFilePath == string.Empty) throw new ArgumentException();

            if (!File.Exists(settingsFilePath))
            {
                if (createIfMissing)
                {
                    // create the file
                    using (XmlWriter writer = new XmlTextWriter(settingsFilePath, Encoding.UTF8))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("SettingsFile");
                        writer.WriteStartElement("SettingsGroups");                        
                        writer.WriteEndElement(); // close groups
                        writer.WriteEndElement(); // close file

                        writer.WriteEndDocument();
                        writer.Flush();
                        writer.Close();
                    }
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }

            m_settingsFilePath = settingsFilePath;

            StreamReader reader;

            // make sure the settings file is not read-only
#if WindowsCE || PocketPC
            FileAttributes attributes = OpenNETCF.IO.FileHelper.GetAttributes(m_settingsFilePath);
#else
            FileAttributes attributes = File.GetAttributes(m_settingsFilePath);
#endif

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
#if WindowsCE || PocketPC
                OpenNETCF.IO.FileHelper.SetAttributes(m_settingsFilePath, attributes &= ~FileAttributes.ReadOnly);
#else
               File.SetAttributes(m_settingsFilePath, attributes &= ~FileAttributes.ReadOnly);
#endif
           }
            reader = File.OpenText(m_settingsFilePath);

            XmlDocument document = new XmlDocument();

            try
            {
                document.LoadXml(reader.ReadToEnd());


                XmlNode root = document.DocumentElement;

                // get the groups
                XmlNode groupsNode = root.SelectSingleNode("SettingsGroups");
                foreach (XmlNode node in groupsNode.ChildNodes)
                {
                    string groupName = node.Attributes["name"].Value;
                    m_groups.Add(groupName);

                    // get each setting in the group
                    XmlNode currentGroupNode = root.SelectSingleNode(groupName);

                    if (currentGroupNode != null)
                    {
                        foreach (XmlNode settingNode in currentGroupNode)
                        {
                            object val = null;
                            try
                            {
                                // get the data type
                                string typeName = settingNode.SelectSingleNode("Type").InnerText;
                                string stringValue = settingNode.SelectSingleNode("Value").InnerText;

                                if (typeName.ToLower() == "null")
                                {
                                    val = null;
                                }
                                else
                                {
                                    Type t = Type.GetType(typeName);

                                    // now create the value
                                    if (t.IsValueType)
                                    {
                                        object valueType = Activator.CreateInstance(t);
                                        MethodInfo parseMethod = t.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(System.String) }, null);
                                        val = parseMethod.Invoke(valueType, new object[] { stringValue });
                                    }
                                    else if (t.FullName == "System.String")
                                    {
                                        val = stringValue;
                                    }
                                    else
                                    {
                                        throw new TypeLoadException(typeName);
                                    }
                                }
                                m_groups[groupName].Settings.Add(settingNode.Name, val);
                            }
                            catch
                            {
                                // invalid node - just ignore it
                            }
                        } // foreach
                    } // if(currentGroupNode != null)
                }
            }
            finally
            {
                reader.Close();
                document = null;
            }
        }

        public void Save()
        {
            
            TextWriter writer = File.CreateText(m_settingsFilePath);
            try
            {
                writer.Write(this.ToXml());
            }
            finally
            {
                writer.Close();
            }
        }

        public SettingGroups Groups
        {
            get { return m_groups; }
        }

        public Setting GetSetting(string groupName, string settingName)
        {
            return GetSetting(groupName, settingName, false);
        }

        public Setting GetSetting(string groupName, string settingName, bool throwIfNotFound)
        {
            try
            {
                return m_groups[groupName].Settings[settingName];
            }
            catch
            {
                if (throwIfNotFound)
                    throw;
                else
                    return null;
            }
        }

        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder body = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='utf-8'?>\r\n");
            sb.Append("<SettingsFile>\r\n");
            sb.Append(m_groups.ToXml());
            sb.Append("</SettingsFile>\r\n");
            return sb.ToString();
        }

    }
}
