using System;
using System.Threading;
using OpenNETCF.Threading;
using OpenNETCF.WindowsCE.Notification;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// This class encapsulates a low resolution Timer that is designed to be fired on large intervals (from seconds to even days).
    /// <remarks>Unlike the Forms or Threading Timers, if the device is asleep when the Tick occurs, the device will wake and the Tick handler will run.</remarks>
    /// </summary>
    public class LargeIntervalTimer
    {
        public event EventHandler Tick;

        private bool m_enabled = false;
        private bool m_oneShot = false;
        private EventWaitHandle m_waitHandle;
        private EventWaitHandle m_quitHandle;
        private string m_eventName;
        private TimeSpan m_interval = new TimeSpan(0, 0, 60);
        private bool m_useFirstTime = false;
        private DateTime m_firstTime = DateTime.MinValue;

        public LargeIntervalTimer()
        {
            m_eventName = Guid.NewGuid().ToString();
            m_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, m_eventName);
            m_quitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        ~LargeIntervalTimer()
        {
            m_waitHandle.Close();
            Enabled = false;
        }

        private void InternalThreadProc()
        {
            int source;

            while (true)
            {
                if (m_useFirstTime)
                {
                    Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", m_eventName), m_firstTime);
                    m_useFirstTime = false;
                }
                else
                {
                    // set up the next event
                    Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", m_eventName), DateTime.Now.Add(m_interval));
                    m_firstTime = DateTime.MinValue;
                }

                source = OpenNETCF.Threading.EventWaitHandle.WaitAny(new WaitHandle[] { m_waitHandle, m_quitHandle });

                // see if it's the event
                if (source == 0)
                {
                    // fire the event if we have a listener
                    if (Tick != null)
                    {
                        Tick(this, null);
                    }
                }
                else
                {
                    return;
                }

                if (m_oneShot)
                {
                    m_enabled = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the Timer is currently running
        /// </summary>
        public bool Enabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                if ((m_enabled && value) || (!m_enabled && !value))
                {
                    return;
                }

                m_enabled = value;

                if (m_enabled)
                {
                    // start the wait thread
                    Thread monitorThread = new Thread(new ThreadStart(InternalThreadProc));
                    monitorThread.Start();
                }
                else
                {
                    // terminate the wait thread
                    m_quitHandle.Set();

                    // unregister the event?
                }
            }
        }

        /// <summary>
        /// Gets or sets the TimeSpan interval between ticks.
        /// <remarks>Note that the resolution of the underlying notification timer used is not conducive to millisecond accuracy so any sub-second portion of the Interval is ignored.</remarks>
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                return m_interval;
            }
            set
            {
                m_interval = new TimeSpan(value.Days, value.Hours, value.Minutes, value.Seconds);
            }
        }

        /// <summary>
        /// If set, the Timer will automatically disable itself after each Tick event is raised
        /// </summary>
        public bool OneShot
        {
            set { m_oneShot = value; }
            get { return m_oneShot; }
        }

        /// <summary>
        /// Sets or get the absolute time for the first Tick event.  This is useful when setting up a periodic tick starting at a fixed time
        /// </summary>
        public DateTime FirstEventTime
        {
            set
            {
                if(value.CompareTo(DateTime.Now) <= 0)
                {
                    // set in the past - just disable
                    m_firstTime = DateTime.MinValue;
                    m_useFirstTime = false;
                }

                m_firstTime = value;
                m_useFirstTime = true;
            }
            get
            {
                return m_firstTime;
            }
        }
    }
}
