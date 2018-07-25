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
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum SW
    {
        /// <summary>
        /// Hides the window and activates another window
        /// </summary>
        HIDE = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        SHOWNORMAL = 1,
        /// <summary>
        /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except the window is not actived.
        /// </summary>
        SHOWNOACTIVATE = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        SHOW = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level window in the Z order.
        /// </summary>
        MINIMIZE = 6,
        /// <summary>
        /// Displays the window in its current size and position. This value is similar to SW_SHOW, except the window is not activated.
        /// </summary>
        SHOWNA = 8,
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        SHOWMAXIMIZED = 11,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        MAXIMIZE = 12,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
        /// </summary>
        RESTORE = 13
    }
}
