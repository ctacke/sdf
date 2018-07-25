using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Provides helper methods for processes.
	/// </summary>
	public class ProcessHelper
	{
		private const int SYS_HANDLE_BASE	 = 64;
		private const int SH_WIN32          = 0;
		private const int SH_CURTHREAD      = 1;
		private const int SH_CURPROC        = 2;
		private const uint SYSHANDLE_OFFSET = 0x004;

		private const uint PUserKDataARM = 0xFFFFC800;
		private const uint PUserKDataX86 = 0x00005800;

	

		#region Current Process
		/// <summary>
		/// This function returns the process identifier of the calling process.
		/// </summary>
		/// <returns>The return value is the process identifier of the calling process.</returns>
		public static int GetCurrentProcessID()
		{
			IntPtr pBase = new IntPtr((int) (GetPUserKData() + SYSHANDLE_OFFSET));
            return Marshal.ReadInt32(pBase, SH_CURPROC * 4);
        }

		
		/// <summary>
		/// Gets a new <see cref="System.Diagnostics.Process"/> component and associates it with the currently active process.
		/// </summary>
        /// <returns>A new <see cref="System.Diagnostics.Process"/> component associated with the process resource that is running the calling application.</returns>
		/// <remarks>Use this method to create a new Process instance and associate it with the process resource on the local computer.</remarks>
		public static IntPtr GetCurrentProcessHandle()
		{
			return new IntPtr(SH_CURPROC + SYS_HANDLE_BASE);
		}

		#endregion

        #region Current Thread
        /// <summary>
		/// This function returns the thread identifier, which is used as a handle of the calling thread. 
		/// </summary>
		/// <returns>The thread identifier of the calling thread indicates success.</returns>
		public static int GetCurrentThreadID()
		{
			IntPtr pBase = new IntPtr((int) (GetPUserKData() + SYSHANDLE_OFFSET));
			return Marshal.ReadInt32(pBase, SH_CURTHREAD * 4);
		}

		/// <summary>
		/// This function returns a pseudohandle for the current thread. 
		/// </summary>
		/// <returns>The return value is a pseudohandle for the current thread.</returns>
		/// <remarks>
		/// <p>A pseudohandle is a special constant that is interpreted as the current thread handle. </p>
		/// <p>The calling thread can use this handle to specify itself whenever a thread handle is required.This handle has the maximum possible access to the thread object.</p>
		/// <p>The function cannot be used by one thread to create a handle that can be used by other threads to refer to the first thread. The handle is always interpreted as referring to the thread that is using it.</p>
		/// <p>The pseudohandle need not be closed when it is no longer needed. Calling the CloseHandle function with this handle has no effect.</p>
		/// </remarks>
		public static IntPtr GetCurrentThreadHandle()
		{
			return new IntPtr(SH_CURTHREAD + SYS_HANDLE_BASE);
        }
        #endregion

        private static uint GetPUserKData()
        {
            uint PUserKData = 0;
            NativeMethods.SystemInfo si = new NativeMethods.SystemInfo();
            NativeMethods.GetSystemInfo(out si);

            switch (si.ProcessorArchitecture)
            {
                case NativeMethods.ProcessorArchitecture.ARM:
                    PUserKData = PUserKDataARM;
                    break;
                //the "x86" constant is for all non-arm architectures so removed exception throw
                default:
                    //case Core.ProcessorArchitecture.Intel:
                    PUserKData = PUserKDataX86;
                    break;

                //throw new NotSupportedException("Unsupported on current processor: " + EnumEx.GetName(typeof(Core.ProcessorArchitecture), si.wProcessorArchitecture));
            }

            return PUserKData;
        }
		


	}
}
