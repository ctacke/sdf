using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Control flags for Windows Zero Config
    /// </summary>
    [Flags]
    public enum WZCControl
    {
        /// <summary>
        /// specifies whether the configuration includes or not a WEP key
        /// </summary>
        WEPKPresent = 0x0001,
        /// <summary>
        /// the WEP Key material (if any) is entered as hexadecimal digits
        /// </summary>
        WEPKXFormat = 0x0002,
        /// <summary>
        /// this configuration should not be stored
        /// </summary>
        Volatile = 0x0004,
        /// <summary>
        /// this configuration is enforced by the policy
        /// </summary>
        Policy = 0x0008,
        /// <summary>
        /// for this configuration 802.1X should be enabled
        /// </summary>
        ONEXEnabled = 0x0010,
        /// <summary>
        /// Key is 40 bit
        /// </summary>
        WEPK40Bit = 0x8000
    }
}
