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
	/// Edit control Messages.
	/// </summary>
	public enum EM: int
	{
    /// <summary>
    /// This message retrieves the starting and ending character positions of the current selection in an edit control.
    /// </summary>
		GETSEL              = 0x00B0,
    /// <summary>
    /// This message selects a range of characters in an edit control.
    /// </summary>
		SETSEL              = 0x00B1,
    /// <summary>
    /// This message retrieves the formatting rectangle of an edit control.
    /// </summary>
		GETRECT             = 0x00B2,
    /// <summary>
    /// This message sets the formatting rectangle of a multiline edit control.
    /// </summary>
		SETRECT             = 0x00B3,
    /// <summary>
    /// This message sets the formatting rectangle of a multiline edit control. 
    /// </summary>
		SETRECTNP           = 0x00B4,
    /// <summary>
    /// This message scrolls the text vertically in a multiline edit control.
    /// </summary>
		SCROLL              = 0x00B5,
    /// <summary>
    /// This message scrolls the text vertically or horizontally in a multiline edit control.
    /// </summary>
		LINESCROLL          = 0x00B6,
    /// <summary>
    /// This message scrolls the caret into view in an edit control.
    /// </summary>
		SCROLLCARET         = 0x00B7,
    /// <summary>
    /// This message determines whether the contents of an edit control have been modified.
    /// </summary>
		GETMODIFY           = 0x00B8,
    /// <summary>
    /// This message sets or clears the modification flag for an edit control.
    /// </summary>
		SETMODIFY           = 0x00B9,
    /// <summary>
    /// This message retrieves the number of lines in a multiline edit control.
    /// </summary>
		GETLINECOUNT        = 0x00BA,
    /// <summary>
    /// This message retrieves the character index of a line in a multiline edit control.
    /// </summary>
		LINEINDEX           = 0x00BB,
    /// <summary>
    /// This message retrieves the length of a line, in characters, in an edit control.
    /// </summary>
		LINELENGTH          = 0x00C1,
    /// <summary>
    /// This message replaces the current selection in an edit control with the specified text.
    /// </summary>
		REPLACESEL          = 0x00C2,
    /// <summary>
    /// This message copies a line of text from an edit control and places the text in a specified buffer. 
    /// </summary>
		GETLINE             = 0x00C4,
    /// <summary>
    /// This message limits the amount of text the user can enter into an edit control.
    /// </summary>
		LIMITTEXT           = 0x00C5,
    /// <summary>
    /// This message determines whether an edit-control operation can be undone; that is, whether the control can respond to the EM_UNDO message.
    /// </summary>
		CANUNDO             = 0x00C6,
    /// <summary>
    /// This message reverses the effect of the last edit control operation.
    /// </summary>
		UNDO                = 0x00C7,
    /// <summary>
    /// This message sets the inclusion flag for soft line-break characters on or off within a multiline edit control.
    /// </summary>
		FMTLINES            = 0x00C8,
    /// <summary>
    /// This message retrieves the index of the line that contains the specified character index in a multiline edit control.
    /// </summary>
		LINEFROMCHAR        = 0x00C9,
    /// <summary>
    /// This message sets the tab stops in a multiline edit control.
    /// </summary>
		SETTABSTOPS         = 0x00CB,
    /// <summary>
    /// This message sets or removes a password character displayed in a single-line edit control when the user types text.
    /// </summary>
		SETPASSWORDCHAR     = 0x00CC,
    /// <summary>
    /// This message resets the undo flag of an edit control.
    /// </summary>
		EMPTYUNDOBUFFER     = 0x00CD,
    /// <summary>
    /// This message determines the uppermost visible line in an edit control.
    /// </summary>
		GETFIRSTVISIBLELINE = 0x00CE,
    /// <summary>
    /// This message sets or removes the read-only style (ES_READONLY) of an edit control.
    /// </summary>
		SETREADONLY         = 0x00CF,
    /// <summary>
    /// This message retrieves the password character displayed in an edit control when the user enters text.
    /// </summary>
		GETPASSWORDCHAR     = 0x00D2,
    /// <summary>
    /// This message sets the widths of the left and right margins for an edit control.
    /// </summary>
		SETMARGINS          = 0x00D3,
    /// <summary>
    /// This message retrieves the widths of the left and right margins for an edit control.
    /// </summary>
		GETMARGINS          = 0x00D4,
    /// <summary>
    /// This message limits the amount of text the user can enter into an edit control.
    /// </summary>
		SETLIMITTEXT        = LIMITTEXT,
    /// <summary>
    /// This message retrieves the current text limit, in characters, for an edit control.
    /// </summary>
		GETLIMITTEXT        = 0x00D5,
    /// <summary>
    /// This message retrieves the coordinates of the specified character in an edit control.
    /// </summary>
		POSFROMCHAR         = 0x00D6,
    /// <summary>
    /// This message retrieves the zero-based character index and zero-based line index of the character nearest the specified point in an edit control.
    /// </summary>
		CHARFROMPOS         = 0x00D7
	}
}
