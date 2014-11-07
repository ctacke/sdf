using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// The status of the notification.
    /// </summary>
    public enum NotificationStatus : int
    {
        /// <summary>
        /// The notification is not currently active.
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// The notification is currently active.
        /// </summary>
        Signalled = 1,
    }
}
