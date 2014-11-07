using System;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Win32.SafeHandles
{
	/// <summary>
	/// Provides common functionality that supports safe Win32 handle types.
	/// </summary>
	public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeHandleMinusOneIsInvalid"/> class.
		/// </summary>
		/// <param name="ownsHandle">true to reliably release the handle during the finalization phase; otherwise, false (not recommended).</param>
		protected SafeHandleMinusOneIsInvalid(bool ownsHandle) : base(new IntPtr(-1), ownsHandle){}

		/// <summary>
		/// Gets a value indicating whether a handle is invalid.
		/// </summary>
		public override bool IsInvalid
		{
			get
			{
				return(handle.ToInt32() == -1);
			}
		}
	}
}
