using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// This class provides power-related functions for CE-based devices
    /// </summary>
    public static class PowerManagement
    {
        private delegate void DeviceNotificationInvoker(PowerStateFlags state);

        private static System.Windows.Forms.Control m_invokerControl = new System.Windows.Forms.Control();
        private static DeviceNotificationInvoker m_invokeProc = new DeviceNotificationInvoker(InvokerProc);

        private static void InvokerProc(PowerStateFlags state)
        {
            DeviceNotification notification = null;

            switch (state)
            {
                case PowerStateFlags.On:
                    notification = PowerUp;
                    break;
                case PowerStateFlags.Suspend:
                    notification = PowerDown;
                    break;
                case PowerStateFlags.Boot:
                    notification = Boot;
                    break;
                case PowerStateFlags.CriticalOff:
                    notification = PowerCritical;
                    break;
                case PowerStateFlags.Idle:
                    notification = PowerIdle;
                    break;
                default:
                    break;
            }

            // if we've got an event handler wired, signal it
            if (notification != null)
            {
                foreach (DeviceNotification dn in notification.GetInvocationList())
                {
                    dn();
                }
            }
        }

        /// <summary>
        /// Fired when the device receives a notification that it has been powered up/wakes
        /// </summary>
        public static event DeviceNotification PowerUp;

        /// <summary>
        /// Fired when the device receives a notification that it has been powered down/suspends
        /// </summary>
        public static event DeviceNotification PowerDown;

        /// <summary>
        /// Fired when the device receives a notification that it has booted
        /// </summary>
        public static event DeviceNotification Boot;

        /// <summary>
        /// Fired when the device receives a notification that a critical power state has been reached
        /// </summary>
        public static event DeviceNotification PowerCritical;

        /// <summary>
        /// Fired when the device receives a notification that it has entered an idle power state
        /// </summary>
        public static event DeviceNotification PowerIdle;
        
        private static P2PMessageQueue m_powerQueue;

        static PowerManagement()
        {
            try
            {
                // Create message queue for power broadcasts
                m_powerQueue = new P2PMessageQueue(true, "PowerEventWaitQueue");
            }
            catch (MissingMethodException)
            {
                // power manager not supported on this platform
                // don't create the thread
                return;
            }
            m_powerQueue.DataOnQueueChanged += new EventHandler(m_powerQueue_DataOnQueueChanged);
            IntPtr hNotif = NativeMethods.RequestPowerNotifications(m_powerQueue.Handle, PowerEventType.PBT_POWERSTATUSCHANGE | PowerEventType.PBT_RESUME | PowerEventType.PBT_TRANSITION);

        }

        /// <summary>
        /// Soft resets the device
        /// </summary>
        public static void SoftReset()
        {
            int ret = NativeMethods.SetSystemPowerState(IntPtr.Zero, PowerStateFlags.Reset, NativeMethods.POWER_FORCE);
            if (ret != NativeMethods.ERROR_SUCCESS)
            {
                int returned = 0;

                if(! NativeMethods.KernelIoControl(NativeMethods.IOCTL_HAL_REBOOT, IntPtr.Zero, 0, IntPtr.Zero, 0, ref returned))
                {
                    throw new Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                }
            }
        }

        /// <summary>
        /// Hard-resets the device.  <b>Use with caution.</b>
        /// </summary>
        public static void HardReset()
        {
            NativeMethods.SetCleanRebootFlag();

            SoftReset();
        }

        /// <summary>
        /// Suspends the device (puts it into sleep mode)
        /// </summary>
        public static void Suspend()
        {
            int ret = NativeMethods.SetSystemPowerState(IntPtr.Zero, PowerStateFlags.Suspend, NativeMethods.POWER_FORCE); 
            if (ret != NativeMethods.ERROR_SUCCESS)
            {
                // try GwesPowerOff
                NativeMethods.GwesPowerOff();
            }
        }

        /// <summary>
        /// Forces the device into Idle state.  For many devices, this will power down the backlight.
        /// </summary>
        public static void SetIdleState()
        {
            int ret = NativeMethods.SetSystemPowerState(IntPtr.Zero, PowerStateFlags.Idle, NativeMethods.POWER_FORCE);
            if (ret != NativeMethods.ERROR_SUCCESS)
            {
                throw new Win32Exception(ret);
            }
        }

        /// <summary>
        /// Resets the system inactivity timer, preventing it from entering Idle state
        /// </summary>
        public static void ResetSystemIdleTimer()
        {
            NativeMethods.SystemIdleTimerReset();
        }

        static void m_powerQueue_DataOnQueueChanged(object sender, EventArgs e)
        {
            PowerBroadcast pb = new PowerBroadcast(1024);
            if (((P2PMessageQueue)sender).Receive(pb, 1000) == ReadWriteResult.OK)
            {
                // Is this an event we wanted?
                if((pb.Message & PowerEventType.PBT_TRANSITION) > 0)
                {
                    // pass events back to the creator thread
                    if((pb.Flags & PowerStateFlags.On) > 0)
                    {
                        m_invokerControl.Invoke(m_invokeProc, PowerStateFlags.On);
                    }
                    if ((pb.Flags & PowerStateFlags.Suspend) > 0)
                    {
                        m_invokerControl.Invoke(m_invokeProc, PowerStateFlags.Suspend);
                    }
                    if ((pb.Flags & PowerStateFlags.Boot) > 0)
                    {
                        m_invokerControl.Invoke(m_invokeProc, PowerStateFlags.Boot);
                    }
                    if ((pb.Flags & PowerStateFlags.CriticalOff) > 0)
                    {
                        m_invokerControl.Invoke(m_invokeProc, PowerStateFlags.CriticalOff);
                    }
                    if ((pb.Flags & PowerStateFlags.Idle) > 0)
                    {
                        m_invokerControl.Invoke(m_invokeProc, PowerStateFlags.Idle);
                    }
                } // if(( pb.Message...
            } // if(((P2PMessageQueue)sender)Receive...
        }
    } // public static class PowerManagement
}
