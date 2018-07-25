using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Operational statii for a NetworkInterface
    /// </summary>
    public enum InterfaceOperationalStatus
    {
        #region --- native definitions ---
        /*
        #define MIB_IF_OPER_STATUS_NON_OPERATIONAL      0
        #define MIB_IF_OPER_STATUS_UNREACHABLE          1
        #define MIB_IF_OPER_STATUS_DISCONNECTED         2
        #define MIB_IF_OPER_STATUS_CONNECTING           3
        #define MIB_IF_OPER_STATUS_CONNECTED            4
        #define MIB_IF_OPER_STATUS_OPERATIONAL          5
        */
        #endregion
        /// <summary>
        /// LAN adapter has been disabled, for example because of an address conflict.
        /// </summary>
        NonOperational = 0,
        /// <summary>
        /// WAN adapter that is not connected.
        /// </summary>
        Unreachable = 1,
        /// <summary>
        /// For LAN adapters: network cable disconnected. For WAN adapters: no carrier
        /// </summary>
        Disconnected = 2,
        /// <summary>
        /// WAN adapter that is in the process of connecting.
        /// </summary>
        Connecting = 3,
        /// <summary>
        /// WAN adapter that is connected to a remote peer.
        /// </summary>
        Connected = 4,
        /// <summary>
        /// Default status for LAN adapters.
        /// </summary>
        Operational = 5
    }
}
