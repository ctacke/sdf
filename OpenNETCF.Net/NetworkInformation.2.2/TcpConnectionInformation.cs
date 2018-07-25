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
