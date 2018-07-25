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

namespace OpenNETCF.AppSettings
{
    public class SettingGroups : IEnumerable<SettingGroup>
    {
        private List<SettingGroup> m_groups;

        internal SettingGroups()
        {
            m_groups = new List<SettingGroup>();
        }

        public void Add(string groupName)
        {
            if(this.Contains(groupName))
                throw new ArgumentException("Group already exists");

            m_groups.Add(new SettingGroup(groupName));
        }

        public void Remove(string groupName)
        {
            for(int i = 0 ; i < m_groups.Count ; i++)
            {
                if(string.Compare(m_groups[i].Name, groupName, true) == 0)
                {
                    m_groups.RemoveAt(i);
                    return;
                }
            }
        }

        public bool Contains(string groupName)
        {
            foreach (SettingGroup group in m_groups)
            {
                if (string.Compare(group.Name, groupName, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public int Count
        {
            get { return m_groups.Count; }
        }

        public SettingGroup this[int index]
        {
            get { return m_groups[index]; }
            set { m_groups[index] = value; }
        }

        public SettingGroup this[string groupName]
        {
            get 
            {
                foreach (SettingGroup group in m_groups)
                {
                    if (string.Compare(group.Name, groupName, true) == 0)
                    {
                        return group;
                    }
                }

                throw new ArgumentException("Group name not found: " + groupName);
            }
            set 
            {
                for(int i = 0 ; i < m_groups.Count ; i++)
                {
                    if (string.Compare(m_groups[i].Name, groupName, true) == 0)
                    {
                        m_groups[i] = value;
                        return;
                    }
                }

                throw new ArgumentException("Group name not found: " + groupName);
            }
        }

        public IEnumerator<SettingGroup> GetEnumerator()
        {
            return m_groups.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_groups.GetEnumerator();
        }

        internal string ToXml()
        {
            StringBuilder header = new StringBuilder();
            StringBuilder body = new StringBuilder();

            header.Append("\t<SettingsGroups>\r\n");
            foreach (SettingGroup group in m_groups)
            {
                header.Append(string.Format("\t\t<group name='{0}' />\r\n", group.Name));

                body.Append(group.ToXml());
            }
            header.Append("\t</SettingsGroups>\r\n");
            header.Append(body.ToString());
            return header.ToString();
        }
    }
}
