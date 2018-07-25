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
	/// Flags used with SetWindowPos.
	/// </summary>
	[Flags()]
	public enum SWP : int
	{
        /// <summary>
        /// Retains current size (ignores the cx and cy members). 
        /// </summary>
		NOSIZE          = 0x0001,
        /// <summary>
        /// Retains current position (ignores the x and y members). 
        /// </summary>
		NOMOVE          = 0x0002,
        /// <summary>
        /// Retains current ordering (ignores the hwndInsertAfter member).
        /// </summary>
		NOZORDER        = 0x0004,
        /// <summary>
        /// Does not redraw changes.
        /// </summary>
		NOREDRAW        = 0x0008,
        /// <summary>
        /// Does not activate the window.
        /// </summary>
		NOACTIVATE      = 0x0010,
        /// <summary>
        /// Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
		FRAMECHANGED    = 0x0020,
        /// <summary>
        /// Displays the window.
        /// </summary>
		SHOWWINDOW      = 0x0040,
        /// <summary>
        /// Hides the window.
        /// </summary>
		HIDEWINDOW      = 0x0080,
        /// <summary>
        /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
		NOCOPYBITS      = 0x0100,
        /// <summary>
        /// Does not change the owner window's position in the Z-order.
        /// </summary>
		NOOWNERZORDER   = 0x0200,
        /// <summary>
        /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
		NOSENDCHANGING  = 0x0400,
        /// <summary>
        /// Draws a frame (defined in the class description for the window) around the window. The window receives a WM_NCCALCSIZE message
        /// </summary>
		DRAWFRAME       = FRAMECHANGED,
        /// <summary>
        /// Same as SWP_NOOWNERZORDER
        /// </summary>
		NOREPOSITION    = NOOWNERZORDER,
        /// <summary>
        /// 
        /// </summary>
		DEFERERASE      = 0x2000,
        /// <summary>
        /// 
        /// </summary>
		ASYNCWINDOWPOS  = 0x4000
	}
}
