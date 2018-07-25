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
