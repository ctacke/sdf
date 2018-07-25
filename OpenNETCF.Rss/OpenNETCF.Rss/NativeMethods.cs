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
using System.Runtime.InteropServices;

//Peter Foot

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Contains native API calls for Threading related functionality.
	/// </summary>
	internal static class NativeMethods
	{
		public const Int32 WAIT_FAILED = -1;
		public const Int32 WAIT_TIMEOUT = 0x102;
		public const Int32 EVENT_ALL_ACCESS = 0x3;
		public const Int32 ERROR_ALREADY_EXISTS = 183;

		//Events
		public enum EVENT
		{
			PULSE = 1,
			RESET = 2,
			SET = 3,
		}
		
		[DllImport("coredll.dll", SetLastError=true)] 
		public static extern bool EventModify(IntPtr hEvent, EVENT ef);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern Int32 WaitForSingleObject(IntPtr hHandle, Int32 dwMilliseconds);
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int WaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, bool fWaitAll, uint dwMilliseconds); 
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int MsgWaitForMultipleObjectsEx(uint nCount, IntPtr[] lpHandles, uint dwMilliseconds, uint dwWakeMask, uint dwFlags); 
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr OpenEvent(Int32 dwDesiredAccess, bool bInheritHandle, string lpName);
		
		//Handle
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern bool CloseHandle(IntPtr hObject);

		//Semaphore
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateSemaphore(IntPtr lpSemaphoreAttributes, Int32 lInitialCount, Int32 lMaximumCount, string lpName);
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern bool ReleaseSemaphore(IntPtr handle, Int32 lReleaseCount, out Int32 previousCount);
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr OpenSemaphore(int desiredAccess, bool inheritHandle, string name);
		

		//Thread
		[DllImport("coredll.dll", EntryPoint="TerminateThread", SetLastError = true)]
		public static extern bool TerminateThread(IntPtr hThread, int dwExitCode);

		[DllImport("coredll.dll", EntryPoint="TerminateThread", SetLastError = true)]
		public static extern bool TerminateThread(uint hThread, int dwExitCode);

		[DllImport("coredll.dll", EntryPoint="SuspendThread", SetLastError = true)]
		public static extern uint SuspendThread(IntPtr hThread);

		[DllImport("coredll.dll", EntryPoint="ResumeThread", SetLastError = true)]
		public static extern uint ResumeThread(IntPtr hThread);

		//Mutex
		[DllImport("coredll.dll", EntryPoint="CreateMutex", SetLastError=true)]		
		public static extern IntPtr CreateMutex(
			IntPtr lpMutexAttributes, 
			bool InitialOwner, 
			string MutexName);		
		
		[DllImport("coredll.dll",EntryPoint="ReleaseMutex", SetLastError=true)]
		public static extern bool ReleaseMutex(IntPtr hMutex);
	}
}
