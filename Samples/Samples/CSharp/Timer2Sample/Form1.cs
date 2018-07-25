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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Timers;

namespace Timer2Sample
{
    public partial class Form1 : Form
    {
        Timer2 m_timer;

        public Form1()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (m_timer == null)
            {
                if (eventBased.Checked)
                {
                    m_timer = new Timer2();
                    m_timer.Elapsed += new ElapsedEventHandler(m_timer_Elapsed);
                }
                else
                {
                    m_timer = new Timer2Subclass();
                    m_timer.UseCallback = true;
                }

                m_timer.SynchronizingObject = status;
                m_timer.Interval = int.Parse(interval.Text);
                m_timer.Resolution = int.Parse(precision.Text);
                m_timer.AutoReset = true;
                m_timer.Start();

                start.Text = "Stop";
            }
            else
            {
                m_timer.Stop();
                m_timer.Dispose();
                m_timer = null;
                start.Text = "Start";
            }
        }

        void m_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            status.Text = "Timer fired at " + DateTime.Now.ToString("HH:mm:ss");
        }
    }

    internal class Timer2Subclass : Timer2
    {
        public bool CallbackHasBeenCalled { get; set; }

        public Timer2Subclass()
            : base()
        {
        }

        public override void TimerCallback()
        {
            SynchronizingObject.Text = "Callback called at " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}