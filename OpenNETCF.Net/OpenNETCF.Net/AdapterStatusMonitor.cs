#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using OpenNETCF.IO;
using OpenNETCF.Threading;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Class giving the ability to monitor NDIS adapters for changes
	/// in their state.  When a change is detected, an event is fired
	/// to all interested parties.  The parameters of the event indicate
	/// the type of status change and the name of the adapter which
	/// changed.
	/// </summary>
	public class AdapterStatusMonitor
	{
        private P2PMessageQueue p2pmq = null;
        private IntPtr ndisAccess = IntPtr.Zero;

		public static AdapterStatusMonitor NDISMonitor = new AdapterStatusMonitor();

        /// <summary>
        /// Event fired when some aspect of the adapter's configuration
        /// or state has changed.
        /// </summary>
        // FIX: Typo: 'Adapater' classes (Bug #10)
        public event AdapterNotificationEventHandler AdapterNotification;

		private AdapterStatusMonitor()
		{
		}

        /// <summary>
        /// Destructor
        /// </summary>
		~AdapterStatusMonitor()
		{
			this.StopStatusMonitoring();
		}

		/// <summary>
		/// Initiates a worker thread to listen for NDIS-reported
		/// changes to the status of the adapter.  Listeners can
		/// register for notification of these changes, which the
		/// thread will send.
		/// </summary>
		public void StartStatusMonitoring()
		{
			if ( !Active )
			{
				// Create a point-to-point message queue to get the notifications.
                p2pmq = new P2PMessageQueue(true, null, NDISUIO_DEVICE_NOTIFICATION.Size, 25);
                p2pmq.DataOnQueueChanged += new EventHandler(p2pmq_DataOnQueueChanged);

                // Ask NDISUIO to send us notification messages for our adapter.
                ndisAccess = FileHelper.CreateFile(
                            NDISUIOPInvokes.NDISUIO_DEVICE_NAME,
                            FileAccess.ReadWrite,
                            FileShare.None,
                            FileCreateDisposition.OpenExisting,
                            NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED);
                if ((int)ndisAccess == FileHelper.InvalidHandle)
                    return;

                NDISUIO_REQUEST_NOTIFICATION ndisRequestNotification =
                    new NDISUIO_REQUEST_NOTIFICATION();
                ndisRequestNotification.hMsgQueue = p2pmq.Handle;
                ndisRequestNotification.dwNotificationTypes =
                    NDISUIOPInvokes.NDISUIO_NOTIFICATION_MEDIA_SPECIFIC_NOTIFICATION |
                    NDISUIOPInvokes.NDISUIO_NOTIFICATION_MEDIA_CONNECT |
                    NDISUIOPInvokes.NDISUIO_NOTIFICATION_MEDIA_DISCONNECT |
                    NDISUIOPInvokes.NDISUIO_NOTIFICATION_BIND |
                    NDISUIOPInvokes.NDISUIO_NOTIFICATION_UNBIND;

                UInt32 xcount = 0;
                if (!NDISUIOPInvokes.DeviceIoControl(ndisAccess,
                    NDISUIOPInvokes.IOCTL_NDISUIO_REQUEST_NOTIFICATION,
                    ndisRequestNotification.getBytes(),
                    NDISUIO_REQUEST_NOTIFICATION.Size,
                    null, 0, ref xcount, IntPtr.Zero))
                {
                    System.Diagnostics.Debug.WriteLine(this, "Error in DeviceIoControl to request notifications!");
                }

			}
		}

        void p2pmq_DataOnQueueChanged(object sender, EventArgs e)
        {
            try
            {
                while (p2pmq != null && p2pmq.MessagesInQueueNow > 0)
                {
                    // Each notification will be of this type.
                    NDISUIO_DEVICE_NOTIFICATION ndisDeviceNotification = new NDISUIO_DEVICE_NOTIFICATION();

                    // Read the event data.
                    if (p2pmq.Receive(ndisDeviceNotification, -1) == ReadWriteResult.OK)
                    {
                        // Handle the event.
                        OnAdapterNotification(new AdapterNotificationArgs(
                            ndisDeviceNotification.ptcDeviceName,
                            (NdisNotificationType)ndisDeviceNotification.dwNotificationType));
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
		/// Stops the worker thread which monitors for changes of status
		/// of the adapter.  This must be done, if monitoring has been
		/// started, before the object is destroyed.
		/// </summary>
		public void StopStatusMonitoring()
		{
			if ( Active )
            {

                UInt32 xcount = 0;
                if (!NDISUIOPInvokes.DeviceIoControl(ndisAccess,
                    NDISUIOPInvokes.IOCTL_NDISUIO_CANCEL_NOTIFICATION,
                    null, 0,
                    null, 0, ref xcount, IntPtr.Zero))
                {
                    System.Diagnostics.Debug.WriteLine(this, "Error in DeviceIoControl to stop notifications!");
                    // ????
                }

                // Don't forget to close our handle to NDISUIO.
                FileHelper.CloseHandle(ndisAccess);

                ndisAccess = IntPtr.Zero;

                // Close the point-to-point message queue.
                P2PMessageQueue q = p2pmq;
                p2pmq = null;
                q.Close();
            }
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
				return ( p2pmq != null );
			}
		}

		/// <summary>
		/// Raises the AdapterNotification event.
		/// </summary>
		/// <param name="e">
		/// An EventArgs that contains the event data.
		/// </param>
		protected virtual void OnAdapterNotification(AdapterNotificationArgs e)
		{
			if (AdapterNotification != null)
			{
				AdapterNotification(this, e);
			}
		}
	}
}
