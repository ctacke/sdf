using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Listbox control styles.
	/// </summary>
	[Flags()]
	public enum LBS : int
	{
		/// <summary>
        /// Parent window receives an input message whenever the user clicks or double-clicks a string
		/// </summary>
		NOTIFY           = 0x0001,
        /// <summary>
        /// Strings in the list box are sorted alphabetically.
        /// </summary>
		SORT             = 0x0002,
        /// <summary>
        /// List-box display is not updated when changes are made. This style can be changed at any time by sending a WM_SETREDRAW message.
        /// </summary>
		NOREDRAW         = 0x0004,
        /// <summary>
        /// String selection is toggled each time the user clicks or double-clicks the string. Any number of strings can be selected.
        /// </summary>
		MULTIPLESEL      = 0x0008,
        /// <summary>
        /// Specifies an owner-draw list box that contains items consisting of strings. The list box maintains the memory and pointers for the strings so the application can use the GetText member function to retrieve the text for a particular item.
        /// </summary>
        OWNERDRAWFIXED = 0x0010,
        OWNERDRAWVARIABLE = 0x0020,
        /// <summary>
        /// Specifies an owner-draw list box that contains items consisting of strings. The list box maintains the memory and pointers for the strings so the application can use the GetText member function to retrieve the text for a particular item.
        /// </summary>
		HASSTRINGS       = 0x0040,
        /// <summary>
        /// Allows a list box to recognize and expand tab characters when drawing its strings. The default tab positions are 32 dialog units. (A dialog unit is a horizontal or vertical distance. One horizontal dialog unit is equal to one-fourth of the current dialog base width unit. The dialog base units are computed based on the height and width of the current system font. The GetDialogBaseUnits Windows function returns the current dialog base units in pixels.) This style should not be used with LBS_OWNERDRAWFIXED.
        /// </summary>
		USETABSTOPS      = 0x0080,
        /// <summary>
        /// The size of the list box is exactly the size specified by the application when it created the list box. Usually, Windows sizes a list box so that the list box does not display partial items.
        /// </summary>
		NOINTEGRALHEIGHT = 0x0100,
        /// <summary>
        /// Specifies a multicolumn list box that is scrolled horizontally. The SetColumnWidth member function sets the width of the columns.
        /// </summary>
		MULTICOLUMN      = 0x0200,
        /// <summary>
        /// The owner of the list box receives WM_VKEYTOITEM or WM_CHARTOITEM messages whenever the user presses a key while the list box has input focus. This allows an application to perform special processing on the keyboard input.
        /// </summary>
		WANTKEYBOARDINPUT= 0x0400,
        /// <summary>
        /// The user can select multiple items using the SHIFT key and the mouse or special key combinations.
        /// </summary>
		EXTENDEDSEL      = 0x0800,
        /// <summary>
        /// The list box shows a disabled vertical scroll bar when the list box does not contain enough items to scroll. Without this style, the scroll bar is hidden when the list box does not contain enough items.
        /// </summary>
		DISABLENOSCROLL  = 0x1000,
        /// <summary>
        /// Specifies a no-data list box. Specify this style when the count of items in the list box will exceed one thousand. A no-data list box must also have the LBS_OWNERDRAWFIXED style, but must not have the LBS_SORT or LBS_HASSTRINGS style.
        /// A no-data list box resembles an owner-drawn list box except that it contains no string or bitmap data for an item. Commands to add, insert, or delete an item always ignore any given item data; requests to find a string within the list box always fail. The system sends the WM_DRAWITEM message to the owner window when an item must be drawn. The itemID member of the DRAWITEMSTRUCT structure passed with the WM_DRAWITEM message specifies the line number of the item to be drawn. A no-data list box does not send a WM_DELETEITEM message.
        /// </summary>
		NODATA           = 0x2000,
        /// <summary>
        /// Strings in the list box are sorted alphabetically, and the parent window receives an input message whenever the user clicks or double-clicks a string. The list box contains borders on all sides.
        /// </summary>
		STANDARD         = (NOTIFY | SORT | WS.VSCROLL | WS.BORDER)
	}

}
