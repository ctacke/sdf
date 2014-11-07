using System;
using System.Threading;
using OpenNETCF.Threading;
using OpenNETCF.WindowsCE.Notification;
using System.Diagnostics;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// This class encapsulates a low resolution Timer that is designed to be fired on large intervals (from seconds to even days).
    /// <remarks>Unlike the Forms or Threading Timers, if the device is asleep when the Tick occurs, the device will wake and the Tick handler will run.</remarks>
    /// </summary>
    public class LargeIntervalTimer : IDisposable
    {
        /// <summary>
        /// Raised when the LargeIntervalTimer period has elapsed
        /// </summary>
        public event EventHandler Tick;

        private bool m_enabled = false;
        private EventWaitHandle m_quitHandle;
        private TimeSpan m_interval = new TimeSpan(0, 0, 60);
        private bool m_useFirstTime = false;
        private DateTime m_firstTime = DateTime.MinValue;
        private bool? m_cachedEnabled = null;
        private object m_interlock = new object();
        private bool m_oneShot = false;

        #if INTEGRATION_TEST
                public int ThreadCount = 0;
                public int TestCondition = -1;
        #else
                private int ThreadCount = 0;
                private int TestCondition = -1;
        #endif

                /// <summary>
        /// Creates an instance of a LargeIntervalTimer with a default interval of 60 seconds
        /// </summary>
        public LargeIntervalTimer()
        {
            m_quitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        /// <summary>
        /// Finalizes the LargeIntervalTimer 
        /// </summary>
        ~LargeIntervalTimer()
        {
            Dispose();
        }

        /// <summary>
        /// If set, the Timer will automatically disable itself after each Tick event is raised
        /// </summary>
        public bool OneShot
        {
            set
            {
                lock (m_interlock) { m_oneShot = value; }
            }

            get
            {
                lock (m_interlock) { return m_oneShot; }
            }
        }

        private void InternalThreadProc(object state)
        {
            ThreadCount++;

            int source;
            string eventName = Guid.NewGuid().ToString();
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventName);
            if (TestCondition == 0) Thread.Sleep(1000);

            try
            {
                while (m_enabled)
                {
                    if (TestCondition == 1) Thread.Sleep(1000);

                    if (m_disposing) return;

                    if (TestCondition == 2) Thread.Sleep(1000);

                    if (m_useFirstTime)
                    {
                        Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", eventName), m_firstTime);
                        m_useFirstTime = false;
                    }
                    else
                    {
                        // set up the next event
                        Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", eventName), DateTime.Now.Add(m_interval));
                        m_firstTime = DateTime.MinValue;
                    }

                    if (TestCondition == 3) Thread.Sleep(1000);

                    if (m_disposing) return;
                    source = OpenNETCF.Threading.EventWaitHandle.WaitAny(new WaitHandle[] { waitHandle, m_quitHandle });

                    if (TestCondition == 4) Thread.Sleep(1000);

                    // see if it's the event
                    if (source == 0)
                    {
                        m_cachedEnabled = null;

                        // fire the event if we have a listener
                        if (Tick != null)
                        {
                            // we need to decouple this call from the current thread or the lock will do nothing
                            ThreadPool.QueueUserWorkItem(new WaitCallback(
                                delegate
                                {
                                    Tick(this, null);
                                }));
                        }

                        if (TestCondition == 5) Thread.Sleep(1000);

                        if (OneShot)
                        {
                            if (TestCondition == 6) Thread.Sleep(1000);

                            if (m_cachedEnabled != null)
                            {
                                if (TestCondition == 7) Thread.Sleep(1000);
                                m_enabled = (m_cachedEnabled == true);
                                if (TestCondition == 8) Thread.Sleep(1000);
                            }
                            else
                            {
                                m_enabled = false;
                            }
                        }
                    }
                    else
                    {
                        m_enabled = false;
                    }
                }
            }
            finally
            {
                waitHandle.Close();
                ThreadCount--;
                if (ThreadCount == 0)
                {
                    m_quitHandle.Reset();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the Timer is currently running
        /// </summary>
        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                lock (m_interlock)
                {
                    m_cachedEnabled = value;

                    if ((m_enabled && value) || (!m_enabled && !value))
                    {
                        return;
                    }

                    m_enabled = value;

                    // force any existing waiting threads to exit
                    if(ThreadCount > 0)
                    {
                        m_quitHandle.Set();
                        Thread.Sleep(1);
                    }

                    if (m_enabled)
                    {
                        // start the wait thread
                        ThreadPool.QueueUserWorkItem(InternalThreadProc);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the TimeSpan interval between ticks.
        /// </summary>
        /// <remarks>
        /// Note that the resolution of the underlying notification timer used is not conducive to millisecond accuracy so any sub-second portion of the Interval is ignored.
        /// The Interval also cannot be less than 15 seconds.
        /// </remarks>
        public TimeSpan Interval
        {
            get { return m_interval; }
            set
            {
                lock (m_interlock)
                {
                    if (value.TotalSeconds < 15) throw new ArgumentException("Interval cannot be less than 15 seconds");

                    m_interval = new TimeSpan(value.Days, value.Hours, value.Minutes, value.Seconds);
                }
            }
        }

        /// <summary>
        /// Sets or get the absolute time for the first Tick event.  This is useful when setting up a periodic tick starting at a fixed time
        /// </summary>
        /// <remarks>
        /// The first tick cannot be less than 15 seconds into the future or it will either fire immediately or never fire.
        /// </remarks>
        public DateTime FirstEventTime
        {
            get { return m_firstTime; }
            set
            {
                lock (m_interlock)
                {
                    if (value.CompareTo(DateTime.Now) <= 0)
                    {
                        // set in the past - just disable
                        m_firstTime = DateTime.MinValue;
                        m_useFirstTime = false;
                    }

                    m_firstTime = value;
                    m_useFirstTime = true;
                }
            }
        }

        private bool m_disposing;

        /// <summary>
        /// When overridden in a derived class, releases the unmanaged resources used by the LargeIntervalTimer, and optionally releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            lock (m_interlock)
            {
                m_disposing = true;

                if (Enabled)
                {
                    Enabled = false;
                }

                if (m_quitHandle != null)
                {
                    m_quitHandle.Set();
                    m_quitHandle.Close();
                    m_quitHandle = null;
                }
            }
        }
    }
}
