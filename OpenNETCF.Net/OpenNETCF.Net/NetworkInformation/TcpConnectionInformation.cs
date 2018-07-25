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
using System.Net;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about the Transmission Control Protocol (TCP) connections
    /// on the local computer.
    /// </summary>
    public class TcpConnectionInformation
    {
        /*
        typedef struct _MIB_TCPROW {
          DWORD dwState; 
          DWORD dwLocalAddr; 
          DWORD dwLocalPort; 
          DWORD dwRemoteAddr; 
          DWORD dwRemotePort; 
        } MIB_TCPROW, *PMIB_TCPROW;
        */
        private byte[] m_data;

        internal const int NATIVE_SIZE = 20;
        private const int STATE_OFFSET = 0;
        private const int LOCAL_ADDRESS_OFFSET = 4;
        private const int LOCAL_PORT_OFFSET = 8;
        private const int REMOTE_ADDRESS_OFFSET = 12;
        private const int REMOTE_PORT_OFFSET = 16;

        /// <summary>
        /// Returns a byte array representing a native MIB_TCPROW structure for a TcpConnectionInformation instance
        /// </summary>
        /// <param name="t">A TcpConnectionInformation instance</param>
        /// <returns>A byte array</returns>
        public static implicit operator byte[](TcpConnectionInformation t)
        {
            return t.m_data;
        }

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.TcpConnectionInformation
        /// class.
        /// </summary>
        internal TcpConnectionInformation()
        {
            m_data = new byte[NATIVE_SIZE];
        }

        /// <summary>
        /// Gets the local endpoint of a Transmission Control Protocol (TCP) connection.
        /// </summary>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                IPAddress address = new IPAddress(BitConverter.ToUInt32(m_data, LOCAL_ADDRESS_OFFSET));
                int port = BitConverter.ToInt32(m_data, LOCAL_PORT_OFFSET);
                IPEndPoint ep = new IPEndPoint(address, port);
                return ep;
            }
        }

        /// <summary>
        /// Gets the remote endpoint of a Transmission Control Protocol (TCP) connection.
        /// </summary>
        public IPEndPoint RemoteEndPoint 
        {
            get
            {
                IPAddress address = new IPAddress(BitConverter.ToUInt32(m_data, REMOTE_ADDRESS_OFFSET));
                int port = BitConverter.ToInt32(m_data, REMOTE_PORT_OFFSET);
                IPEndPoint ep = new IPEndPoint(address, port);
                return ep;
            }
        }

        /// <summary>
        /// Gets the state of this Transmission Control Protocol (TCP) connection.
        /// </summary>
        public TcpState State 
        {
            get { return (TcpState)BitConverter.ToInt32(m_data, STATE_OFFSET); } 
        }
    }
}
