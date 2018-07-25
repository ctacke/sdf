using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    /// <summary>
    /// The StrengthType enumeration provides a list of 
    /// relative RF Ethernet signal strength values that 
    /// correspond to the strengths displayed by Windows CE
    /// itself.
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.StrengthType instead", false)]
    public enum StrengthType
    {
        /// <summary>
        /// The adapter for which signal strength was requested is not
        /// a wireless network adapter or does not report its signal
        /// strength in the standard way
        /// </summary>
        NotAWirelessAdapter,

        /// <summary>
        /// The adapter is not receiving a network signal
        /// </summary>
        NoSignal,

        /// <summary>
        /// The network signal has very low strength
        /// </summary>
        VeryLow,

        /// <summary>
        /// The network signal has low strength
        /// </summary>
        Low,

        /// <summary>
        /// The network signal is good
        /// </summary>
        Good,

        /// <summary>
        /// The network signal is very good
        /// </summary>
        VeryGood,

        /// <summary>
        /// The network signal is excellent
        /// </summary>
        Excellent
    }
}
