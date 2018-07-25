using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using OpenNETCF.ComponentModel;
using System.Reflection;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// This class represents the IP-to-physical address mapping table (ARP table) for all NetworkInterfaces on the local device
    /// </summary>
    public class ArpTable : IEnumerable<ArpEntry>, IDisposable
    {
        /// <summary>
        /// Fired when the ARP table changes
        /// </summary>
        public event EventHandler OnChange;

        private NetworkInterface[] m_adapters;
        private Thread m_notifyWatchThread;
        private List<ArpEntry> m_entryList;

        private static ArpTable m_singleton;

        /// <summary>
        /// Gets the local device's IP Routing Table
        /// </summary>
        /// <returns></returns>
        public static ArpTable GetArpTable()
        {
            if (m_singleton == null)
            {
                m_singleton = new ArpTable();
            }

            return m_singleton;
        }

        private ArpTable()
        {
            Refresh();

            m_notifyWatchThread = new Thread(new ThreadStart(NotificationThreadProc));
            m_notifyWatchThread.IsBackground = true;
            m_notifyWatchThread.Start();
        }

        private void NotificationThreadProc()
        {
            IntPtr waitHandle = IntPtr.Zero;

            int errorCode = NativeMethods.NotifyAddrChange(ref waitHandle, IntPtr.Zero);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            while (true)
            {
                if (NativeMethods.WaitForSingleObject(waitHandle, 1000) == 0)
                {
                    Refresh();

                    if (OnChange != null)
                    {
                        OnChange(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// This function creates an Address Resolution Protocol (ARP) entry in the ARP table on the local computer
        /// </summary>
        /// <param name="item">ArpEntry to add to the table</param>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The Win32 call to CreateIpNetEntry failed
        /// </exception>
        public void Add(ArpEntry item)
        {
            int errorCode = NativeMethods.CreateIpNetEntry(item.GetEntryBytes());

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// repopulates the internal ARP list
        /// </summary>
        internal void Refresh()
        {
            m_adapters = NetworkInterface.GetAllNetworkInterfaces();

            byte[] data;
            int size = 0;

            // get required size
            int errorCode = NativeMethods.GetIpNetTable(IntPtr.Zero, ref size, 1);

            if (errorCode == NativeMethods.NO_DATA)
            {
                m_entryList = new List<ArpEntry>();
                return;
            }
            // allocate
            data = new byte[size];

            errorCode = NativeMethods.GetIpNetTable(data, ref size, 1);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            /*
            typedef struct _MIB_IPNETTABLE {
              DWORD dwNumEntries; 
              MIB_IPNETROW table[ANY_SIZE]; 
            } MIB_IPNETTABLE, *PMIB_IPNETTABLE;          
            */

            int entries = BitConverter.ToInt32(data, 0);
            int offset = 4;
            m_entryList = new List<ArpEntry>(entries);
            for (int i = 0; i < entries; i++)
            {
                ArpEntry entry = new ArpEntry(data, offset, m_adapters);

                m_entryList.Add(entry);

                offset += ArpEntry.SIZE;
            }
        }

        /// <summary>
        /// Clears all ARP entries for the specified NetworkInterface
        /// </summary>
        /// <param name="adapter"></param>
        public void Clear(NetworkInterface adapter)
        {
            NativeMethods.FlushIpNetTable(adapter.Index);
            Refresh();
        }

        /// <summary>
        /// Clears all ARP entries for all NetworkInterfaces
        /// </summary>
        public void Clear()
        {
            m_adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in m_adapters)
            {
                NativeMethods.FlushIpNetTable(adapter.Index);
            }
            Refresh();
        }

        /// <summary>
        /// Checks whether the collection contains the specified System.Net.NetworkInformation.ArpEntry
        /// object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(ArpEntry item)
        {
            foreach(ArpEntry entry in m_entryList)
            {
                if(entry.Equals(item))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Copies the collection to the specified array.
        /// </summary>
        /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
        /// <param name="arrayIndex">The zero-based index in array at which the copy begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// array is multidimensional.-or- offset is equal to or greater than the length
        /// of array.-or- The number of elements in this OpenNETCF.Net.NetworkInformation.ArpEntry
        /// is greater than the available space from offset to the end of the destination
        /// array.
        /// </exception>
        public void CopyTo(ArpEntry[] array, int arrayIndex)
        {
            m_entryList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of OpenNETCF.Net.NetworkInformation.ArpEntry types
        /// in this collection.
        /// </summary>
        public int Count
        {
            get { return m_entryList.Count; }
        }

        /// <summary>
        /// This function deletes an ARP entry from the ARP table on the local computer
        /// </summary>
        /// <param name="item">ArpEntry to remove</param>
        /// <returns>true if removal was successful, otherwise false</returns>
        public bool Remove(ArpEntry item)
        {
            int errorCode = NativeMethods.DeleteIpNetEntry(item.GetEntryBytes());

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return errorCode == NativeMethods.NO_ERROR;
        }


        /// <summary>
        /// Returns an enumerator that iterates through the routes in the table
        /// </summary>
        /// <returns>An IEnumerator</returns>
        public IEnumerator<ArpEntry> GetEnumerator()
        {
            return m_entryList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_entryList.GetEnumerator();
        }

        /// <summary>
        /// This function sends an ARP request to obtain the physical address that corresponds to the specified destination IP address.
        /// </summary>
        /// <param name="destination">Specifies the destination IP address. The ARP request attempts to obtain the physical address that corresponds to this IP address.</param>
        /// <returns>The physical address that corresponds to the IP address specified by the destination parameter</returns>
        public static PhysicalAddress SendArpRequest(IPAddress destination)
        {
            return SendArpRequest(destination, null);
        }

        /// <summary>
        /// This function sends an ARP request to obtain the physical address that corresponds to the specified destination IP address.
        /// </summary>
        /// <param name="destination">Specifies the destination IP address. The ARP request attempts to obtain the physical address that corresponds to this IP address.</param>
        /// <param name="source">Specifies the IP address of the sender.</param>
        /// <returns>The physical address that corresponds to the IP address specified by the destination parameter</returns>
        public static PhysicalAddress SendArpRequest(IPAddress destination, IPAddress source)
        {
            uint destAddress = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);
            uint srcAddress = 0;

            if ((source != null) && (source != IPAddress.None))
            {
                srcAddress = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);
            }

            uint mac = 0;
            uint macLen = 0;

            int errorCode = NativeMethods.SendARP(destAddress, srcAddress, out mac, out macLen);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            // TODO: check this.  Not sure how a 6-byte MAC is supposed to fit into a 4-byte ULONG, but the PB docs and headers show it this way
            PhysicalAddress address = new PhysicalAddress(BitConverter.GetBytes(mac));

            return address;
        }

        /// <summary>
        /// This function creates a Proxy Address Resolution Protocol (PARP) entry on the local computer for the specified IP address.
        /// </summary>
        /// <param name="adapter">The NetworkInterface on which to proxy ARP for the IP address specified by the address parameter. In other words, when an ARP request for address is received on this interface, the local computer responds with the physical address of this interface. If this interface is of a type that does not support ARP, such as PPP, then the call fails.</param>
        /// <param name="address">Specifies the IP address for which this computer acts as a proxy. The address must meet the following condition: (address &amp; mask) == address.</param>
        /// <param name="mask">Specifies the mask for the IP address specified by the dwAddress parameter. This can be either a host mask, for example, 255.255.255.255, or a subnet mask. If a host mask is used, a proxy is created only for the specified address. If a subnet mask is used, a proxy is created for all IP addresses within the subnet, except for the subnet broadcast address. In the subnet broadcast address, unmasked bits are either all set to zero (0) or all set to 1. </param>
        public static void CreateProxyArpEntry(NetworkInterface adapter, IPAddress address, IPAddress mask)
        {
            uint intAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint intMask = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);

            int errorCode = NativeMethods.CreateProxyArpEntry(intAddress, intMask, adapter.Index);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// This function deletes the PARP entry on the local computer specified by the address and adapter parameters.
        /// </summary>
        /// <param name="adapter">The NetworkInterface on which this computer is supporting proxy ARP for the IP address specified by the address parameter</param>
        /// <param name="address">The IP address for which this computer is acting as a proxy. </param>
        /// <param name="mask">The subnet mask for the IP address specified by the address parameter.</param>
        public static void DeleteProxyArpEntry(NetworkInterface adapter, IPAddress address, IPAddress mask)
        {
            uint intAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint intMask = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);

            int errorCode = NativeMethods.DeleteProxyArpEntry(intAddress, intMask, adapter.Index);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// Gets or sets an entry in the ARP table based on index
        /// </summary>
        /// <param name="index">Position of the entry</param>
        /// <returns>An ArpEntry instance</returns>
        public ArpEntry this[int index]
        {
            get
            {
                return m_entryList[index];
            }
            set
            {
                int errorCode = NativeMethods.SetIpNetEntry(value.GetEntryBytes());
                if (errorCode != NativeMethods.NO_ERROR)
                {
                    throw new NetworkInformationException(errorCode);
                }
            }
        }

        /// <summary>
        /// Releases resources for the ARP table
        /// </summary>
        public void Dispose()
        {
            try
            {
                m_notifyWatchThread.Abort();
            }
            finally
            {
                m_entryList = null;
                m_adapters = null;
                m_singleton = null;
            }
        }
    }
}
