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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using OpenNETCF.Threading;
using System.Windows.Forms;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Delegate for device notification events
    /// </summary>
    public delegate void DeviceNotification();

    /// <summary>
    /// This class provides access to common device management functionality
    /// </summary>
    public static class DeviceManagement
    {
        /// <summary>
        /// Fired when the device time is changed via the UI or API
        /// </summary>
        public static event DeviceNotification TimeChanged;
        /// <summary>
        /// Fired when the device timezone is changed via the UI or API
        /// </summary>
        public static event DeviceNotification TimeZoneChanged;
        /// <summary>
        /// Fired when a PCCard in the system is added or removed
        /// </summary>
        public static event DeviceNotification PCCardChanged;
        /// <summary>
        /// Fired when a IrDA device is discovered
        /// </summary>
        public static event DeviceNotification IRDiscovered;
        /// <summary>
        /// Fired when a network is connected
        /// </summary>
        public static event DeviceNotification NetworkConnected;
        /// <summary>
        /// Fired when a network is disconnected
        /// </summary>
        public static event DeviceNotification NetworkDisconnected;
        /// <summary>
        /// Fired when AC power is applied
        /// </summary>
        public static event DeviceNotification ACPowerApplied;
        /// <summary>
        /// Fired when ACPower is removed
        /// </summary>
        public static event DeviceNotification ACPowerRemoved;
        /// <summary>
        /// Fired when a device restore has been completed
        /// </summary>
        public static event DeviceNotification DeviceRestoreComplete;
        /// <summary>
        /// Fired when a serial connection is detected (RS232 or USB)
        /// </summary>
        public static event DeviceNotification SerialDeviceDetected;
        /// <summary>
        /// Fired when ActiveSync synchonization is complete
        /// </summary>
        public static event DeviceNotification SynchronizationComplete;
        /// <summary>
        /// Fired when the device wakes from sleep
        /// </summary>
        public static event DeviceNotification DeviceWake;
        /// <summary>
        /// Fired when the device's name is changed
        /// </summary>
        public static event DeviceNotification DeviceNameChange;
        /// <summary>
        /// Fired when an RNDIS device is detected
        /// </summary>
        public static event DeviceNotification RNDISDeviceDetected;
        /// <summary>
        /// Fired when the devices internet proxy is changed
        /// </summary>
        public static event DeviceNotification InternetProxyChange;

        private const int TIME_CHANGE_INDEX = 0;
        private const int TIMEZONE_CHANGE_INDEX = 1;
        private const int PCCARD_CHANGE_INDEX = 2;
        private const int IR_DISCOVERED_INDEX = 3;
        private const int NET_CONNECT_INDEX = 4;
        private const int NET_DISCONNECT_INDEX = 5;
        private const int AC_APPLIED_INDEX = 6;
        private const int AC_REMOVED_INDEX = 7;
        private const int RESTORE_INDEX = 8;
        private const int SERIAL_DETECT_INDEX = 9;
        private const int SYNC_COMPLETE_INDEX = 10;
        private const int WAKE_INDEX = 11;
        private const int NAME_CHANGE_INDEX = 12;
        private const int RNDIS_INDEX = 13;
        private const int PROXY_CHANGE_INDEX = 14;

        private const int EVENT_COUNT = 15;

        // we use this to get events back to the primary thread
        private static Control m_invoker = new Control();

        private static DeviceNotification m_currentEvent;

        private static Thread m_notificationThread;
        internal static bool m_killThread = false;
        
        // this is needed to ensure we get a finalizer to clean up our events
        private static DeviceManagementInternals m_internals;

        private const string NOTIFICATION_EVENT_PREFIX = "\\\\.\\Notifications\\NamedEvents\\";
        private const string TIME_CHANGE_EVENT = "SDF_TimeChange";
        private const string TIMEZONE_CHANGE_EVENT = "SDF_TimeZoneChange";
        private const string PCCARD_CHANGE_EVENT = "SDF_PCCardChange";
        private const string IR_DISCOVERED_EVENT = "SDF_IRDiscovered";
        private const string NET_CONNECT_EVENT = "SDF_NetConnected";
        private const string NET_DISCONNECT_EVENT = "SDF_NetDisconnected";
        private const string AC_APPLIED_EVENT = "SDF_ACApplied";
        private const string AC_REMOVED_EVENT = "SDF_ACRemoved";
        private const string RESTORE_EVENT = "SDF_Restoreded";
        private const string SERIAL_DETECT_EVENT = "SDF_SerialDetected";
        private const string SYNC_COMPLETE_EVENT = "SDF_SyncCompleted";
        private const string WAKE_EVENT = "SDF_DeviceWake";
        private const string NAME_CHANGE_EVENT = "SDF_NameChanged";
        private const string RNDIS_EVENT = "SDF_RNDIS";
        private const string PROXY_CHANGE_EVENT = "SDF_ProxyChanged";


        internal class DeviceManagementInternals
        {
            internal static EventWaitHandle[] m_events = new EventWaitHandle[EVENT_COUNT];

            internal DeviceManagementInternals()
            {
                // create all the notitification listener events
                m_events[TIME_CHANGE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, TIME_CHANGE_EVENT);

                m_events[TIMEZONE_CHANGE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, TIMEZONE_CHANGE_EVENT);

                m_events[PCCARD_CHANGE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, PCCARD_CHANGE_EVENT);

                m_events[IR_DISCOVERED_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, IR_DISCOVERED_EVENT);

                m_events[NET_CONNECT_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, NET_CONNECT_EVENT);

                m_events[NET_DISCONNECT_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, NET_DISCONNECT_EVENT);

                m_events[AC_APPLIED_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, AC_APPLIED_EVENT);

                m_events[AC_REMOVED_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, AC_REMOVED_EVENT);

                m_events[RESTORE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, RESTORE_EVENT);

                m_events[SERIAL_DETECT_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, SERIAL_DETECT_EVENT);

                m_events[SYNC_COMPLETE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, SYNC_COMPLETE_EVENT);

                m_events[WAKE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, WAKE_EVENT);

                m_events[NAME_CHANGE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, NAME_CHANGE_EVENT);

                m_events[RNDIS_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, RNDIS_EVENT);

                m_events[PROXY_CHANGE_INDEX] = new EventWaitHandle(
                    false, EventResetMode.AutoReset, PROXY_CHANGE_EVENT);

                // set up all of the CeRunAppAtEvent notifications
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + TIME_CHANGE_EVENT, NotificationEvent.TimeChange);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + TIMEZONE_CHANGE_EVENT, NotificationEvent.TimeZoneChange);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + PCCARD_CHANGE_EVENT, NotificationEvent.DeviceChange);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + IR_DISCOVERED_EVENT, NotificationEvent.IRDiscovered);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NET_CONNECT_EVENT, NotificationEvent.NetConnect);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NET_DISCONNECT_EVENT, NotificationEvent.NetDisconnect);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + AC_APPLIED_EVENT, NotificationEvent.OnACPower);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + AC_REMOVED_EVENT, NotificationEvent.OffACPower);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + RESTORE_EVENT, NotificationEvent.RestoreEnd);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + SERIAL_DETECT_EVENT, NotificationEvent.RS232Detected);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + SYNC_COMPLETE_EVENT, NotificationEvent.SyncEnd);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + WAKE_EVENT, NotificationEvent.Wakeup);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NAME_CHANGE_EVENT, NotificationEvent.MachineNameChange);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + RNDIS_EVENT, NotificationEvent.RndisFNDetected);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + PROXY_CHANGE_EVENT, NotificationEvent.InternetProxyChange);

                m_invoker.Disposed += new EventHandler(m_invoker_Disposed);
            }

            void m_invoker_Disposed(object sender, EventArgs e)
            {
                // the invoker control was disposed, which is bad.  Kill the worker thread now
                m_killThread = true;
            }

            ~DeviceManagementInternals()
            {
                // shut down the worker thread
                m_killThread = true;

                foreach (EventWaitHandle ewh in m_events)
                {
                    ewh.Close();
                }

                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + TIME_CHANGE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + TIMEZONE_CHANGE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + PCCARD_CHANGE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + IR_DISCOVERED_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NET_CONNECT_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NET_DISCONNECT_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + AC_APPLIED_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + AC_REMOVED_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + RESTORE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + SERIAL_DETECT_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + SYNC_COMPLETE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + WAKE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + NAME_CHANGE_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + RNDIS_EVENT, NotificationEvent.None);
                NativeMethods.CeRunAppAtEvent(NOTIFICATION_EVENT_PREFIX + PROXY_CHANGE_EVENT, NotificationEvent.None);
            }
        }

        static DeviceManagement()
        {
            // use an internal class instance so we can get a finalizer for a static class
            m_internals = new DeviceManagementInternals();

            // fire up the notification event listener thread
            m_notificationThread = new Thread(new ThreadStart(NotificationListenerProc));
            m_notificationThread.IsBackground = true;
            m_notificationThread.Name = "SDF DeviceManagement Event Notification Thread";

            m_notificationThread.Start();
        }
        
        private static void NotificationListenerProc()
        {
            int eventIndex = 0;

            EventHandler marshaler = new EventHandler(NotificationMarshaler);

            while (! m_killThread)
            {
                // have to have a timeout or this thread never dies, even when the calling app is stopped
                eventIndex = EventWaitHandle.WaitAny(DeviceManagementInternals.m_events, 1000, true);

                switch (eventIndex)
                {
                    case TIME_CHANGE_INDEX:
                        m_currentEvent = TimeChanged;
                        break;
                    case TIMEZONE_CHANGE_INDEX:
                        m_currentEvent = TimeZoneChanged;
                        break;
                    case PCCARD_CHANGE_INDEX:
                        m_currentEvent = PCCardChanged;
                        break;
                    case IR_DISCOVERED_INDEX:
                        m_currentEvent = IRDiscovered;
                        break;
                    case NET_CONNECT_INDEX:
                        m_currentEvent = NetworkConnected;
                        break;
                    case NET_DISCONNECT_INDEX:
                        m_currentEvent = NetworkDisconnected;
                        break;
                    case AC_APPLIED_INDEX:
                        m_currentEvent = ACPowerApplied;
                        break;
                    case AC_REMOVED_INDEX:
                        m_currentEvent = ACPowerRemoved;
                        break;
                    case RESTORE_INDEX:
                        m_currentEvent = DeviceRestoreComplete;
                        break;
                    case SERIAL_DETECT_INDEX:
                        m_currentEvent = SerialDeviceDetected;
                        break;
                    case SYNC_COMPLETE_INDEX:
                        m_currentEvent = SynchronizationComplete;
                        break;
                    case WAKE_INDEX:
                        m_currentEvent = DeviceWake;
                        break;
                    case NAME_CHANGE_INDEX:
                        m_currentEvent = DeviceNameChange;
                        break;
                    case RNDIS_INDEX:
                        m_currentEvent = RNDISDeviceDetected;
                        break;
                    case PROXY_CHANGE_INDEX:
                        m_currentEvent = InternetProxyChange;
                        break;
                    default:
                        // most likely a timeout
                        m_currentEvent = null;
                        break;
                }

                if (m_currentEvent != null)
                {
                    try
                    {
                        m_invoker.Invoke(marshaler);
                    }
                    catch (Exception)
                    {
                        // failure here is catastropic, so just exit the thread
                        return;
                    }
                }
            }
        }

        private static void NotificationMarshaler(object o, EventArgs a)
        {
            if (m_currentEvent != null)
            {
                m_currentEvent();

                m_currentEvent = null;
            }
        }

        /// <summary>
        /// Displays the device's touchpanel calibration screen
        /// </summary>
        public static void ShowCalibrationScreen()
        {
            NativeMethods.TouchCalibrate();
        }

        #region Get Device ID

        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;

        private static byte[] GetRawDeviceID()
        {
            // Initialize the output buffer to the size of a Win32 DEVICE_ID structure
            Int32 dwOutBytes = 0;
            //set an initial buffer size
            Int32 nBuffSize = 256;
            byte[] outbuff = new byte[nBuffSize];

            bool done = false;

            // Set DEVICEID.dwSize to size of buffer.  Some platforms look at
            // this field rather than the nOutBufSize param of KernelIoControl
            // when determining if the buffer is large enough.
            //
            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);

            // Loop until the device ID is retrieved or an error occurs
            while (!done)
            {
                if (NativeMethods.KernelIoControl(NativeMethods.IOCTL_HAL_GET_DEVICEID, null, 0, outbuff,
                        nBuffSize, ref dwOutBytes))
                {
                    done = true;
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    switch (error)
                    {
                        case ERROR_NOT_SUPPORTED:
                            throw new NotSupportedException("IOCTL_HAL_GET_DEVICEID is not supported on this device", new Exception("" + error));

                        case ERROR_INSUFFICIENT_BUFFER:
                            // The buffer wasn't big enough for the data.  The
                            // required size is in the first 4 bytes of the output
                            // buffer (DEVICE_ID.dwSize).
                            nBuffSize = BitConverter.ToInt32(outbuff, 0);
                            outbuff = new byte[nBuffSize];

                            // Set DEVICEID.dwSize to size of buffer.  Some
                            // platforms look at this field rather than the
                            // nOutBufSize param of KernelIoControl when
                            // determining if the buffer is large enough.
                            //
                            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
                            break;

                        default:
                            throw new Exception("Unexpected error: " + error);
                    }
                }
            }

            //return the raw buffer - a DEVICE_ID structure
            return outbuff;
        }

        /// <summary>
        /// Returns a Guid representing the unique idenitifier of the device.
        /// </summary>
        /// <returns></returns>
        public static Guid GetDeviceGuid()
        {
            byte[] outbuff = GetRawDeviceID();

            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4); //	DEVICE_ID.dwPresetIDOffset
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8); // DEVICE_ID.dwPresetSize
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc); // DEVICE_ID.dwPlatformIDOffset
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10); // DEVICE_ID.dwPlatformIDBytes

            byte[] guidbytes = new byte[16];

            Buffer.BlockCopy(outbuff, dwPresetIDOffset + dwPresetIDSize - 10, guidbytes, 0, 10);
            Buffer.BlockCopy(outbuff, dwPlatformIDOffset + dwPlatformIDSize - 6, guidbytes, 10, 6);

            return new Guid(guidbytes);
        }

        /// <summary>
        /// Returns a string containing a unique identifier for the device.
        /// </summary>
        /// <returns>Devices unique ID.</returns>
        public static string GetDeviceID()
        {
            byte[] outbuff = GetRawDeviceID();

            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4); //	DEVICE_ID.dwPresetIDOffset
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8); // DEVICE_ID.dwPresetSize
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc); // DEVICE_ID.dwPlatformIDOffset
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10); // DEVICE_ID.dwPlatformIDBytes
            StringBuilder sb = new StringBuilder();

            for (int i = dwPresetIDOffset; i < dwPresetIDOffset + dwPresetIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            sb.Append("-");
            for (int i = dwPlatformIDOffset; i < dwPlatformIDOffset + dwPlatformIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            return sb.ToString();

        }
        #endregion

        /// <summary>
        /// Returns the SystemInfo for the device
        /// </summary>
        public static SystemInfo SystemInformation
        {
            get
            {
                SystemInfo si = new SystemInfo();

                NativeMethods.GetSystemInfo(out si);

                return si;
            }
        }

        #region Platform Name
        /// <summary>
        /// Returns a string which identifies the device platform.
        /// </summary>
        /// <remarks>Valid values include:-
        /// <list type="bullet">
        /// <item><term>PocketPC</term><description>Pocket PC device or Emulator</description></item>
        /// <item><term>SmartPhone</term><description>Smartphone 2003 Device or Emulator</description></item>
        /// <item><term>CEPC platform</term><description>Windows CE.NET Emulator</description></item></list>
        /// Additional platform types will have other names.
        /// Useful when writing library code targetted at multiple platforms.</remarks>
        public static string PlatformName
        {
            get
            {
                //allocate buffer to receive value
                byte[] buffer = new byte[48];

                //call native function
                if (!NativeMethods.SystemParametersInfo(NativeMethods.SystemParametersInfoAction.GetPlatformType, buffer.Length, buffer, NativeMethods.SystemParametersInfoFlags.None))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Retrieving platform name failed");
                }

                //get string from buffer contents
                string platformname = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

                //trim any trailing null characters
                return platformname.Substring(0, platformname.IndexOf("\0"));
            }
        }
        #endregion

        #region Oem Info
        /// <summary>
        /// Returns OEM specific information from the device. This may include Model number
        /// </summary>
        public static string OemInfo
        {
            get
            {
                //allocate buffer to receive value
                byte[] buffer = new byte[128];

                //call native function
                if (!NativeMethods.SystemParametersInfo(NativeMethods.SystemParametersInfoAction.GetOemInfo, buffer.Length, buffer, NativeMethods.SystemParametersInfoFlags.None))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Retrieving OEM info failed");
                }

                //get string from buffer contents
                string oeminfo = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

                //trim any trailing null characters
                return oeminfo.Substring(0, oeminfo.IndexOf("\0"));

            }
        }
        #endregion

        /// <summary>
        /// This function sets the standard input, output, or error output destination path.
        /// </summary>
        /// <param name="stream">Specifies the standard stream to modify</param>
        /// <param name="path">Specifies the device driver to which the output is sent. For example, "TEL1:" specifies the telnet device driver or "\MyLogFile.txt" for a text file output</param>
        public static void SetStdioPath(StdIoStream stream, string path)
        {
            NativeMethods.SetStdioPath(stream, path);
        }

        /// <summary>
        /// This function retrieves the name of the device driver being used for a standard input, output, or error output operation.
        /// </summary>
        /// <param name="stream">The standard stream of interest</param>
        /// <returns>The current path for the standard stream</returns>
        public static string GetStdioPath(StdIoStream stream)
        {
            StringBuilder sb = new StringBuilder(256);

            NativeMethods.GetStdioPath(stream, sb, sb.Capacity);

            return sb.ToString();
        }

       
    }
}
