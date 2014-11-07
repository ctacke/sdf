using System;
using System.Runtime.InteropServices;
using System.Threading;

using OpenNETCF.IO;
using OpenNETCF.WindowsCE.Messaging;
using System.Reflection;

namespace OpenNETCF.WindowsCE
{
  /// <summary>
  /// Class for receiving device notifications of all sorts (storage card
  /// insertions/removals, etc.)  When a change is detected, an event is 
  /// fired to all interested parties.  The parameters of the event 
  /// indicate the GUID of the device interface that changed, whether the
  /// device is now connected or disconnected from the system, and the
  /// name of the device (COM1:, for example), which changed.
  /// </summary>
  public class DeviceStatusMonitor : IDisposable
  {
    private Guid m_deviceClass;
    private bool m_fAll = false;
    private P2PMessageQueue m_p2pmq = null;
    private IntPtr m_requestHandle = IntPtr.Zero;

    /// <summary>
    /// Event fired when some aspect of the device's connected status
    /// has changed.
    /// </summary>
    public event DeviceNotificationEventHandler DeviceNotification;

    /// <summary>
    /// Constructor for DeviceStatusMonitor.  Specifies
    /// the class of notifications desired and whether
    /// notifications should be fired for already-attached
    /// devices.
    /// </summary>
    /// <param name="devclass">
    /// GUID of device class to monitor (or Guid.Empty to 
    /// monitor *all* device notifications).
    /// </param>
    /// <param name="notifyAlreadyConnectedDevices">
    /// Set to true to receive notifiations for devices
    /// which were attached before we started monitoring;
    /// set to false to see new events only.
    /// </param>
    public DeviceStatusMonitor(Guid devclass, bool notifyAlreadyConnectedDevices)
    {
      m_deviceClass = devclass;
      m_fAll = notifyAlreadyConnectedDevices;
    }

    /// <summary>
    /// Constructor for DeviceStatusMonitor.  Specifies
    /// the class of notifications desired and whether
    /// notifications should be fired for already-attached
    /// devices.
    /// </summary>
    /// <param name="deviceClass">
    /// Device class to monitor.
    /// </param>
    /// <param name="notifyAlreadyConnectedDevices">
    /// Set to true to receive notifiations for devices
    /// which were attached before we started monitoring;
    /// set to false to see new events only.
    /// </param>
    public DeviceStatusMonitor(DeviceClass deviceClass, bool notifyAlreadyConnectedDevices)
    {
      object[] attribs = deviceClass.GetType().GetField(deviceClass.ToString()).GetCustomAttributes(typeof(DeviceClassAtrribute), false);
      if ((attribs == null) || (attribs.Length != 1)) throw new ArgumentException("Unknown Device Class");

      DeviceClassAtrribute dca = attribs[0] as DeviceClassAtrribute;
      m_deviceClass = dca.Guid;
      m_fAll = notifyAlreadyConnectedDevices;
    }

    /// <summary>
    /// Destructor stops status monitoring.
    /// </summary>
    ~DeviceStatusMonitor()
    {
      Dispose(false);
    }

    /// <summary>
    /// The Active property is true when the status is
    /// being monitored.  If status monitoring is not
    /// occurring, Active is false.
    /// </summary>
    public bool Active
    {
      get
      {
        return (m_p2pmq != null);
      }
    }

    /// <summary>
    /// Stops the worker thread which monitors for changes of status
    /// of the adapter.  This must be done, if monitoring has been
    /// started, before the object is destroyed.
    /// </summary>
    public void StopStatusMonitoring()
    {
      if (Active)
      {
        // Close the point-to-point message queue.
        P2PMessageQueue q = m_p2pmq;
        m_p2pmq = null;
        q.Close();

        // Stop notifications to us.
        StopDeviceNotifications(m_requestHandle);

        m_requestHandle = IntPtr.Zero;
      }
    }

    /// <summary>
    /// Initiates a worker thread to listen for reports of device
    /// changes.  Listeners can register for notification of these 
    /// changes, which the thread will send.
    /// </summary>
    public void StartStatusMonitoring()
    {
      if (!Active)
      {
        // Create a point-to-point message queue to get the notifications.
        m_p2pmq = new P2PMessageQueue(true);
        m_p2pmq.DataOnQueueChanged += new EventHandler(p2pmq_DataOnQueueChanged);

        // Ask the system to notify our message queue when devices of
        // the indicated class are added or removed.
        m_requestHandle = RequestDeviceNotifications(m_deviceClass.ToByteArray(), m_p2pmq.Handle, m_fAll);
      }
    }

