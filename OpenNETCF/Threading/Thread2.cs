using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Creates and controls a thread, sets its priority, and gets its status.
	/// </summary>
	public class Thread2
	{
		Thread			    m_thread;
		ThreadStart		    m_start;
        SafeThreadHandle    m_handle;
		IntPtr			    m_terminationEvent;
        int                 m_startPriority = (int)ThreadPriority.Normal;
        int                 m_startQuantum = -1;
        ThreadState m_state;

		#region static methods
		// these method maintain the Microsoft Thread interface so users don't have to use both Thread classes

		/// <summary>
		/// Blocks the current thread for the specified number of milliseconds.
		/// </summary>
		/// <param name="millisecondsTimeout">Amount of time to block</param>
		public static void Sleep(int millisecondsTimeout)
		{
			Thread.Sleep(millisecondsTimeout);
		}

		/// <summary>
		/// Blocks the current thread for the specified span of time.
		/// </summary>
		/// <param name="timeout">Amount of time to block</param>
		public static void Sleep(TimeSpan timeout)
		{
			Sleep(Convert.ToInt32(timeout.TotalMilliseconds));
		}
		
		/// <summary>
		/// Allocates an unnamed data slot on all the threads.
		/// </summary>
		/// <returns>A <see cref="LocalDataStoreSlot"/>.</returns>
		public static System.LocalDataStoreSlot AllocateDataSlot()
		{
			return Thread.AllocateDataSlot();
		}

		/// <summary>
		/// Allocates a named data slot on all threads.
		/// </summary>
		/// <param name="name">The name of the data slot to be allocated.</param>
		/// <returns>A <see cref="LocalDataStoreSlot"/>.</returns>
		public static System.LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Thread.AllocateNamedDataSlot(name);
		}

		/// <summary>
		/// Eliminates the association between a name and a slot, for all threads in the process.
		/// </summary>
		/// <param name="name">The name of the data slot to be freed.</param>
		public static void FreeNamedDataSlot(string name)
		{
			Thread.FreeNamedDataSlot(name);
		}

		/// <summary>
		/// Retrieves the value from the specified slot on the current thread.
		/// </summary>
		/// <param name="slot">The <see cref="LocalDataStoreSlot"/> from which to get the value.</param>
		/// <returns>The value retrieved</returns>
		public static object GetData(LocalDataStoreSlot slot)
		{
			return Thread.GetData(slot);
		}

		/// <summary>
		/// Looks up a named data slot.
		/// </summary>
		/// <param name="name">The name of the local data slot.</param>
		/// <returns>A <see cref="LocalDataStoreSlot"/> allocated for this thread.</returns>
		public static System.LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Thread.GetNamedDataSlot(name);
		}

		/// <summary>
		/// Sets the data in the specified slot on the currently running thread, for that thread's current domain.
		/// </summary>
		/// <param name="slot">The <see cref="LocalDataStoreSlot"/> in which to set the value.</param>
		/// <param name="data">The value to be set.</param>
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			Thread.SetData(slot, data);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the thread.
		/// </summary>
		/// <value>A string containing the name of the thread, or a null reference (Nothing in Visual Basic) if no name was set.</value>
		public string Name
		{
			get { return m_thread.Name; }
            set { m_thread.Name = value; }
		}

        /// <summary>
        /// Gets a unique identifier for the current managed thread
        /// </summary>
        public int ManagedThreadId
        {
            get { return m_thread.ManagedThreadId; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not a thread is a background thread
        /// </summary>
        public bool IsBackground
        {
            get { return m_thread.IsBackground; }
            set { m_thread.IsBackground = value; }
        }

		/// <summary>
		/// Gets a value indicating the execution status of the current thread.
		/// </summary>
		/// <value><b>true</b> if this thread has been started and has not terminated normally or aborted; otherwise, <b>false</b>.</value>
		public bool IsAlive
		{
			get
			{ 
				if((m_state == ThreadState.Stopped)	| (m_state == ThreadState.Unstarted))
					return false;

				return true;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread.
		/// </summary>
		/// <value>One of the <see cref="System.Threading.ThreadPriority"/> values. The default value is Normal.</value>
		public System.Threading.ThreadPriority Priority
		{
			get{ return m_thread.Priority; }
			set
			{ 
				if(m_state == ThreadState.Stopped)
					throw new ThreadStateException();

				m_thread.Priority = value; 
			}
		}

        /// <summary>
        /// Gets or sets a Thread2's quantum in milliseconds.  Use zero for "run to completion". Unless modified by the OEM, the system default is 100ms
        /// </summary>
        /// <remarks>
        /// <b>WARNING:</b> Adjusting a thread quantum with this property can lead to application and even device deadlock or unpredictability.  Use only with caution and strong knowledge of the target system.
        /// </remarks>
        public int RealTimeQuantum
        {
            get { return NativeMethods.CeGetThreadQuantum(m_handle.DangerousGetHandle()); }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Quantum must be greater than zero");
                }

                if (State == ThreadState.Unstarted)
                {
                    m_startQuantum = value;
                }
                else
                {
                    if (!NativeMethods.CeSetThreadQuantum(m_handle.DangerousGetHandle(), value))
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to set thread quantum");
                    }
                }
            }
        }
        
        /// <summary>
		/// Gets or sets a priority value outside of application priority space
		/// </summary>
		/// <remarks>
		/// <b>WARNING:</b> Adjusting a thread priority with this property can lead to application and even device deadlock or unpredictability.  Use only with caution and strong knowledge of the target system.  Do <u>not</u> use this Property for normal Priority settings.
		/// </remarks>
		public int RealTimePriority
		{
			get { return NativeMethods.CeGetThreadPriority(m_handle.DangerousGetHandle()); }
			set
			{
				if((value < 0) || (value > 255))
				{
					throw new ArgumentOutOfRangeException("Priority must be between 0 and 255");
				}

                if (State == ThreadState.Unstarted)
                {
                    m_startPriority = value;
                }
                else
                {
                    if (!NativeMethods.CeSetThreadPriority(m_handle.DangerousGetHandle(), value))
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to set thread priority");
                    }
                }
			}
		}

		/// <summary>
		/// Returns the Thread2 instance's current <see cref="ThreadState"/>
		/// </summary>
		public ThreadState State
		{
			get { return m_state; }
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.Threading.Thread"/> equivalent for the Thread2 instance
		/// </summary>
		/// <param name="thread2">The Thread2 to convert</param>
		/// <returns>A <see cref="System.Threading.Thread"/></returns>
		public static implicit operator Thread (Thread2 thread2)
		{
			return thread2.m_thread;
		}

		/// <summary>
		/// Initializes a new instance of the Thread2 class.
		/// </summary>
		/// <param name="start">A <see cref="System.Threading.ThreadStart"/> delegate that references the methods to be invoked when this thread begins executing.</param>
		public Thread2(System.Threading.ThreadStart start)
		{
			if(start == null)
				throw new ArgumentNullException();

			m_terminationEvent = NativeMethods.CreateEvent(IntPtr.Zero, true, false, Guid.NewGuid().ToString());

			m_start = start;
			m_thread = new Thread(new ThreadStart(ShimProc));
			//m_name = null;
			m_state = ThreadState.Unstarted;
            m_handle = new SafeThreadHandle(GetThreadHandle);
		}

		/// <summary>
		/// Causes the operating system to change the state of the current instance to <see cref="ThreadState.Running"/>.
		/// </summary>
		public void Start()
		{
			lock(this)
			{
				m_thread.Start();
                m_handle.Open();
                m_state = ThreadState.Running;

                if (m_startPriority != (int)ThreadPriority.Normal)
                {
                    RealTimePriority = m_startPriority;
                }

                if (m_startQuantum >= 0)
                {
                    RealTimeQuantum = m_startQuantum;
                }
			}
		}

		/// <summary>
		/// Either suspends the thread, or if the thread is already suspended, has no effect.
		/// </summary>
		public void Suspend()
		{
			if((m_state == ThreadState.Unstarted) || (m_state == ThreadState.Stopped))
				throw new ThreadStateException();

			m_state = ThreadState.SuspendRequested;

			lock(this)
			{
                NativeMethods.SuspendThread(m_handle.DangerousGetHandle());
			}

			m_state = ThreadState.Suspended;
		}

		/// <summary>
		/// Resumes a thread that has been suspended.
		/// </summary>
		public void Resume()
		{
			if(m_state != ThreadState.Suspended)
				throw new ThreadStateException();

			lock(this)
			{
                NativeMethods.ResumeThread(m_handle.DangerousGetHandle());
			}
			m_state = ThreadState.Running;
		}

		private IntPtr GetThreadHandle()
		{
			return ((IntPtr)typeof(Thread).GetField("m_ThreadId", BindingFlags.NonPublic|BindingFlags.Instance).GetValue(m_thread));
		}

        /// <summary>
        /// Raises a <see cref="ThreadAbortException"/> in the thread on which it is invoked, to begin the process of terminating the thread while also providing exception information about the thread termination. Calling this method usually terminates the thread.
        /// </summary>
        /// <param name="stateInfo"></param>
        public void Abort(object stateInfo)
        {
            lock (this)
            {
                m_thread.Abort(stateInfo);
            }
        }

		/// <summary>
        /// Raises a <see cref="ThreadAbortException"/> in the thread on which it is invoked, to begin the process of terminating the thread while also providing exception information about the thread termination. Calling this method usually terminates the thread.
        /// </summary>
		public void Abort()
		{	
			lock(this)
			{
                m_thread.Abort();
			}
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <returns><b>true</b> if the thread has terminated;</returns>
		public bool Join()
		{
			return Join(NativeMethods.INFINITE);
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns><b>true</b> if the thread has terminated; <b>false</b> if the thread has not terminated after the amount of time specified by the <i>timeout</i> parameter has elapsed.</returns>
		public bool Join(TimeSpan timeout)
		{
			return Join(Convert.ToInt32(timeout.TotalMilliseconds));
		}

		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait for the thread to terminate.</param>
		/// <returns><b>true</b> if the thread has terminated; <b>false</b> if the thread has not terminated after the amount of time specified by the <i>millisecondsTimeout</i> parameter has elapsed.</returns>
		public bool Join(int millisecondsTimeout)
		{
			// mimic full framework functionality
			if((m_state == ThreadState.Unstarted) || (m_state == ThreadState.Stopped))
				throw new ThreadStateException();

			lock(this)
			{
				m_state = ThreadState.WaitSleepJoin;

				if(NativeMethods.WaitForSingleObject(m_terminationEvent, millisecondsTimeout) == 0)
				{
					m_state = ThreadState.Stopped;
					return true;
				}

				return false;
			}
		}

		private void ShimProc()
		{
			// call into actual thread proc
			m_start();

			// set the termination event to notify Joins
			NativeMethods.EventModify(m_terminationEvent, NativeMethods.EVENT.SET);
			
			// set the thread to stopped
			m_state = ThreadState.Stopped;

			// yield
			Thread.Sleep(0);
		}
	}

	/// <summary>
	/// Specifies the execution states of a <see cref="Thread2"/>.
	/// </summary>
	public enum ThreadState : int
	{
		/// <summary>
		/// Thread is unstarted.
		/// </summary>
		Unstarted,
		/// <summary>
		/// Thread is running.
		/// </summary>
		Running,
		/// <summary>
		/// Thread is waiting in a Join.
		/// </summary>
		WaitSleepJoin,
		/// <summary>
		/// Suspend has been called but not acted upon.
		/// </summary>
		SuspendRequested,
		/// <summary>
		/// Thread is suspended.
		/// </summary>
		Suspended,
		/// <summary>
		/// Thread has either terminated or been Aborted.
		/// </summary>
		Stopped,
	}
}
