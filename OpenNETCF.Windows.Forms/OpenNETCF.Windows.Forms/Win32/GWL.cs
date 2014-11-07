using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Parameters for GetWindowLong.
	/// </summary>
	public enum GWL : int
	{
		/// <summary>
		/// 
		/// </summary>
		WNDPROC   =      (-4),
		/// <summary>
		/// 
		/// </summary>
		HINSTANCE =      (-6),
		/// <summary>
		/// 
		/// </summary>
		HWNDPARENT=      (-8),
		/// <summary>
		/// 
		/// </summary>
		STYLE     =      (-16),
		/// <summary>
		/// 
		/// </summary>
		EXSTYLE   =      (-20),
		/// <summary>
		/// 
		/// </summary>
		USERDATA  =      (-21),
		/// <summary>
		/// 
		/// </summary>
		ID        =      (-12),
	}
}
