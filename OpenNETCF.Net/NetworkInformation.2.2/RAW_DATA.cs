using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Encapsulates generic data blob
    /// </summary>
    internal struct RAW_DATA : IDisposable
    {
        //byte[] m_data;
        private uint m_cbData;
        private IntPtr m_lpData;

        public RAW_DATA(byte[] data)
        {
            //m_data = new byte[0];
            m_lpData = IntPtr.Zero;
            m_cbData = (uint)data.Length;
            lpData = data;
        }
        public uint cbData { get { return m_cbData; } }
        public byte[] lpData
        {
            get
            {
                if (m_lpData == IntPtr.Zero)
                    return null;
                byte[] data = new byte[m_cbData];
                Marshal.Copy(m_lpData, data, 0, (int)m_cbData);
                return data;
            }
            set
            {
                FreeMemory();
                m_lpData = Marshal.AllocHGlobal(value.Length);
                Marshal.Copy(value, 0, m_lpData, value.Length);
            }
        }

        public IntPtr lpDataDirect
        {
            get
            {
                return m_lpData;
            }
        }

        internal void Clear()
        {
            m_lpData = IntPtr.Zero;
            m_cbData = 0;
        }
        #region IDisposable Members

        public void Dispose()
        {
            FreeMemory();
        }

        private void FreeMemory()
        {
            if (m_lpData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_lpData);
                m_lpData = IntPtr.Zero;
            }
        }
        #endregion
    }
}
