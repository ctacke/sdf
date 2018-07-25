using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides Internet Protocol (IP) statistical data.
    /// </summary>
    public class IPGlobalStatistics
    {
        private MibIpStats m_stats;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPGlobalStatistics
        /// class.
        /// </summary>
        /// <param name="stats"></param>
        internal IPGlobalStatistics(MibIpStats stats)
        {
            m_stats = stats;
        }

        /// <summary>
        /// Sets or Gets the default time-to-live (TTL) value for Internet Protocol (IP) packets.
        /// </summary>
        public int DefaultTtl
        {
            get { return m_stats.dwDefaultTTL; }
            set
            {
                MibIpStats stats = new MibIpStats();
                stats.dwDefaultTTL = value;
                
                // #define MIB_USE_CURRENT_TTL         ((DWORD)-1)
                // #define MIB_USE_CURRENT_FORWARDING  ((DWORD)-1)
                stats.dwForwarding = -1;

                int errorCode = NativeMethods.SetIpStatistics(ref stats);

                if (errorCode != NativeMethods.NO_ERROR)
                {
                    throw new NetworkInformationException(errorCode);
                }
            }
        }

        /// <summary>
        /// Sets or Gets a System.Boolean value that specifies whether Internet Protocol (IP)
        /// packet forwarding is enabled.
        /// </summary>
        public bool ForwardingEnabled
        {
            get { return (m_stats.dwForwarding != 0); }
            set
            {
                MibIpStats stats = new MibIpStats();

                // #define MIB_USE_CURRENT_TTL         ((DWORD)-1)
                // #define MIB_USE_CURRENT_FORWARDING  ((DWORD)-1)
                stats.dwForwarding = value ? 1 : 0;
                stats.dwDefaultTTL = -1;

                int errorCode = NativeMethods.SetIpStatistics(ref stats);

                if (errorCode != NativeMethods.NO_ERROR)
                {
                    throw new NetworkInformationException(errorCode);
                }
            }
        }

        /// <summary>
        /// Gets the number of network interfaces.
        /// </summary>
        public int NumberOfInterfaces
        {
            get { return m_stats.dwNumIf; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) addresses assigned to the local
        /// computer.
        /// </summary>
        public int NumberOfIPAddresses
        {
            get { return m_stats.dwNumAddr; }
        }

        /// <summary>
        /// Gets the number of routes in the Internet Protocol (IP) routing table.
        /// </summary>
        public int NumberOfRoutes
        {
            get { return m_stats.dwNumRoutes; }
        }

        /// <summary>
        /// Gets the number of outbound Internet Protocol (IP) packets.
        /// </summary>
        public int OutputPacketRequests
        {
            get { return m_stats.dwOutRequests; }
        }

        /// <summary>
        /// Gets the number of routes that have been discarded from the routing table.
        /// </summary>
        public int OutputPacketRoutingDiscards
        {
            get { return m_stats.dwRoutingDiscards; }
        }

        /// <summary>
        /// Gets the number of transmitted Internet Protocol (IP) packets that have been
        /// discarded.
        /// </summary>
        public int OutputPacketsDiscarded
        {
            get { return m_stats.dwOutDiscards; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets for which the local computer
        /// could not determine a route to the destination address.
        /// </summary>
        public int OutputPacketsWithNoRoute
        {
            get { return m_stats.dwOutNoRoutes; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that could not be fragmented.
        /// </summary>
        public int PacketFragmentFailures
        {
            get { return m_stats.dwFragFails; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that required reassembly.
        /// </summary>
        public int PacketReassembliesRequired
        {
            get { return m_stats.dwReasmReqds; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that were not successfully
        /// reassembled.
        /// </summary>
        public int PacketReassemblyFailures
        {
            get { return m_stats.dwReasmFails; }
        }

        /// <summary>
        /// Gets the maximum amount of time within which all fragments of an Internet
        /// Protocol (IP) packet must arrive.
        /// </summary>
        public int PacketReassemblyTimeout
        {
            get { return m_stats.dwReasmTimeout; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets fragmented.
        /// </summary>
        public int PacketsFragmented
        {
            get { return m_stats.dwFragOks; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets reassembled.
        /// </summary>
        public int PacketsReassembled
        {
            get { return m_stats.dwReasmOks; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets received.
        /// </summary>
        public int ReceivedPackets
        {
            get { return m_stats.dwInReceives; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets delivered.
        /// </summary>
        public int ReceivedPacketsDelivered
        {
            get { return m_stats.dwInDelivers; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets that have been received
        /// and discarded.
        /// </summary>
        public int ReceivedPacketsDiscarded
        {
            get { return m_stats.dwInDiscards; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets forwarded.
        /// </summary>
        public int ReceivedPacketsForwarded
        {
            get { return m_stats.dwForwDatagrams; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets with address errors that
        /// were received.
        /// </summary>
        public int ReceivedPacketsWithAddressErrors
        {
            get { return m_stats.dwInAddrErrors; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets with header errors that
        /// were received.
        /// </summary>
        public int ReceivedPacketsWithHeadersErrors
        {
            get { return m_stats.dwInHdrErrors; }
        }

        /// <summary>
        /// Gets the number of Internet Protocol (IP) packets received on the local machine
        /// with an unknown protocol in the header.
        /// </summary>
        public int ReceivedPacketsWithUnknownProtocol
        {
            get { return m_stats.dwInUnknownProtos; }
        }
    }
}
