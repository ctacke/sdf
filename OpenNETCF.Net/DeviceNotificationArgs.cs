using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DeviceNotificationEventHandler(object sender, DeviceNotificationArgs e);

    /// <summary>
    /// DeviceNotificationArgs passed to interested parties
    /// when a device notification is fired.  Contains the
    /// device class GUID, a flag indicating whether the
    /// device is attached or detached, and the device name.
    /// </summary>
    public class DeviceNotificationArgs : System.EventArgs
    {
        /// <summary>
        /// GUID of the interface/type/class of the device
        /// being attached or detached.
        /// </summary>
        public Guid DeviceInterfaceGUID;

        /// <summary>
        /// True if, after the latest event, the device is
        /// connected; false, otherwise.
        /// </summary>
        public bool DeviceAttached;

        /// <summary>
        /// The device name being attached/detached.  Might
        /// be a stream driver name like COM1:, or something
        /// more descriptive like Power Manager, depending
        /// on the GUID.
        /// </summary>
        public string DeviceName;

        /// <summary>
        /// Constructor for notification arguments.
        /// </summary>
        /// <param name="g">Device class GUID</param>
        /// <param name="att">Device attached, true or false</param>
        /// <param name="nam">Device name</param>
        public DeviceNotificationArgs(Guid g,
            bool att, string nam)
        {
            DeviceInterfaceGUID = g;
            DeviceAttached = att;
            DeviceName = nam;
        }
    }
}
