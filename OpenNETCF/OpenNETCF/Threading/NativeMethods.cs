using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Contains native API calls for Threading related functionality.
	/// </summary>
	internal static partial class NativeMethods
	{
		public const Int32 WAIT_FAILED = -1;
		public const Int32 WAIT_TIMEOUT = 0x102;
		public const Int32 EVENT_ALL_ACCESS = 0x3;
		public const Int32 ERROR_ALREADY_EXISTS = 183;
        public const Int32 INFINITE = -1;

		//Events
		public enum EVENT
		{
			PULSE = 1,
			RESET = 2,
			SET = 3,
		}
		
		[DllImport("coredll.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EventModify(IntPtr hEvent, EVENT ef);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern Int32 WaitForSingleObject(IntPtr hHandle, Int32 dwMilliseconds);
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int WaitForMultipleObjects(int nCount, IntPtr[] lpHandles, bool fWaitAll, int dwMilliseconds); 
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int MsgWaitForMultipleObjectsEx(uint nCount, IntPtr[] lpHandles, uint dwMilliseconds, uint dwWakeMask, uint dwFlags); 
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr OpenEvent(Int32 dwDesiredAccess, bool bInheritHandle, string lpName);
		
		//Handle
		[DllImport("coredll.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		//Semaphore
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateSemaphore(IntPtr lpSemaphoreAttributes, Int32 lInitialCount, Int32 lMaximumCount, string lpName);
		
        [DllImport("coredll.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseSemaphore(IntPtr handle, Int32 lReleaseCount, out Int32 previousCount);
		
        [DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr OpenSemaphore(int desiredAccess, bool inheritHandle, string name);
		

		//Thread
		[DllImport("coredll.dll", EntryPoint="TerminateThread", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateThread(IntPtr hThread, int dwExitCode);

		[DllImport("coredll.dll", EntryPoint="TerminateThread", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TerminateThread(uint hThread, int dwExitCode);

		[DllImport("coredll.dll", EntryPoint="SuspendThread", SetLastError = true)]
		public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("coredll.dll", EntryPoint = "SuspendThread", SetLastError = true)]
        public static extern uint SuspendThread(uint hThread);

        [DllImport("coredll.dll", EntryPoint = "ResumeThread", SetLastError = true)]
		public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("coredll.dll", EntryPoint = "ResumeThread", SetLastError = true)]
        public static extern uint ResumeThread(uint hThread);

        [DllImport("coredll.dll", EntryPoint = "CeGetThreadPriority", SetLastError = true)]
		public static extern int CeGetThreadPriority(IntPtr hThread);

		[DllImport("coredll.dll", EntryPoint="CeSetThreadPriority", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CeSetThreadPriority(IntPtr hThread, int nPriority);

        [DllImport("coredll.dll", EntryPoint = "CeSetThreadPriority", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CeSetThreadPriority(uint hThread, int nPriority);

        [DllImport("coredll.dll", EntryPoint = "CeSetThreadQuantum", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CeSetThreadQuantum(IntPtr hThread, int dwTime);

        [DllImport("coredll.dll", EntryPoint = "CeSetThreadQuantum", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CeSetThreadQuantum(uint hThread, int dwTime);

        [DllImport("coredll.dll", EntryPoint = "CeGetThreadQuantum", SetLastError = true)]
        public static extern int CeGetThreadQuantum(IntPtr hThread);

        [DllImport("coredll.dll", EntryPoint = "InitializeCriticalSection", SetLastError = true)]
        public static extern void InitializeCriticalSection(IntPtr lpCriticalSection);

        [DllImport("coredll.dll", EntryPoint = "EnterCriticalSection", SetLastError = true)]
        public static extern void EnterCriticalSection(IntPtr lpCriticalSection);

        [DllImport("coredll.dll", EntryPoint = "LeaveCriticalSection", SetLastError = true)]
        public static extern void LeaveCriticalSection(IntPtr lpCriticalSection);

        [DllImport("coredll.dll", EntryPoint = "DeleteCriticalSection", SetLastError = true)]
        public static extern void DeleteCriticalSection(IntPtr lpCriticalSection);
        
        //Mutex
		[DllImport("coredll.dll", SetLastError=true)]		
		public static extern IntPtr CreateMutex(
			IntPtr lpMutexAttributes, 
			bool InitialOwner, 
			string MutexName);		
		
		[DllImport("coredll.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseMutex(IntPtr hMutex);
    }
}
