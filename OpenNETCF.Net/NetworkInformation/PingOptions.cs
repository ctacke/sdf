using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Used to control how Ping data packets are transmitted.
	/// </summary>
	public class PingOptions
	{
		int ttl = 128;
		bool dontFragment;

		/// <summary>
		/// Gets or sets the number of routing nodes that can forward the Ping data before it is discarded.
		/// </summary>
		public int Ttl
		{
			set
			{
				ttl = value;
			}
			get
			{
				return ttl;
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that controls fragmentation of the data sent to the remote host.
		/// </summary>
		public bool DontFragment
		{
			set
			{
				dontFragment = value;
			}
			get
			{
				return dontFragment;
			}
		}

		/// <summary>
		/// Initializes a new instance of the PingOptions class.
		/// </summary>
		public PingOptions()
		{
		}

		internal PingOptions(IcmpEchoReply reply)
		{
            ttl = reply.ttl;
			dontFragment = (reply.flags & IPOptions.DontFragmentFlag) > 0;
		}

		/// <summary>
		/// Initializes a new instance of the PingOptions class and sets the Time to Live and fragmentation values.
		/// </summary>
		/// <param name="ttl">Specifies the number of times the Ping data packets can be forwarded.</param>
		/// <param name="dontFragment">True to prevent data sent to the remote host from being fragmented; otherwise, false.</param>
		public PingOptions(int ttl, bool dontFragment)
		{
			if (ttl <= 0)
			{
				throw new ArgumentOutOfRangeException("ttl");
			}

			this.ttl = ttl;
			this.dontFragment = dontFragment;
		}
	}
}
