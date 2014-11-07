using System;

namespace OpenNETCF.Net.NetworkInformation
{
	internal class IcmpEchoReply
	{
		internal uint address = 0;
		internal uint status = 0;
		internal uint roundTripTime = 0;
		internal ushort dataSize = 0;
		internal ushort reserved = 0;
		internal IntPtr data = IntPtr.Zero;
		/* IPOptions structure */
		internal byte ttl = 0; 
		internal byte tos = 0;
		internal byte flags = 0;
		internal byte optionsSize = 0;
		internal IntPtr optionsData = IntPtr.Zero;
	}
}
