using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Reports the status of sending an Internet Control Message Protocol (ICMP) echo message to a computer.
	/// </summary>
	public enum IPStatus
	{
        /// <summary>
        /// The ICMP echo request failed for an unknown reason.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// The ICMP echo request succeeded; an ICMP echo reply was received. When you
        /// get this status code, the other System.Net.NetworkInformation.PingReply properties
        /// contain valid data.
        /// </summary>
        Success = 0,
        /// <summary>
        /// The ICMP echo request failed because the network that contains the destination
        /// computer is not reachable.
        /// </summary>
        DestinationNetworkUnreachable = 11002,
        /// <summary>
        /// The ICMP echo request failed because the destination computer is not reachable.
        /// </summary>
        DestinationHostUnreachable = 11003,
        /// <summary>
        /// The ICMP echo request failed because contact with the destination computer
        /// is administratively prohibited.
        /// </summary>
        DestinationProhibited = 11004,
        /// <summary>
        /// The ICMP echo request failed because the destination computer that is specified
        /// in an ICMP echo message is not reachable, because it does not support the
        /// packet's protocol.
        /// </summary>
        DestinationProtocolUnreachable = 11004,
        /// <summary>
        /// The ICMP echo request failed because the port on the destination computer
        /// is not available.
        /// </summary>
        DestinationPortUnreachable = 11005,
        /// <summary>
        /// The ICMP echo request failed because of insufficient network resources.
        /// </summary>
        NoResources = 11006,
        /// <summary>
        /// The ICMP echo request failed because it contains an invalid option.
        /// </summary>
        BadOption = 11007,
        /// <summary>
        /// The ICMP echo request failed because of a hardware error.
        /// </summary>
        HardwareError = 11008,
        /// <summary>
        /// The ICMP echo request failed because the packet containing the request is
        /// larger than the maximum transmission unit (MTU) of a node (router or gateway)
        /// located between the source and destination. The MTU defines the maximum size
        /// of a transmittable packet.
        /// </summary>
        PacketTooBig = 11009,
        /// <summary>
        /// The ICMP echo Reply was not received within the allotted time. The default
        /// time allowed for replies is 5 seconds. You can change this value using the
        /// Overload:System.Net.NetworkInformation.Ping.Send or Overload:System.Net.NetworkInformation.Ping.SendAsync
        /// methods that take a timeout parameter.
        /// </summary>
        TimedOut = 11010,
        /// <summary>
        /// The ICMP echo request failed because there is no valid route between the
        /// source and destination computers.
        /// </summary>
        BadRoute = 11012,
        /// <summary>
        /// The ICMP echo request failed because its Time to Live (TTL) value reached
        /// zero, causing the forwarding node (router or gateway) to discard the packet.
        /// </summary>
        TtlExpired = 11013,
        /// <summary>
        /// The ICMP echo request failed because the packet was divided into fragments
        /// for transmission and all of the fragments were not received within the time
        /// allotted for reassembly. RFC 2460 (available at www.ietf.org) specifies 60
        /// seconds as the time limit within which all packet fragments must be received.
        /// </summary>
        TtlReassemblyTimeExceeded = 11014,
        /// <summary>
        /// The ICMP echo request failed because a node (router or gateway) encountered
        /// problems while processing the packet header. This is the status if, for example,
        /// the header contains invalid field data or an unrecognized option.
        /// </summary>
        ParameterProblem = 11015,
        /// <summary>
        /// The ICMP echo request failed because the packet was discarded. This occurs
        /// when the source computer's output queue has insufficient storage space, or
        /// when packets arrive at the destination too quickly to be processed.
        /// </summary>
        SourceQuench = 11016,
        /// <summary>
        /// The ICMP echo request failed because the destination IP address cannot receive
        /// ICMP echo requests or should never appear in the destination address field
        /// of any IP datagram. For example, calling Overload:System.Net.NetworkInformation.Ping.Send
        /// and specifying IP address "000.0.0.0" returns this status.
        /// </summary>
        BadDestination = 11018,
        /// <summary>
        /// The ICMP echo request failed because the destination computer that is specified
        /// in an ICMP echo message is not reachable; the exact cause of problem is unknown.
        /// </summary>
        DestinationUnreachable = 11040,
        /// <summary>
        /// The ICMP echo request failed because its Time to Live (TTL) value reached
        /// zero, causing the forwarding node (router or gateway) to discard the packet.
        /// </summary>
        TimeExceeded = 11041,
        /// <summary>
        /// The ICMP echo request failed because the header is invalid.
        /// </summary>
        BadHeader = 11042,
        /// <summary>
        /// The ICMP echo request failed because the Next Header field does not contain
        /// a recognized value. The Next Header field indicates the extension header
        /// type (if present) or the protocol above the IP layer, for example, TCP or
        /// UDP.
        /// </summary>
        UnrecognizedNextHeader = 11043,
        /// <summary>
        /// The ICMP echo request failed because of an ICMP protocol error.
        /// </summary>
        IcmpError = 11044,
        /// <summary>
        /// The ICMP echo request failed because the source address and destination address
        /// that are specified in an ICMP echo message are not in the same scope. This
        /// is typically caused by a router forwarding a packet using an interface that
        /// is outside the scope of the source address. Address scopes (link-local, site-local,
        /// and global scope) determine where on the network an address is valid.
        /// </summary>
        DestinationScopeMismatch = 11045,
	}
}
