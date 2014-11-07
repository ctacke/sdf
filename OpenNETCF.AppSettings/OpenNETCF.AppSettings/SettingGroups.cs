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
