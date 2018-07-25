#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
