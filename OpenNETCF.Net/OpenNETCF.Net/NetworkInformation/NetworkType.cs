using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    // typedef enum NDIS_802_11_NETWORK_TYPE {
    //         Ndis802_11FH,
    //         Ndis802_11DS,
    //         Ndis802_11OFDM5,
    //         Ndis802_11OFDM24,
    //         Ndis802_11NetworkTypeMax
    // } NDIS_802_11_NETWORK_TYPE;

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
