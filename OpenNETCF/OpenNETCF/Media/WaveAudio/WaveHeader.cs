using System;
using OpenNETCF.Win32;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
    //typedef struct WAVEHDR {
    //    LPSTR	lpData;
    //    DWORD	dwBufferLength;
    //    DWORD	dwBytesRecorded;
    //    DWORD	dwUser;
    //    DWORD	dwFlags;
    //    DWORD	dwLoops;
    //    struct WAVEHDR *lpNext;
    //    DWORD	reserved;
    //} WAVEHDR;

    /// <summary>
    /// Internal wrapper around WAVEHDR
    /// Facilitates asynchronous operations
    /// </summary>
    internal class WaveHeader : IDisposable
    {
        private byte[] m_headerData;
        private GCHandle m_headerHandle;
        private GCHandle m_bufferHandle;
        private IntPtr m_pBuffer;

        public WaveHeader(byte[] data)
        {
            InitFromData(data, data.Length);
        }

        /// <summary>
        /// Creates WaveHeader and fills it with wave data
        /// </summary>
        /// <param name="data">wave data bytes</param>
        /// <param name="datalength">length of Wave data</param>
        public WaveHeader(byte[] data, int datalength)
        {
            InitFromData(data, datalength);
        }

        /// <summary>
        /// Constructor for WaveHeader class
        /// Allocates a buffer of required size
        /// </summary>
        /// <param name="BufferSize"></param>
        public WaveHeader(int BufferSize)
        {
            InitFromData(null, BufferSize);
        }

        public WaveHeader()
        {
            InitFromData(null, 32);
        }

        private void InitFromData(byte[] data, int datalength)
        {
            BufferData = data == null ? new byte[datalength] : data;

            m_bufferHandle = GCHandle.Alloc(BufferData, GCHandleType.Pinned);
            m_pBuffer = m_bufferHandle.AddrOfPinnedObject();

            HeaderLength = 32;
            m_headerData = new byte[HeaderLength];
            m_headerHandle = GCHandle.Alloc(m_headerData, GCHandleType.Pinned);
            Pointer = m_headerHandle.AddrOfPinnedObject();

            BufferPointer = m_pBuffer;
            BufferLength = BufferData.Length;
        }

        public int HeaderLength { get; private set; }
        public byte[] BufferData { get; private set; }

        public IntPtr Pointer { get; private set; }

        public unsafe IntPtr BufferPointer
        {
            get
            {
                return new IntPtr(*(int*)Pointer);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value.ToInt32()), 0, m_headerData, 0, 4);
            }
        }

        public byte[] GetBytes()
        {
            return m_headerData;
        }

        internal unsafe int BufferLength
        {
            get
            {
                int* p = (int*)Pointer;
                p += 1;
                return *p;
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_headerData, 4, 4);
            }
        }

        internal unsafe int BytesRecorded
        {
            get
            {
                int* p = (int*)Pointer;
                p += 2;
                return *p;
            }
        }

        internal unsafe WHDR_FLAGS Flags
        {
            get
            {
                int* p = (int*)Pointer;
                p += 4;
                return (WHDR_FLAGS)(*p);
            }
        }

        public void Dispose()
        {
            m_headerHandle.Free();
            m_bufferHandle.Free();
        }

    }
}
