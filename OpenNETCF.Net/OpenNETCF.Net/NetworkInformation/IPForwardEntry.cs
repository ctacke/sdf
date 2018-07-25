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
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef struct _MIB_IPFORWARDROW {
      000 DWORD dwForwardDest; 
      004 DWORD dwForwardMask; 
      008 DWORD dwForwardPolicy; 
      012 DWORD dwForwardNextHop; 
      016 DWORD dwForwardIfIndex; 
      020 DWORD dwForwardType; 
      024 DWORD dwForwardProto; 
      028 DWORD dwForwardAge; 
      032 DWORD dwForwardNextHopAS; 
      036 DWORD dwForwardMetric1; 
      040 DWORD dwForwardMetric2; 
      044 DWORD dwForwardMetric3; 
      048 DWORD dwForwardMetric4; 
      052 DWORD dwForwardMetric5; 
    } MIB_IPFORWARDROW, *PMIB_IPFORWARDROW;
    */
    /// <summary>
    /// This class contains information that describes an IP network route.
    /// </summary>
    public class IPForwardEntry
    {
        private byte[] m_data;
        private INetworkInterface m_adapter;

        internal byte[] GetEntryBytes()
        {
            return m_data;
        }

        /// <summary>
        /// Creates a copy of this IPForwardEntry
        /// </summary>
        /// <returns>A shallow copy of the current IPForwardEntry</returns>
        public IPForwardEntry Clone()
        {
            return new IPForwardEntry((byte[])m_data.Clone(), 0, m_adapter);
        }

        /// <summary>
        /// Creates a new IPForwardEntry usable for the specified adapter
        /// </summary>
        /// <param name="adapter">Local NetworkInterface for which this entry will be used</param>
        public IPForwardEntry(INetworkInterface adapter) 
        {
            m_data = new byte[SIZE];
            Metric2 = Metric3 = Metric4 = Metric5 = 0xffffffff;
            m_adapter = adapter;
            Protocol = IPProtocol.NetworkManagement;
            Policy = 0;
        }

        internal IPForwardEntry(byte[] data, int offset, INetworkInterface associatedAdapter)
        {
            m_data = new byte[SIZE];
            Buffer.BlockCopy(data, offset, m_data, 0, SIZE);
            Metric2 = Metric3 = Metric4 = Metric5 = 0xffffffff;

            m_adapter = associatedAdapter;
        }

        internal IPForwardEntry(byte[] data, int offset, INetworkInterface[] adapterList)
        {
            m_data = new byte[SIZE];
            Buffer.BlockCopy(data, offset, m_data, 0, SIZE);
            Metric2 = Metric3 = Metric4 = Metric5 = 0xffffffff;

            foreach (NetworkInterface intf in adapterList)
            {
                if (intf.Index == this.AdapterIndex)
                {
                    m_adapter = intf;
                    return;
                }
            }
        }

        internal static int SIZE = 56;

        private static int DESTINATION_OFFSET = 0;
        private static int MASK_OFFSET = 4;
        private static int POLICY_OFFSET = 8;
        private static int NEXT_HOP_OFFSET = 12;
        private static int INDEX_OFFSET = 16;
        private static int TYPE_OFFSET = 20;
        private static int PROTO_OFFSET = 24;
        private static int AGE_OFFSET = 28;
        private static int NEXT_HOP_SN_OFFSET = 32;
        private static int METRIC1_OFFSET = 36;
        private static int METRIC2_OFFSET = 40;
        private static int METRIC3_OFFSET = 44;
        private static int METRIC4_OFFSET = 48;
        private static int METRIC5_OFFSET = 52;

        internal int AdapterIndex
        {
            get
            {
                return BitConverter.ToInt32(m_data, INDEX_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, INDEX_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the NetworkInterface that this entry is associated with
        /// </summary>
        public INetworkInterface NetworkInterface
        {
            get { return m_adapter; }
        }

        /// <summary>
        /// Gets the protocol that generated the route. 
        /// </summary>
        public IPProtocol Protocol
        {
            get
            {
                return (IPProtocol)BitConverter.ToUInt32(m_data, PROTO_OFFSET);
            }
            internal set
            {
                Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, PROTO_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets the IP address of the destination host. 
        /// </summary>
        public IPAddress Destination
        {
            get
            {
                return new IPAddress(BitConverter.ToUInt32(m_data, DESTINATION_OFFSET));
            }
            set
            {
                Buffer.BlockCopy(value.GetAddressBytes(), 0, m_data, DESTINATION_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets the Subnet mask of the destination host.
        /// </summary>
        public IPAddress SubnetMask
        {
            get
            {
                return new IPAddress(BitConverter.ToUInt32(m_data, MASK_OFFSET));
            }
            set
            {
                Buffer.BlockCopy(value.GetAddressBytes(), 0, m_data, MASK_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets the IP address of the next hop in the route. 
        /// </summary>
        public IPAddress NextHop
        {
            get
            {
                return new IPAddress(BitConverter.ToUInt32(m_data, NEXT_HOP_OFFSET));
            }
            set
            {
                Buffer.BlockCopy(value.GetAddressBytes(), 0, m_data, NEXT_HOP_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets the route type as defined in RFC 1354.
        /// </summary>
        public RouteType RouteType
        {
            get
            {
                return (RouteType) BitConverter.ToInt32(m_data, TYPE_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, TYPE_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the age of the route in seconds.
        /// </summary>
        public int Age
        {
            get
            {
                return BitConverter.ToInt32(m_data, AGE_OFFSET);
            }
            internal set
            {
                Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, AGE_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the autonomous system number of the next hop.
        /// </summary>
        public int NextHopSystemNumber
        {
            get
            {
                return BitConverter.ToInt32(m_data, NEXT_HOP_SN_OFFSET);
            }
            internal set
            {
                Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, NEXT_HOP_SN_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the set of conditions that would cause the selection of a multi-path route. This member is typically in IP TOS format. For more information, see RFC 1354.
        /// </summary>
        [CLSCompliant(false)]
        public uint Policy
        {
            get
            {
                return BitConverter.ToUInt32(m_data, POLICY_OFFSET);
            }
            private set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, POLICY_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets a routing-protocol-specific metric value. This metric value is documented in RFC 1354.
        /// </summary>
        [CLSCompliant(false)]
        public uint Metric
        {
            get
            {
                return BitConverter.ToUInt32(m_data, METRIC1_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, METRIC1_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets a routing-protocol-specific metric value. This metric value is documented in RFC 1354.
        /// </summary>
//        [CLSCompliant(false)]
        private uint Metric2
        {
            get
            {
                return BitConverter.ToUInt32(m_data, METRIC2_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, METRIC2_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets a routing-protocol-specific metric value. This metric value is documented in RFC 1354.
        /// </summary>
//        [CLSCompliant(false)]
        private uint Metric3
        {
            get
            {
                return BitConverter.ToUInt32(m_data, METRIC3_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, METRIC3_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets a routing-protocol-specific metric value. This metric value is documented in RFC 1354.
        /// </summary>
//        [CLSCompliant(false)]
        private uint Metric4
        {
            get
            {
                return BitConverter.ToUInt32(m_data, METRIC4_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, METRIC4_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets or sets a routing-protocol-specific metric value. This metric value is documented in RFC 1354.
        /// </summary>
//        [CLSCompliant(false)]
        private uint Metric5
        {
            get
            {
                return BitConverter.ToUInt32(m_data, METRIC5_OFFSET);
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, METRIC5_OFFSET, 4);
            }
        }
    }
}
