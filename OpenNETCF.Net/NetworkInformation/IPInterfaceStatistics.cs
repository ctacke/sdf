using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Provides statistical data for a network interface on the local computer.
	/// </summary>
	public abstract class IPInterfaceStatistics
	{
		protected IPInterfaceStatistics()
		{
		}

		/// <summary>
		/// Gets the number of bytes received on the interface.
		/// </summary>
		public abstract long BytesReceived { get; }
		/// <summary>
		/// Gets the number of bytes sent on the interface.
		/// </summary>
		public abstract long BytesSent { get; }
		/// <summary>
		/// Gets the number of incoming packets discarded.
		/// </summary>
		public abstract long IncomingPacketsDiscarded { get; }
		/// <summary>
		/// Gets the number of incoming packets with errors.
		/// </summary>
		public abstract long IncomingPacketsWithErrors { get; }
		/// <summary>
		/// Gets the number of incoming packets with an unknown protocol.
		/// </summary>
		public abstract long IncomingUnknownProtocolPackets { get; }
		/// <summary>
		/// Gets the number of non-unicast packets received on the interface.
		/// </summary>
		public abstract long NonUnicastPacketsReceived { get; }
		/// <summary>
		/// Gets the number of non-unicast packets sent on the interface.
		/// </summary>
		public abstract long NonUnicastPacketsSent { get; }
		/// <summary>
		/// Gets the number of outgoing packets that were discarded.
		/// </summary>
		public abstract long OutgoingPacketsDiscarded { get; }
		/// <summary>
		/// Gets the number of outgoing packets with errors.
		/// </summary>
		public abstract long OutgoingPacketsWithErrors { get; }
		/// <summary>
		/// Gets the length of the output queue.
		/// </summary>
		public abstract long OutputQueueLength { get; }
		/// <summary>
		/// Gets the number of unicast packets received on the interface.
		/// </summary>
		public abstract long UnicastPacketsReceived { get; }
		/// <summary>
		/// Gets the number of unicast packets sent on the interface.
		/// </summary>
		public abstract long UnicastPacketsSent { get; }
	}
}
