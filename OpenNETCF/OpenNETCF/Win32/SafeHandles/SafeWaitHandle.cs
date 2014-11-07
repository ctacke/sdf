using System;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Win32.SafeHandles
{
	/// <summary>
	/// Represents a wrapper class for wait handles.
	/// </summary>
	public class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeWaitHandle"/> class.
		/// </summary>
		/// <param name="existingHandle">The pre-existing handle to use.</param>
		/// <param name="ownsHandle">true to reliably release the handle during the finalization phase; otherwise, false (not recommended).</param>
		public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		/// <summary>
		/// Executes the code required to free a handle.
		/// </summary>
		/// <returns>true if the handle is released successfully; false if a catastrophic failure occurs.</returns>
		protected override bool ReleaseHandle()
		{
			return OpenNETCF.IO.NativeMethods.CloseHandle(handle);
		}

	}
}
