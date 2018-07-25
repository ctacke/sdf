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
    /// <summary>
    /// Encapsulates an entry in the ARP table
    /// </summary>
    public class ArpEntry
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
        public ArpEntry Clone()
        {
            return new ArpEntry((byte[])m_data.Clone(), 0, m_adapter);
        }

        /*
        #define MAXLEN_PHYSADDR 8
        typedef struct _MIB_IPNETROW {
          DWORD dwIndex; 
          DWORD dwPhysAddrLen; 
          BYTE bPhysAddr[MAXLEN_PHYSADDR]; 
          DWORD dwAddr; 
          DWORD dwType; 
        } MIB_IPNETROW, *PMIB_IPNETROW;
        */
        internal ArpEntry(byte[] data, int offset, INetworkInterface associatedAdapter)
        {
            m_data = new byte[SIZE];
            Buffer.BlockCopy(data, offset, m_data, 0, SIZE);

            m_adapter = associatedAdapter;
        }

        internal ArpEntry(byte[] data, int offset, INetworkInterface[] adapterList)
        {
            m_data = new byte[SIZE];
            Buffer.BlockCopy(data, offset, m_data, 0, SIZE);

            foreach (NetworkInterface intf in adapterList)
            {
                if (intf.Index == this.AdapterIndex)
                {
                    m_adapter = intf;
                    return;
                }
            }
        }

        /// <summary>
        /// Creates an instance of an ArpEntry
        /// </summary>
        /// <param name="adapter">NetworkInterface on the local machine with which this entry will be associated</param>
        /// <param name="address">IP Address of the entry</param>
        /// <param name="mac">Physical (MAC) address of the entry</param>
        /// <param name="entryType">Type of the entry</param>
        public ArpEntry(NetworkInterface adapter, IPAddress address, PhysicalAddress mac, ArpEntryType entryType)
        {
            m_adapter = adapter;
            m_data = new byte[SIZE];

            this.IPAddress = address;
            this.PhysicalAddress = mac;
            this.ArpEntryType = entryType;
            this.AdapterIndex = adapter.Index;
        }

        internal static int SIZE = 24;
        private static int INDEX_OFFSET = 0;
        private static int MAC_SIZE_OFFSET = 4;
        private static int MAC_OFFSET = 8;
        private static int IP_OFFSET = 16;
        private static int TYPE_OFFSET = 20;

        internal int AdapterIndex
        {
            get
            {
                return BitConverter.ToInt32(m_data, INDEX_OFFSET);
            }
            private set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, INDEX_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the NetworkInterface that this ARP table entry is associated with
        /// </summary>
        public INetworkInterface NetworkInterface
        {
            get { return m_adapter; }
        }

        /// <summary>
        /// Gets the physical address of the device
        /// </summary>
        public PhysicalAddress PhysicalAddress
        {
            get
            {
                int length = BitConverter.ToInt32(m_data, MAC_SIZE_OFFSET);
                byte[] mac = new byte[length];
                Buffer.BlockCopy(m_data, MAC_OFFSET, mac, 0, length);
                return new PhysicalAddress(mac);
            }
            private set
            {               
                byte[] address = value.GetAddressBytes();
                Buffer.BlockCopy(address, 0, m_data, MAC_OFFSET, address.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(address.Length), 0, m_data, MAC_SIZE_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the IP Address of the device
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                uint ip = BitConverter.ToUInt32(m_data, IP_OFFSET);
                return new IPAddress(ip);
            }
            private set
            {
                Buffer.BlockCopy(value.GetAddressBytes(), 0, m_data, IP_OFFSET, 4);
            }
        }

        /// <summary>
        /// Gets the type of ARP entry
        /// </summary>
        public ArpEntryType ArpEntryType
        {
            get
            {
                return (ArpEntryType)BitConverter.ToInt32(m_data, TYPE_OFFSET);
            }
            private set
            {
                Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, TYPE_OFFSET, 4);
            }
        }

        /// <summary>
        /// Compares two ArpEntry objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ArpEntry entry = (ArpEntry)obj;

            if (!entry.IPAddress.Equals(this.IPAddress))
            {
                return false;
            }
            if (!entry.PhysicalAddress.Equals(this.PhysicalAddress))
            {
                return false;
            }
            if (!entry.AdapterIndex.Equals(this.AdapterIndex))
            {
                return false;
            }
            if(entry.ArpEntryType != this.ArpEntryType)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash value for an ArpEntry
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return IPAddress.GetHashCode() | 
                PhysicalAddress.GetHashCode() | 
                AdapterIndex.GetHashCode() | 
                ArpEntryType.GetHashCode();
        }

    }
}
