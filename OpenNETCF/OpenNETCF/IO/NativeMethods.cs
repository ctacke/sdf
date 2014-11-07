using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenNETCF.IO
{
    internal static class NativeMethods
    {
        #region Drive P/Invokes

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool GetDiskFreeSpaceEx(string directoryName, ref long freeBytesAvailable, ref long totalBytes, ref long totalFreeBytes);

        #endregion

        #region File P/Invokes

        internal const int ERROR_NO_MORE_FILES = 18;
        internal const uint GENERIC_READ = 0x80000000;
        internal const uint OPEN_EXISTING = 3;

        [DllImport("coredll.dll", EntryPoint = "CreateFile", SetLastError = true)]
        internal static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            int lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            int hTemplateFile);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl<TInput, TOutput>(
            IntPtr hDevice,
            uint dwIoControlCode,
            ref TInput lpInBuffer,
            int nInBufferSize,
            ref TOutput lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped)
            where TInput : struct
            where TOutput : struct;

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl<TInput>(
            IntPtr hDevice,
            uint dwIoControlCode,
            ref TInput lpInBuffer,
            int nInBufferSize,
            ref IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped)
            where TInput : struct;

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl<TOutput>(
            IntPtr hDevice,
            uint dwIoControlCode,
            ref IntPtr lpInBuffer,
            int nInBufferSize,
            ref TOutput lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped)
            where TOutput : struct;

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal unsafe static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            void* lpInBuffer,
            int nInBufferSize,
            void* lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            int nInBufferSize,
            byte[] lpOutBuffer,
            out int nOutBufferSize,
            IntPtr lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            int nInBufferSize,
            byte[] lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            IntPtr lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            byte[] lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        internal static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "SetFilePointer", SetLastError = true)]
        internal static extern int SetFilePointer(
            IntPtr hFile,
            int lDistanceToMove,
            int lpDistanceToMoveHigh,
            SeekOrigin dwMoveMethod);

        [DllImport("coredll.dll", EntryPoint = "WriteFile", SetLastError = true)]
        internal static extern bool WriteFile(
            IntPtr hFile,
            Byte[] lpBuffer,
            int nNumberOfBytesToWrite,
            ref int lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "ReadFile", SetLastError = true)]
        internal static extern bool ReadFile(
            IntPtr hFile,
            byte[] lpBuffer,
            int nNumberOfBytesToRead,
            ref int lpNumberOfBytesRead,
            IntPtr lpOverlapped);

        [DllImport("coredll.dll", EntryPoint = "GetFileAttributes", SetLastError = true)]
        internal static extern uint GetFileAttributes(string lpFileName);

        [DllImport("coredll.dll", EntryPoint = "SetFileAttributes", SetLastError = true)]
        internal static extern bool SetFileAttributes(string lpFileName, uint dwFileAttributes);

        [DllImport("coredll.dll", EntryPoint = "SetFileTime", SetLastError = true)]
        internal static extern bool SetFileTime(IntPtr hFile, byte[] lpCreationTime, byte[] lpLastAccessTime, byte[] lpLastWriteTime);

        #endregion

        #region Memory P/Invokes

        internal const uint MEM_RESERVE = 0x2000;
        internal const uint PAGE_NOACCESS = 0x0001;
        internal const uint PAGE_READWRITE = 0x0004;
        internal const uint PAGE_NOCACHE = 0x200;
        internal const uint PAGE_PHYSICAL = 0x400;
        internal const uint MEM_RELEASE = 0x8000;

        [DllImport("coredll.dll", EntryPoint = "VirtualAlloc", SetLastError = true)]
        internal static extern IntPtr VirtualAlloc(uint lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("coredll.dll", EntryPoint = "VirtualCopy", SetLastError = true)]
        internal static extern int VirtualCopy(IntPtr lpvDest, IntPtr lpvSrc, uint cbSize, uint fdwProtect);

        [DllImport("coredll.dll", EntryPoint = "VirtualFree", SetLastError = true)]
        internal static extern int VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);

        #endregion

        #region Watcher P/Invokes
        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern int SHChangeNotifyRegister(
            IntPtr hwnd,
            ref SHCHANGENOTIFYENTRY pshcne);

        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern int SHChangeNotifyRegister(
            IntPtr hwnd,
            IntPtr pshcne);

        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern void SHChangeNotifyFree(IntPtr pshcne);

        [DllImport("aygshell.dll", SetLastError = true)]
        internal static extern int SHChangeNotifyDeregister(
            IntPtr hwnd);

        internal struct SHCHANGENOTIFYENTRY
        {
            internal int dwEventMask;
            internal IntPtr pszWatchDir;
            internal int fRecursive;
        }

        #endregion

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern IntPtr ActivateDevice(string lpszDevKey, uint dwClientInfo);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool DeactivateDevice(IntPtr hDevice);
    }
}
