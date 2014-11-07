using System;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.WindowsCE
{
    internal class DeviceDetail : Message
    {
        public const int MAX_DEVDETAIL_SIZE = 64 + 16 + 12;
        public const int MAX_DEVCLASS_NAMELEN = 64;

        internal byte[] data = new byte[MAX_DEVDETAIL_SIZE];

        public DeviceDetail()
        {            
           
        }

        public Guid guidDevClass
        {
            get
            {
                // Extract a 16-byte array from the structure and send that
                // to Guid to make a new Guid.
                byte[] b = new byte[16];
                Array.Copy(data, 0, b, 0, b.Length);

                return new Guid(b);
            }
        }

        public int dwReserved
        {
            get
            {
                return BitConverter.ToInt32(data, 16);
            }
        }

        public bool fAttached
        {
            get
            {
                return BitConverter.ToBoolean(data, 20);
            }
        }

        public int cbName
        {
            get
            {
                return BitConverter.ToInt32(data, 24);
            }
        }

        public string szName
        {
            get
            {
                String s = System.Text.Encoding.Unicode.GetString(data, 28, cbName);
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
