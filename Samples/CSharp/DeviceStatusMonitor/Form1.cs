using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.WindowsCE;
using System.Diagnostics;

namespace DeviceStatusMonitorSample
{
  public partial class Form1 : Form
  {
    DeviceStatusMonitor dsm;

    public Form1()
    {
      dsm = new DeviceStatusMonitor(DeviceClass.Any, true);
      dsm.DeviceNotification += new DeviceNotificationEventHandler(OnDeviceNotification);
      dsm.StartStatusMonitoring();

      InitializeComponent();
    }

    void OnDeviceNotification(object sender, DeviceNotificationArgs e)
    {
      if (eventList.InvokeRequired)
      {
        eventList.Invoke(new DeviceNotificationEventHandler(OnDeviceNotification), new object[] { sender, e });
        return;
      }

      eventList.Items.Insert(0, string.Format("Class '{0}' Name '{1}' {2}", e.DeviceClass.ToString(), e.DeviceName, e.DeviceAttached ? "attached" : "detached"));
    }
  }
}