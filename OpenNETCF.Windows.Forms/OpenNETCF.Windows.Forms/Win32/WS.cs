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
	/// Window style flags.
	/// </summary>
	[Flags()]
	public enum WS : int
	{
		/// <summary>
		/// Creates a window that has a thin-line border. 
		/// </summary>
		BORDER = 0x00800000,
 
		/// <summary>
		/// Creates a window that has a title bar (includes the BORDER style). 
		/// </summary>
		CAPTION = 0x00C00000,
 
		/// <summary>
		/// Creates a child window. This style cannot be used with the POPUP style. 
		/// </summary>
		CHILD = 0x40000000,
 
		/// <summary>
		/// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window. 
		/// </summary>
		CLIPSIBLINGS = 0x04000000,
		CLIPCHILDREN = 0x02000000,

		/// <summary>
		/// Creates a window that is initially disabled. A disabled window cannot receive input from the user. 
		/// </summary>
		DISABLED = 0x08000000,
 
		/// <summary>
		/// Creates a window that has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar. 
		/// </summary>
		DLGFRAME = 0x00400000,
 
		/// <summary>
		/// Specifies the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the GROUP style. The first control in each group usually has the TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys. 
		/// </summary>
		GROUP = 0x00020000,
 
		/// <summary>
		/// Creates a window that has a horizontal scroll bar. 
		/// </summary>
		HSCROLL = 0x00100000,
  
		/// <summary>
		/// Creates a window that has a Maximize button. Cannot be combined with the EX_CONTEXTHELP style.  
		/// </summary>
		MAXIMIZEBOX = 0x00020000,
  
		/// <summary>
		/// Creates a window that has a Minimize button. Cannot be combined with the EX_CONTEXTHELP style.  
		/// </summary>
		MINIMIZEBOX = 0x00010000,
 
		/// <summary>
		/// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the TILED style. 
		/// </summary>
		OVERLAPPED = BORDER | CAPTION,
  
		/// <summary>
		/// Creates a pop-up window. This style cannot be used with the CHILD style. 
		/// </summary>
		POPUP = unchecked((int)0x80000000),
   
		/// <summary>
		/// Creates a window that has a Close (X) button in the non-client area. 
		/// </summary>
		SYSMENU = 0x00080000,
 
		/// <summary>
		/// Specifies a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the TABSTOP style. 
		/// </summary>
		TABSTOP = 0x00010000,
 
		/// <summary>
		/// Creates a window that has a sizing border. Same as the SIZEBOX style. 
		/// </summary>
		THICKFRAME = 0x00040000,
  
		/// <summary>
		/// Creates a window that is initially visible. 
		/// </summary>
		VISIBLE = 0x10000000,
 
		/// <summary>
		/// Creates a window that has a vertical scroll bar. 
		/// </summary>
		VSCROLL = 0x00200000
	}
}