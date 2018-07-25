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

      m_invokerControl.Disposed += new EventHandler(OnDispose);
    }

    private static void OnDispose(object sender, EventArgs e)
    {
      // static classes can't have finalizers, but if the m_invokerControl is being disposed, it's safe to assume that the PowerManagement 
      // class instance has been destroyed, probably by application exit, and we need to clean house to prevent worker threads from using
      // any disposed resources (like m_invokerControl) and to just be a general good citizen with the notification subsystem.

      NativeMethods.StopPowerNotifications(m_powerQueue.Handle);
      m_powerQueue.Close();
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

        if (!NativeMethods.KernelIoControl(NativeMethods.IOCTL_HAL_REBOOT, IntPtr.Zero, 0, IntPtr.Zero, 0, ref returned))
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
      try
      {
        PowerBroadcast pb = new PowerBroadcast(1024);
        if (((P2PMessageQueue)sender).Receive(pb, 1000) == ReadWriteResult.OK)
        {
          // Is this an event we wanted?
          if ((pb.Message & PowerEventType.PBT_TRANSITION) > 0)
          {
            // pass events back to the creator thread
            if ((pb.Flags & PowerStateFlags.On) > 0)
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
      catch (System.ObjectDisposedException)
      {
        // this might get thrown if the PowerManagement class gets disposed and we get a power event before the callback queue gets disposed
        // the finalizer has been updated to try to address this situation and never get here, but this handler has been added just in case
      }
    }
  } // public static class PowerManagement
}
