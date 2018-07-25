using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsMobile.Forms
{
    /// <summary>
    /// Specifies values representing possible states for an <see cref="VoiceRecorder">Voice Recorder</see> control.
    /// </summary>
    [Flags]
    public enum VoiceRecorderStyles : int
    {
        /// <summary>
        /// The <b>OK/CANCEL</b> is not displayed. 
        /// </summary>
        NoOKCancel = 0x0001,
        /// <summary>
        /// The <b>OK</b> is not displayed. 
        /// </summary>
        NoOK = 0x0008,
        /// <summary>
        /// No <b>RECORD</b> button displayed.
        /// </summary>
        NoRecord = 0x0010,
        /// <summary>
        /// Immediatly play supplied file when launched.
        /// </summary>
        PlayMode = 0x0020,
        /// <summary>
        /// Immediately record when launched.
        /// </summary>
        RecordMode = 0x0080,
        /// <summary>
        /// Dismiss control when stopped.
        /// </summary>
        StopDismiss = 0x0100
    }
}
