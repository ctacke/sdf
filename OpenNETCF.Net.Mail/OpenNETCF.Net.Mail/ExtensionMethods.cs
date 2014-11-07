using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF
{
    public static class ExtensionMethods
    {
        public static string BodyName(this Encoding encoding)
        {
            return encoding.WebName;
        }

        public static string FormatString(this System.Resources.ResourceManager resource, string name, object args)
        {
            return FormatString(resource, name, new object[] { args });
        }

        public static string FormatString(this System.Resources.ResourceManager resource, string format, params object[] args)
        {
            if ((args == null) || (args.Length <= 0))
            {
                return format;
            }
            for (int i = 0; i < args.Length; i++)
            {
                string str2 = args[i] as string;
                if ((str2 != null) && (str2.Length > 1024))
                {
                    args[i] = str2.Substring(0, 1021) + "...";
                }
            }
            return string.Format(format, args);
        }
    }
}
