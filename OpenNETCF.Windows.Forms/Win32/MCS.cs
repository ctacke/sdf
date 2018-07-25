using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// MonthCalendar styles.
	/// </summary>
	internal enum MCS :int
	{
		DAYSTATE        = 0x0001,
		MULTISELECT     = 0x0002,
		WEEKNUMBERS     = 0x0004,
		SHOWNONE        = 0x0080,
		NOTODAYCIRCLE   = 0x0008,
		NOTODAY         = 0x0010,
	}
}
