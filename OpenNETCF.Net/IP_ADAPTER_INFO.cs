using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;
namespace OpenNETCF.Net
{
    internal class IP_ADAPTER_INFO
    {
        protected byte[] data;
        protected uint firstnextIndex = 0;
        protected uint firstnextOffset = 0;
        protected uint ourSize = 0;
        protected uint ourBase = 0;

        // Main constructor.  This figures out how much space
        // is needed to hold the list of adapters, allocates
        // a byte array for that space, and gets the list
        // from GetAdaptersInfo().
        unsafe public IP_ADAPTER_INFO()
        {
            // Find out how much space we need to store the
            // adapter list.
            int size = 0;
            int err = AdapterPInvokes.GetAdaptersInfo(null, ref size);
            if (err != 0)
            {
                // This is what we'd expect: there is not enough room in the
                // buffer, so the size is set and an error is returned.
                if (err == 111)
                {
                    // ToDo: Handle buffer-too-small.
                }
            }

            // Check for size = 0 (no adapters, presumably).
            ourSize = (uint)size;
            if (ourSize == 0)
            {
                data = null;
            }
            else
            {
                data = new byte[size];

                // We need to lock this in memory until we can
                // get its address.  Since GetAdaptersInfo will
                // be storing Next pointers from adapter information
                // to adapter information, we need to know what
                // the base for those pointers is.  We can then
                // calculate the offset into the byte array of
                // IP_ADAPTER_INFO from that.
                // Fix the data array in memory.  We need to do
                // this to store the base address of the array.
                // The GetAdaptersInfo call will put various Next
                // pointers in the structure and we need to know
                // what the base address against which those are
                // measured is.  With that, we can figure out what
                // offset in the data array they reference.
                fixed (byte* b = &data[0])
                {
                    // Save the base address.
                    ourBase = (uint)b;

                    // Actually call GetAdaptersInfo.
                    size = (int)ourSize;
                    err = AdapterPInvokes.GetAdaptersInfo(data, ref size);
                }

                if (err != 0)
                    data = null;
            }
        }

        protected const int IP_ADAPTER_INFO_SIZE = 640;
        protected IP_ADAPTER_INFO(byte[] datain, uint offset)
        {
            // Create an internal-only copy of this structure,
            // making it easy to get to the fields of one
            // of the items in the linked list, based on its
            // offset within the byte[] of the main
            // instance of an IP_ADAPTER_INFO.
            ourSize = IP_ADAPTER_INFO_SIZE;
            data = new byte[IP_ADAPTER_INFO_SIZE];
            Array.Copy(datain, (int)offset, data, 0, IP_ADAPTER_INFO_SIZE);
        }

        public Adapter FirstAdapter()
        {
            if (data == null)
                return null;

            // Reset the indexing.
            firstnextIndex = 0;
            if (this.Next == 0)	// !!!!
                firstnextOffset = 0;	// !!!!
            else						// !!!!
                firstnextOffset = this.Next - ourBase;

            // Since we are creating this adapter based on
            // the first entry in our table, we can just pass
            // 'this' to do it.
            return new Adapter(this);
        }
        public Adapter NextAdapter()
        {
            // Starting at the current offset in our 'data'
            // member, get the Next field, subtrace its pointer
            // from the initial pointer value to find the
            // new offset, and create an adapter starting
            // at that point in the 'data' member.

            if (data == null)
                return null;

            // Handle no more case.
            if (firstnextOffset == 0)
                return null;

            IP_ADAPTER_INFO newinfo = new IP_ADAPTER_INFO(data,
                firstnextOffset);

            firstnextIndex++;

            // Now, use the Next field of the new info 
            // structure to update where we find the next
            // one after that in our list.
            if (newinfo.Next == 0)
                firstnextOffset = 0;
            else
                firstnextOffset = newinfo.Next - ourBase;

            return new Adapter(newinfo);
        }

        // Accessors for fields of the item.
        protected const int NextOffset = 0;
        public uint Next
        {
            get { return BitConverter.ToUInt32(data, NextOffset); }
        }

        protected const int ComboIndexOffset = NextOffset + 4;
        public int ComboIndex
        {
            get { return BitConverter.ToInt32(data, ComboIndexOffset); }
        }

        protected const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        protected const int MAX_ADAPTER_NAME_LENGTH = 256;
        protected const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

