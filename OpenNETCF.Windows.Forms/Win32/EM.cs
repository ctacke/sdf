using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Edit control Messages.
	/// </summary>
	public enum EM: int
	{
		GETSEL              = 0x00B0,
		SETSEL              = 0x00B1,
		GETRECT             = 0x00B2,
		SETRECT             = 0x00B3,
		SETRECTNP           = 0x00B4,
		SCROLL              = 0x00B5,
		LINESCROLL          = 0x00B6,
		SCROLLCARET         = 0x00B7,
		GETMODIFY           = 0x00B8,
		SETMODIFY           = 0x00B9,
		GETLINECOUNT        = 0x00BA,
		LINEINDEX           = 0x00BB,
		LINELENGTH          = 0x00C1,
		REPLACESEL          = 0x00C2,
		GETLINE             = 0x00C4,
		LIMITTEXT           = 0x00C5,
		CANUNDO             = 0x00C6,
		UNDO                = 0x00C7,
		FMTLINES            = 0x00C8,
		LINEFROMCHAR        = 0x00C9,
		SETTABSTOPS         = 0x00CB,
		SETPASSWORDCHAR     = 0x00CC,
		EMPTYUNDOBUFFER     = 0x00CD,
		GETFIRSTVISIBLELINE = 0x00CE,
		SETREADONLY         = 0x00CF,
		GETPASSWORDCHAR     = 0x00D2,
		SETMARGINS          = 0x00D3,
		GETMARGINS          = 0x00D4,
		SETLIMITTEXT        = LIMITTEXT,
		GETLIMITTEXT        = 0x00D5,
		POSFROMCHAR         = 0x00D6,
		CHARFROMPOS         = 0x00D7
	}
}
