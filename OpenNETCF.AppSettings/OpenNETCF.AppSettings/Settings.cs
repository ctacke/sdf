using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.AppSettings
{
    public class Settings : IEnumerable<Setting>
    {
        private List<Setting> m_settings;
        internal Settings()
        {
            m_settings = new List<Setting>();
        }

        public IEnumerator<Setting> GetEnumerator()
        {
            return m_settings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_settings.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return m_settings.Count;
            }
        }

        public void Add(Setting setting)
        {
            m_settings.Add(setting);
        }

        public void Add(string name, object value)
        {
            foreach (Setting s in m_settings)
            {
                if (string.Compare(s.Name, name, true) == 0)
                {
                    throw new ArgumentException("Setting already exists: " + name);
                }
            }

            m_settings.Add(new Setting(name, value));
        }

        public void Remove(string name)
        {
            for(int i = 0 ; i < m_settings.Count ; i++)
            {
                if (string.Compare(m_settings[i].Name, name, true) == 0)
                {
                    m_settings.RemoveAt(i);
                    return;
                }
            }
            throw new ArgumentException("Setting not found: " + name);
        }

        public Setting this[int index]
        {
            get { return m_settings[index]; }
            set { m_settings[index] = value; }
        }

        public Setting this[string settingName]
        {
            get 
            {
                foreach (Setting s in m_settings)
                {
                    if (string.Compare(s.Name, settingName, true) == 0)
                    {
                        return s;
                    }
                }

                // didn't exist, so add it with a null value
                Add(settingName, null);

                // find what we just added
                foreach (Setting s in m_settings)
                {
                    if (string.Compare(s.Name, settingName, true) == 0)
                    {
                        return s;
                    }
                }

                // should never happen
                throw new Exception("Failed to get setting");
            }
            set 
            {
                for (int i = 0; i < m_settings.Count; i++)
                {
                    if (string.Compare(m_settings[i].Name, settingName, true) == 0)
                    {
                        m_settings[i] = value;
                        return;
                    }
                }

                // not found, so add it
                if (string.Compare(value.Name, settingName, true) != 0)
                {
                    throw new ArgumentException("settingName parameter must match Name in the incoming Setting");
                }

                Add(value);
            }
        }
    }
}
