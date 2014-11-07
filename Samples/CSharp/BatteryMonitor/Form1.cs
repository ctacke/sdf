using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BatteryMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //OpenNETCF.WindowsCE.DeviceManagement.ACPowerRemoved += new OpenNETCF.WindowsCE.DeviceNotification(DeviceManagement_ACPowerRemoved);
            //OpenNETCF.WindowsCE.DeviceManagement.DeviceWake += new OpenNETCF.WindowsCE.DeviceNotification(DeviceManagement_DeviceWake);
            //OpenNETCF.WindowsCE.DeviceManagement.ACPowerApplied += new OpenNETCF.WindowsCE.DeviceNotification(DeviceManagement_ACPowerApplied);
            //OpenNETCF.WindowsCE.DeviceManagement.TimeChanged += new OpenNETCF.WindowsCE.DeviceNotification(DeviceManagement_TimeChanged);
        }

        void DeviceManagement_TimeChanged()
        {
            MessageBox.Show("time changed");
        }

        void DeviceManagement_ACPowerApplied()
        {
            MessageBox.Show("Power");
        }

        void DeviceManagement_DeviceWake()
        {
            MessageBox.Show("Device wake");
        }

        void DeviceManagement_ACPowerRemoved()
        {
            MessageBox.Show("No Power!");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.batteryMonitor1.PrimaryBatteryLifeTrigger = (int)numericUpDown1.Value;
        }

        private void batteryMonitor1_PrimaryBatteryLifeNotification(object sender, EventArgs e)
        {
            this.menuItem1.Enabled = true;
            this.textBox21.Text = "Plugin now!!";
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.menuItem1.Enabled = false;
            this.batteryMonitor1.Enabled = true;
            this.textBox21.Text = "Monitor Started....";
        }

    }
}