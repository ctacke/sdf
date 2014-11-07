using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;

namespace OpenNETCF.ToolHelp
{
	/// <summary>
	/// Wrapper around the ToolHelp ProcessEntry information
	/// </summary>
	/// <remarks>
	/// This class requires the toolhelp32.dll
	/// </remarks>
	public partial class ProcessEntry
	{
		private PROCESSENTRY32 m_pe;

		private ProcessEntry(PROCESSENTRY32 pe)
		{
			m_pe = pe;
		}

		/// <summary>
		/// Get the short name of the current process
		/// </summary>
		/// <returns>The current process name</returns>
		public override string ToString()
		{
			return System.IO.Path.GetFileName(m_pe.ExeFile);
		}

		/// <summary>
		/// Base address for the process
		/// </summary>
		[CLSCompliant(false)]
		public uint BaseAddress
		{
			get { return m_pe.BaseAddress; }
		}

		/// <summary>
		/// Number of execution threads started by the process.
		/// </summary>
		public int ThreadCount
		{
			get { return (int)m_pe.cntThreads; }
		}

		/// <summary>
		/// Identifier of the process. The contents of this member can be used by Win32 API elements. 
		/// </summary>
		[CLSCompliant(false)]
		public uint ProcessID
		{
			get	{ return (uint)m_pe.ProcessID; }
		}

		/// <summary>
		/// Null-terminated string that contains the path and file name of the executable file for the process. 
		/// </summary>
		public string ExeFile
		{
			get { return m_pe.ExeFile; }
		}

		/// <summary>
		/// Kill the Process
		/// </summary>
		public void Kill()
		{
			IntPtr hProcess;

            hProcess = ProcessAPI.OpenProcess(ProcessAPI.PROCESS_TERMINATE, false, this.ProcessID);

            if (hProcess.ToInt32() != ProcessAPI.INVALID_HANDLE_VALUE) 
			{
				bool bRet;
                bRet = ProcessAPI.TerminateProcess(hProcess, 0);
                ProcessAPI.CloseHandle(hProcess);
			}
		}

		/// <summary>
		/// Rerieves an array of all running processes
		/// </summary>
		/// <returns></returns>
		public static ProcessEntry[] GetProcesses()
		{
			ArrayList procList = new ArrayList();

            IntPtr handle = ToolhelpAPI.CreateToolhelp32Snapshot(ToolhelpAPI.TH32CS_SNAPPROCESS | ToolhelpAPI.TH32CS_SNAPNOHEAPS, 0);

			if ((int)handle > 0)
			{
                try
                {
                    PROCESSENTRY32 peCurrent;
                    PROCESSENTRY32 pe32 = new PROCESSENTRY32();

                    //Get byte array to pass to the API calls
                    byte[] peBytes = pe32.ToByteArray();
                    byte[] empty = new byte[peBytes.Length];
                    peBytes.CopyTo(empty, 0);

                    //Get the first process
                    int retval = ToolhelpAPI.Process32First(handle, peBytes);

                    while (retval == 1)
                    {
                        //Convert bytes to the class
                        peCurrent = new PROCESSENTRY32(peBytes);
                        //New instance
                        ProcessEntry proc = new ProcessEntry(peCurrent);

                        procList.Add(proc);

                        retval = ToolhelpAPI.Process32Next(handle, peBytes);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception: " + ex.Message);
                }
                finally
                {
                    //Close handle
                    ToolhelpAPI.CloseToolhelp32Snapshot(handle);
                }
				
				return (ProcessEntry[])procList.ToArray(typeof(ProcessEntry));
			}
			else
			{
				throw new Exception("Unable to create snapshot");
			}
		}
    }
}
