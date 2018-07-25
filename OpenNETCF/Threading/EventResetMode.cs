using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Threading
{
    /// <summary>
    /// Indicates whether an <see cref="EventWaitHandle"/> is reset automatically or manually.
    /// </summary>
    public enum EventResetMode
    {
        /// <summary>
        /// When signaled, the <see cref="EventWaitHandle"/> resets automatically after releasing a single thread.
        /// If no threads are waiting, the EventWaitHandle remains signaled until a thread blocks, and resets after releasing the thread.
        /// </summary>
        AutoReset = 0,
        /// <summary>
        /// When signaled, the <see cref="EventWaitHandle"/> releases all waiting threads, and remains signaled until it is manually reset.
        /// </summary>
        ManualReset = 1,
    }
}
