using System;
using System.Runtime.InteropServices;
using System.Threading;

using OpenNETCF.IO;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Class for receiving device notifications of all sorts (storage card
	/// insertions/removals, etc.)  When a change is detected, an event is 
	/// fired to all interested parties.  The parameters of the event 
	/// indicate the GUID of the device interface that changed, whether the
	/// device is now connected or disconnected from the system, and the
	/// name of the device (COM1:, for example), which changed.
	/// </summary>
	public class DeviceStatusMonitor
	{
        private Guid devClass;
        private bool fAll = false;
        private P2PMessageQueue p2pmq = null;
        private IntPtr requestHandle = IntPtr.Zero;

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
		public DeviceStatusMonitor( Guid devclass, bool notifyAlreadyConnectedDevices )
		{
			devClass = devclass;
			fAll = notifyAlreadyConnectedDevices;
		}

		/// <summary>
		/// Destructor stops status monitoring.
		/// </summary>
		~DeviceStatusMonitor()
		{
			this.StopStatusMonitoring();
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
                return (p2pmq != null);
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
                P2PMessageQueue q = p2pmq;
                p2pmq = null;
                q.Close();

                // Stop notifications to us.
                StopDeviceNotifications(requestHandle);

                requestHandle = IntPtr.Zero;
            }
        }

		/// <summary>
		/// Initiates a worker thread to listen for reports of device
		/// changes.  Listeners can register for notification of these 
		/// changes, which the thread will send.
		/// </summary>
		public void StartStatusMonitoring()
		{
			if ( !Active )
			{
				// Create a point-to-point message queue to get the notifications.
                p2pmq = new P2PMessageQueue(true);
                p2pmq.DataOnQueueChanged += new EventHandler(p2pmq_DataOnQueueChanged);

                // Ask the system to notify our message queue when devices of
                // the indicated class are added or removed.
                requestHandle = RequestDeviceNotifications(devClass.ToByteArray(), p2pmq.Handle, fAll);
			}
		}

        void p2pmq_DataOnQueueChanged(object sender, EventArgs e)
        {
            try
            {
                // Read the event data.
                while (p2pmq != null && p2pmq.MessagesInQueueNow > 0)
                {
                    DeviceDetail devDetail = new DeviceDetail();
                    if (p2pmq.Receive(devDetail, 0) == ReadWriteResult.OK)
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

		// Global constant devclass values for use with the monitor.
		#region ---------- Notification GUIDs -----------
		/// <summary>
		/// Fired when the block driver for a Storage Manager
		/// device is loaded.
		/// </summary>
		public static Guid BLOCK_DRIVER_GUID = new Guid( "{A4E7EDDA-E575-4252-9D6B-4195D48BB865}" );

		/// <summary>
		/// Fired when the store, managed by the Storage
		/// Manager is ready.
		/// </summary>
		public static Guid STORE_MOUNT_GUID  = new Guid( "{C1115848-46FD-4976-BDE9-D79448457004}" );

		/// <summary>
		/// Fired when a FAT filesystem is loaded for a device.
		/// </summary>
		public static Guid FATFS_MOUNT_GUID = new Guid( "{169E1941-04CE-4690-97AC-776187EB67CC}" );

		/// <summary>
		/// Fired when a CDFS filesystem is loaded.
		/// </summary>
		public static Guid CDFS_MOUNT_GUID = new Guid( "{72D75746-D54A-4487-B7A1-940C9A3F259A}" );

		/// <summary>
		/// Fired when a UDFS filesystem is loaded.
		/// </summary>
		public static Guid UDFS_MOUNT_GUID = new Guid( "{462FEDA9-D478-4b00-86BB-51A8E3D10890}" );

		/// <summary>
		/// Fired when a CDDA filesystem is loaded.
		/// </summary>
		public static Guid CDDA_MOUNT_GUID = new Guid( "{BA6B1343-7980-4d0c-9290-762D527B33AB}" );
		#endregion

        #region P/Invoke declarations.
        [DllImport ("coredll.dll", SetLastError=true)]
		private static extern IntPtr RequestDeviceNotifications( 
			byte[] devclass, IntPtr hMsgQ, bool fAll );

		[DllImport ("coredll.dll", SetLastError=true)]
		private static extern bool StopDeviceNotifications( IntPtr h );
        #endregion
    }
}
