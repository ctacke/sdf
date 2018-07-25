using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Timers
{
    /// <summary>
    /// Provides data for the Elapsed event.
    /// </summary>
    public class ElapsedEventArgs : EventArgs
    {
        DateTime m_signalTime;
        internal ElapsedEventArgs(DateTime signalTime)
        {
            m_signalTime = signalTime;
        }

        /// <summary>
        /// Gets the time the Elapsed event was raised.
        /// </summary>
        public DateTime SignalTime 
        {
            get { return m_signalTime; }
        }

    }
}
