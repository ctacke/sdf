using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// List of interface names
    /// </summary>
    internal struct INTFS_KEY_TABLE : IDisposable
    {
        private uint dwNumIntfs;
        public IntPtr pData;

        public INTFS_KEY_TABLE(uint size)
        {
            dwNumIntfs = size;
            if (size != 0)
            {
                pData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * (int)size);
            }
            else
                pData = IntPtr.Zero;
        }

        public string this[uint i]
        {
            get
            {
                return Marshal.PtrToStringUni((IntPtr)(Marshal.ReadInt32(pData, (int)i * 4)));
            }
        }

        public uint Count
        {
            get { return dwNumIntfs; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (pData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        #endregion
    }
}
