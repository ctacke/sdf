using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// ComboBox Messages.
	/// </summary>
	public enum CB: int
	{
        /// <summary>
        /// An application sends a CB_GETEDITSEL message to get the starting and ending character positions of the current selection in the edit control of a combo box
        /// </summary>
		GETEDITSEL              = 0x0140,
        /// <summary>
        /// An application sends a CB_LIMITTEXT message to limit the length of the text the user may type into the edit control of a combo box
        /// </summary>
		LIMITTEXT               = 0x0141,
        /// <summary>
        /// An application sends a CB_SETEDITSEL message to select characters in the edit control of a combo box
        /// </summary>
		SETEDITSEL              = 0x0142,
        /// <summary>
        /// An application sends a CB_ADDSTRING message to add a string to the list box of a combo box. If the combo box does not have the CBS_SORT style, the string is added to the end of the list. Otherwise, the string is inserted into the list, and the list is sorted
        /// </summary>
		ADDSTRING               = 0x0143,
        /// <summary>
        /// An application sends a CB_DELETESTRING message to delete a string in the list box of a combo box
        /// </summary>
		DELETESTRING            = 0x0144,
        /// <summary>
        /// An application sends a CB_GETCOUNT message to retrieve the number of items in the list box of a combo box.
        /// </summary>
		GETCOUNT                = 0x0146,
        /// <summary>
        /// An application sends a CB_GETCURSEL message to retrieve the index of the currently selected item, if any, in the list box of a combo box.
        /// </summary>
		GETCURSEL               = 0x0147,
        /// <summary>
        /// An application sends a CB_GETLBTEXT message to retrieve a string from the list of a combo box
        /// </summary>
		GETLBTEXT               = 0x0148,
        /// <summary>
        /// An application sends a CB_GETLBTEXTLEN message to retrieve the length, in characters, of a string in the list of a combo box.
        /// </summary>
		GETLBTEXTLEN            = 0x0149,
        /// <summary>
        /// An application sends a CB_INSERTSTRING message to insert a string into the list box of a combo box. Unlike the CB_ADDSTRING message, the CB_INSERTSTRING message does not cause a list with the CBS_SORT style to be sorted
        /// </summary>
		INSERTSTRING            = 0x014A,
        /// <summary>
        /// An application sends a CB_RESETCONTENT message to remove all items from the list box and edit control of a combo box
        /// </summary>
		RESETCONTENT            = 0x014B,
        /// <summary>
        /// An application sends a CB_FINDSTRING message to search the list box of a combo box for an item beginning with the characters in a specified string
        /// </summary>
		FINDSTRING              = 0x014C,
        /// <summary>
        /// An application sends a CB_SELECTSTRING message to search the list of a combo box for an item that begins with the characters in a specified string. If a matching item is found, it is selected and copied to the edit control.
        /// </summary>
		SELECTSTRING            = 0x014D,
        /// <summary>
        /// An application sends a CB_SETCURSEL message to select a string in the list of a combo box. If necessary, the list scrolls the string into view. The text in the edit control of the combo box changes to reflect the new selection, and any previous selection in the list is removed
        /// </summary>
		SETCURSEL               = 0x014E,
        /// <summary>
        /// An application sends a CB_SHOWDROPDOWN message to show or hide the list box of a combo box that has the CBS_DROPDOWN or CBS_DROPDOWNLIST style
        /// </summary>
		SHOWDROPDOWN            = 0x014F,
        /// <summary>
        /// An application sends a CB_GETITEMDATA message to a combo box to retrieve the application-supplied value associated with the specified item in the combo box.
        /// </summary>
		GETITEMDATA             = 0x0150,
        /// <summary>
        /// An application sends a CB_SETITEMDATA message to set the value associated with the specified item in a combo box
        /// </summary>
		SETITEMDATA             = 0x0151,
        /// <summary>
        /// An application sends a CB_GETDROPPEDCONTROLRECT message to retrieve the screen coordinates of a combo box in its dropped-down state
        /// </summary>
		GETDROPPEDCONTROLRECT   = 0x0152,
        /// <summary>
        /// An application sends a CB_SETITEMHEIGHT message to set the height of list items or the selection field in a combo box.
        /// </summary>
		SETITEMHEIGHT           = 0x0153,
        /// <summary>
        /// An application sends a CB_GETITEMHEIGHT message to determine the height of list items or the selection field in a combo box
        /// </summary>
		GETITEMHEIGHT           = 0x0154,
        /// <summary>
        /// An application sends a CB_SETEXTENDEDUI message to select either the default user interface or the extended user interface for a combo box that has the CBS_DROPDOWN or CBS_DROPDOWNLIST style
        /// </summary>
		SETEXTENDEDUI           = 0x0155,
        /// <summary>
        /// An application sends a CB_GETEXTENDEDUI message to determine whether a combo box has the default user interface or the extended user interface
        /// </summary>
		GETEXTENDEDUI           = 0x0156,
        /// <summary>
        /// An application sends a CB_GETDROPPEDSTATE message to determine whether the list box of a combo box is dropped down.
        /// </summary>
		GETDROPPEDSTATE         = 0x0157,
        /// <summary>
        /// An application sends a CB_FINDSTRINGEXACT message to find the first list box string in a combo box that matches the string specified in the lParam parameter.
        /// </summary>
		FINDSTRINGEXACT         = 0x0158,
        /// <summary>
        /// An application sends a CB_SETLOCALE message to set the current locale of the combo box. If the combo box has the CBS_SORT style and strings are added using CB_ADDSTRING, the locale of a combo box affects how list items are sorted.
        /// </summary>
		SETLOCALE               = 0x0159,
        /// <summary>
        /// An application sends a CB_GETLOCALE message to retrieve the current locale of the combo box. The locale is used to determine the correct sorting order of displayed text for combo boxes with the CBS_SORT style and text added by using the CB_ADDSTRING message.
        /// </summary>
		GETLOCALE               = 0x015A,
        /// <summary>
        /// An application sends the CB_GETTOPINDEX message to retrieve the zero-based index of the first visible item in the list box portion of a combo box. Initially, the item with index 0 is at the top of the list box, but if the list box contents have been scrolled, another item may be at the top.
        /// </summary>
		GETTOPINDEX             = 0x015b,
        /// <summary>
        /// An application sends the CB_SETTOPINDEX message to ensure that a particular item is visible in the list box of a combo box. The system scrolls the list box contents so that either the specified item appears at the top of the list box or the maximum scroll range has been reached.
        /// </summary>
		SETTOPINDEX             = 0x015c,
        /// <summary>
        /// An application sends the CB_GETHORIZONTALEXTENT message to retrieve from a combo box the width, in pixels, by which the list box can be scrolled horizontally (the scrollable width). This is applicable only if the list box has a horizontal scroll bar.
        /// </summary>
		GETHORIZONTALEXTENT     = 0x015d,
        /// <summary>
        /// An application sends the CB_SETHORIZONTALEXTENT message to set the width, in pixels, by which a list box can be scrolled horizontally (the scrollable width). If the width of the list box is smaller than this value, the horizontal scroll bar horizontally scrolls items in the list box. If the width of the list box is equal to or greater than this value, the horizontal scroll bar is hidden or, if the combo box has the CBS_DISABLENOSCROLL style, disabled.
        /// </summary>
		SETHORIZONTALEXTENT     = 0x015e,
        /// <summary>
        /// An application sends the CB_GETDROPPEDWIDTH message to retrieve the minimum allowable width, in pixels, of the list box of a combo box with the CBS_DROPDOWN or CBS_DROPDOWNLIST style
        /// </summary>
		GETDROPPEDWIDTH         = 0x015f,
        /// <summary>
        /// An application sends the CB_SETDROPPEDWIDTH message to set the maximum allowable width, in pixels, of the list box of a combo box with the CBS_DROPDOWN or CBS_DROPDOWNLIST style.
        /// </summary>
		SETDROPPEDWIDTH         = 0x0160,
        /// <summary>
        /// An application sends the CB_INITSTORAGE message before adding a large number of items to the list box portion of a combo box. This message allocates memory for storing list box items.
        /// </summary>
		INITSTORAGE             = 0x0161
	}
}
