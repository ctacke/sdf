using System;
using System.Collections;

namespace OpenNETCF.ToolHelp
{
	/// <summary>
	/// Wrapper around the ToolHelp ProcessEntry information
	/// </summary>
	/// <remarks>
	/// This class requires the toolhelp32.dll
	/// </remarks>
	public partial class ThreadEntry
	{
		private THREADENTRY32 m_te;

		private ThreadEntry(THREADENTRY32 te)
		{
			m_te = te;
		}

		/// <summary>
		/// Get the thread ID
		/// </summary>
		/// <returns>The current Thread ID</returns>
		public override string ToString()
		{
			return m_te.th32ThreadID.ToString();
		}

		/// <summary>
		/// Terminates the currently referenced thread (based on its Thread ID)
		/// </summary>
		/// <returns></returns>
		public bool Terminate()
		{
            return ProcessAPI.TerminateThread(m_te.th32ThreadID, 0);
		}

		/// <summary>
		/// Number of references to the thread. A thread exists as long as its usage count is nonzero. As soon as its usage count becomes zero, a thread terminates.
		/// </summary>
		public int Usage
		{
			get { return (int)m_te.cntUsage; }
		}

		/// <summary>
		/// Identifier of the thread. This identifier is compatible with the thread identifier returned by the CreateProcess function.
		/// </summary>
		[CLSCompliant(false)]
		public uint ThreadID
		{
			get { return m_te.th32ThreadID; }
		}

		/// <summary>
		/// Identifier of the process that created the thread. The contents of this member can be used by Win32 API elements. 
		/// </summary>
		[CLSCompliant(false)]
		public uint OwnerProcessID
		{
			get { return m_te.th32OwnerProcessID; }
		}

		/// <summary>
		/// Initial priority level assigned to a thread, which can be a value from 0 to 255.
		/// </summary>
		public int BasePriority
		{
			get { return (int)m_te.tpBasePri; }
		}

		/// <summary>
		/// Change in the priority level of a thread. This value is a signed delta from the base priority level assigned to the thread.
		/// </summary>
		public int DeltaPriority
		{
			get { return (int)m_te.tpDeltaPri; }
		}

		/// <summary>
		/// Process identifier where the thread is executing.
		/// </summary>
		[CLSCompliant(false)]
		public uint CurrentProcessID
		{
			get { return m_te.th32CurrentProcessID; }
		}
		
		/// <summary>
		/// Retrieves an array of all running threads
		/// </summary>
		/// <returns></returns>
		public static ThreadEntry[] GetThreads()
		{
			return GetThreads(0);
		}

		/// <summary>
		/// Retrieves an array of all running threads for a specified process
		/// </summary>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static ThreadEntry[] GetThreads(uint processID)
		{
			ArrayList threadList = new ArrayList();

            IntPtr handle = ToolhelpAPI.CreateToolhelp32Snapshot(ToolhelpAPI.TH32CS_SNAPTHREAD, processID);

			if ((int)handle > 0)
			{
				try
				{
					THREADENTRY32 teCurrent;
					THREADENTRY32 te32 = new THREADENTRY32();

					//Get byte array to pass to the API calls
					byte[] teBytes = te32.ToByteArray();

					//Get the first process
                    int retval = ToolhelpAPI.Thread32First(handle, teBytes);

					while(retval == 1)
					{
						//Convert bytes to the class
						teCurrent = new THREADENTRY32(teBytes);

						if((processID == 0) || (teCurrent.th32OwnerProcessID == processID))
						{
							//New instance
							ThreadEntry tentry = new ThreadEntry(teCurrent);
			
							threadList.Add(tentry);
						}

                        retval = ToolhelpAPI.Thread32Next(handle, teBytes);
					}
				}
				catch(Exception ex)
				{
					throw new Exception("Exception: " + ex.Message);
				}
				
				//Close handle
                ToolhelpAPI.CloseToolhelp32Snapshot(handle); 
				
				return (ThreadEntry[])threadList.ToArray(typeof(ThreadEntry));

			}
			else
			{
				throw new Exception("Unable to create snapshot");
			}
		}
	}
}
