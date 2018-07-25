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

namespace OpenNETCF.Net.NetworkInformation
{
    internal sealed class MibIfRow
    {
        private byte[] m_data;

        #region --- native definition from iprtrmib.h
        /*
        #define MAX_INTERFACE_NAME_LEN 256
        #define MAXLEN_IFDESCR 256
        #define MAXLEN_PHYSADDR 8

        typedef struct _MIB_IFROW
        {
            WCHAR   wszName[MAX_INTERFACE_NAME_LEN];
            DWORD	dwIndex;
            DWORD	dwType;
            DWORD	dwMtu;
            DWORD	dwSpeed;
            DWORD	dwPhysAddrLen;
            BYTE	bPhysAddr[MAXLEN_PHYSADDR]; 
            DWORD	dwAdminStatus;          // offset 540
            DWORD	dwOperStatus;
            DWORD	dwLastChange;
            DWORD	dwInOctets;
            DWORD	dwInUcastPkts;          
            DWORD	dwInNUcastPkts;         // offset 560
            DWORD	dwInDiscards;
            DWORD	dwInErrors;
            DWORD	dwInUnknownProtos;         
            DWORD	dwOutOctets;            
            DWORD	dwOutUcastPkts;         // offset 580
            DWORD	dwOutNUcastPkts;
            DWORD	dwOutDiscards;
            DWORD	dwOutErrors;
            DWORD	dwOutQLen;
            DWORD	dwDescrLen;
            BYTE	bDescr[MAXLEN_IFDESCR]; // offset 600
        } MIB_IFROW,*PMIB_IFROW;
        */
        #endregion

        #region --- contant definitions ---
        private const int MAX_INTERFACE_NAME_LEN = (256 * 2); // wchars
        private const int MAXLEN_IFDESCR = 256;
        private const int MAXLEN_PHYSADDR = 8;

        private const int NAME_OFFSET = 0;
        private const int NAME_LENGTH = MAX_INTERFACE_NAME_LEN;
        private const int INDEX_OFFSET = NAME_OFFSET + NAME_LENGTH;
        private const int INDEX_LENGTH = 4;
        private const int TYPE_OFFSET = INDEX_OFFSET + INDEX_LENGTH;
        private const int TYPE_LENGTH = 4;
        private const int MTU_OFFSET = TYPE_OFFSET + TYPE_LENGTH;
        private const int MTU_LENGTH = 4;
        private const int SPEED_OFFSET = MTU_OFFSET + MTU_LENGTH;
        private const int SPEED_LENGTH = 4;
        private const int PHYS_ADDR_LEN_OFFSET = SPEED_OFFSET + SPEED_LENGTH;
        private const int PHYS_ADDR_LEN_LENGTH = 4;
        private const int PHYS_ADDR_OFFSET = PHYS_ADDR_LEN_OFFSET + PHYS_ADDR_LEN_LENGTH;
        private const int PHYS_ADDR_LENGTH = MAXLEN_PHYSADDR;
        private const int ADMIN_STATUS_OFFSET = PHYS_ADDR_OFFSET + PHYS_ADDR_LENGTH;
        private const int ADMIN_STATUS_LENGTH = 4;
        private const int OPER_STATUS_OFFSET = ADMIN_STATUS_OFFSET + ADMIN_STATUS_LENGTH;
        private const int OPER_STATUS_LENGTH = 4;
        private const int LAST_CHANGE_OFFSET = OPER_STATUS_OFFSET + OPER_STATUS_LENGTH;
        private const int LAST_CHANGE_LENGTH = 4;
        private const int IN_OCTETS_OFFSET = LAST_CHANGE_OFFSET + LAST_CHANGE_LENGTH;
        private const int IN_OCTETS_LENGTH = 4;
        private const int IN_UCAST_OFFSET = IN_OCTETS_OFFSET + IN_OCTETS_LENGTH;
        private const int IN_UCAST_LENGTH = 4;
        private const int IN_NUCAST_OFFSET = IN_UCAST_OFFSET + IN_UCAST_LENGTH;
        private const int IN_NUCAST_LENGTH = 4;
        private const int IN_DISCARDS_OFFSET = IN_NUCAST_OFFSET + IN_NUCAST_LENGTH;
        private const int IN_DISCARDS_LENGTH = 4;
        private const int IN_ERRORS_OFFSET = IN_DISCARDS_OFFSET + IN_DISCARDS_LENGTH;
        private const int IN_ERRORS_LENGTH = 4;
        private const int IN_UNK_PROTOS_OFFSET = IN_ERRORS_OFFSET + IN_ERRORS_LENGTH;
        private const int IN_UNK_PROTOS_LENGTH = 4;
        private const int OUT_OCTETS_OFFSET = IN_UNK_PROTOS_OFFSET + IN_UNK_PROTOS_LENGTH;
        private const int OUT_OCTETS_LENGTH = 4;
        private const int OUT_UCAST_OFFSET = OUT_OCTETS_OFFSET + OUT_OCTETS_LENGTH;
        private const int OUT_UCAST_LENGTH = 4;
        private const int OUT_NUCAST_OFFSET = OUT_UCAST_OFFSET + OUT_UCAST_LENGTH;
        private const int OUT_NUCAST_LENGTH = 4;
        private const int OUT_DISCARDS_OFFSET = OUT_NUCAST_OFFSET + OUT_NUCAST_LENGTH;
        private const int OUT_DISCARDS_LENGTH = 4;
        private const int OUT_ERRORS_OFFSET = OUT_DISCARDS_OFFSET + OUT_DISCARDS_LENGTH;
        private const int OUT_ERRORS_LENGTH = 4;
        private const int OUT_QLEN_OFFSET = OUT_ERRORS_OFFSET + OUT_ERRORS_LENGTH;
        private const int OUT_QLEN_LENGTH = 4;
        private const int DESC_LEN_OFFSET = OUT_QLEN_OFFSET + OUT_QLEN_LENGTH;
        private const int DESC_LEN_LENGTH = 4;
        private const int DESC_OFFSET = DESC_LEN_OFFSET + DESC_LEN_LENGTH;
        private const int DESC_LENGTH = MAXLEN_IFDESCR;

