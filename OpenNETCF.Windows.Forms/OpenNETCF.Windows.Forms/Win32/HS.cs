using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// HTML control styles.
	/// </summary>
	[Flags()]
	internal enum HS : int
	{
		NOFITTOWINDOW        = 0x0001,
		CONTEXTMENU          = 0x0002,
		CLEARTYPE            = 0x0004,
		NOSCRIPTING          = 0x0008,
		INTEGRALPAGING       = 0x0010,
		NOSCROLL             = 0x0020,
		NOIMAGES             = 0x0040,
		NOSOUNDS             = 0x0080,
		NOACTIVEX            = 0x0100,
		NOSELECTION          = 0x0200,
		NOFOCUSRECT          = 0x0400,
	}
}
