using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Encapsualtes the local device's IP routing table
    /// </summary>
    public class IPRoutingTable : IEnumerable<IPForwardEntry>, IDisposable
    {
        /// <summary>
        /// Fired when the routing tbale changes
        /// </summary>
        public event EventHandler OnChange;

        private INetworkInterface[] m_adapters;
        private Thread m_notifyWatchThread;
        private List<IPForwardEntry> m_routeList;
        private static IPRoutingTable m_singleton;

        /// <summary>
        /// Gets the local device's IP Routing Table
        /// </summary>
        /// <returns></returns>
        public static IPRoutingTable GetRoutingTable()
        {
            if (m_singleton == null)
            {
                m_singleton = new IPRoutingTable();
            }

            return m_singleton;
        }

        private IPRoutingTable()
        {
            Refresh();

            m_notifyWatchThread = new Thread(new ThreadStart(NotificationThreadProc));
            m_notifyWatchThread.IsBackground = true;
            m_notifyWatchThread.Start();
        }

        private void NotificationThreadProc()
        {
            IntPtr waitHandle = IntPtr.Zero;

            int errorCode = NativeMethods.NotifyRouteChange(ref waitHandle, IntPtr.Zero);
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
        /// This function creates a route in the local computer's IP routing table.
        /// </summary>
        /// <param name="item">IPForwardEntry to add to the table</param>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The Win32 call to CreateIpForwardEntry failed
        /// </exception>
        public void Add(IPForwardEntry item)
        {
            uint address = BitConverter.ToUInt32(item.Destination.GetAddressBytes(), 0);
            uint netmask = BitConverter.ToUInt32(item.SubnetMask.GetAddressBytes(), 0);

            if ((address & netmask) != address)
            {
                throw new ArgumentException("SubnetMask is invalid (destination & netmask != destination)");
            }

            // update/validate fields by cloning
            IPForwardEntry itemClone = item.Clone();
            itemClone.Age = 0;
            itemClone.Protocol = IPProtocol.NetworkManagement;
            itemClone.RouteType = RouteType.Final;
            itemClone.NextHopSystemNumber = 0;

            int errorCode = NativeMethods.CreateIpForwardEntry(itemClone.GetEntryBytes());

            if (errorCode == NativeMethods.INVALID_ARGUMENT)
            {
                throw new ArgumentException("Unable to add route.  Ensure it is valid.");
            }

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// repopulates the internal route table
        /// </summary>
        internal void Refresh()
        {
            byte[] data;
            int size = 0;

            // get required size
            int errorCode = NativeMethods.GetIpForwardTable(IntPtr.Zero, ref size, 1);

            if (errorCode == NativeMethods.NO_DATA)
            {
                return;
            }
            // allocate
            data = new byte[size];

            errorCode = NativeMethods.GetIpForwardTable(data, ref size, 1);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            m_adapters = NetworkInterface.GetAllNetworkInterfaces();

            int entries = BitConverter.ToInt32(data, 0);
            int offset = 4;
            m_routeList = new List<IPForwardEntry>(entries);

            int loopbackIndex = NetworkInterface.LoopbackInterfaceIndex;

            for (int i = 0; i < entries; i++)
            {
                IPForwardEntry entry = new IPForwardEntry(data, offset, m_adapters);
                if (entry.NetworkInterface == null)
                {
                    entry.AdapterIndex = loopbackIndex;
                }

                m_routeList.Add(entry);

                offset += IPForwardEntry.SIZE;
            }
        }

        /// <summary>
        /// Copies the collection to the specified array.
        /// </summary>
        /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
        /// <param name="arrayIndex">The zero-based index in array at which the copy begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// array is multidimensional.-or- offset is equal to or greater than the length
        /// of array.-or- The number of elements in this OpenNETCF.Net.NetworkInformation.IPForwardEntry
        /// is greater than the available space from offset to the end of the destination
        /// array.
        /// </exception>
        public void CopyTo(IPForwardEntry[] array, int arrayIndex)
        {
            m_routeList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of OpenNETCF.Net.NetworkInformation.IPForwardEntry types
        /// in this collection.
        /// </summary>
        public int Count
        {
            get { return m_routeList.Count; }
        }

        /// <summary>
        /// This function deletes an existing route in the local computer's IP routing table.
        /// </summary>
        /// <param name="destination">IPAddress of the destination to which the route should be deleted</param>
        /// <returns>true if removal was successful, otherwise false</returns>
        public unsafe bool Remove(IPAddress destination)
        {
            byte[] data;
            int size = 0;
            uint dest = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);

            // get required size
            int errorCode = NativeMethods.GetIpForwardTable(IntPtr.Zero, ref size, 1);

            if (errorCode == NativeMethods.NO_DATA)
            {
                return false;
            }

            // allocate
            data = new byte[size];

            errorCode = NativeMethods.GetIpForwardTable(data, ref size, 1);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            bool success = true;

            // walk the routing table, removing any route info with this destination
            fixed (byte* pData = data)
            {
                byte* p = pData;

                int entries = *(int*)p;
                p += 4;

                for (int i = 0; i < entries; i++)
                {
                    if (*(uint*)p == dest)
                    {
                        errorCode = NativeMethods.DeleteIpForwardEntry(p);
                        success = (success && (errorCode == NativeMethods.NO_ERROR));
                    }
                    p += IPForwardEntry.SIZE;
                }
            }

            Refresh();

            return success;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the routes in the table
        /// </summary>
        /// <returns>An IEnumerator</returns>
        public IEnumerator<IPForwardEntry> GetEnumerator()
        {
            return m_routeList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_routeList.GetEnumerator();
        }

        /// <summary>
        /// Gets an IPForwardEntry from the table based on its index
        /// </summary>
        /// <param name="index">The table position</param>
        /// <returns>An IPForwardEntry</returns>
        public IPForwardEntry this[int index]
        {
            get
            {
                return m_routeList[index];
            }
            set
            {
                int errorCode = NativeMethods.SetIpForwardEntry(value.GetEntryBytes());
                if (errorCode != NativeMethods.NO_ERROR)
                {
                    throw new NetworkInformationException(errorCode);
                }
            }
        }

        /// <summary>
        /// Gets the round-trip time in milliseconds to the destination specified. 
        /// </summary>
        /// <param name="destination">IP adddress of the destination</param>
        /// <returns>Round-trip time in milliseconds</returns>
        public static int GetRoundTripTime(IPAddress destination)
        {
            int hops = 0;
            int maxHops = 100;
            int rtt = 0;

            uint address = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);
            int errorCode = NativeMethods.GetRTTAndHopCount(address, ref hops, maxHops, ref rtt);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return rtt;
        }

        /// <summary>
        /// Gets the hop count to the destination specified. 
        /// </summary>
        /// <param name="destination">IP adddress of the destination</param>
        /// <returns>Number of hops to to destination</returns>
        public static int GetHopCount(IPAddress destination)
        {
            int hops = 0;
            int maxHops = 100;
            int rtt = 0;

            uint address = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);
            int errorCode = NativeMethods.GetRTTAndHopCount(address, ref hops, maxHops, ref rtt);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return hops;
        }

        /// <summary>
        /// This function retrieves the NetworkInterface that has the best route to the specified IP address.
        /// </summary>
        /// <param name="destination">IP Address of the destination</param>
        /// <returns>The local NetworkInterface that provides the best route</returns>
        public static NetworkInterface GetBestInterface(IPAddress destination)
        {
            if (destination.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new NotSupportedException(
                    string.Format("Destination address family '{0}' not supported", destination.AddressFamily.ToString()));
            }

            uint address = BitConverter.ToUInt32(destination.GetAddressBytes(), 0);
            int index = 0;

            int errorCode = NativeMethods.GetBestInterface(address, ref index);
            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            foreach (NetworkInterface intf in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (intf.Index == index)
                {
                    return intf;
                }
            }

            return null;
        }

        /// <summary>
        /// Releases resources for this instance
        /// </summary>
        public void Dispose()
        {
            try
            {
                m_notifyWatchThread.Abort();
            }
            finally
            {
                m_routeList = null;
                m_adapters = null;
                m_singleton = null;
            }
        }
    }
}
