using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// This class represents the data returned by the 
    /// OID_802_11_BSSID_LIST query to an RF Ethernet adapter.
    /// It is just used during parsing of the returned data
    /// and really should not persist.
    /// </summary>
    internal class NDIS_802_11_BSSID_LIST
    {
        internal byte[] data;

        protected const int NumberOfItemsOffset = 0;
        protected const int BssidOffset = 4;

        public NDIS_802_11_BSSID_LIST(byte[] d)
        {
            data = d;
        }

        public uint NumberOfItems
        {
            get { return BitConverter.ToUInt32(data, NumberOfItemsOffset); }
        }

        public BSSID Item(int index)
        {
            // You can't just do this!  Some adapters seem
            // to use different sizes for *each element* of
            // the array!  You have to step from one to the
            // next to the next to get to the indicated item
            // index.
            int offset = 0;
            for (int i = 0; i < index; i++)
            {
                // Get the length of the item we are presently
                // pointing to.
                int len = BitConverter.ToInt32(data, BssidOffset + offset);
                offset += len;
            }

            // Return the current offset.  This is the start
            // of the data for the indicated item in the list.
            return new BSSID(data, BssidOffset + offset);
        }
    }
}
