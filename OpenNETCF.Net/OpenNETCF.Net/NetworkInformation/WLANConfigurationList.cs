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
    RAW_DATA data;

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
      data = rd;
    }

    public WLANConfigurationList(uint itemCount)
    {
      // We are creating a new list and the caller
      // wants it to contain itemCount items.
      int elemSize = WLANConfiguration.SizeOf;
      byte[] d = new byte[ConfigOffset + elemSize * itemCount];
      data = new RAW_DATA(d);

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
            (IntPtr)((uint)(data.lpDataDirect) + ConfigOffset + (int)elemSize * i),
            buint.Length);
      }
    }

    public uint NumberOfItems
    {
      get
      {
        if ((data.lpData == null) || (data.lpData.Length < 4))
          return 0;
        else
          return BitConverter.ToUInt32(data.lpData, NumberOfItemsOffset);
      }
      set
      {
        byte[] buint = BitConverter.GetBytes(value);
        Marshal.Copy(buint, 0,
            (IntPtr)((uint)(data.lpDataDirect) + NumberOfItemsOffset),
            buint.Length);
      }
    }

    public uint BaseIndex
    {
      get { return BitConverter.ToUInt32(data.lpData, BaseIndexOffset); }
      set
      {
        byte[] buint = BitConverter.GetBytes(value);
        Marshal.Copy(buint, 0,
            (IntPtr)((uint)(data.lpDataDirect) + BaseIndexOffset),
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
        currentSize = BitConverter.ToInt32(data.lpData, ConfigOffset);
        offset += currentSize;
      }

      WLANConfiguration config = new WLANConfiguration(currentSize);
      //jsm Bug 148 - BlockCopy was referencing data beyond data.lpData and causing ArgumentOutOfBounds Exception
      int bytesToCopyA = data.lpData.Length - (offset - currentSize);	//this is the most we'll ever take
      int bytesToCopyB = bytesToCopyA <= currentSize ? bytesToCopyA : currentSize;
      Buffer.BlockCopy(data.lpData, offset - currentSize, config.Data, 0, bytesToCopyB);
      //Buffer.BlockCopy(data.lpData, offset - currentSize, config.Data, 0, currentSize);
      return config;
    }

    public void SetItem(int index, WLANConfiguration wlc)
    {
      // Make sure data array is large enough to get element size.
      if ((ConfigOffset + sizeof(int)) <= data.lpData.Length)
      {
        // Figure out how big each element in the array
        // is.
        int elemSize = BitConverter.ToInt32(data.lpData, ConfigOffset);

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
            (IntPtr)((uint)(data.lpDataDirect) + ConfigOffset + index * (int)elemSize),
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
        return data;
      }
    }
  }
}
