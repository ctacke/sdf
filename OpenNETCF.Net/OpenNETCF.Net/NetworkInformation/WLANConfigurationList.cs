using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// This class represents the data returned by the 
  /// WZCQueryInterface() call to WZC.  There are several
  /// values in the INTF_ENTRY struct returned by that
  /// query, including a couple of SSID lists (those SSID
  /// values currently 'audible' to the adapter and the
  /// preferred list).  This type is how those lists are
  /// returned.
  /// </summary>
  internal class WLANConfigurationList
  {
        private RAW_DATA m_data;
        private bool m_dataNeedsDisposal = false;

    // The memory layout of this structure is:
    //	ULONG           NumberOfItems;  // number of elements in the array below
    //	ULONG           Index;          // [start] index in the array below
    //	WZC_WLAN_CONFIG Config[1];      // array of WZC_WLAN_CONFIGs

    // ctacke: 12/13/06 WZC_WLAN_CONFIG length appears to be 164 bytes

    static int NumberOfItemsOffset = 0;
    static int BaseIndexOffset = 4;
    static int ConfigOffset = 8;

    public WLANConfigurationList(RAW_DATA rd)
    {
            m_data = rd;
    }

    public WLANConfigurationList(uint itemCount)
    {
      // We are creating a new list and the caller
      // wants it to contain itemCount items.
      int elemSize = WLANConfiguration.SizeOf;
      byte[] d = new byte[ConfigOffset + elemSize * itemCount];
            m_data = new RAW_DATA(d);
            m_dataNeedsDisposal = true;

      // Set the list up.
      this.NumberOfItems = itemCount;
      this.BaseIndex = 0;

      // Set the size of each element in the array.
      for (int i = 0; i < itemCount; i++)
      {
        // Set the 32-bit value at the start of each
        // WZC_WLAN_CONFIG structure to the size of
        // that structure.
        byte[] buint = BitConverter.GetBytes(elemSize);
        Marshal.Copy(buint, 0,
                    (IntPtr)((uint)(m_data.lpDataDirect) + ConfigOffset + (int)elemSize * i),
            buint.Length);
      }
    }

	// Finalizer to get rid of data class variable (unmanaged resource)
	~WLANConfigurationList()
	{
            if (m_dataNeedsDisposal)
            {
			// We only dispose RAW_DATA if we did the allocation
                m_data.Dispose();
                m_dataNeedsDisposal = false;
            }
	}

    public uint NumberOfItems
    {
      get
      {
                if ((m_data.lpData == null) || (m_data.lpData.Length < 4))
          return 0;
        else
                    return BitConverter.ToUInt32(m_data.lpData, NumberOfItemsOffset);
      }
      set
      {
        byte[] buint = BitConverter.GetBytes(value);
        Marshal.Copy(buint, 0,
                    (IntPtr)((uint)(m_data.lpDataDirect) + NumberOfItemsOffset),
            buint.Length);
      }
    }

    public uint BaseIndex
    {
            get { return BitConverter.ToUInt32(m_data.lpData, BaseIndexOffset); }
      set
      {
        byte[] buint = BitConverter.GetBytes(value);
        Marshal.Copy(buint, 0,
                    (IntPtr)((uint)(m_data.lpDataDirect) + BaseIndexOffset),
            buint.Length);
      }
    }

    public WLANConfiguration Item(int index)
    {
      int offset = ConfigOffset;
      int currentSize = 0;

      // don't assume every element is equal size
      for (int i = 0; i <= index; i++)
      {
                currentSize = BitConverter.ToInt32(m_data.lpData, ConfigOffset);
        offset += currentSize;
      }

      WLANConfiguration config = new WLANConfiguration(currentSize);
      //jsm Bug 148 - BlockCopy was referencing data beyond data.lpData and causing ArgumentOutOfBounds Exception
            int bytesToCopyA = m_data.lpData.Length - (offset - currentSize);	//this is the most we'll ever take
      int bytesToCopyB = bytesToCopyA <= currentSize ? bytesToCopyA : currentSize;
            Buffer.BlockCopy(m_data.lpData, offset - currentSize, config.Data, 0, bytesToCopyB);
      //Buffer.BlockCopy(data.lpData, offset - currentSize, config.Data, 0, currentSize);
      return config;
    }

    public void SetItem(int index, WLANConfiguration wlc)
    {
      // Make sure data array is large enough to get element size.
            if ((ConfigOffset + sizeof(int)) <= m_data.lpData.Length)
      {
        // Figure out how big each element in the array
        // is.
                int elemSize = BitConverter.ToInt32(m_data.lpData, ConfigOffset);

        if (elemSize > wlc.Data.Length)
        {
          elemSize = wlc.Data.Length;
        }
        // Use the actual index to get the indicated element
        // in the Config list.  Note that we use lpDataDirect,
        // so that we're copying to the internal array of our
        // data, not cloning it first, then copying to the clone
        // (which, of course, has no effect).
        Marshal.Copy(wlc.Data, 0,
                    (IntPtr)((uint)(m_data.lpDataDirect) + ConfigOffset + index * (int)elemSize),
            elemSize);
      }
    }

    public RAW_DATA rawData
    {
      get
      {
        // Return the raw data object that represents
        // the list content.  This might be used when
        // changing a preferred list of SSID values for
        // an adapter.
                return m_data;
      }
    }
  }
}
