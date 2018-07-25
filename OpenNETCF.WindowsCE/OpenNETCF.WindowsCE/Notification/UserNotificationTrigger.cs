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
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Defines what event activates a notification.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class UserNotificationTrigger
    {
        internal int dwSize;
        int dwType;
        int dwEvent;
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpszApplication;
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpszArguments;
        internal SYSTEMTIME stStartTime;
        internal SYSTEMTIME stEndTime;

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="UserNotificationTrigger"/>
        /// </summary>
        public UserNotificationTrigger()
        {
            dwSize = 52;
            lpszApplication = "";
            stStartTime = new SYSTEMTIME();
            stEndTime = new SYSTEMTIME();
        }
        #endregion

        #region Type
        /// <summary>
        /// Specifies the type of notification.
        /// </summary>
        public NotificationType Type
        {
            get
            {
                return (NotificationType)dwType;
            }
            set
            {
                dwType = (int)value;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Specifies the type of event should Type = Event.
        /// </summary>
        public NotificationEvent Event
        {
            get
            {
                return (NotificationEvent)dwEvent;
            }
            set
            {
                dwEvent = (int)value;
            }
        }
        #endregion

        #region Application
        /// <summary>
        /// Name of the application to execute.
        /// </summary>
        public string Application
        {
            get
            {
                return lpszApplication;
            }
            set
            {
                lpszApplication = value;
            }
        }
        #endregion

        #region Arguments
        /// <summary>
        /// Command line (without the application name). 
        /// </summary>
        public string Arguments
        {
            get
            {
                return lpszArguments;
            }
            set
            {
                lpszArguments = value;
            }
        }
        #endregion

        #region Start Time
        /// <summary>
        /// Specifies the beginning of the notification period.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return stStartTime.ToDateTime();
            }
            set
            {
                stStartTime = SYSTEMTIME.FromDateTime(value);
            }
        }
        #endregion

        #region End Time
        /// <summary>
        /// Specifies the end of the notification period. 
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return stEndTime.ToDateTime();
            }
            set
            {
                stEndTime = SYSTEMTIME.FromDateTime(value);
            }
        }
        #endregion
    }
}
