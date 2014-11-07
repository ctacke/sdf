using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// MonthCalendar notifications.
	/// </summary>
	internal enum MCN : int
	{
		FIRST           =   -750,
		SELCHANGE       = (FIRST + 1),
		GETDAYSTATE     = (FIRST + 3),
		SELECT          = (FIRST + 4),
		SELECTNONE      = (FIRST + 5),
	}
}
