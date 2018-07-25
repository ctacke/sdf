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
using OpenNETCF.WindowsCE;

namespace SystemEventWatcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void deviceWake_CheckStateChanged(object sender, EventArgs e)
        {
            if(deviceWake.Checked)
            {
                DeviceManagement.DeviceWake += OnWake;
            }
            else
            {
                DeviceManagement.DeviceWake -= OnWake;
            }
        }

        private void deviceTime_CheckStateChanged(object sender, EventArgs e)
        {
            if (deviceTime.Checked)
            {
                DeviceManagement.TimeChanged += OnTimeChanged;
                DeviceManagement.TimeZoneChanged += OnTZChanged;
            }
            else
            {
                DeviceManagement.TimeChanged -= OnTimeChanged;
                DeviceManagement.TimeZoneChanged -= OnTZChanged;
            }
        }

        private void deviceNetwork_CheckStateChanged(object sender, EventArgs e)
        {
            if (deviceNetwork.Checked)
            {
                DeviceManagement.NetworkConnected += OnNetConnect;
                DeviceManagement.NetworkDisconnected += OnNetDisconnect;
                DeviceManagement.RNDISDeviceDetected += OnRNDIS;
            }
            else
            {
                DeviceManagement.NetworkConnected -= OnNetConnect;
                DeviceManagement.NetworkDisconnected -= OnNetDisconnect;
                DeviceManagement.RNDISDeviceDetected -= OnRNDIS;
            }
        }

        private void devicePower_CheckStateChanged(object sender, EventArgs e)
        {
            if (devicePower.Checked)
            {
                DeviceManagement.ACPowerApplied += OnACApplied;
                DeviceManagement.ACPowerApplied += OnACRemoved;
            }
            else
            {
                DeviceManagement.ACPowerApplied -= OnACApplied;
                DeviceManagement.ACPowerApplied -= OnACRemoved;
            }
        }

        private void deviceSerial_CheckStateChanged(object sender, EventArgs e)
        {
            if (deviceSerial.Checked)
            {
                DeviceManagement.SerialDeviceDetected += OnSerialDevice;
            }
            else
            {
                DeviceManagement.SerialDeviceDetected -= OnSerialDevice;
            }
        }

        private void powerBoot_CheckStateChanged(object sender, EventArgs e)
        {
            if (powerBoot.Checked)
            {
                PowerManagement.Boot += OnBoot;
            }
            else
            {
                PowerManagement.Boot -= OnBoot;
            }
        }

        private void powerUp_CheckStateChanged(object sender, EventArgs e)
        {
            if (powerUp.Checked)
            {
                PowerManagement.PowerUp += OnPowerUp;
            }
            else
            {
                PowerManagement.PowerUp -= OnPowerUp;
            }
        }

        private void powerDown_CheckStateChanged(object sender, EventArgs e)
        {
            if (powerDown.Checked)
            {
                PowerManagement.PowerDown += OnPowerDown;
            }
            else
            {
                PowerManagement.PowerDown -= OnPowerDown;
            }
        }

        private void powerCritical_CheckStateChanged(object sender, EventArgs e)
        {
            if (powerCritical.Checked)
            {
                PowerManagement.PowerCritical += OnPowerCritical;
            }
            else
            {
                PowerManagement.PowerCritical -= OnPowerCritical;
            }
        }

        private void powerIdle_CheckStateChanged(object sender, EventArgs e)
        {
            if (powerIdle.Checked)
            {
                PowerManagement.PowerIdle += OnPowerIdle;
            }
            else
            {
                PowerManagement.PowerIdle -= OnPowerIdle;
            }
        }

        void StateChange(string state)
        {
            string msg = string.Format("{0} : {1}", DateTime.Now.ToString("HH:mm:ss"), state);
            eventList.Items.Insert(0, msg);
        }

        void OnBoot() { StateChange("Power - Boot"); }
        void OnPowerUp() { StateChange("Power - Up"); }
        void OnPowerDown() { StateChange("Power - Down"); }
        void OnPowerIdle() { StateChange("Power - Idle"); }
        void OnPowerCritical() { StateChange("Power - Critical"); }

        void OnSerialDevice() { StateChange("Serial Connection detected"); }
        void OnACApplied() { StateChange("AC Power applied"); }
        void OnACRemoved() { StateChange("AC Power removed"); }
        void OnNetConnect() { StateChange("Network Connected"); }
        void OnNetDisconnect() { StateChange("Network Disconnected"); }
        void OnRNDIS() { StateChange("RNDIS device detected"); }
        void OnTZChanged() { StateChange("Timezone Changed"); }
        void OnTimeChanged() { StateChange("Time Changed"); }
        void OnWake() { StateChange("Device Wake"); }
    }
}