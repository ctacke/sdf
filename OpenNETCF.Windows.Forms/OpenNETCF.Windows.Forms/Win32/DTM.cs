using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// DateTimePicker Messages.
	/// </summary>
	internal enum DTM : int
	{
	GETSYSTEMTIME = 0x1001,
	SETSYSTEMTIME = 0x1002,
	GETRANGE = 0x1003,
	SETRANGE = 0x1004,
	SETFORMATA = 0x1005,
	SETFORMATW = 0x1032,
	SETMCCOLOR = 0x1006,
	GETMCCOLOR = 0x1007,
	GETMONTHCAL = 0x1008,
	SETMCFONT = 0x1009,
	GETMCFONT = 0x100A,
	}
}
