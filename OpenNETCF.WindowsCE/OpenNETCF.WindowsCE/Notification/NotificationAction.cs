using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Specifies the action to take when a notification event occurs.
    /// </summary>
    [Flags()]
    public enum NotificationAction : int
    {
        /// <summary>
        /// Flashes the LED.
        /// </summary>
        Led = 1,
        /// <summary>
        /// Vibrates the device.
        /// </summary>
        Vibrate = 2,
        /// <summary>
        /// Displays the user notification dialog box.
        /// </summary>
        Dialog = 4,
        /// <summary>
        /// Plays the sound specified.
        /// </summary>
        Sound = 8,
        /// <summary>
        /// Repeats the sound for 10–15 seconds.
        /// </summary>
        Repeat = 16,
        /// <summary>
        /// Dialog box z-order flag.
        /// Set if the notification dialog box should come up behind the password.
        /// </summary>
        Private = 32,
    }
}
