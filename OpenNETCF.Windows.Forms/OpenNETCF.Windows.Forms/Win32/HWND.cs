using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Preset window handles.
	/// </summary>
	public enum HWND : int
	{
		TOP        = 0,
		BOTTOM     = 1,
		TOPMOST    = -1,
		NOTOPMOST  = -2,
		MESSAGE = -3,
	}
}
