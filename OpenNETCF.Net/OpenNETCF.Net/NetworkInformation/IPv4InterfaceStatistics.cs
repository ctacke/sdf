using System;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides statistical data for a network interface on the local computer.
    /// </summary>
    public class IPv4InterfaceStatistics
    {
        #region --- contant definitions ---
        private const int MAX_INTERFACE_NAME_LEN = (256 * 2); // wchars
        private const int MAXLEN_IFDESCR = 256;
        private const int MAXLEN_PHYSADDR = 8;

        private const int NAME_OFFSET = 0;
        private const int NAME_LENGTH = MAX_INTERFACE_NAME_LEN;
        private const int INDEX_OFFSET = NAME_OFFSET + NAME_LENGTH;
        private const int INDEX_LENGTH = 4;
        private const int TYPE_OFFSET = INDEX_OFFSET + INDEX_LENGTH;
        private const int TYPE_LENGTH = 4;
        private const int MTU_OFFSET = TYPE_OFFSET + TYPE_LENGTH;
        private const int MTU_LENGTH = 4;
        private const int SPEED_OFFSET = MTU_OFFSET + MTU_LENGTH;
        private const int SPEED_LENGTH = 4;
        private const int PHYS_ADDR_LEN_OFFSET = SPEED_OFFSET + SPEED_LENGTH;
        private const int PHYS_ADDR_LEN_LENGTH = 4;
        private const int PHYS_ADDR_OFFSET = PHYS_ADDR_LEN_OFFSET + PHYS_ADDR_LEN_LENGTH;
        private const int PHYS_ADDR_LENGTH = MAXLEN_PHYSADDR;
        private const int ADMIN_STATUS_OFFSET = PHYS_ADDR_OFFSET + PHYS_ADDR_LENGTH;
        private const int ADMIN_STATUS_LENGTH = 4;
        private const int OPER_STATUS_OFFSET = ADMIN_STATUS_OFFSET + ADMIN_STATUS_LENGTH;
        private const int OPER_STATUS_LENGTH = 4;
        private const int LAST_CHANGE_OFFSET = OPER_STATUS_OFFSET + OPER_STATUS_LENGTH;
        private const int LAST_CHANGE_LENGTH = 4;
        private const int IN_OCTETS_OFFSET = LAST_CHANGE_OFFSET + LAST_CHANGE_LENGTH;
        private const int IN_OCTETS_LENGTH = 4;
        private const int IN_UCAST_OFFSET = IN_OCTETS_OFFSET + IN_OCTETS_LENGTH;
        private const int IN_UCAST_LENGTH = 4;
        private const int IN_NUCAST_OFFSET = IN_UCAST_OFFSET + IN_UCAST_LENGTH;
        private const int IN_NUCAST_LENGTH = 4;
        private const int IN_DISCARDS_OFFSET = IN_NUCAST_OFFSET + IN_NUCAST_LENGTH;
        private const int IN_DISCARDS_LENGTH = 4;
        private const int IN_ERRORS_OFFSET = IN_DISCARDS_OFFSET + IN_DISCARDS_LENGTH;
        private const int IN_ERRORS_LENGTH = 4;
        private const int IN_UNK_PROTOS_OFFSET = IN_ERRORS_OFFSET + IN_ERRORS_LENGTH;
        private const int IN_UNK_PROTOS_LENGTH = 4;
        private const int OUT_OCTETS_OFFSET = IN_UNK_PROTOS_OFFSET + IN_UNK_PROTOS_LENGTH;
        private const int OUT_OCTETS_LENGTH = 4;
        private const int OUT_UCAST_OFFSET = OUT_OCTETS_OFFSET + OUT_OCTETS_LENGTH;
        private const int OUT_UCAST_LENGTH = 4;
        private const int OUT_NUCAST_OFFSET = OUT_UCAST_OFFSET + OUT_UCAST_LENGTH;
        private const int OUT_NUCAST_LENGTH = 4;
        private const int OUT_DISCARDS_OFFSET = OUT_NUCAST_OFFSET + OUT_NUCAST_LENGTH;
        private const int OUT_DISCARDS_LENGTH = 4;
        private const int OUT_ERRORS_OFFSET = OUT_DISCARDS_OFFSET + OUT_DISCARDS_LENGTH;
        private const int OUT_ERRORS_LENGTH = 4;
        private const int OUT_QLEN_OFFSET = OUT_ERRORS_OFFSET + OUT_ERRORS_LENGTH;
        private const int OUT_QLEN_LENGTH = 4;
        private const int DESC_LEN_OFFSET = OUT_QLEN_OFFSET + OUT_QLEN_LENGTH;
        private const int DESC_LEN_LENGTH = 4;
        private const int DESC_OFFSET = DESC_LEN_OFFSET + DESC_LEN_LENGTH;
        private const int DESC_LENGTH = MAXLEN_IFDESCR;

        /// <summary>
        /// Length in bytes of a NetworkInterface class
        /// </summary>
        internal const int Size = DESC_OFFSET + DESC_LENGTH; // 864 (0x360) bytes 
        #endregion

        private byte[] m_data;
        private MibIfRow m_mibifRow;

        internal IPv4InterfaceStatistics(MibIfRow mibifRow)
        {
            m_mibifRow = mibifRow;

            m_data = new byte[Size];
            if (NativeMethods.GetIfEntry(m_mibifRow) != NativeMethods.NO_ERROR)
            {
                int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                throw new NetworkInformationException(error);
            }
        }

        /// <summary>
        /// Gets the number of bytes received on the interface.
        /// </summary>
        public long BytesReceived
        {
            get { return m_mibifRow.OctetsReceived * 8; }
        }

        /// <summary>
        /// Gets the number of bytes sent on the interface.
        /// </summary>
        public long BytesSent
        {
            get { return m_mibifRow.OctetsSent * 8; }
        }

        /// <summary>
        /// Gets the number of incoming packets discarded.
        /// </summary>
        public long IncomingPacketsDiscarded
        {
            get { return m_mibifRow.DiscardedIncomingPackets; }
        }


        /// <summary>
        /// Gets the number of incoming packets with errors.
        /// </summary>
        public long IncomingPacketsWithErrors
        {
            get { return m_mibifRow.ErrorIncomingPackets; }
        }

        /// <summary>
        /// Gets the number of incoming packets with an unknown protocol.
        /// </summary>
        public long IncomingUnknownProtocolPackets
        {
            get { return m_mibifRow.UnknownIncomingPackets; }
        }

        /// <summary>
        /// Gets the number of non-unicast packets received on the interface.
        /// </summary>
        public long NonUnicastPacketsReceived
        {
            get { return m_mibifRow.NonUnicastPacketsReceived; }
        }

        /// <summary>
        /// Gets the number of non-unicast packets sent on the interface.
        /// </summary>
        public long NonUnicastPacketsSent
        {
            get { return m_mibifRow.NonUnicastPacketsSent; }
        }

        /// <summary>
        /// Gets the number of outgoing packets that were discarded.
        /// </summary>
        public long OutgoingPacketsDiscarded
        {
            get { return m_mibifRow.DiscardedOutgoingPackets; }
        }

        /// <summary>
        /// Gets the number of outgoing packets with errors.
        /// </summary>
        public long OutgoingPacketsWithErrors
        {
            get { return m_mibifRow.ErrorOutgoingPackets; }
        }

        /// <summary>
        /// Gets the length of the output queue.
        /// </summary>
        public long OutputQueueLength
        {
            get { return m_mibifRow.OutputQueueLength; }
        }

        /// <summary>
        /// Gets the number of unicast packets received on the interface.
        /// </summary>
        public long UnicastPacketsReceived
        {
            get { return m_mibifRow.UnicastPacketsReceived; }
        }

        /// <summary>
        /// Gets the number of unicast packets sent on the interface.
        /// </summary>
        public long UnicastPacketsSent
        {
            get { return m_mibifRow.UnicastPacketsSent; }
        }
    }
}
