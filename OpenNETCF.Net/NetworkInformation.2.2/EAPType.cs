using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// The EAP_TYPE enumeration is used when configuring a
    /// connection to an EAP-enabled network.
    /// </summary>
    public enum EAPType
    {
        /// <summary>
        /// MD5 authentication
        /// </summary>
        MD5 = 4,

        /// <summary>
        /// EAP-TLS authentication
        /// </summary>
        TLS = 13,

        /// <summary>
        /// PEAP authentication
        /// </summary>
        PEAP = 25,

        /// <summary>
        /// MS-CHAP version 2 authentication
        /// </summary>
        MSCHAPv2 = 26
    }
}
