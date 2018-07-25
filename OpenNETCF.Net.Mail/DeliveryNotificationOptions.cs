using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mail
{
    /// <summary>
    /// Describes the delivery notification options for e-mail.  This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum DeliveryNotificationOptions
    {
        /// <summary>
        /// No notification.
        /// </summary>
        None = 0,
        /// <summary>
        /// Notify if the delivery is unsuccessful.
        /// </summary>
        OnFailure = 2,
        /// <summary>
        /// Notify if the delivery is successful.
        /// </summary>
        OnSuccess = 1,
        /// <summary>
        /// Notify if the delivery is delayed
        /// </summary>
        Delay = 4,

        /// <summary>
        /// Never notify.
        /// </summary>
        Never = 0x8000000,
    }

 

 

}
