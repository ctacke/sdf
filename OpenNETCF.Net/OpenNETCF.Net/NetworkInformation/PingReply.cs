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