        /// <summary>
        /// Length in bytes of a MibIfRow structure
        /// </summary>
        public const int Size = DESC_OFFSET + DESC_LENGTH; // 864 (0x360) bytes 
        #endregion

        #region --- operators ---
        public static implicit operator byte[](MibIfRow row)
        {
            return row.m_data;
        }

        #endregion

            /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.NetworkInterface class.
        /// </summary>
        internal MibIfRow()
        {
            m_data = new byte[Size];
        }

        internal unsafe MibIfRow(byte* pdata, int offset)
        {
            m_data = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(new IntPtr(pdata + offset), m_data, 0, Size); 
        }

        /// <summary>
        /// Gets the name of the network adapter.
        /// </summary>
        public string Name
        {
            get
            {
                string name = Encoding.Unicode.GetString(m_data, NAME_OFFSET, NAME_LENGTH);
                return name.Substring(0, name.IndexOf('\0'));
            }
        }

        /// <summary>
        /// Specifies the index that identifies the interface
        /// </summary>
        public int Index
        {
            internal set 
            {
                byte[] data = BitConverter.GetBytes(value);
                Buffer.BlockCopy(data, 0, m_data, INDEX_OFFSET, data.Length);
            }
            get { return BitConverter.ToInt32(m_data, INDEX_OFFSET); }
        }

        /// <summary>
        /// Specifies the type of interface
        /// </summary>
        public NetworkInterfaceType NetworkInterfaceType
        {
            get { return (NetworkInterfaceType)BitConverter.ToInt32(m_data, TYPE_OFFSET); }
        }

        /// <summary>
        /// Specifies the Maximum Transmission Unit (MTU).
        /// </summary>
        public int MTU
        {
            get { return BitConverter.ToInt32(m_data, MTU_OFFSET); }
        }

        /// <summary>
        /// Specifies the speed of the interface in bits per second
        /// </summary>
        public int Speed
        {
			//jsm - Bug 199: We were returning MTU value here instead of speed
			get { return BitConverter.ToInt32(m_data, SPEED_OFFSET); }
        }

