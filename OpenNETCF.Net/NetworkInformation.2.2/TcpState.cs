using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies the states of a Transmission Control Protocol (TCP) connection.
    /// </summary>
    public enum TcpState
    {
        /// <summary>
        /// The TCP connection state is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The TCP connection is closed.
        /// </summary>
        Closed = 1,
        /// <summary>
        /// The local endpoint of the TCP connection is listening for a connection request
        /// from any remote endpoint.
        /// </summary>
        Listen = 2,
        /// <summary>
        /// The local endpoint of the TCP connection has sent the remote endpoint a segment
        /// header with the synchronize (SYN) control bit set and is waiting for a matching
        /// connection request.
        /// </summary>
        SynSent = 3,
        /// <summary>
        /// The local endpoint of the TCP connection has sent and received a connection
        /// request and is waiting for an acknowledgment.
        /// </summary>
        SynReceived = 4,
        /// <summary>
        /// The TCP handshake is complete. The connection has been established and data
        /// can be sent.
        /// </summary>
        Established = 5,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for a connection termination
        /// request from the remote endpoint or for an acknowledgement of the connection
        /// termination request sent previously.
        /// </summary>
        FinWait1 = 6,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for a connection termination
        /// request from the remote endpoint.
        /// </summary>
        FinWait2 = 7,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for a connection termination
        /// request from the local user.
        /// </summary>
        CloseWait = 8,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for an acknowledgement
        /// of the connection termination request sent previously.
        /// </summary>
        Closing = 9,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for the final acknowledgement
        /// of the connection termination request sent previously.
        /// </summary>
        LastAck = 10,
        /// <summary>
        /// The local endpoint of the TCP connection is waiting for enough time to pass
        /// to ensure that the remote endpoint received the acknowledgement of its connection
        /// termination request.
        /// </summary>
        TimeWait = 11,
        /// <summary>
        /// The transmission control buffer (TCB) for the TCP connection is being deleted.
        /// </summary>
        DeleteTcb = 12,
    }
}
