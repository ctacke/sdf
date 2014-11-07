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

        private int NumberOfItemsOffset = 0;
        private int FIRST_INDEX_OFFSET = 0;
        private int BssidOffset = 0;

        private bool m_isWzc;

        public NDIS_802_11_BSSID_LIST(byte[] d, bool isWzc)
        {
            data = d;

            NumberOfItemsOffset = 0;
            FIRST_INDEX_OFFSET = (isWzc) ? 4 : 0;
            BssidOffset = NumberOfItemsOffset + FIRST_INDEX_OFFSET + 4;
            m_isWzc = isWzc;
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
            return new BSSID(data, BssidOffset + offset, m_isWzc);
        }
    }
}
