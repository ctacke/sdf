using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.ToolHelp
{
    internal static class ProcessAPI
    {
        public const int PROCESS_TERMINATE = 1;
        public const int INVALID_HANDLE_VALUE = -1;

        private static IProcessAPI m_api;

        static ProcessAPI()
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                m_api = new ProcessAPICE();
            }
            else
            {
                m_api = new ProcessAPIWin32();
            }
        }

        public static IntPtr OpenProcess(uint fdwAccess, bool fInherit, uint IDProcess)
        {
            return m_api.OpenProcess(fdwAccess, fInherit, IDProcess);
        }

        public static int CloseHandle(IntPtr hObject)
        {
            return m_api.CloseHandle(hObject);
        }

        public static bool TerminateProcess(IntPtr hProcess, int uExitCode)
        {
            return m_api.TerminateProcess(hProcess, uExitCode);
        }

        public static bool TerminateThread(uint hThread, int dwExitCode)
        {
            return m_api.TerminateThread(hThread, dwExitCode);
        }

        public static uint SuspendThread(uint hThread)
        {
            return m_api.SuspendThread(hThread);
        }

        public static uint ResumeThread(uint hThread)
        {
            return m_api.ResumeThread(hThread);
        }

        public static int CeGetThreadPriority(IntPtr hThread)
        {
            return m_api.CeGetThreadPriority(hThread);
        }

        public static bool CeSetThreadPriority(uint hThread, int nPriority)
        {
            return m_api.CeSetThreadPriority(hThread, nPriority);
        }

        public static bool CeSetThreadQuantum(uint hThread, int dwTime)
        {
            return m_api.CeSetThreadQuantum(hThread, dwTime);
        }

        public static int CeGetThreadQuantum(IntPtr hThread)
        {
            return m_api.CeGetThreadQuantum(hThread);
        }

        #region --- IProcessAPI interface ---
        private interface IProcessAPI
        {
            IntPtr OpenProcess(uint fdwAccess, bool fInherit, uint IDProcess);
            int CloseHandle(IntPtr hObject);
            bool TerminateProcess(IntPtr hProcess, int uExitCode);
            bool TerminateThread(uint hThread, int dwExitCode);
            uint SuspendThread(uint hThread);
            uint ResumeThread(uint hThread);
            int GetThreadPriority(IntPtr hThread);
            bool SetThreadPriority(uint hThread, int nPriority);
            int CeGetThreadPriority(IntPtr hThread);
            bool CeSetThreadPriority(uint hThread, int nPriority);
            bool CeSetThreadQuantum(uint hThread, int dwTime);
            int CeGetThreadQuantum(IntPtr hThread);
        }
        #endregion

        #region --- CE Implementation ---
        private class ProcessAPICE : IProcessAPI
        {
            private const string DLL = "coredll.dll";
            public IntPtr OpenProcess(uint fdwAccess, bool fInherit, uint IDProcess)
            {
                return OpenProcessAPI(fdwAccess, fInherit, IDProcess);
            }

            public int CloseHandle(IntPtr hObject)
            {
                return CloseHandleAPI(hObject);
            }

            public bool TerminateProcess(IntPtr hProcess, int uExitCode)
            {
                return TerminateProcessAPI(hProcess, uExitCode);
            }

            public bool TerminateThread(uint hThread, int dwExitCode)
            {
                return TerminateThreadAPI(hThread, dwExitCode);
            }

            public uint SuspendThread(uint hThread)
            {
                return SuspendThreadAPI(hThread);
            }

            public uint ResumeThread(uint hThread)
            {
                return ResumeThreadAPI(hThread);
            }

            public int GetThreadPriority(IntPtr hThread)
            {
                return GetThreadPriorityAPI(hThread);
            }

            public bool SetThreadPriority(uint hThread, int nPriority)
            {
                return SetThreadPriorityAPI(hThread, nPriority);
            }

            public int CeGetThreadPriority(IntPtr hThread)
            {
                return CeGetThreadPriorityAPI(hThread);
            }

            public bool CeSetThreadPriority(uint hThread, int nPriority)
            {
                return CeSetThreadPriorityAPI(hThread, nPriority);
            }

            public bool CeSetThreadQuantum(uint hThread, int dwTime)
            {
                return CeSetThreadQuantumAPI(hThread, dwTime);
            }

            public int CeGetThreadQuantum(IntPtr hThread)
            {
                return CeGetThreadQuantumAPI(hThread);
            }

            [DllImport(DLL, EntryPoint = "OpenProcess", SetLastError = true)]
            public extern static IntPtr OpenProcessAPI(uint fdwAccess, bool fInherit, uint IDProcess);

            [DllImport(DLL, EntryPoint = "CloseHandle", SetLastError = true)]
            public static extern int CloseHandleAPI(IntPtr hObject);

            [DllImport(DLL, EntryPoint = "TerminateProcess", SetLastError = true)]
            public extern static bool TerminateProcessAPI(IntPtr hProcess, int uExitCode);

            [DllImport(DLL, EntryPoint = "TerminateThread", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool TerminateThreadAPI(uint hThread, int dwExitCode);

            [DllImport(DLL, EntryPoint = "SuspendThread", SetLastError = true)]
            public static extern uint SuspendThreadAPI(uint hThread);

            [DllImport(DLL, EntryPoint = "ResumeThread", SetLastError = true)]
            public static extern uint ResumeThreadAPI(uint hThread);

            [DllImport(DLL, EntryPoint = "GetThreadPriority", SetLastError = true)]
            public static extern int GetThreadPriorityAPI(IntPtr hThread);

            [DllImport(DLL, EntryPoint = "SetThreadPriority", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetThreadPriorityAPI(uint hThread, int nPriority);

            [DllImport(DLL, EntryPoint = "CeGetThreadPriority", SetLastError = true)]
            public static extern int CeGetThreadPriorityAPI(IntPtr hThread);

            [DllImport(DLL, EntryPoint = "CeSetThreadPriority", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CeSetThreadPriorityAPI(uint hThread, int nPriority);

            [DllImport(DLL, EntryPoint = "CeSetThreadQuantum", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CeSetThreadQuantumAPI(uint hThread, int dwTime);

            [DllImport(DLL, EntryPoint = "CeGetThreadQuantum", SetLastError = true)]
            public static extern int CeGetThreadQuantumAPI(IntPtr hThread);
        }
        #endregion

        #region --- Desktop implementation ---
        private class ProcessAPIWin32 : IProcessAPI
        {
            private const string DLL = "kernel32.dll";
            public IntPtr OpenProcess(uint fdwAccess, bool fInherit, uint IDProcess)
            {
                return OpenProcessAPI(fdwAccess, fInherit, IDProcess);
            }

            public int CloseHandle(IntPtr hObject)
            {
                return CloseHandleAPI(hObject);
            }

            public bool TerminateProcess(IntPtr hProcess, int uExitCode)
            {
                return TerminateProcessAPI(hProcess, uExitCode);
            }

            public bool TerminateThread(uint hThread, int dwExitCode)
            {
                return TerminateThreadAPI(hThread, dwExitCode);
            }

            public uint SuspendThread(uint hThread)
            {
                return SuspendThreadAPI(hThread);
            }

            public uint ResumeThread(uint hThread)
            {
                return ResumeThreadAPI(hThread);
            }

            public int GetThreadPriority(IntPtr hThread)
            {
                return GetThreadPriorityAPI(hThread);
            }

            public bool SetThreadPriority(uint hThread, int nPriority)
            {
                return SetThreadPriorityAPI(hThread, nPriority);
            }

            public int CeGetThreadPriority(IntPtr hThread)
            {
                throw new PlatformNotSupportedException("This is a Windows CE-specific call");
            }

            public bool CeSetThreadPriority(uint hThread, int nPriority)
            {
                throw new PlatformNotSupportedException("This is a Windows CE-specific call");
            }

            public bool CeSetThreadQuantum(uint hThread, int dwTime)
            {
                throw new PlatformNotSupportedException("This is a Windows CE-specific call");
            }

            public int CeGetThreadQuantum(IntPtr hThread)
            {
                throw new PlatformNotSupportedException("This is a Windows CE-specific call");
            }

            [DllImport(DLL, EntryPoint = "OpenProcess", SetLastError = true)]
            public extern static IntPtr OpenProcessAPI(uint fdwAccess, bool fInherit, uint IDProcess);

            [DllImport(DLL, EntryPoint = "CloseHandle", SetLastError = true)]
            public static extern int CloseHandleAPI(IntPtr hObject);

            [DllImport(DLL, EntryPoint = "TerminateProcess", SetLastError = true)]
            public extern static bool TerminateProcessAPI(IntPtr hProcess, int uExitCode);

            [DllImport(DLL, EntryPoint = "TerminateThread", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool TerminateThreadAPI(uint hThread, int dwExitCode);

            [DllImport(DLL, EntryPoint = "SuspendThread", SetLastError = true)]
            public static extern uint SuspendThreadAPI(uint hThread);

            [DllImport(DLL, EntryPoint = "ResumeThread", SetLastError = true)]
            public static extern uint ResumeThreadAPI(uint hThread);

            [DllImport(DLL, EntryPoint = "GetThreadPriority", SetLastError = true)]
            public static extern int GetThreadPriorityAPI(IntPtr hThread);

            [DllImport(DLL, EntryPoint = "SetThreadPriority", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetThreadPriorityAPI(uint hThread, int nPriority);
        }
        #endregion
    }
}