    void p2pmq_DataOnQueueChanged(object sender, EventArgs e)
    {
      try
      {
        // Read the event data.
        while (m_p2pmq != null && m_p2pmq.MessagesInQueueNow > 0)
        {
          DeviceDetail devDetail = new DeviceDetail();
          if (m_p2pmq.Receive(devDetail, 0) == ReadWriteResult.OK)
          {
            // Handle the event.
            OnDeviceNotification(new DeviceNotificationArgs(devDetail.guidDevClass, devDetail.fAttached, devDetail.szName));
          }
          else
          {
            this.StopStatusMonitoring();
          }
        }
      }
      catch (ApplicationException)
      {
        this.StopStatusMonitoring();
      }
    }

    /// <summary>
    /// Raises the DeviceNotification event.
    /// </summary>
    /// <param name="e">
    /// An EventArgs that contains the event data.
    /// </param>
    protected virtual void OnDeviceNotification(DeviceNotificationArgs e)
    {
      if (DeviceNotification != null)
      {
        DeviceNotification(this, e);
      }
    }

    /// <summary>
    /// Returns <b>true</b> if the instance has been disposed, otherwise <b>false</b>.
    /// </summary>
    public bool IsDisposed { get; private set; }

    protected virtual void Dispose(bool disposing)
    {
      if (!IsDisposed)
      {
        this.StopStatusMonitoring();
      }
      IsDisposed = true;
    }

    /// <summary>
    /// Disposes resources used by the class instance
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    // Global constant devclass values for use with the monitor.
    #region ---------- Notification GUIDs -----------
    // kept for backward compat
    internal const string _BLOCK_DRIVER_GUID = "{A4E7EDDA-E575-4252-9D6B-4195D48BB865}";
    internal const string _STORE_MOUNT_GUID = "{C1115848-46FD-4976-BDE9-D79448457004}";
    internal const string _FATFS_MOUNT_GUID = "{169E1941-04CE-4690-97AC-776187EB67CC}";
    internal const string _CDFS_MOUNT_GUID = "{72D75746-D54A-4487-B7A1-940C9A3F259A}";
    internal const string _UDFS_MOUNT_GUID = "{462FEDA9-D478-4b00-86BB-51A8E3D10890}";
    internal const string _CDDA_MOUNT_GUID = "{BA6B1343-7980-4d0c-9290-762D527B33AB}";

    /// <summary>
    /// Fired when the block driver for a Storage Manager
    /// device is loaded.
    /// </summary>
    public static Guid BLOCK_DRIVER_GUID = new Guid(_BLOCK_DRIVER_GUID);

    /// <summary>
    /// Fired when the store, managed by the Storage
    /// Manager is ready.
    /// </summary>
    public static Guid STORE_MOUNT_GUID = new Guid(_STORE_MOUNT_GUID);

    /// <summary>
    /// Fired when a FAT filesystem is loaded for a device.
    /// </summary>
    public static Guid FATFS_MOUNT_GUID = new Guid(_FATFS_MOUNT_GUID);

    /// <summary>
    /// Fired when a CDFS filesystem is loaded.
    /// </summary>
    public static Guid CDFS_MOUNT_GUID = new Guid(_CDFS_MOUNT_GUID);

    /// <summary>
    /// Fired when a UDFS filesystem is loaded.
    /// </summary>
    public static Guid UDFS_MOUNT_GUID = new Guid(_UDFS_MOUNT_GUID);

    /// <summary>
    /// Fired when a CDDA filesystem is loaded.
    /// </summary>
    public static Guid CDDA_MOUNT_GUID = new Guid(_CDDA_MOUNT_GUID);

    #endregion

    #region P/Invoke declarations.
    [DllImport("coredll.dll", SetLastError = true)]
    private static extern IntPtr RequestDeviceNotifications(
      byte[] devclass, IntPtr hMsgQ, bool fAll);

    [DllImport("coredll.dll", SetLastError = true)]
    private static extern bool StopDeviceNotifications(IntPtr h);
    #endregion
  }
}
