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
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides Transmission Control Protocol (TCP) statistical data.
    /// </summary>
    public class TcpStatistics
    {
        private byte[] m_data;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.TcpStatistics
        /// class.
        /// </summary>
        internal TcpStatistics(AddressFamily family)
        {
            m_data = new byte[60];

            int errorCode = NativeMethods.GetTcpStatisticsEx(m_data, family);
            if (errorCode == NativeMethods.NOT_SUPPORTED)
            {
                throw new System.PlatformNotSupportedException("The local device does not support IPv6");
            }
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /*
        typedef struct _MIB_TCPSTATS {
          DWORD dwRtoAlgorithm; 
          DWORD dwRtoMin; 
          DWORD dwRtoMax; 
          DWORD dwMaxConn; 
          DWORD dwActiveOpens; 
          DWORD dwPassiveOpens; 
          DWORD dwAttemptFails; 
          DWORD dwEstabResets; 
          DWORD dwCurrEstab; 
          DWORD dwInSegs; 
          DWORD dwOutSegs; 
          DWORD dwRetransSegs; 
          DWORD dwInErrs; 
          DWORD dwOutRsts; 
          DWORD dwNumConns; 
        } MIB_TCPSTATS, *PMIB_TCPSTATS;
        */

        private const int RTO_ALG_OFFSET = 0 * 4;
        private const int RTO_MIN_OFFSET = 1 * 4;
        private const int RTO_MAX_OFFSET = 2 * 4;
        private const int MAX_CON_OFFSET = 3 * 4;
        private const int ACTIVE_OPEN_OFFSET = 4 * 4;
        private const int PASSIVE_OPEN_OFFSET = 5 * 4;
        private const int ATTEMPT_FAILS_OFFSET = 6 * 4;
        private const int ESTAB_RESET_OFFSET = 7 * 4;
        private const int CURR_ESTAB_OFFSET = 8 * 4;
        private const int IN_SEGS_OFFSET = 9 * 4;
        private const int OUT_SEGS_OFFSET = 10 * 4;
        private const int RETRAN_SEGS_OFFSET = 11 * 4;
        private const int IN_ERR_OFFSET = 12 * 4;
        private const int OUT_RESET_OFFSET = 13 * 4;
        private const int NUM_CONN_OFFSET = 14 * 4;

        /// <summary>
        /// Gets the number of accepted Transmission Control Protocol (TCP) connection
        /// requests.
        /// </summary>
        public int ConnectionsAccepted
        { 
            get { return BitConverter.ToInt32(m_data, PASSIVE_OPEN_OFFSET); } 
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) connection requests
        /// made by clients.
        /// </summary>
        public int ConnectionsInitiated
        {
            get { return BitConverter.ToInt32(m_data, ACTIVE_OPEN_OFFSET); }
        }

        /// <summary>
        /// Specifies the total number of Transmission Control Protocol (TCP) connections
        /// established.
        /// </summary>
        public int CumulativeConnections
        {
            get { return BitConverter.ToInt32(m_data, NUM_CONN_OFFSET); }
        }

        /// <summary>
        /// Gets the number of current Transmission Control Protocol (TCP) connections.
        /// </summary>
        public int CurrentConnections
        {
            get { return BitConverter.ToInt32(m_data, CURR_ESTAB_OFFSET); } 
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) errors received.
        /// </summary>
        public int ErrorsReceived
        {
            get { return BitConverter.ToInt32(m_data, IN_ERR_OFFSET); }
        }

        /// <summary>
        /// Gets the number of failed Transmission Control Protocol (TCP) connection
        /// attempts.
        /// </summary>
        public int FailedConnectionAttempts
        {
            get { return BitConverter.ToInt32(m_data, ATTEMPT_FAILS_OFFSET); }
        }

        /// <summary>
        /// Gets the maximum number of supported Transmission Control Protocol (TCP)
        /// connections.
        /// </summary>
        public int MaximumConnections
        {
            get { return BitConverter.ToInt32(m_data, MAX_CON_OFFSET); } 
        }

        /// <summary>
        /// Gets the maximum retransmission time-out value for Transmission Control Protocol
        /// (TCP) segments.
        /// </summary>
        public int MaximumTransmissionTimeout
        {
            get { return BitConverter.ToInt32(m_data, RTO_MAX_OFFSET); }
        }


        /// <summary>
        /// Gets the minimum retransmission time-out value for Transmission Control Protocol
        /// (TCP) segments.
        /// </summary>
        public int MinimumTransmissionTimeout
        {
            get { return BitConverter.ToInt32(m_data, RTO_MIN_OFFSET); }
        }

        /// <summary>
        /// Gets the number of RST packets received by Transmission Control Protocol
        /// (TCP) connections.
        /// </summary>
        public int ResetConnections
        {
            get { return BitConverter.ToInt32(m_data, ESTAB_RESET_OFFSET); }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments sent with
        /// the reset flag set.
        /// </summary>
        public int ResetsSent
        {
            get { return BitConverter.ToInt32(m_data, OUT_RESET_OFFSET); }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments received.
        /// </summary>
        public int SegmentsReceived
        {
            get { return BitConverter.ToInt32(m_data, IN_SEGS_OFFSET); }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments re-sent.
        /// </summary>
        public int SegmentsResent
        {
            get { return BitConverter.ToInt32(m_data, RETRAN_SEGS_OFFSET); }
        }

        /// <summary>
        /// Gets the number of Transmission Control Protocol (TCP) segments sent.
        /// </summary>
        public int SegmentsSent
        {
            get { return BitConverter.ToInt32(m_data, OUT_SEGS_OFFSET); }
        }

    }
}
