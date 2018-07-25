#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
