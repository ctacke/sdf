using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// MonthCalendar Colors.
	/// </summary>
	internal enum MCSC :int
	{
		BACKGROUND   = 0,   // the background color (between months)
		TEXT         = 1,   // the dates
		TITLEBK      = 2,   // background of the title
		TITLETEXT    = 3,
		MONTHBK      = 4,   // background within the month cal
		TRAILINGTEXT = 5,   // the text color of header & trailing days
	}
}
