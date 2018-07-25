using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// 
	/// </summary>
	public class Networking
	{
		private Networking() {}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        [Obsolete("This method is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces() instead", false)]
        public static AdapterCollection GetAdapters()
		{
			return ( new AdapterCollection() );
		}

        /// <summary>
        /// This function converts a ushort from host to TCP/IP network byte order, which is big-endian.
        /// </summary>
        /// <param name="hostShort">16-bit number in host byte order.</param>
        /// <returns>This function returns the value in TCP/IP network byte order.</returns>
        [CLSCompliant(false)]
        public static ushort NetworkOrder(ushort hostShort)
        {
            return NativeMethods.htons(hostShort);
        }

        /// <summary>
        /// This function converts a uint from host to TCP/IP network byte order, which is big-endian.
        /// </summary>
        /// <param name="hostInt">32-bit number in host byte order.</param>
        /// <returns>This function returns the value in TCP/IP network byte order.</returns>
        [CLSCompliant(false)]
        public static uint NetworkOrder(uint hostInt)
        {
            return NativeMethods.htonl(hostInt);
        }

        /// <summary>
        /// This function converts a ushort from TCP/IP network order to host byte order, which is little-endian on Intel processors.
        /// </summary>
        /// <param name="networkShort">16-bit number in TCP/IP network byte order.</param>
        /// <returns>This function always returns a value in host byte order. If the networkShort parameter was already in host byte order, then no operation is performed.</returns>
        [CLSCompliant(false)]
        public static ushort HostOrder(ushort networkShort)
        {
            return NativeMethods.ntohs(networkShort);
        }

        /// <summary>
        /// This function converts a uint from TCP/IP network order to host byte order, which is little-endian on Intel processors.
        /// </summary>
        /// <param name="networkInt">32-bit number in TCP/IP network byte order.</param>
        /// <returns>This function always returns a value in host byte order. If the networkInt parameter was already in host byte order, then no operation is performed.</returns>
        [CLSCompliant(false)]
        public static uint HostOrder(uint networkInt)
        {
            return NativeMethods.ntohl(networkInt);
        }
    }
}
