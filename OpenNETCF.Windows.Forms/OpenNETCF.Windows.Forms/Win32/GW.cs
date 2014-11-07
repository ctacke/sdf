using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Window relationships used with GetWindow.
	/// </summary>
	public enum GW : int
	{
		/// <summary>
		/// 
		/// </summary>
		HWNDFIRST  =      0,
		/// <summary>
		/// 
		/// </summary>
		HWNDLAST   =      1,
		/// <summary>
		/// 
		/// </summary>
		HWNDNEXT   =      2,
		/// <summary>
		/// 
		/// </summary>
		HWNDPREV   =      3,
		/// <summary>
		/// 
		/// </summary>
		OWNER      =      4,
		/// <summary>
		/// 
		/// </summary>
		CHILD      =      5,
	}
}
