using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Services
{
    internal static class NativeMethods
    {
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr GetServiceHandle(string szPrefix, string szDllName, uint pdwDllBuf);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr GetServiceHandle(string szPrefix, IntPtr szDllName, uint pdwDllBuf);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int DeregisterService(IntPtr hDevice);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr ActivateService(string lpszDevKey, uint dwClientInfo);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr RegisterService(string lpszType, uint dwIndex, string lpszLib, uint dwInfo);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int EnumServices(byte[] pBuffer, out int pdwServiceEntries, ref int pdwBufferLen);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int ServiceIoControl(IntPtr hService, ServiceIoctl dwIoControlCode, IntPtr lpInBuf,
            int nInBufSize, IntPtr lpOutBuf, int nOutBufSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int ServiceIoControl(IntPtr hService, ServiceIoctl dwIoControlCode, ref int lpInBuf,
            int nInBufSize, IntPtr lpOutBuf, int nOutBufSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int ServiceIoControl(IntPtr hService, ServiceIoctl dwIoControlCode, string lpInBuf,
            int nInBufSize, IntPtr lpOutBuf, int nOutBufSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int ServiceIoControl(IntPtr hService, ServiceIoctl dwIoControlCode, IntPtr lpInBuf,
            int nInBufSize, out int lpOutBuf, int nOutBufSize, out int lpBytesReturned, IntPtr lpOverlapped);

        public enum ServiceIoctl
        {
            Start = 0x01040004,
            Stop = 0x01040008,
            Refresh = 0x0104000C,
            Install = 0x01040010,
            Uninstall = 0x01040014,
            Unload = 0x01040018,
            Control = 0x0104001C,
            Status = 0x01040020,
            Debug = 0x01040024,
            Console = 0x01040028
        }
    }
}