        /// <summary>
        /// Specifies the length of the physical address specified by the bPhysAddr member. 
        /// </summary>
        private int PhysAddrLength
        {
            get
            {
                int length = BitConverter.ToInt32(m_data, PHYS_ADDR_LEN_OFFSET);
                if (length <= MAXLEN_PHYSADDR)
                {
                    return length;
                }
                // invalid length
                return 0;
            }
        }

        /// <summary>
        /// Specifies if the interface is administratively enabled or disabled. 
        /// </summary>
        public OperationalStatus OperationalStatus
        {
            get { return (OperationalStatus)BitConverter.ToInt32(m_data, ADMIN_STATUS_OFFSET); }
        }

        /// <summary>
        /// Specifies the operational status of the interface
        /// </summary>
        public InterfaceOperationalStatus InterfaceOperationalStatus
        {
            get { return (InterfaceOperationalStatus)BitConverter.ToInt32(m_data, OPER_STATUS_OFFSET); }
        }

        /// <summary>
        /// Specifies the length of time, in centaseconds (10^-2 sec), that elapsed between January 1, 1601, and the last change of the operational status of the interface (connection). The value rolls over after 2^32 centaseconds
        /// </summary>
        public uint LastChange
        {
            get { return BitConverter.ToUInt32(m_data, LAST_CHANGE_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of octets of data received through this interface
        /// </summary>
        public uint OctetsReceived
        {
            get { return BitConverter.ToUInt32(m_data, IN_OCTETS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of unicast packets received through this interface
        /// </summary>
        public int UnicastPacketsReceived
        {
            get { return BitConverter.ToInt32(m_data, IN_UCAST_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of non-unicast packets received through this interface. Broadcast and multicast packets are included.
        /// </summary>
        public int NonUnicastPacketsReceived
        {
            get { return BitConverter.ToInt32(m_data, IN_NUCAST_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of incoming packets that were discarded even though they did not have errors. 
        /// </summary>
        public int DiscardedIncomingPackets
        {
            get { return BitConverter.ToInt32(m_data, IN_DISCARDS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of incoming packets that were discarded because of errors.
        /// </summary>
        public int ErrorIncomingPackets
        {
            get { return BitConverter.ToInt32(m_data, IN_ERRORS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of incoming packets that were discarded because the protocol was unknown
        /// </summary>
        public int UnknownIncomingPackets
        {
            get { return BitConverter.ToInt32(m_data, IN_UNK_PROTOS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of octets of data sent through this interface.
        /// </summary>
        public uint OctetsSent
        {
            get { return BitConverter.ToUInt32(m_data, OUT_OCTETS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of unicast packets sent through this interface.
        /// </summary>
        public int UnicastPacketsSent
        {
            get { return BitConverter.ToInt32(m_data, OUT_UCAST_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of non-unicast packets sent through this interface. Broadcast and multicast packets are included.
        /// </summary>
        public int NonUnicastPacketsSent
        {
            get { return BitConverter.ToInt32(m_data, OUT_NUCAST_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of outgoing packets that were discarded even though they did not have errors. 
        /// </summary>
        public int DiscardedOutgoingPackets
        {
            get { return BitConverter.ToInt32(m_data, OUT_DISCARDS_OFFSET); }
        }

        /// <summary>
        /// Specifies the number of outgoing packets that were discarded because of errors.
        /// </summary>
        public int ErrorOutgoingPackets
        {
            get { return BitConverter.ToInt32(m_data, OUT_ERRORS_OFFSET); }
        }

        /// <summary>
        /// Specifies the output queue length
        /// </summary>
        public int OutputQueueLength
        {
            get { return BitConverter.ToInt32(m_data, OUT_QLEN_OFFSET); }
        }

        /// <summary>
        /// Specifies the length of the physical address specified by the bPhysAddr member. 
        /// </summary>
        private int DescLength
        {
            get { return BitConverter.ToInt32(m_data, DESC_LEN_OFFSET); }
        }

        /// <summary>
        /// Gets a System.String that describes this interface.
        /// </summary>
        public string Description
        {
            get
            {
                if (DescLength == 0)
                {
                    return "";
                }
                return Encoding.ASCII.GetString(m_data, DESC_OFFSET, DescLength).TrimEnd('\0');
            }
        }
    }
}
