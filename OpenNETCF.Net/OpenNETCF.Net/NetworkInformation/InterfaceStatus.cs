using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// The current status of a NetworkInterface
    /// </summary>
    [Flags]
    public enum InterfaceStatus
    {
        /// <summary>
        /// A reset condition has started
        /// </summary>
        ResetStart = 0x01,
        /// <summary>
        /// A reset condition has ended
        /// </summary>
        ResetEnd = 0x02,
        /// <summary>
        /// The network media (e.g. cable) has been connected
        /// </summary>
        MediaConnect = 0x04,
        /// <summary>
        /// The network media (e.g. cable) has been disconnected
        /// </summary>
        MediaDisconnect = 0x08,
        /// <summary>
        /// The interface has been bound by NDIS
        /// </summary>
        Bind = 0x10,
        /// <summary>
        /// The interface has been unbound by NDIS
        /// </summary>
        Unbind = 0x20
    }
}