        protected const int AdapterNameOffset = ComboIndexOffset + 4;
        public String AdapterName
        {
            get
            {
                String s = Encoding.ASCII.GetString(data, AdapterNameOffset, MAX_ADAPTER_NAME_LENGTH);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int DescriptionOffset = AdapterNameOffset + MAX_ADAPTER_NAME_LENGTH + 4;
        public String Description
        {
            get
            {
                String s = Encoding.ASCII.GetString(data, DescriptionOffset, MAX_ADAPTER_DESCRIPTION_LENGTH);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int PhysAddressLengthOffset = DescriptionOffset + MAX_ADAPTER_DESCRIPTION_LENGTH + 4;
        public int PhysAddressLength
        {
            get { return BitConverter.ToInt32(data, PhysAddressLengthOffset); }
        }

        protected const int PhysAddressOffset = PhysAddressLengthOffset + 4;
        public byte[] PhysAddress
        {
            get
            {
                byte[] b = new byte[MAX_ADAPTER_ADDRESS_LENGTH];
                Array.Copy(data, PhysAddressOffset, b, 0, MAX_ADAPTER_ADDRESS_LENGTH);
                return b;
            }
        }

        protected const int IndexOffset = PhysAddressOffset + MAX_ADAPTER_ADDRESS_LENGTH;
        public int Index
        {
            get { return BitConverter.ToInt32(data, IndexOffset); }
        }

        protected const int TypeOffset = IndexOffset + 4;
        public int Type
        {
            get { return BitConverter.ToInt32(data, TypeOffset); }
        }

        protected const int DHCPEnabledOffset = TypeOffset + 4;
        public bool DHCPEnabled
        {
            get { return BitConverter.ToBoolean(data, DHCPEnabledOffset); }
        }

        protected const int CurrentIpAddressOffset = DHCPEnabledOffset + 4;
        public String CurrentIpAddress
        {
            // The CurrentIpAddress field is a pointer to an 
            // IP_ADDRESS_STRING structure, not a string itself,
            // so we have to do some magic to make this work.
            get
            {
                IntPtr p = new IntPtr(BitConverter.ToInt32(data, CurrentIpAddressOffset));
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
                    return s.Substring(0, l);
                return s;
            }
        }

        // The current subnet mask is part of the same
        // IP_ADDRESS_STRING that contains the CurrentIpAddress.
        // We simply extract a different piece of that 
        // structure to get it.s
        public String CurrentSubnetMask
        {
            // The CurrentIpAddress field is a pointer to an 
            // IP_ADDRESS_STRING structure, not a string itself,
            // so we have to do some magic to make this work.
            get
            {
                IntPtr p = new IntPtr(BitConverter.ToInt32(data, CurrentIpAddressOffset));
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
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int IpAddressListOffset = CurrentIpAddressOffset + 4;
#if notdefined
		public string IpAddressList
		{
			get
			{
				return null;	// ????
			}
		}
#endif

        // The offset is the start of the address list plus the
        // size of the IP_ADDRESS_STRING structure which includes
        // the Next, IpAddress, IpMask, and Context fields.
        protected const int GatewayListOffset = IpAddressListOffset + 4 + 16 + 16 + 4;
        public String Gateway
        {
            // The GatewayList field is a structure of type
            // IP_ADDRESS_STRING.  We have to extract the bits
            // we want.
            get
            {
                // The string itself is stored after the Next
                // field in the IP_ADDRESS_STRING structure
                // (offset 4).
                String s = Encoding.ASCII.GetString(data, GatewayListOffset + 4, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int DHCPServerOffset = GatewayListOffset + 4 + 16 + 16 + 4;
        public String DHCPServer
        {
            // The DhcpServer field is a structure of type
            // IP_ADDRESS_STRING.  We have to extract the bits
            // we want.
            get
            {
                // The string itself is stored after the Next
                // field in the IP_ADDRESS_STRING structure
                // (offset 4).
                String s = Encoding.ASCII.GetString(data, DHCPServerOffset + 4, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int HaveWINSOffset = DHCPServerOffset + 4 + 16 + 16 + 4;
        public bool HaveWINS
        {
            get { return BitConverter.ToBoolean(data, HaveWINSOffset); }
        }

        protected const int PrimaryWINSServerOffset = HaveWINSOffset + 4;
        public String PrimaryWINSServer
        {
            get
            {
                String s = Encoding.ASCII.GetString(data, PrimaryWINSServerOffset + 4, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int SecondaryWINSServerOffset = PrimaryWINSServerOffset + 4 + 16 + 16 + 4;
        public String SecondaryWINSServer
        {
            get
            {
                String s = Encoding.ASCII.GetString(data, SecondaryWINSServerOffset + 4, 16);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        protected const int LeaseObtainedOffset = SecondaryWINSServerOffset + 4 + 16 + 16 + 4;
        public DateTime LeaseObtained
        {
            get { return Marshal2.Time_tToDateTime(BitConverter.ToUInt32(data, LeaseObtainedOffset)); }
        }

        protected const int LeaseExpiresOffset = LeaseObtainedOffset + 4;
        public DateTime LeaseExpires
        {
            get { return Marshal2.Time_tToDateTime(BitConverter.ToUInt32(data, LeaseExpiresOffset)); }
        }

        /*
        IP_ADDR_STRING IpAddressList;
        */

        public static implicit operator byte[](IP_ADAPTER_INFO ipinfo)
        {
            return ipinfo.data;
        }
    }
}
