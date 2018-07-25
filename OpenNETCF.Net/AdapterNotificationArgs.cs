using System;

namespace OpenNETCF.Net
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    [Obsolete("Please use AdapterNotificationEventHandler", true)]
    public delegate void AdapaterNotificationEventHandler(object sender, AdapterNotificationArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    // FIX: Typo: 'Adapater' classes (Bug #10)
    public delegate void AdapterNotificationEventHandler(object sender, AdapterNotificationArgs e);

    /// <summary>
    /// Arguments for Adapter Notifications
    /// </summary>
    public class AdapterNotificationArgs : System.EventArgs
    {
        /// <summary>
        /// Adapter Name
        /// </summary>
        public string AdapterName;
        /// <summary>
        /// Type of Notification
        /// </summary>
        public NdisNotificationType NotificationType;

        public AdapterNotificationArgs(string aName, NdisNotificationType nType)
        {
            AdapterName = aName;
            NotificationType = nType;
        }
    }
}
