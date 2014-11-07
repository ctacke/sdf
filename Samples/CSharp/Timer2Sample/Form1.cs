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