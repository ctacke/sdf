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