using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Reports the status of sending an Internet Control Message Protocol (ICMP) echo message to a computer.
	/// </summary>
	public enum IPStatus
	{
        /// <summary>
        /// Bad destination
        /// </summary>
		BadDestination = 0x2b0a,
        /// <summary>
        /// Bad header
        /// </summary>
		BadHeader = 0x2b22,
        /// <summary>
        /// Bad Option
        /// </summary>
		BadOption = 0x2aff,
        /// <summary>
        /// Bad route
        /// </summary>
		BadRoute = 0x2b04,
        /// <summary>
        /// Destination Host Unreachable
        /// </summary>
		DestinationHostUnreachable = 0x2afb,
        /// <summary>
        /// Destination Network Unreachable
        /// </summary>
		DestinationNetworkUnreachable = 0x2afa,
        /// <summary>
        /// Destination Port Unreachable
        /// </summary>
		DestinationPortUnreachable = 0x2afd,
        /// <summary>
        /// Destination Prohibited
        /// </summary>
		DestinationProhibited = 0x2afc,
        /// <summary>
        /// Destination Protocol Unreachable
        /// </summary>
		DestinationProtocolUnreachable = 0x2afc,
        /// <summary>
        /// Destination Scope Mismatch
        /// </summary>
		DestinationScopeMismatch = 0x2b25,
        /// <summary>
        /// Destination Unreachable
        /// </summary>
		DestinationUnreachable = 11040,
        /// <summary>
        /// Hardware Error
        /// </summary>
		HardwareError = 0x2b00,
        /// <summary>
        /// Icmp Error
        /// </summary>
		IcmpError = 0x2b24,
        /// <summary>
        /// No Resources
        /// </summary>
		NoResources = 0x2afe,
        /// <summary>
        /// Packet Too Big
        /// </summary>
		PacketTooBig = 0x2b01,
        /// <summary>
        /// Parameter Problem
        /// </summary>
		ParameterProblem = 0x2b07,
        /// <summary>
        /// Source Quench
        /// </summary>
		SourceQuench = 0x2b08,
        /// <summary>
        /// Success
        /// </summary>
		Success = 0,
        /// <summary>
        /// Timed Out
        /// </summary>
		TimedOut = 11010,
        /// <summary>
        /// Time Exceeded
        /// </summary>
		TimeExceeded = 0x2b21,
        /// <summary>
        /// TTL Expired
        /// </summary>
		TtlExpired = 0x2b05,
        /// <summary>
        /// TTL Reassembly Time Exceeded
        /// </summary>
		TtlReassemblyTimeExceeded = 0x2b06,
        /// <summary>
        /// Unknown
        /// </summary>
		Unknown = -1,
        /// <summary>
        /// Unrecognized Next Header
        /// </summary>
		UnrecognizedNextHeader = 0x2b23
	}
}
