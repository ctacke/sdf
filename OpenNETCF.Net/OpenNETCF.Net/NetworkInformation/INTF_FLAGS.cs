using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    [Flags]
    internal enum INTF_FLAGS : uint
    {
        INTF_ALL = 0xffffffff,

        INTF_ALL_FLAGS = 0x0000ffff,
        /// <summary>
        /// mask for the configuration mode (NDIS_802_11_NETWORK_INFRASTRUCTURE value)
        /// </summary>
        INTF_CM_MASK = 0x00000007,
        /// <summary>
        /// zero conf enabled for this interface
        /// </summary>
        INTF_ENABLED = 0x00008000,
        /// <summary>
        /// attempt to connect to visible non-preferred networks also
        /// </summary>
        INTF_FALLBACK = 0x00004000,
        /// <summary>
        /// 802.11 OIDs are supported by the driver/firmware (can't be set)
        /// </summary>
        INTF_OIDSSUPP = 0x00002000,
        /// <summary>
        /// the service parameters are volatile.
        /// </summary>
        INTF_VOLATILE = 0x00001000,
        /// <summary>
        /// the service parameters are enforced by the policy.
        /// </summary>
        INTF_POLICY = 0x00000800,

        INTF_DESCR = 0x00010000,
        INTF_NDISMEDIA = 0x00020000,
        INTF_PREFLIST = 0x00040000,
        INTF_CAPABILITIES = 0x00080000,


        INTF_ALL_OIDS = 0xfff00000,
        INTF_HANDLE = 0x00100000,
        INTF_INFRAMODE = 0x00200000,
        INTF_AUTHMODE = 0x00400000,
        INTF_WEPSTATUS = 0x00800000,
        INTF_SSID = 0x01000000,
        INTF_BSSID = 0x02000000,
        INTF_BSSIDLIST = 0x04000000,
        INTF_LIST_SCAN = 0x08000000,
        INTF_ADDWEPKEY = 0x10000000,
        INTF_REMWEPKEY = 0x20000000,
        /// <summary>
        /// reload the default WEP_KEY
        /// </summary>
        INTF_LDDEFWKEY = 0x40000000,
    }
}
