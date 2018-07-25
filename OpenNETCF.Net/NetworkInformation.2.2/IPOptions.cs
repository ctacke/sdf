using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// The ip_option_information structure describes the options to be
	/// included in the header of an IP packet. The TTL, TOS, and Flags
	/// values are carried in specific fields in the header. The OptionsData
	/// bytes are carried in the options area following the standard IP header.
	/// With the exception of source route options, this data must be in the
	/// format to be transmitted on the wire as specified in RFC 791. A source
	/// route option should contain the full route - first hop thru final
	/// destination - in the route data. The first hop will be pulled out of the
	/// data and the option will be reformatted accordingly. Otherwise, the route
	/// option should be formatted as specified in RFC 791.
	/// </summary>
	internal struct IPOptions
	{
		internal IPOptions(PingOptions options)
		{
			ttl = 0x80;
			tos = 0;
			optionsSize = 0;
			flags = 0;
			optionsData = IntPtr.Zero;

			if (options != null)
			{
				this.ttl = (byte) options.Ttl;
				if (options.DontFragment)
				{
					this.flags = DontFragmentFlag;
				}
			}
		}

		/// <summary>
		/// Time To Live.
		/// </summary>
		internal byte ttl; 
		/// <summary>
		/// Type Of Service.
		/// </summary>
		internal byte tos;
		/// <summary>
		/// IP header flags.
		/// </summary>
		internal byte flags;
		/// <summary>
		/// Size in bytes of options data.
		/// </summary>
		internal byte optionsSize;
		internal IntPtr optionsData;

		internal const int DontFragmentFlag = 2;
		internal const int IPOptionsSize = 8;
	}
}
