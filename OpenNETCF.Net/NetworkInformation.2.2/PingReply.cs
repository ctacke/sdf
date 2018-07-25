using System.Net;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Provides information about the status and data resulting from a Send operation.
	/// </summary>
	public class PingReply
	{
		private IPAddress address;
		private byte[] buffer;
		private IPStatus ipStatus;
		private long rtt;
		private PingOptions options;

		/// <summary>
		/// Gets the options used to transmit the reply to an Internet Control Message Protocol (ICMP) echo request.
		/// </summary>
		public PingOptions Options
		{
			get
			{
				return options;
			}
		}

		/// <summary>
		/// Gets the address of the host that sends the Internet Control Message Protocol (ICMP) echo reply.
		/// </summary>
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		/// <summary>
		/// Gets the buffer of data received in an Internet Control Message Protocol (ICMP) echo reply message.
		/// </summary>
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds taken to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message.
		/// </summary>
		public long RoundTripTime
		{
			get
			{
				return this.rtt;
			}
		}

		/// <summary>
		/// Gets the status of an attempt to send an Internet Control Message Protocol (ICMP) echo request and receive the corresponding ICMP echo reply message.
		/// </summary>
		public IPStatus Status
		{
			get
			{
				return this.ipStatus;
			}
		}
 
		internal PingReply(IcmpEchoReply reply)
		{
			address = new IPAddress((long) reply.address);
			ipStatus = (IPStatus) reply.status;
			options = new PingOptions(reply);
			if (this.ipStatus == IPStatus.Success)
			{
				rtt = reply.roundTripTime;
				buffer = new byte[reply.dataSize];
				Marshal.Copy(reply.data, this.buffer, 0, reply.dataSize);
			}
			else
			{
				buffer = new byte[0];
			}
		}
	}
}
