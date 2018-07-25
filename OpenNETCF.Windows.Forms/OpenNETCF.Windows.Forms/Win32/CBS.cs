#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Combobox control styles.
	/// </summary>
	[Flags()]
	public enum CBS
	{
        /// <summary>
        /// Similar to CBS_SIMPLE, except that the list box is not displayed unless the user selects an icon next to the edit control.
        /// </summary>
		DROPDOWN          = 0x0002,
        /// <summary>
        /// Similar to CBS_DROPDOWN, except that the edit control is replaced by a static text item that displays the current selection in the list box
        /// </summary>
		DROPDOWNLIST      = 0x0003,
        /// <summary>
        /// Automatically scrolls the text in an edit control to the right when the user types a character at the end of the line. If this style is not set, only text that fits within the rectangular boundary is allowed.
        /// </summary>
		AUTOHSCROLL       = 0x0040,
        /// <summary>
        /// character set and then back to the Windows character set. This ensures proper character conversion when the application calls the CharToOem function to convert a Windows string in the combo box to OEM characters. This style is most useful for combo boxes that contain file names and applies only to combo boxes created with the CBS_SIMPLE or CBS_DROPDOWN style.
        /// </summary>
		OEMCONVERT        = 0x0080,
        /// <summary>
        /// Automatically sorts strings added to the list box
        /// </summary>
		SORT              = 0x0100,
        /// <summary>
        /// Specifies that an owner-drawn combo box contains items consisting of strings. The combo box maintains the memory and address for the strings so the application can use the CB_GETLBTEXT message to retrieve the text for a particular item.
        /// </summary>
		HASSTRINGS        = 0x0200,
        /// <summary>
        /// Specifies that the size of the combo box is exactly the size specified by the application when it created the combo box. Normally, the system sizes a combo box so that it does not display partial items
        /// </summary>
		NOINTEGRALHEIGHT  = 0x0400,
        /// <summary>
        /// Shows a disabled vertical scroll bar in the list box when the box does not contain enough items to scroll. Without this style, the scroll bar is hidden when the list box does not contain enough items.
        /// </summary>
		DISABLENOSCROLL   = 0x0800,
        /// <summary>
        /// Converts to uppercase all text in both the selection field and the list.
        /// </summary>
		UPPERCASE         = 0x2000,
        /// <summary>
        /// Converts to lowercase all text in both the selection field and the list.
        /// </summary>
		LOWERCASE         = 0x4000,		
	}
}
