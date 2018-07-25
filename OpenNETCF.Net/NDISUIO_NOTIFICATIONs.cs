using System;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net
{
    internal class NDISUIO_REQUEST_NOTIFICATION
    {
        public const int Size = 8;
        byte[] data = new byte[Size];

        public IntPtr hMsgQueue
        {
            get
            {
                return (IntPtr)BitConverter.ToUInt32(data, 0);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((UInt32)value);
                Buffer.BlockCopy(bytes, 0, data, 0, 4);
            }
        }

        public uint dwNotificationTypes
        {
            get
            {
                return BitConverter.ToUInt32(data, 4);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, data, 4, 4);
            }
        }

        public byte[] getBytes()
        {
            return data;
        }
    }
}
