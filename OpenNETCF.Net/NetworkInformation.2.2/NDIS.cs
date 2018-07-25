using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net.NetworkInformation
{
    internal sealed class NDIS : OpenNETCF.IO.StreamInterfaceDriver
    {
        public const uint IOCTL_NDIS_BIND_ADAPTER = 0x00170032;
        public const uint IOCTL_NDIS_REBIND_ADAPTER = 0x0017002e;
        public const uint IOCTL_NDIS_UNBIND_ADAPTER = 0x00170036;

        private NDIS()
            : base("NDS0:")
        {
        }

        public static void BindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_BIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static void UnbindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_UNBIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static void RebindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_REBIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }
    }
}
