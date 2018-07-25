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
