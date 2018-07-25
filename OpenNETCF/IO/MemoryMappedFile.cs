using System;
using System.Runtime.InteropServices;
using System.IO;

namespace OpenNETCF.IO
{
    public class MemoryMappedFile : Stream
    {
        private IntPtr m_mutex;
        private string m_name;
        private IntPtr m_mapping;
        private IntPtr m_view;
        private long m_length;
        private long m_pos = 0;

        public static MemoryMappedFile CreateInMemoryMap(string name, long maxSize)
        {
            return new MemoryMappedFile(new IntPtr(INVALID_HANDLE_VALUE), name, maxSize, IntPtr.Zero);
        }

        public static MemoryMappedFile CreateWithFileBacking(string backingFile, bool failIfFileExists, long maxSize)
        {
            IntPtr fileHandle = new IntPtr(INVALID_HANDLE_VALUE);

            if (backingFile == null)
            {
                throw new ArgumentException();
            }

            backingFile = backingFile.Trim();
            if (backingFile.Length == 0)
            {
                throw new ArgumentException();
            }

            // Is this the first instance of this mapping object for this backing file?
            // We must only call CreateFileForMapping *once*
            string mutexName = backingFile + "_MEM_MAP";

            IntPtr mutex = CreateMutex(IntPtr.Zero, true, backingFile);
            if (mutex == IntPtr.Zero) 
            {
                throw new System.ComponentModel.Win32Exception();
            }

            if (Marshal.GetLastWin32Error() != ERROR_ALREADY_EXISTS)
            {
                // we're the first caller - create the file
                if (failIfFileExists && File.Exists(backingFile))
                {
                    throw new IOException(string.Format("File '{0}' Already Exists", backingFile));
                }

                fileHandle = CreateFileForMapping(backingFile, 0xC0000000, 0x03, IntPtr.Zero, FileMode.OpenOrCreate, 0x80, IntPtr.Zero);

                if (fileHandle.ToInt32() == INVALID_HANDLE_VALUE)
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }

            return new MemoryMappedFile(fileHandle, backingFile, maxSize, mutex);
        }

        private MemoryMappedFile(IntPtr fileHandle, string name, long maxSize, IntPtr mutex)
        {
            m_mutex = mutex;
            m_name = name;
            m_length = maxSize;

            m_mapping = CreateFileMapping(
                fileHandle, 
                IntPtr.Zero, 
                PageProtection.ReadWrite, 
                (uint)(maxSize >> 32), 
                (uint)(maxSize & 0xFFFFFFFF), 
                m_name);

            if (m_mapping == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            m_view = MapViewOfFile(m_mapping, SECTION_ALL_ACCESS, 0, 0, 0);

            if (m_view == IntPtr.Zero)
            {
                CloseHandle(m_mapping);
                throw new System.ComponentModel.Win32Exception();
            }
 
        }

        public string Name
        {
            get { return m_name; }
        }

        public override void Close()
        {
            Flush();

            if (m_mutex != IntPtr.Zero)
                CloseHandle(m_mutex);

            if(m_mapping != IntPtr.Zero)
                CloseHandle(m_mapping);

            if (m_view != IntPtr.Zero)
                UnmapViewOfFile(m_view);

            base.Close();
        }

        public override long Length
        {
            get { return m_length; }
        }

        public override long Position
        {
            // TODO: add bounds checking
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        public override void Flush()
        {
            FlushViewOfFile(m_mapping, (uint)Length);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            // TODO: add bounds checking
            switch (origin)
            {
                case SeekOrigin.Current:
                    m_pos += offset;
                    break;
                case SeekOrigin.Begin:
                    m_pos = offset;
                    break;
                case SeekOrigin.End:
                    m_pos = Length - offset;
                    break;
            }

            return m_pos;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // TODO: add bounds checking
            Marshal.Copy(PosPtr, buffer, offset, count);
            m_pos += count;
            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // TODO: add bounds checking
            Marshal.Copy(buffer, offset, PosPtr, count);
            m_pos += count;
        }

        private IntPtr PosPtr
        {
            get { return new IntPtr(m_view.ToInt32() + m_pos); }
        }

        #region --- Capability Properties ---
        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanTimeout { get { return false; } }
        public override bool CanWrite { get { return true; } }
        #endregion

        #region --- Unsupported methods/properties ---
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int WriteTimeout
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int ReadTimeout
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }        
        #endregion

        #region --- P/Invokes ---
        /*
        HANDLE CreateFileForMapping( 
          LPCTSTR lpFileName, 
          DWORD dwDesiredAccess, 
          DWORD dwShareMode, 
          LPSECURITY_ATTRIBUTES lpSecurityAttributes, 
          DWORD dwCreationDisposition, 
          DWORD dwFlagsAndAttributes, 
          HANDLE hTemplateFile 
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr CreateFileForMapping(
            string lpFileName, 
            uint dwDesiredAccess, 
            int dwShareMode, 
            IntPtr lpSecurityAttributes, 
            FileMode dwCreationDisposition, 
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        /*
        HANDLE CreateFileMapping(
          HANDLE hFile, 
          LPSECURITY_ATTRIBUTES lpFileMappingAttributes, 
          DWORD flProtect, 
          DWORD dwMaximumSizeHigh, 
          DWORD dwMaximumSizeLow, 
          LPCTSTR lpName 
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            IntPtr lpFileMappingAttributes,
            PageProtection flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName);

        /*
        LPVOID WINAPI MapViewOfFile(
          HANDLE hFileMappingObject,
          DWORD dwDesiredAccess,
          DWORD dwFileOffsetHigh,
          DWORD dwFileOffsetLow,
          SIZE_T dwNumberOfBytesToMap
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        /*
        BOOL WINAPI UnmapViewOfFile(
          LPCVOID lpBaseAddress
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        /*
        HANDLE WINAPI CreateMutex(
          LPSECURITY_ATTRIBUTES lpMutexAttributes,
          BOOL bInitialOwner,
          LPCTSTR lpName
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr CreateMutex(
            IntPtr lpMutexAttributes, 
            bool bInitialOwner, 
            string lpName);

        /*
        BOOL WINAPI CloseHandle(
          HANDLE hObject
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        /*
        BOOL FlushViewOfFile(
          LPCVOID lpBaseAddress,
          DWORD dwNumberOfBytesToFlush
        );
        */
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool FlushViewOfFile(IntPtr lpBaseAddress, uint dwNumberOfBytesToFlush);

        private const uint FILE_MAPPING_FLAGS = 0x90000000; // FILE_FLAG_WRITE_THROUGH | FILE_FLAG_RANDOM_ACCESS
        private const int ERROR_ALREADY_EXISTS = 183;
        private const int INVALID_HANDLE_VALUE = -1;
        private const int SECTION_ALL_ACCESS = 0xF001F;

        private enum PageProtection
        {
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08
        }
        #endregion
    }
}
