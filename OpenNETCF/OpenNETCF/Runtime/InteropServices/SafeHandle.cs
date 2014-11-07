using System;

namespace OpenNETCF.Runtime.InteropServices
{
	/// <summary>
	/// Represents a wrapper class for operating system handles.
	/// </summary>
	public abstract class SafeHandle : IDisposable
	{
		private bool m_closed;
		private bool m_ownsHandle;
		private IntPtr m_invalid;

        /// <summary>
        /// Specifies the handle to be wrapped.
        /// </summary>
        protected IntPtr handle;


		/// <summary>
		/// Initializes a new instance of the SafeHandle class with the specified invalid handle value.
		/// </summary>
		/// <param name="invalidHandleValue">The value of an invalid handle (usually 0 or -1).</param>
		/// <param name="ownsHandle">true to reliably let SafeHandle release the handle during the finalization phase; otherwise, false (not recommended).</param>
		protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle)
		{
			m_invalid = invalidHandleValue;
			handle = invalidHandleValue;
			m_closed = false;
			m_ownsHandle = ownsHandle;
			if(!m_ownsHandle)
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the handle is closed.
		/// </summary>
		/// <value>true if the handle is closed; otherwise, false.</value>
		public bool IsClosed
		{
			get
			{
				return m_closed;
			}
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the handle value is invalid.
		/// </summary>
		/// <value>true if the handle is valid; otherwise, false.</value>
		public abstract bool IsInvalid
		{
			get;
		}

		/// <summary>
		/// Marks the handle for releasing and freeing resources.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Returns the value of the <see cref="handle"/> field.
		/// </summary>
		/// <returns></returns>
		public IntPtr DangerousGetHandle()
		{
			return handle;
		}

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns></returns>
		protected abstract bool ReleaseHandle();

		/// <summary>
		/// Sets the handle to the specified pre-existing handle.
		/// </summary>
		/// <param name="handle"></param>
		protected void SetHandle(IntPtr handle)
		{
			this.handle = handle;
		}

		/// <summary>
		/// Marks a handle as invalid.
		/// </summary>
		public void SetHandleAsInvalid()
		{
            handle = m_invalid;
			GC.SuppressFinalize(this);
		}

		#region IDisposable Members

		protected void Dispose(bool disposing)
		{
			if(m_ownsHandle && !IsClosed && !IsInvalid)
			{
				ReleaseHandle();
			}

			if(disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Marks the handle for releasing and freeing resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			
		}

		/// <summary>
		/// Frees all resources associated with the handle.
		/// </summary>
		~SafeHandle()
		{
			Dispose(false);
		}
		#endregion
	}
}
