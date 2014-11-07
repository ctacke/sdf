using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides User Datagram Protocol (UDP) statistical data.
    /// </summary>
    public class UdpStatistics
    {
        private byte[] m_data;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.UdpStatistics
        /// class.
        /// </summary>
        internal UdpStatistics(AddressFamily family)
        {
            m_data = new byte[20];

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
        typedef struct _MIB_UDPSTATS {
          DWORD dwInDatagrams; 
          DWORD dwNoPorts; 
          DWORD dwInErrors; 
          DWORD dwOutDatagrams; 
          DWORD dwNumAddrs; 
        } MIB_UDPSTATS, *PMIB_UDPSTATS;
        */

        private const int DATAGRAMS_IN_OFFSET = 0 * 4;
        private const int DISCARDS_OFFSET = 1 * 4;
        private const int ERRORS_OFFSET = 2 * 4;
        private const int DATAFGRAMS_OUT_OFFSET = 3 * 4;
        private const int CONNECTIONS_OFFSET = 4 * 4;


        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received.
        /// </summary>
        public int DatagramsReceived
        {
            get { return BitConverter.ToInt32(m_data, DATAGRAMS_IN_OFFSET); }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were sent.
        /// </summary>
        public int DatagramsSent
        {
            get { return BitConverter.ToInt32(m_data, DATAFGRAMS_OUT_OFFSET); }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received
        /// and discarded because of port errors.
        /// </summary>
        public int IncomingDatagramsDiscarded
        {
            get { return BitConverter.ToInt32(m_data, DISCARDS_OFFSET); }
        }

        /// <summary>
        /// Gets the number of User Datagram Protocol (UDP) datagrams that were received
        /// and discarded because of errors other than bad port information.
        /// </summary>
        public int IncomingDatagramsWithErrors
        {
            get { return BitConverter.ToInt32(m_data, ERRORS_OFFSET); }
        }

        /// <summary>
        /// Gets the number of local endpoints that are listening for User Datagram Protocol
        /// (UDP) datagrams.
        /// </summary>
        public int UdpListeners
        {
            get { return BitConverter.ToInt32(m_data, CONNECTIONS_OFFSET); }
        }
    }
}
