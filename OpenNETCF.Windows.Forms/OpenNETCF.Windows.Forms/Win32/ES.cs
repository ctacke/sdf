using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Edit control styles.
	/// </summary>
	[Flags()]
	public enum ES: int
	{
        /// <summary>
        /// Left aligned
        /// </summary>
		LEFT        = 0x0000,
        /// <summary>
        /// Center aligned
        /// </summary>
		CENTER      = 0x0001,
        /// <summary>
        /// Right aligned
        /// </summary>
		RIGHT       = 0x0002,
        /// <summary>
        /// Designates a multiline edit control. The default is single-line edit control. 
        /// When the multiline edit control is in a dialog box, the default response to pressing the ENTER key is to activate the default button. To use the ENTER key as a carriage return, use the ES_WANTRETURN style.
        /// </summary>
		MULTILINE   = 0x0004,
        /// <summary>
        /// Force uppercase
        /// </summary>
		UPPERCASE   = 0x0008,
        /// <summary>
        /// Force lowercase
        /// </summary>
		LOWERCASE   = 0x0010,
        /// <summary>
        /// Displays an asterisk (*) for each character typed into the edit control. This style is valid only for single-line edit controls
        /// </summary>
		PASSWORD    = 0x0020,
        /// <summary>
        /// Show vertical scrollbars automatically
        /// </summary>
		AUTOVSCROLL = 0x0040,
        /// <summary>
        /// Show horizontal scrollbars automatically
        /// </summary>
		AUTOHSCROLL = 0x0080,
        /// <summary>
        /// Negates the default behavior for an edit control. The default behavior hides the selection when the control loses the input focus and inverts the selection when the control receives the input focus. If you specify ES_NOHIDESEL, the selected text is inverted, even if the control does not have the focus.
        /// </summary>
		NOHIDESEL   = 0x0100,
        /// <summary>
        /// Combobox
        /// </summary>
		COMBOBOX    = 0x0200,
        /// <summary>
        /// Converts text entered in the edit control. The text is converted from the Windows character set to the OEM character set and then back to the Windows character set. This ensures proper character conversion when the application calls the CharToOem function to convert a Windows string in the edit control to OEM characters. This style is most useful for edit controls that contain file names that will be used on file systems that do not support Unicode.
        /// </summary>
		OEMCONVERT  = 0x0400,
        /// <summary>
        /// Prevents the user from typing or editing text in the edit control
        /// </summary>
		READONLY    = 0x0800,
        /// <summary>
        /// Specifies that a carriage return be inserted when the user presses the ENTER key while entering text into a multiline edit control in a dialog box. If you do not specify this style, pressing the ENTER key has the same effect as pressing the dialog box's default push button. This style has no effect on a single-line edit control.
        /// </summary>
		WANTRETURN  = 0x1000,
        /// <summary>
        /// Allows only digits to be entered into the edit control. Note that, even with this set, it is still possible to paste non-digits into the edit control.
        /// </summary>
		NUMBER      = 0x2000,
	}
}
