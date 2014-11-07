using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace OpenNETCF.WindowsCE
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
    public Guid DeviceInterfaceGUID { get; private set; }

    /// <summary>
    /// True if, after the latest event, the device is
    /// connected; false, otherwise.
    /// </summary>
    public bool DeviceAttached { get; private set; }

    /// <summary>
    /// The device name being attached/detached.  Might
    /// be a stream driver name like COM1:, or something
    /// more descriptive like Power Manager, depending
    /// on the GUID.
    /// </summary>
    public string DeviceName { get; private set; }

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

    public DeviceClass DeviceClass
    {
      get
      {
        foreach(FieldInfo fi in typeof(DeviceClass).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField))
        {
          object[] attribs = fi.GetCustomAttributes(typeof(DeviceClassAtrribute), false);
          if (attribs.Length > 0)
          {
            DeviceClassAtrribute dca = attribs[0] as DeviceClassAtrribute;
            if (dca.Guid.CompareTo(DeviceInterfaceGUID) == 0)
            {
              return (DeviceClass)Enum.Parse(typeof(DeviceClass), fi.Name, true);
            }
          }
        }

        Debug.WriteLine(string.Format("Class '{0}' Named '{1}' is unknown.\r\nIf you can define it, please report it to OpenNETCF for inclusion in later builds", this.DeviceInterfaceGUID, this.DeviceName));
        return DeviceClass.Unknown;
      }
    }
  }
}
