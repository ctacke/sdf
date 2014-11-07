using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.AppSettings
{
    public class SettingGroup
    {
        private string m_name;
        private Settings m_settings;
        internal SettingGroup(string name)
        {
            m_name = name;
            m_settings = new Settings();
        }

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public Settings Settings
        {
            get
            {
                return m_settings;
            }
            set
            {
                m_settings = value;
            }
        }

        internal string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("\t<{0}>\r\n", Name));
            foreach (Setting s in Settings)
            {
                sb.Append(s.ToXml());
            }
            sb.Append(string.Format("\t</{0}>\r\n", Name));
            return sb.ToString();
        }
    }
}
