using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenNETCF.Threading{

	/// <summary>
	/// Limits the number of threads that can access a resource, or a particular type of resource, concurrently.
	/// </summary>
	public class Semaphore : WaitHandle 
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="Semaphore"/> class, specifying the maximum number of concurrent entries, and optionally reserving some entries for the calling thread.
		/// </summary>
		/// <param name="initialCount"> The initial number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
		public Semaphore(int initialCount, int maximumCount):this(initialCount, maximumCount, null){}


		/// <summary>
		/// Initializes a new instance of the <see cref="Semaphore"/> class, specifying the maximum number of concurrent entries, optionally reserving some entries for the calling thread, and optionally specifying the name of a system semaphore object.
		/// </summary>
		/// <param name="initialCount"> The initial number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="name">The name of a system-wide named semaphore object.</param>
		public Semaphore(int initialCount, int maximumCount, string name)
        {
			this.ValidateArgs(initialCount,maximumCount);

			IntPtr hwnd = NativeMethods.CreateSemaphore(IntPtr.Zero, initialCount, maximumCount, name);
			if (hwnd.ToInt32() == 0) {
				throw new ApplicationException("could not create semaphore " + System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString() + " " + name);
			}

			this.Handle = hwnd;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Semaphore"/> class, specifying the maximum number of concurrent entries, optionally reserving some entries for the calling thread, optionally specifying the name of a system semaphore object, and specifying an out parameter that indicates whether a new system object was created.
		/// </summary>
		/// <param name="initialCount"> The initial number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="name">The name of a system-wide named semaphore object.</param>
		/// <param name="createdNew"> When this method returns, contains true if a new system object was created; otherwise false. This parameter is passed uninitialized.</param>
		public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew)
        {
			this.ValidateArgs(initialCount,maximumCount);

			IntPtr hwnd = NativeMethods.CreateSemaphore(IntPtr.Zero, initialCount, maximumCount, name);
			Int32 er = 0;
			if (hwnd.ToInt32() == 0) {
				er = Marshal.GetLastWin32Error();
				throw new ApplicationException("could not create semaphore " + er.ToString() + " " + name);
			}
			if (er == NativeMethods.ERROR_ALREADY_EXISTS) {
				createdNew = false;
			}else{
				createdNew = true;
			}

			this.Handle = hwnd;
		}

        #region OpenExisting
        /// <summary>
        /// Opens an existing named semaphore.
        /// </summary>
        /// <param name="name">The name of a system semaphore.</param>
        /// <returns>A <see cref="Semaphore"/> object that represents a named system semaphore.</returns>
        /// <remarks>The OpenExisting method attempts to open only existing named semaphores.
        /// If a system semaphore with the specified name does not exist, this method throws an exception instead of creating the system semaphore.
        /// It is possible to create multiple <see cref="Semaphore"/> objects that represent the same named system semaphore.
        /// Two calls to this method with the same value for name do not necessarily return the same <see cref="Semaphore"/> object, even though the objects returned both represent the same named system semaphore.</remarks>
        /// <exception cref="ArgumentException">name is a zero-length string.
        /// -or-
        /// name is longer than 260 characters.</exception>
        /// <exception cref="ArgumentNullException">name is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="WaitHandleCannotBeOpenedException">The named semaphore does not exist.</exception>
        public static Semaphore OpenExisting(string name)
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

            bool created;
            Semaphore s = new Semaphore(0,0, name, out created);
            if (created)
            {
                //semaphore doesnt already exist
                s.Dispose(true);
                throw new WaitHandleCannotBeOpenedException();
            }
            return s;
        }
        #endregion

		/// <summary>
		/// Exits the semaphore, returning the previous count.
		/// </summary>
		/// <returns>An integer value representing the count on the semaphore before the Overload:Semaphore.Release method was called.</returns>
		public int Release()
        {
			return this.Release(1);
		}


		/// <summary>
		/// Exits the semaphore, returning the previous count.
		/// </summary>
		/// <param name="releaseCount">releaseCount: The number of times to exit the semaphore.</param>
		/// <returns>An integer value representing the count on the semaphore before the Overload:Semaphore.Release method was called.</returns>
        /// <exception cref="ArgumentOutOfRangeException">releaseCount is less than 1.</exception>
		public int Release(int releaseCount)
        {
			if (releaseCount < 1)
            {
				throw new ArgumentOutOfRangeException("releaseCount must be greater than 1, not " + releaseCount.ToString());
			}
			int ret;
			if (NativeMethods.ReleaseSemaphore(this.Handle, releaseCount, out ret) == true)
            {
				return ret;
			}

            throw new SemaphoreFullException("The semaphore count is already at the maximum value.");
		}

		/// <summary>
		/// When overridden in a derived class, blocks the current thread until the current Threading.WaitHandle receives a signal.
		/// </summary>
		/// <returns>true if the current instance receives a signal. if the current instance is never signaled, WaitHandle.WaitOne() never returns.</returns>
		public override bool WaitOne()
        {
			return WaitOne(-1, false);
		}

		/// <summary>
		/// When overridden in a derived class, blocks the current thread until the current Threading.WaitHandle receives a signal, using 32-bit signed integer to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
		/// <param name="notApplicableOnCE">Just pass false</param>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		public override bool WaitOne(Int32 millisecondsTimeout, bool notApplicableOnCE)
        {
			return (NativeMethods.WaitForSingleObject(this.Handle, millisecondsTimeout) != NativeMethods.WAIT_TIMEOUT);
		}


		
		/// <summary>
		/// When overridden in a derived class, blocks the current thread until the current instance receives a signal, using a TimeSpan to measure the time interval and specifying whether to exit the synchronization domain before the wait.
		/// </summary>
		/// <param name="aTs">A TimeSpan that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <param name="notApplicableOnCE">Just pass false</param>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		public bool WaitOne(TimeSpan aTs, bool notApplicableOnCE)
        {
			return (NativeMethods.WaitForSingleObject(this.Handle, (int)aTs.TotalMilliseconds) != NativeMethods.WAIT_TIMEOUT);
		}

		
		/// <summary>
		/// Releases all resources held by the current <see cref="WaitHandle"/>
		/// </summary>
		public override void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explicitDisposing"></param>
        protected override void Dispose(bool explicitDisposing)
        {
            if (this.Handle != InvalidHandle)
            {
                NativeMethods.CloseHandle(this.Handle);
                this.Handle = InvalidHandle;
            }
            base.Dispose(explicitDisposing);
        }

		//called from ctors to reduce duplication
		private void ValidateArgs(int initialCount, int maximumCount)
        {
			if ((initialCount > maximumCount) || (maximumCount < 1) || (initialCount < 0)) {
				throw new ArgumentException("RTFM please on what arguments are valid");
			}
		}
	}
}
