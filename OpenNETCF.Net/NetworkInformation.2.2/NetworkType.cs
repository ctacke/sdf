using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Enumeration returned in the NetworkTypeInUse property.
    /// Indicates the general type of radio network in use.
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// Indicates the physical layer of the frequency hopping spread-spectrum radio
        /// </summary>
        FH,
        /// <summary>
        /// Indicates the physical layer of the direct sequencing spread-spectrum radio
        /// </summary>
        DS,
        /// <summary>
        /// Indicates the physical layer for 5-GHz Orthagonal Frequency Division Multiplexing radios
        /// </summary>
        OFDM5,
        /// <summary>
        /// Indicates the physical layer for 24-GHz Orthagonal Frequency Division Multiplexing radios
        /// </summary>
        OFDM24
    }
}
