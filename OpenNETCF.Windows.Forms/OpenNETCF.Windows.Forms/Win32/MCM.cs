using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// MonthCalendar Messages.
	/// </summary>
	internal enum MCM : int
	{
		FIRST				= 0x1000,
		GETCURSEL			= (FIRST + 1),
		SETCURSEL			= (FIRST + 2),
		GETMAXSELCOUNT		= (FIRST + 3),
		SETMAXSELCOUNT		= (FIRST + 4),
		GETSELRANGE			= (FIRST + 5),
		SETSELRANGE			= (FIRST + 6),
		GETMONTHRANGE		= (FIRST + 7),
		SETDAYSTATE			= (FIRST + 8),
		GETMINREQRECT		= (FIRST + 9),
		SETCOLOR            = (FIRST + 10),
		GETCOLOR            = (FIRST + 11),
		SETTODAY			= (FIRST + 12),
		GETTODAY			= (FIRST + 13),
		HITTEST				= (FIRST + 14),
		SETFIRSTDAYOFWEEK	= (FIRST + 15),
		GETFIRSTDAYOFWEEK	= (FIRST + 16),
		GETRANGE			= (FIRST + 17),
		SETRANGE			= (FIRST + 18),
		GETMONTHDELTA		= (FIRST + 19),
		SETMONTHDELTA		= (FIRST + 20),
		GETMAXTODAYWIDTH	= (FIRST + 21),
		GETMAXNONEWIDTH		= (FIRST + 22),
	}
}
