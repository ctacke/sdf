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