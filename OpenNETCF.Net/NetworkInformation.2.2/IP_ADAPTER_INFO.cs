using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    internal class IP_ADAPTER_INFO
    {
        protected byte[] m_data;

        protected const int IP_ADAPTER_INFO_SIZE = 640;

        unsafe internal IP_ADAPTER_INFO(byte* pData)
        {
            m_data = new byte[IP_ADAPTER_INFO_SIZE];

            Marshal.Copy(new IntPtr(pData), m_data, 0, IP_ADAPTER_INFO_SIZE);
        }

        // Accessors for fields of the item.
        protected const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        protected const int MAX_ADAPTER_NAME_LENGTH = 256;
        protected const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

        protected const int NextOffset = 0;
        protected const int ComboIndexOffset = NextOffset + 4;
        protected const int AdapterNameOffset = ComboIndexOffset + 4;
        protected const int DescriptionOffset = AdapterNameOffset + MAX_ADAPTER_NAME_LENGTH + 4;
        protected const int PhysAddressLengthOffset = DescriptionOffset + MAX_ADAPTER_DESCRIPTION_LENGTH + 4;
        protected const int PhysAddressOffset = PhysAddressLengthOffset + 4;
        protected const int IndexOffset = PhysAddressOffset + MAX_ADAPTER_ADDRESS_LENGTH;
        protected const int TypeOffset = IndexOffset + 4;
        protected const int DHCPEnabledOffset = TypeOffset + 4;
        protected const int CurrentIpAddressOffset = DHCPEnabledOffset + 4;
        protected const int GatewayListOffset = IpAddressListOffset + 4 + 16 + 16 + 4;
        protected const int DHCPServerOffset = GatewayListOffset + 4 + 16 + 16 + 4;
        protected const int HaveWINSOffset = DHCPServerOffset + 4 + 16 + 16 + 4;
        protected const int PrimaryWINSServerOffset = HaveWINSOffset + 4;
        protected const int SecondaryWINSServerOffset = PrimaryWINSServerOffset + 4 + 16 + 16 + 4;
        protected const int LeaseObtainedOffset = SecondaryWINSServerOffset + 4 + 16 + 16 + 4;
        protected const int LeaseExpiresOffset = LeaseObtainedOffset + 4;

        public uint Next
        {
            get { return BitConverter.ToUInt32(m_data, NextOffset); }
        }

        public int ComboIndex
        {
            get { return BitConverter.ToInt32(m_data, ComboIndexOffset); }
        }

        public String AdapterName
        {
            get
            {
                String s = Encoding.ASCII.GetString(m_data, AdapterNameOffset, MAX_ADAPTER_NAME_LENGTH);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        public String Description
        {
            get
            {
                String s = Encoding.ASCII.GetString(m_data, DescriptionOffset, MAX_ADAPTER_DESCRIPTION_LENGTH);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        public int PhysicalAddressLength
        {
            get { return BitConverter.ToInt32(m_data, PhysAddressLengthOffset); }
        }

        public PhysicalAddress PhysicalAddress
        {
            get
            {
                int len = PhysicalAddressLength;
                byte[] b = new byte[len];
                Array.Copy(m_data, PhysAddressOffset, b, 0, len);
                PhysicalAddress pa;
                try
                {
                    pa = new PhysicalAddress(b);
                }
                catch
                {
                    return PhysicalAddress.None;
                }
                return pa;
            }
        }

        public int Index
        {
            get { return BitConverter.ToInt32(m_data, IndexOffset); }
        }

        public NetworkInterfaceType Type
        {
            get { return  (NetworkInterfaceType)BitConverter.ToInt32(m_data, TypeOffset); }
        }

        public bool DHCPEnabled
        {
            get { return BitConverter.ToBoolean(m_data, DHCPEnabledOffset); }
        }

        public IPAddress CurrentIpAddress
        {
            // The CurrentIpAddress field is a pointer to an 
            // IP_ADDRESS_STRING structure, not a string itself,
            // so we have to do some magic to make this work.
            get
            {
                IntPtr p = new IntPtr(BitConverter.ToInt32(m_data, CurrentIpAddressOffset));
                if (p == IntPtr.Zero)
                    return null;

                // Here, I'm going to extract the 16 bytes of
                // the IP address string from the data pointed
                // to by the CurrentIpAddress pointer.  The
                // IP address part of what's pointed to starts
                // at offset 4 from the pointer value (skip the
                // Next field).  From there, we just copy 16
                // bytes, the length of the IP address string,
                // to a local byte array, which can easily be
                // converted to a managed string below.
                byte[] b = new byte[16];
                IntPtr p4 = new IntPtr(p.ToInt32() + 4);
                Marshal.Copy(p4, b, 0, 16);

                // The string itself is stored after the Next
                // field in the IP_ADDRESS_STRING structure
                // (offset 4).
                String s = Encoding.ASCII.GetString(b, 0, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    s = s.Substring(0, l);

                return IPAddress.Parse(s);
            }
        }

        // The current subnet mask is part of the same
        // IP_ADDRESS_STRING that contains the CurrentIpAddress.
        // We simply extract a different piece of that 
        // structure to get it.s
        public IPAddress CurrentSubnetMask
        {
            // The CurrentIpAddress field is a pointer to an 
            // IP_ADDRESS_STRING structure, not a string itself,
            // so we have to do some magic to make this work.
            get
            {
                IntPtr p = new IntPtr(BitConverter.ToInt32(m_data, CurrentIpAddressOffset));
                if (p == IntPtr.Zero)
                    return null;

                // Here, I'm going to extract the 16 bytes of
                // the subnet address string from the data pointed
                // to by the CurrentIpAddress pointer.  The
                // mask address part of what's pointed to starts
                // at offset 4+16 from the pointer value (skip 
                // the Next field and the IP address field, 
                // which has length 16).  From there, we just 
                // copy 16 bytes, the length of the mask 
                // string, to a local byte array, which can 
                // easily be converted to a managed string 
                // below.
                byte[] b = new byte[16];
                IntPtr p4 = new IntPtr(p.ToInt32() + 4 + 16);
                Marshal.Copy(p4, b, 0, 16);

                // The string itself is stored after the Next
                // and IpAddresss fields in the 
                // IP_ADDRESS_STRING structure (offset 4 + 16).
                String s = Encoding.ASCII.GetString(b, 0, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    s = s.Substring(0, l);

                return IPAddress.Parse(s);
            }
        }

        protected const int IpAddressListOffset = CurrentIpAddressOffset + 4;

        // The offset is the start of the address list plus the
        // size of the IP_ADDRESS_STRING structure which includes
        // the Next, IpAddress, IpMask, and Context fields.
        public unsafe GatewayIPAddressInformationCollection GatewayList
        {
            get
            {
                UnicastIPAddressInformationCollection collection = new UnicastIPAddressInformationCollection();
                fixed (byte* p = m_data)
                {
                    byte* pAddressList = p;
                    pAddressList += GatewayListOffset;
                    collection = IPUtility.ParseAddressesFromPointer(pAddressList);
                }
                return (GatewayIPAddressInformationCollection)collection;
            }
        }

        public unsafe IPAddressCollection DHCPServerList
        {
            get
            {
                UnicastIPAddressInformationCollection collection = new UnicastIPAddressInformationCollection();
                fixed (byte* p = m_data)
                {
                    byte* pAddressList = p;
                    pAddressList += DHCPServerOffset;
                    collection = IPUtility.ParseAddressesFromPointer(pAddressList);
                }
                return (IPAddressCollection)collection;
            }
        }

        public bool HaveWINS
        {
            get { return BitConverter.ToBoolean(m_data, HaveWINSOffset); }
        }

        public unsafe IPAddressCollection PrimaryWINSServer
        {
            get
            {
                UnicastIPAddressInformationCollection collection = new UnicastIPAddressInformationCollection();
                fixed (byte* p = m_data)
                {
                    byte* pAddressList = p;
                    pAddressList += PrimaryWINSServerOffset;
                    collection = IPUtility.ParseAddressesFromPointer(pAddressList);
                }
                return (IPAddressCollection)collection;
            }
        }

        public unsafe IPAddressCollection SecondaryWINSServer
        {
            get
            {
                UnicastIPAddressInformationCollection collection = new UnicastIPAddressInformationCollection();
                fixed (byte* p = m_data)
                {
                    byte* pAddressList = p;
                    pAddressList += PrimaryWINSServerOffset;
                    collection = IPUtility.ParseAddressesFromPointer(pAddressList);
                }
                return (IPAddressCollection)collection;
            }
        }

        internal static DateTime Time_tToDateTime(uint time_t)
        {
            long win32FileTime = 10000000 * (long)time_t + 116444736000000000;
            return DateTime.FromFileTimeUtc(win32FileTime);
        }

        public DateTime LeaseObtained
        {
            get { return Time_tToDateTime(BitConverter.ToUInt32(m_data, LeaseObtainedOffset)); }
        }

        public DateTime LeaseExpires
        {
            get { return Time_tToDateTime(BitConverter.ToUInt32(m_data, LeaseExpiresOffset)); }
        }

        public static implicit operator byte[](IP_ADAPTER_INFO ipinfo)
        {
            return ipinfo.m_data;
        }
    }
}
