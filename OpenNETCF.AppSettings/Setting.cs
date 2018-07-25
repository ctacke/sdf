using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.AppSettings
{
    public class Setting
    {
        private string m_name;
        private object m_value;

        public Setting(string name) : this(name, null) {}

        public Setting(string name, object value)
        {
            m_name = name;
            m_value = value;
        }

        public string Name
        {
            get { return m_name; }
        }

        public object Value
        {
            set { m_value = value; }
            get { return m_value; }
        }

        internal string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("\t\t<{0}>\r\n", Name));
            if (Value == null)
            {                
                sb.Append("\t\t\t<Type>null</Type>\r\n");
                sb.Append("\t\t\t<Value>null</Value>\r\n");
            }
            else
            {
                sb.Append(string.Format("\t\t\t<Type>{0}</Type>\r\n", Value.GetType().FullName));
                sb.Append(string.Format("\t\t\t<Value>{0}</Value>\r\n", Value.ToString()));
            }
            sb.Append(string.Format("\t\t</{0}>\r\n", Name));
            return sb.ToString();
        }

        #region --- operators ---
        public static implicit operator bool(Setting s)
        {
            object val = s.Value;
            if (val is bool)
            {
                return (bool)val;
            }

            throw new InvalidCastException("Setting cannot be used as a bool");
        }

        public static implicit operator string(Setting s)
        {
            object val = s.Value;
            if (val is string)
            {
                return (string)val;
            }

            throw new InvalidCastException("Setting cannot be used as a string");
        }

        public static implicit operator int(Setting s)
        {
            object val = s.Value;
            if (val is int)
            {
                return (int)val;
            }

            throw new InvalidCastException("Setting cannot be used as a int");
        }

        public static implicit operator uint(Setting s)
        {
            object val = s.Value;
            if (val is uint)
            {
                return (uint)val;
            }

            throw new InvalidCastException("Setting cannot be used as a uint");
        }

        public static implicit operator short(Setting s)
        {
            object val = s.Value;
            if (val is short)
            {
                return (short)val;
            }

            throw new InvalidCastException("Setting cannot be used as a short");
        }

        public static implicit operator ushort(Setting s)
        {
            object val = s.Value;
            if (val is ushort)
            {
                return (ushort)val;
            }

            throw new InvalidCastException("Setting cannot be used as a ushort");
        }

        public static implicit operator byte(Setting s)
        {
            object val = s.Value;
            if (val is byte)
            {
                return (byte)val;
            }

            throw new InvalidCastException("Setting cannot be used as a byte");
        }

        public static implicit operator float(Setting s)
        {
            object val = s.Value;
            if (val is float)
            {
                return (float)val;
            }

            throw new InvalidCastException("Setting cannot be used as a float");
        }

        #endregion
    }
}
