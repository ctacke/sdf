
using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// GetDateTimeRange.
	/// </summary>
	internal enum GDTR : int
	{
		MIN     = 0x0001,
		MAX     = 0x0002,
	}

	internal enum GDT : int
	{
		ERROR = -1,
		VALID = 0,
		NONE = 1,
	}
}
