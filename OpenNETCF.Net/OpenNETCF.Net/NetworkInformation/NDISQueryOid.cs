using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    internal class NDIS_REQUEST
    {
    }

    internal class NDISQueryOid
    {
        protected byte[] m_data;
        protected int ourSize;
        public int Size
        {
            get { return ourSize; }
        }

        protected const int NDISUIO_QUERY_OID_SIZE = 12;
        public NDISQueryOid(byte[] data)
        {
            int extrasize = data.Length;
            // Most of the cases we'll use will have a size
            // of just sizeof( DWORD ), but you never know.
            ourSize = 8 + extrasize;
            m_data = new byte[ourSize];
            Buffer.BlockCopy(data, 0, m_data, DataOffset, data.Length);
        }

        public NDISQueryOid(int extrasize)
        {
            // Most of the cases we'll use will have a size
            // of just sizeof( DWORD ), but you never know.
            ourSize = NDISUIO_QUERY_OID_SIZE + extrasize;
            m_data = new byte[ourSize];
        }

        protected const int OidOffset = 0;
        public uint Oid
        {
            get { return BitConverter.ToUInt32(m_data, OidOffset); }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, m_data, OidOffset, 4);
            }
        }

        protected const int ptcDeviceNameOffset = OidOffset + 4;
        public unsafe byte* ptcDeviceName
        {
            get
            {
                return (byte*)BitConverter.ToUInt32(m_data, ptcDeviceNameOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((UInt32)value);
                Buffer.BlockCopy(bytes, 0, m_data, ptcDeviceNameOffset, 4);
            }
        }

        protected const int DataOffset = ptcDeviceNameOffset + 4;
        public byte[] Data
        {
            get
            {
                byte[] b = new byte[ourSize - DataOffset];
                Array.Copy(m_data, DataOffset, b, 0, ourSize - DataOffset);
                return b;
            }
            set
            {
                ourSize = 8 + value.Length;
                m_data = new byte[ourSize];
                Buffer.BlockCopy(value, 0, m_data, DataOffset, value.Length);
            }
        }

        public byte[] getBytes()
        {
            return m_data;
        }

        public static implicit operator byte[](NDISQueryOid qoid)
        {
            return qoid.m_data;
        }
    }
}
