using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// ListView Messages.
	/// </summary>
	public enum LVM: int
	{
        FIRST = 0x1000,
        GETHEADER = FIRST + 31,
        SETICONSPACING = FIRST + 53,
        GETSUBITEMRECT = FIRST + 56,
        GETITEMSTATE = FIRST + 44,
        GETITEMTEXTW = FIRST + 115,
        INSERTITEMA = FIRST + 7,
        INSERTITEMW = FIRST + 77,
        INSERTCOLUMNA = FIRST + 27,
        INSERTCOLUMNW = FIRST + 97,
        DELETECOLUMN = FIRST + 28,
        GETCOLUMNA = FIRST + 25,
        GETCOLUMNW = FIRST + 95,
        SETEXTENDEDLISTVIEWSTYLE = FIRST + 54,
        SETITEMA = FIRST + 6,
        SETITEMW = FIRST + 76,
        EDITLABELA = FIRST + 23,
        EDITLABELW = FIRST + 118,
        DELETEITEM = FIRST + 8,
        SETBKCOLOR = FIRST + 1,
        GETBKCOLOR = FIRST + 0,
        GETTEXTBKCOLOR = FIRST + 37,
        SETTEXTBKCOLOR = FIRST + 38,
        DELETEALLITEMS = FIRST + 9,
        GETNEXTITEM = FIRST + 12,
        SETITEMCOUNT = FIRST + 47,
        GETITEMCOUNT = FIRST + 4,
        SETCOLUMNWIDTH = FIRST + 30,
        GETITEMRECT = FIRST + 14,
        EDITLABEL = FIRST + 23,
        GETVIEWRECT = FIRST + 34,
        SETITEMPOSITION = FIRST + 15,
        GETTOPINDEX = FIRST + 39,
        GETCOUNTPERPAGE = FIRST + 40,
    }

}
