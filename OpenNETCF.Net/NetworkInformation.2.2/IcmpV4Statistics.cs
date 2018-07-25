using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef struct _MIB_ICMP {
      MIBICMPINFO stats; 
    } MIB_ICMP, *PMIB_ICMP;
     
    typedef struct _MIBICMPINFO {
      MIBICMPSTATS icmpInStats; 
      MIBICMPSTATS icmpOutStats; 
    } MIBICMPINFO;
    
    typedef struct _MIBICMPSTATS {
      DWORD dwMsgs; 
      DWORD dwErrors; 
      DWORD dwDestUnreachs; 
      DWORD dwTimeExcds; 
      DWORD dwParmProbs; 
      DWORD dwSrcQuenchs; 
      DWORD dwRedirects; 
      DWORD dwEchos; 
      DWORD dwEchoReps; 
      DWORD dwTimestamps; 
      DWORD dwTimestampReps; 
      DWORD dwAddrMasks; 
      DWORD dwAddrMaskReps; 
    } MIBICMPSTATS;
    */

    /// <summary>
    /// Provides Internet Control Message Protocol for IPv4 (ICMPv4) statistical
    /// data for the local computer.
    /// </summary>
    public class IcmpV4Statistics
    {
        private const int INPUT_OFFSET = 0;
        private const int OUTPUT_OFFSET = 52;

        private const int MESSAGES_OFFSET               = 0 * 4;
        private const int ERRORS_OFFSET                 = 1 * 4;
        private const int DEST_UNREACHABLE_OFFSET       = 2 * 4;
        private const int TIME_EXCEEDS_OFFSET           = 3 * 4;
        private const int PARAM_PROBS_OFFSET            = 4 * 4;
        private const int SRC_QUENCH_OFFSET             = 5 * 4;
        private const int REDIRECT_OFFSET               = 6 * 4;
        private const int ECHO_OFFSET                   = 7 * 4;
        private const int ECHO_REPLIES_OFFSET           = 8 * 4;
        private const int TIMESTAMPS_OFFSET             = 8 * 4;
        private const int TIMESTAMP_REPLIES_OFFSET      = 8 * 4;
        private const int ADDRESS_MASK_REQUESTS_OFFSET  = 11 * 4;
        private const int ADDRESS_MASK_REPLIES_OFFSET   = 12 * 4;

        private byte[] m_data;

        internal static int GetICMPStats(AddressFamily family, out int[] data)
        {
            byte[] icmp = new byte[104];

            int errorCode = NativeMethods.GetIcmpStatisticsEx(icmp, family);
            data = null;
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the OpenNETCF.Net.NetworkInformation.IcmpV4Statistics
        /// class.
        /// </summary>
        internal IcmpV4Statistics()
        {
            m_data = new byte[104];

            int errorCode = NativeMethods.GetIcmpStatisticsEx(m_data, AddressFamily.InterNetwork);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address
        /// Mask Reply messages that were received.
        /// </summary>
        public int AddressMaskRepliesReceived 
        {
            get 
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ADDRESS_MASK_REPLIES_OFFSET);
            } 
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address
        /// Mask Reply messages that were sent.
        /// </summary>
        public int AddressMaskRepliesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + ADDRESS_MASK_REPLIES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address
        /// Mask Request messages that were received.
        /// </summary>
        public int AddressMaskRequestsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ADDRESS_MASK_REQUESTS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Address
        /// Mask Request messages that were sent.
        /// </summary>
        public int AddressMaskRequestsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + ADDRESS_MASK_REQUESTS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages
        /// that were received because of a packet having an unreachable address in its
        /// destination.
        /// </summary>
        public int DestinationUnreachableMessagesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + DEST_UNREACHABLE_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages
        /// that were sent because of a packet having an unreachable address in its destination.
        /// </summary>
        public int DestinationUnreachableMessagesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + DEST_UNREACHABLE_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo
        /// Reply messages that were received.
        /// </summary>
        public int EchoRepliesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ECHO_REPLIES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo
        /// Reply messages that were sent.
        /// </summary>
        public int EchoRepliesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + ECHO_REPLIES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo
        /// Request messages that were received.
        /// </summary>
        public int EchoRequestsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ECHO_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Echo
        /// Request messages that were sent.
        /// </summary>
        public int EchoRequestsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ECHO_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) error
        /// messages that were received.
        /// </summary>
        public int ErrorsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + ERRORS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) error
        /// messages that were sent.
        /// </summary>
        public int ErrorsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + ERRORS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol messages that were received.
        /// </summary>
        public int MessagesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + MESSAGES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) messages
        /// that were sent.
        /// </summary>
        public int MessagesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + MESSAGES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Parameter
        /// Problem messages that were received.
        /// </summary>
        public int ParameterProblemsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + PARAM_PROBS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Parameter
        /// Problem messages that were sent.
        /// </summary>
        public int ParameterProblemsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + PARAM_PROBS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Redirect
        /// messages that were received.
        /// </summary>
        public int RedirectsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + REDIRECT_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Redirect
        /// messages that were sent.
        /// </summary>
        public int RedirectsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + REDIRECT_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Source
        /// Quench messages that were received.
        /// </summary>
        public int SourceQuenchesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + SRC_QUENCH_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Source
        /// Quench messages that were sent.
        /// </summary>
        public int SourceQuenchesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + SRC_QUENCH_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Time
        /// Exceeded messages that were received.
        /// </summary>
        public int TimeExceededMessagesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + TIME_EXCEEDS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Time
        /// Exceeded messages that were sent.
        /// </summary>
        public int TimeExceededMessagesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + TIME_EXCEEDS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp
        /// Reply messages that were received.
        /// </summary>
        public int TimestampRepliesReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + TIMESTAMP_REPLIES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp
        /// Reply messages that were sent.
        /// </summary>
        public int TimestampRepliesSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + TIMESTAMP_REPLIES_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp
        /// Request messages that were received.
        /// </summary>
        public int TimestampRequestsReceived
        {
            get
            {
                return BitConverter.ToInt32(m_data, INPUT_OFFSET + TIMESTAMPS_OFFSET);
            }
        }

        /// <summary>
        /// Gets the number of Internet Control Message Protocol version 4 (ICMPv4) Timestamp
        /// Request messages that were sent.
        /// </summary>
        public int TimestampRequestsSent
        {
            get
            {
                return BitConverter.ToInt32(m_data, OUTPUT_OFFSET + TIMESTAMPS_OFFSET);
            }
        }
    }
}
