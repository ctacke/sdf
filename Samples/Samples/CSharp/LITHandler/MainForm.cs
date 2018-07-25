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
using System.Threading;
using System.Diagnostics;

namespace LITHandler
{
    public partial class MainForm : Form
    {
        private LargeIntervalTimer m_timer = new LargeIntervalTimer();

        public MainForm()
        {
            InitializeComponent();
            start.Click += new EventHandler(start_Click);
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (m_timer.Enabled)
            {
                start.Text = "Enable Timer";

                // stop the timer
                m_timer.Enabled = false;
            }
            else
            {
                int first = int.Parse(firstTick.Text);
                int interval = int.Parse(tickInterval.Text);

                start.Text = "Disable Timer";

                // update a label to show when we started
                started.Text = DateTime.Now.ToString("HH:mm:ss");

                // set the timer's properties
                m_timer.OneShot = false;                                //run forever
                m_timer.FirstEventTime = DateTime.Now.AddSeconds(first);   // start in 15 seconds
                m_timer.Interval = new TimeSpan(0, 0, interval);              // fire every 30 seconds after that

                m_timer.Tick += new EventHandler(OnTick);

                // start the timer
                m_timer.Enabled = true;
            }
        }

        void OnTick(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(OnTick), new object[] { sender, e });
                return;
            }

            // update a label to show we've ticked
            lastTick.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}