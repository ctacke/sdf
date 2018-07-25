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
	/// Contains information about notification events.
	/// </summary>
    [StructLayout(LayoutKind.Sequential)]
	public class UserNotificationInfoHeader
	{
        /// <summary>
        /// Notification handle
        /// </summary>
        public int hNotification;
        /// <summary>
        /// Status
        /// </summary>
        public int dwStatus;
        /// <summary>
        /// Notification trigger
        /// </summary>
        public UserNotificationTrigger pcent;
        /// <summary>
        /// Notification object
        /// </summary>
        public UserNotification pceun;

		#region Constructor
		/// <summary>
		/// Create a new instance of UserNotificationInfoHeader
		/// </summary>
		public UserNotificationInfoHeader()
		{
		}
		#endregion
	
		#region Handle
		/// <summary>
		/// Handle to the notification.
		/// </summary>
		public int Handle
		{
			get
			{
                return hNotification;
			}
            set
            {
                hNotification = value;
            }
		}
		#endregion

		#region Status
		/// <summary>
		/// Indicates current state of the notification.
		/// </summary>
		public NotificationStatus Status
		{
			get
			{
                return (NotificationStatus)dwStatus;
			}
            set
            {
                dwStatus = (int)value;
            }
		}
		#endregion

		#region User Notification Trigger
		/// <summary>
		/// The <see cref="UserNotificationTrigger"/> object
		/// </summary>
		public UserNotificationTrigger UserNotificationTrigger
		{
			get
			{
                return pcent;
			}
            set
            {
                pcent = value;
            }
		}
		#endregion

		#region User Notification
		/// <summary>
		/// The <see cref="UserNotification"/> object.
		/// </summary>
		public UserNotification UserNotification
		{
			get
			{
                return pceun;
			}
            set
            {
                pceun = value;
            }
		}
		#endregion
	}
}
