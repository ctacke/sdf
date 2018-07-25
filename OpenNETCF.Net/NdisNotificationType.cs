using System;

namespace OpenNETCF.Net
{
    /// <summary>
    /// The NdisNotificationType enumeration defines the types
    /// of notifications which can be requested from NDIS.
    /// </summary>
    public enum NdisNotificationType
    {
        /// <summary>
        /// NdisResetStart is set when an adapter is being
        /// reset.
        /// </summary>
        NdisResetStart = 0x00000001,

        /// <summary>
        /// NdisResetEnd is sent when the reset process on an
        /// adapter is complete and the adapter is ready to be
        /// rebound.
        /// </summary>
        NdisResetEnd = 0x00000002,

        /// <summary>
        /// NdisMediaConnect is set when the communications
        /// media, an Ethernet cable for example, is connected
        /// to the adapter.
        /// </summary>
        NdisMediaConnect = 0x00000004,

        /// <summary>
        /// NdisMediaDisconnect is set when the communciations
        /// media, an Ethernet cable for example, is disconnected
        /// from the adapter.
        /// </summary>
        NdisMediaDisconnect = 0x00000008,

        /// <summary>
        /// NdisBind is set when one or more protocols, TCP/IP
        /// typically, is bound to the adapter.
        /// </summary>
        NdisBind = 0x00000010,

        /// <summary>
        /// NdisUnbind is set when the adapter is unbound from
        /// one or more protocols.
        /// </summary>
        NdisUnbind = 0x00000020,

        /// <summary>
        /// NdisMediaSpecific is set when some notification not
        /// generally defined for all adapter types occurred.
        /// </summary>
        NdisMediaSpecific = 0x00000040,
    }
}
