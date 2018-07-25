using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OpenNETCF.Timers
{
    /// <summary>
    /// Parent for any component containing a Timer2 class
    /// </summary>
    public abstract class ISynchronizationInvoker : Control
    {
        /// <summary>
        /// Delegate on which timer events will be marshaled to the UI thread
        /// </summary>
        public ElapsedEventHandler TimerElapsed;
    }
}
