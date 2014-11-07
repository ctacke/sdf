using System;
using System.Collections.Generic;
using System.Text;

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
  /// Provides Internet Control Message Protocol for Internet Protocol version
  /// 6 (ICMPv6) statistical data for the local computer.
  /// </summary>
  public abstract class IcmpV6Statistics
  {
    /// <summary>
    /// Initializes a new instance of the OpenNETCF.Net.NetworkInformation.IcmpV6Statistics
    /// class.
    /// </summary>
    protected IcmpV6Statistics()
    {
    }

    // Returns:
    //     An System.Int64 value that specifies the total number of Destination Unreachable
    //     messages received.
    /// <summary>
    /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages
    /// that were received because of a packet having an unreachable address in its
    /// destination.
    /// </summary>
    public abstract long DestinationUnreachableMessagesReceived { get; }

    // Returns:
    //     An System.Int64 value that specifies the total number of Destination Unreachable
    //     messages sent.
    /// <summary>
    /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages
    /// that were sent because of a packet having an unreachable address in its destination.
    /// </summary>
    public abstract long DestinationUnreachableMessagesSent { get; }

    // Returns:
    //     An System.Int64 value that specifies the total number of number of ICMP Echo
    //     Reply messages received.
    /// <summary>
    /// Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo
    /// Reply messages that were received.
    /// </summary>
    public abstract long EchoRepliesReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo
    //     Reply messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of number of ICMP Echo
    //     Reply messages sent.
    public abstract long EchoRepliesSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo
    //     Request messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of number of ICMP Echo
    //     Request messages received.
    public abstract long EchoRequestsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Echo
    //     Request messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of number of ICMP Echo
    //     Request messages sent.
    public abstract long EchoRequestsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error
    //     messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP error messages
    //     received.
    public abstract long ErrorsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) error
    //     messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP error messages
    //     sent.
    public abstract long ErrorsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Group management Protocol (IGMP) Group Membership
    //     Query messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Query messages received.
    public abstract long MembershipQueriesReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Group management Protocol (IGMP) Group Membership
    //     Query messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Query messages sent.
    public abstract long MembershipQueriesSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Group Management Protocol (IGMP) Group Membership
    //     Reduction messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Reduction messages received.
    public abstract long MembershipReductionsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Group Management Protocol (IGMP) Group Membership
    //     Reduction messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Reduction messages sent.
    public abstract long MembershipReductionsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Group Management Protocol (IGMP) Group Membership
    //     Report messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Report messages received.
    public abstract long MembershipReportsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Group Management Protocol (IGMP) Group Membership
    //     Report messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Group Membership
    //     Report messages sent.
    public abstract long MembershipReportsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages
    //     received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMPv6 messages
    //     received.
    public abstract long MessagesReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) messages
    //     sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMPv6 messages
    //     sent.
    public abstract long MessagesSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor
    //     Advertisement messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Neighbor Advertisement
    //     messages received.
    public abstract long NeighborAdvertisementsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor
    //     Advertisement messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Neighbor Advertisement
    //     messages sent.
    public abstract long NeighborAdvertisementsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor
    //     Solicitation messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Neighbor Solicitation
    //     messages received.
    public abstract long NeighborSolicitsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Neighbor
    //     Solicitation messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Neighbor Solicitation
    //     messages sent.
    public abstract long NeighborSolicitsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Packet
    //     Too Big messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Packet Too
    //     Big messages received.
    public abstract long PacketTooBigMessagesReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Packet
    //     Too Big messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Packet Too
    //     Big messages sent.
    public abstract long PacketTooBigMessagesSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter
    //     Problem messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Parameter Problem
    //     messages received.
    public abstract long ParameterProblemsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Parameter
    //     Problem messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Parameter Problem
    //     messages sent.
    public abstract long ParameterProblemsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect
    //     messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Redirect messages
    //     received.
    public abstract long RedirectsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Redirect
    //     messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Redirect messages
    //     sent.
    public abstract long RedirectsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router
    //     Advertisement messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Router Advertisement
    //     messages received.
    public abstract long RouterAdvertisementsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router
    //     Advertisement messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Router Advertisement
    //     messages sent.
    public abstract long RouterAdvertisementsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router
    //     Solicitation messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Router Solicitation
    //     messages received.
    public abstract long RouterSolicitsReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Router
    //     Solicitation messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of Router Solicitation
    //     messages sent.
    public abstract long RouterSolicitsSent { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time
    //     Exceeded messages received.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Time Exceeded
    //     messages received.
    public abstract long TimeExceededMessagesReceived { get; }
    //
    // Summary:
    //     Gets the number of Internet Control Message Protocol version 6 (ICMPv6) Time
    //     Exceeded messages sent.
    //
    // Returns:
    //     An System.Int64 value that specifies the total number of ICMP Time Exceeded
    //     messages sent.
    public abstract long TimeExceededMessagesSent { get; }
  }
}
