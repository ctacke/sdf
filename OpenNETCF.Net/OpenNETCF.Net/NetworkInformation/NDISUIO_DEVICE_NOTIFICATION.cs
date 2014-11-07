using System;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net
{
    internal class NDISUIO_DEVICE_NOTIFICATION : Message
    {
        public const int Size = 532;

        internal byte[] data = new byte[Size];

        public NDISUIO_DEVICE_NOTIFICATION()
        {
        }

        public int dwNotificationType
        {
            get
            {
                return BitConverter.ToInt32(data, 0);
            }
        }

        public string ptcDeviceName
        {
            get
            {
                String s = System.Text.Encoding.Unicode.GetString(data, 4, Size - 4);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        public override byte[] MessageBytes
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }

}
