#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
