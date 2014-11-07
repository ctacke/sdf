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

        public const int DefaultInMemoryMapSize = 0x10000; // 64k
        public const int UseFileSizeForMaximumMapSize = 0;

        /// <summary>
        /// Creates an unnamed MemoryMappedFile instance using an in-memory map as the backing object with a maximum size of <b>DefaultInMemoryMapSize</b>
        /// </summary>
        /// <returns></returns>
        public static MemoryMappedFile CreateInMemoryMap()
        {
            return CreateInMemoryMap(null, DefaultInMemoryMapSize);
        }

        /// <summary>
        /// Creates a named MemoryMappedFile instance using an in-memory map as the backing object with a maximum size of <b>DefaultInMemoryMapSize</b>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MemoryMappedFile CreateInMemoryMap(string name)
        {
            return CreateInMemoryMap(name, DefaultInMemoryMapSize);
        }

        /// <summary>
        /// Creates a named MemoryMappedFile instance using an in-memory map as the backing object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static MemoryMappedFile CreateInMemoryMap(string name, long maxSize)
        {
            return new MemoryMappedFile(new IntPtr(INVALID_HANDLE_VALUE), name, maxSize, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a MemoryMappedFile instance using a file system file as the backing object
        /// </summary>
        /// <param name="backingFile"></param>
        /// <param name="failIfFileExists"></param>
        /// <param name="maxSize">A positive number or the UseFileSizeForMaximumMapSize constant</param>
        /// <returns></returns>
        public static MemoryMappedFile CreateWithFileBacking(string backingFile, bool failIfFileExists, long maxSize)
        {
            IntPtr fileHandle = new IntPtr(INVALID_HANDLE_VALUE);

            if (backingFile == null) throw new ArgumentNullException("backingFile");

            backingFile = backingFile.Trim();
            if (backingFile.Length == 0) throw new ArgumentException();
            if (maxSize < 0) throw new ArgumentException("Size must be >= 0");

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

        /// <summary>
        /// The name of the MemoryMapped file provided during construction
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// Closes and releases all resources associated with the stream
        /// </summary>
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

        /// <summary>
        /// Gets the maximum length of the MemoryMappedFile
        /// </summary>
        public override long Length
        {
            get { return m_length; }
        }

        /// <summary>
        /// Gets or sets the current position within the MemoryMappedFile stream
        /// </summary>
        public override long Position
        {
            get { return m_pos; }
            set
            {
                // bounds checking
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Attempt to seek before start of stream");
                }
                if (value > Length)
                {
                    throw new ArgumentOutOfRangeException("Attempt to seek after end of stream");
                }

                m_pos = value;
            }
        }

        /// <summary>
        /// Flushes all pending writes
        /// </summary>
        public override void Flush()
        {
            FlushViewOfFile(m_mapping, (uint)Length);
        }

        /// <summary>
        /// Moves the current stream position pointer
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPos = 0;

            switch (origin)
            {
                case SeekOrigin.Current:
                    newPos = m_pos + offset;
                    break;
                case SeekOrigin.Begin:
                    newPos = offset;
                    break;
                case SeekOrigin.End:
                    newPos = Length + offset;
                    break;
            }

            // bounds checking
            if (newPos < 0)
            {
                throw new ArgumentOutOfRangeException("Attempt to seek before start of stream");
            }
            if (newPos > Length)
            {
                throw new ArgumentOutOfRangeException("Attempt to seek after end of stream");
            }

            m_pos = newPos;

            return m_pos;
        }

        /// <summary>
        /// Reads data from the MemoryMappedFile
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            long startPos = Position + offset;
            long endPos = Position + offset + count;

            if (startPos < 0) throw new ArgumentOutOfRangeException("Attempt to read before the start of the stream");
            if (endPos > Length) throw new EndOfStreamException("Attempt to read past end of the stream");

            Marshal.Copy(PosPtr, buffer, offset, count);
            m_pos += count;
            return count;
        }

        /// <summary>
        /// Writes data to the MemoryMappedFile
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            long startPos = Position + offset;
            long endPos = Position + offset + count;

            if (startPos < 0) throw new ArgumentOutOfRangeException("Attempt to write before the start of the stream");
            if (endPos > Length) throw new EndOfStreamException("Attempt to write past end of the stream");

            Marshal.Copy(buffer, offset, PosPtr, count);
            m_pos += count;
        }

        private IntPtr PosPtr
        {
            get { return new IntPtr(m_view.ToInt32() + m_pos); }
        }

        #region --- Capability Properties ---
        /// <summary>
        /// Always returns <b>true</b> for MemoryMappedFiles
        /// </summary>
        public override bool CanRead { get { return true; } }
        /// <summary>
        /// Always returns <b>true</b> for MemoryMappedFiles
        /// </summary>
        public override bool CanSeek { get { return true; } }
        /// <summary>
        /// Always returns <b>false</b> for MemoryMappedFiles
        /// </summary>
        public override bool CanTimeout { get { return false; } }
        /// <summary>
        /// Always returns <b>true</b> for MemoryMappedFiles
        /// </summary>
        public override bool CanWrite { get { return true; } }
        #endregion

        #region --- Unsupported methods/properties ---
        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        /// <param name="value">Not Supported</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        public override int WriteTimeout
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        public override int ReadTimeout
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
        public override int EndRead(IAsyncResult asyncResult)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a NotSupportedException
        /// </summary>
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
