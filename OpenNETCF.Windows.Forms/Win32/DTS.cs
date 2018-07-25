using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// DateTimePicker Styles.
	/// </summary>
	[Flags()]
	internal enum DTS : int
	{
		UPDOWN = 0x0001, // use UPDOWN instead of MONTHCAL
		SHOWNONE = 0x0002, // allow a NONE or checkbox selection
		SHORTDATEFORMAT = 0x0000, // use the short date format (app must forward WM_WININICHANGE messages)
		LONGDATEFORMAT = 0x0004, // use the long date format (app must forward WM_WININICHANGE messages)
		TIMEFORMAT = 0x0009, // use the time format (app must forward WM_WININICHANGE messages)
		APPCANPARSE = 0x0010, // allow user entered strings (app MUST respond to DTN_USERSTRING)
		RIGHTALIGN = 0x0020, // right-align popup instead of left-align it
		NONEBUTTON = 0x0080, // use NONE button instead of checkbox
	}
}
