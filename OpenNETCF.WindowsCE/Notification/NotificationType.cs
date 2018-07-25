using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Specifies the type of notification.
    /// </summary>
    public enum NotificationType : int
    {
        /// <summary>
        /// System event notification.
        /// </summary>
        Event = 1,
        /// <summary>
        /// Time-based notification.
        /// </summary>
        Time = 2,
        /// <summary>
        /// Time-based notification that is active for the time period between <see cref="M:OpenNETCF.Win32.Notify.NotificationTrigger.StartTime"/> and <see cref="M:OpenNETCF.Win32.Notify.NotificationTrigger.EndTime"/>.
        /// </summary>
        Period = 3,
        /// <summary>
        /// Equivalent to using the SetUserNotification function.
        /// The standard command line is supplied.
        /// </summary>
        ClassicTime = 4,
    }
}
