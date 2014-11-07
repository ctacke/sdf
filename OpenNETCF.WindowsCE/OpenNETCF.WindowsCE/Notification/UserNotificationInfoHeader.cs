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
