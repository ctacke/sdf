using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>
    /// Strings passed on the command line when an event occurs that the app has requested via CeRunAppAtEvent.  
    /// </summary>
    /// <remarks>Note that some of these strings will be used as the command line *prefix*, since the rest of the command line will be used as a parameter.</remarks>
    public enum NotificationCommandLine
    {
        /// <summary>
        /// String passed on the command line when an app is run as the result of a call to <see cref="M:OpenNETCF.Win32.Notify.Notify.RunAppAtTime"/>.
        /// </summary>
        AppRunAtTime,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
        /// </summary>
        AppRunToHandleNotification,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the system time settings are changed.
        /// </summary>
        AppRunAfterTimeChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after synchronisation.
        /// </summary>
        AppRunAfterSync,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device is connected to AC power.
        /// </summary>
        AppRunAtAcPowerOn,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the AC power is disconnected.
        /// </summary>
        AppRunAtAcPowerOff,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device connects to a LAN.
        /// </summary>
        AppRunAtNetConnect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device disconnects from a LAN.
        /// </summary>
        AppRunAtNetDisconnect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
        /// </summary>
        AppRunAtDeviceChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when another device is discovered using IR.
        /// </summary>
        AppRunAtIrDiscovery,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when a serial port connection is attempted.
        /// </summary>
        AppRunAtRs232Detect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after a system restore.
        /// </summary>
        AppRunAfterRestore,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device wakes up from standby.
        /// </summary>
        AppRunAfterWakeup,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device time-zone is changed.
        /// </summary>
        AppRunAfterTzChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after an extended event.
        /// </summary>
        AppRunAfterExtendedEvent,
    }
}
