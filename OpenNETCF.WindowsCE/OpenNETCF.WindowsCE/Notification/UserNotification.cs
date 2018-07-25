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
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Contains information used for a user notification.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class UserNotification
    {
        int ActionFlags;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszDialogTitle;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszDialogText;
        [MarshalAs(UnmanagedType.LPWStr)]
        string pwszSound;
        int nMaxSound;
        int dwReserved;

        /// <summary>
        /// Create a new instance of the UserNotification class
        /// </summary>
        public UserNotification()
        {
        }

        #region Action
        /// <summary>
        /// Any combination of the <see cref="NotificationAction"/> members.  
        /// </summary>
        /// <value>Flags which specifies the action(s) to be taken when the notification is triggered.</value>
        /// <remarks>Flags not valid on a given hardware platform will be ignored.</remarks>
        public NotificationAction Action
        {
            get
            {
                return (NotificationAction)ActionFlags;
            }
            set
            {
                ActionFlags = (int)value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Required if NotificationAction.Dialog is set, ignored otherwise
        /// </summary>
        public string Title
        {
            get
            {
                return pwszDialogTitle;
            }
            set
            {
                pwszDialogTitle = value;
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Required if NotificationAction.Dialog is set, ignored otherwise.
        /// </summary>
        public string Text
        {
            get
            {
                return pwszDialogText;
            }
            set
            {
                pwszDialogText = value;
            }
        }
        #endregion

        #region Sound
        /// <summary>
        /// Sound string as supplied to PlaySound.
        /// </summary>
        public string Sound
        {
            get
            {
                return pwszSound;
            }
            set
            {
                pwszSound = value;
            }
        }

        internal int MaxSound
        {
            get
            {
                return nMaxSound;
            }
            set
            {
                nMaxSound = value;
            }
        }
        #endregion
    }
}
