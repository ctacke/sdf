using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Represents a thread synchronization event.
	/// </summary>
	public partial class EventWaitHandle : WaitHandle
    {
        public const int WaitTimeout = 0x102;

		/// <summary>
		/// Opens an existing named synchronization event.
		/// </summary>
		/// <param name="name">The name of a system event.</param>
		/// <returns>A <see cref="EventWaitHandle"/> object that represents the named system event.</returns>
        /// <exception cref="ArgumentException">name is a zero-length string.
        /// -or-
        /// name is longer than 260 characters.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="WaitHandleCannotBeOpenedException">The named system event does not exist.</exception>
		public static EventWaitHandle OpenExisting(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length < 1)
            {
                throw new ArgumentException("name is a zero-length string.");
            }
            if (name.Length > 260)
            {
                throw new ArgumentException("name is longer than 260 characters.");
            }

            IntPtr h = OpenEvent(EVENT_ALL_ACCESS, false, name);
            if (h == IntPtr.Zero)
            {
                throw new WaitHandleCannotBeOpenedException();
            }
			return new EventWaitHandle(h);
		}

        /// <summary>
		/// Sets the state of the event to nonsignaled, causing threads to block.
		/// </summary>
		/// <returns>true if the function succeeds; otherwise, false.</returns>
		public bool Reset()
        {
			return EventModify(this.Handle, EVENT.RESET);
		}

		/// <summary>
		/// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
		/// </summary>
		/// <returns>true if the function succeeds; otherwise, false.</returns>
		public bool Set()
        {
			return EventModify(this.Handle, EVENT.SET);
		}
        
        /// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait 
		/// handle is initially signaled, and whether it resets automatically or manually.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
		/// <param name="mode">An EventResetMode value that determines whether the event resets automatically or manually.</param>
		public EventWaitHandle(bool initialState, EventResetMode mode):this(initialState, mode, null){}

		/// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait handle is initially signaled, whether it resets automatically or manually, and the name of a system synchronization event.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
		/// <param name="mode">An Threading.EventResetMode value that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
		public EventWaitHandle(bool initialState, EventResetMode mode, string name):this(CreateEvent(IntPtr.Zero, mode == EventResetMode.ManualReset, initialState, name)){}

		/// <summary>
		/// Initializes a newly created <see cref="EventWaitHandle"/> object, specifying whether the wait handle is initially signaled, whether it resets automatically or manually, the name of a system synchronization event, and a bool variable whose value after the call indicates whether the named system event was created.
		/// </summary>
		/// <param name="initialState">true to set the initial state to signaled, false to set it to nonsignaled.</param>
		/// <param name="mode">An Threading.EventResetMode value that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
		/// <param name="createdNew">When this method returns, contains true if the calling thread was granted initial ownership of the named system event; otherwise, false. This parameter is passed uninitialized.</param>
		public EventWaitHandle(bool initialState, EventResetMode mode, string name, out bool createdNew)
        {
			IntPtr h = CreateEvent(IntPtr.Zero, mode == EventResetMode.ManualReset, initialState, name);
			if (h.Equals(IntPtr.Zero)){
				throw new ApplicationException("Cannot create " + name);
			}
			createdNew = (Marshal.GetLastWin32Error() != ERROR_ALREADY_EXISTS);
			this.Handle = h;
		}

        ~EventWaitHandle()
        {
            Dispose(false);
        }

        /// <summary>
		/// When overridden in a derived class, blocks the current thread until the current <see cref="WaitHandle"/> receives a signal.
		/// </summary>
		/// <returns>true if the current instance receives a signal. if the current instance is never signaled, <see cref="WaitOne(Int32,bool)"/> never returns.</returns>
		public override bool WaitOne()
        {
			return WaitOne(-1, false);
		}

		/// <summary>
        /// When overridden in a derived class, blocks the current thread until the current <see cref="WaitHandle"/> receives a signal, using 32-bit signed integer to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
		/// <param name="exitContext">Not Supported - Just pass false.</param>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		public override bool WaitOne(Int32 millisecondsTimeout, bool exitContext)
        {
			return (WaitForSingleObject(this.Handle, millisecondsTimeout) != WAIT_TIMEOUT);
		}

        private static int WaitMultiple(IntPtr[] handles, int millisecondsTimeout, bool waitAll)
        {
            return WaitForMultipleObjects(handles.Length, handles, waitAll, millisecondsTimeout);
        }

        private static int WaitMultiple(WaitHandle[] waitHandles, int millisecondsTimeout, bool waitAll)
        {
            if (waitHandles == null)
            {
                throw new ArgumentNullException("waitHandles cannot be a null array");
            }

            for (int i = 0; i < waitHandles.Length; i++)
            {
                if (waitHandles[i] == null)
                {
                    throw new ArgumentNullException("waitHandle " + i.ToString() + " cannot be null");
                }
            }

            if (millisecondsTimeout < -1)
            {
                throw new ArgumentOutOfRangeException("milliseconds must be non-negative");
            }

            if (waitHandles.Length == 0)
            {
                throw new ApplicationException("waitHandles cannot be empty");
            }

            IntPtr[] handles = new IntPtr[waitHandles.Length];

            for(int i = 0 ; i < handles.Length ; i++)
            {
                handles[i] = waitHandles[i].Handle;
            }

            return WaitMultiple(handles, millisecondsTimeout, false);
        }

        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to measure the time interval.
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        /// <returns>The array index of the object that satisfied the wait, or WaitTimeout if no object satisfied the wait and a time interval equivalent to timeout has passed.</returns>
        public static int WaitAny(WaitHandle[] waitHandles)
        {
            return WaitAny(waitHandles, Timeout.Infinite, false);
        }

        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to measure the time interval.
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or Timeout.Infinite (-1) to wait indefinitely</param>
        /// <param name="exitContext">Unsupported in the Compact Framework.  This parameter is for compatibility and is ignored.</param>
        /// <returns>The array index of the object that satisfied the wait, or WaitTimeout if no object satisfied the wait and a time interval equivalent to timeout has passed.</returns>
        public static int WaitAny (WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitMultiple(waitHandles, millisecondsTimeout, false);
        }

        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to measure the time interval.
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        /// <returns>The array index of the object that satisfied the wait, or WaitTimeout if no object satisfied the wait and a time interval equivalent to timeout has passed.</returns>
        public static int WaitAny(IntPtr[] waitHandles)
        {
            return WaitAny(waitHandles, System.Threading.Timeout.Infinite, false);
        }

        /// <summary>
        /// Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to measure the time interval.
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or Timeout.Infinite (-1) to wait indefinitely</param>
        /// <param name="exitContext">Unsupported in the Compact Framework.  This parameter is for compatibility and is ignored.</param>
        /// <returns>The array index of the object that satisfied the wait, or WaitTimeout if no object satisfied the wait and a time interval equivalent to timeout has passed.</returns>
        public static int WaitAny(IntPtr[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitMultiple(waitHandles, millisecondsTimeout, false);
        }

        // I've commented these out for now because the CE docs indicate WaitForMultipleObjects must only take FALSE for bWaitAll 
        /*
        /// <summary>
        /// Waits for all the elements in the specified array to receive a signal. 
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        public static bool WaitAll(WaitHandle[] waitHandles)
        {
            return (WaitAll(waitHandles, System.Threading.Timeout.Infinite, true) >= 0);
        }

        /// <summary>
        /// Waits for all the elements in the specified array to receive a signal. 
        /// </summary>
        /// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or Timeout.Infinite (-1) to wait indefinitely</param>
        /// <param name="exitContext">Unsupported in the Compact Framework.  This parameter is for compatibility and is ignored.</param>
        /// <returns></returns>
        public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return (WaitMultiple(waitHandles, millisecondsTimeout, true) >= 0);
        }
        */

		/// <summary>
		/// When overridden in a derived class, blocks the current thread until the current instance receives a signal, using a <see cref="TimeSpan"/> to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="timeout">A TimeSpan that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <param name="exitContext">Not Supported - Just pass false.</param>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		public bool WaitOne(TimeSpan timeout, bool exitContext)
        {
			return (WaitForSingleObject(this.Handle, (int)timeout.TotalMilliseconds) != WAIT_TIMEOUT);
		}

        /// <summary>
        /// When overridden in a derived class, releases all resources held by the current <see cref="WaitHandle"/>.
        /// </summary>
		public override void Close()
        {
            Dispose(true);
			GC.SuppressFinalize(this);
		}

		private EventWaitHandle(IntPtr aHandle):base()
        {
			if (aHandle.Equals(IntPtr.Zero)){
				throw new WaitHandleCannotBeOpenedException();
			}
			this.Handle = aHandle;
		}

        #region IDisposable Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="explicitDisposing"></param>
        protected override void Dispose(bool explicitDisposing)
        {
            if (this.Handle != InvalidHandle)
            {
                CloseHandle(this.Handle);
                this.Handle = InvalidHandle;
            }
            base.Dispose(explicitDisposing);
        }

        #endregion

        private const Int32 WAIT_FAILED = -1;
        private const Int32 WAIT_TIMEOUT = 0x102;
        private const Int32 EVENT_ALL_ACCESS = 0x3;
        private const Int32 ERROR_ALREADY_EXISTS = 183;

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr OpenEvent(Int32 dwDesiredAccess, bool bInheritHandle, string lpName);
    
        //Events
        private enum EVENT
        {
            PULSE = 1,
            RESET = 2,
            SET = 3,
        }

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EventModify(IntPtr hEvent, EVENT ef);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern Int32 WaitForSingleObject(IntPtr hHandle, Int32 dwMilliseconds);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int WaitForMultipleObjects(int nCount, IntPtr[] lpHandles, bool fWaitAll, int dwMilliseconds);

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
