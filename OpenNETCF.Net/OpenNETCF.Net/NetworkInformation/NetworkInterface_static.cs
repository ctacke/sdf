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
using System.Runtime.InteropServices;
using System.Text;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// Provides configuration and statistical information for a network interface.
  /// </summary>
  partial class NetworkInterface
  {
    /// <summary>
    /// Returns objects that describe the network interfaces on the local computer.
    /// </summary>
    /// <returns>
    /// A System.Net.NetworkInformation.NetworkInterface array that contains objects
    /// that describe the available network interfaces, or an empty array if no interfaces
    /// are detected.
    /// </returns>
    public unsafe static INetworkInterface[] GetAllNetworkInterfaces()
    {

      NetworkInterface[] interfaceList;
      uint size;

      // get buffer size requirement
      NativeMethods.GetInterfaceInfo(null, out size);

      byte[] ifTable = new byte[size];
      // pin the table buffer
      fixed (byte* pifTable = ifTable)
      {
        byte* p = pifTable;

        /* table looks like this:
            typedef struct _IP_INTERFACE_INFO {
              LONG NumAdapters; 
              IP_ADAPTER_INDEX_MAP Adapter[1]; 
            } IP_INTERFACE_INFO, *PIP_INTERFACE_INFO;
                
            typedef struct _IP_ADAPTER_INDEX_MAP {
              ULONG Index; 
              WCHAR Name [MAX_ADAPTER_NAME]; 
            } IP_ADAPTER_INDEX_MAP, *PIP_ADAPTER_INDEX_MAP;
         */

        // get the table data
        NativeMethods.GetInterfaceInfo(pifTable, out size);

        // get interface count
        int interfaceCount = *p;

        interfaceList = new NetworkInterface[interfaceCount];

        p += 4;

        // get each interface
        for (int i = 0; i < interfaceCount; i++)
        {
          // get interface index
          int index = (int)*((int*)p);
          p += 4;

          // get interface name
          byte[] nameBytes = new byte[256];
          Marshal.Copy(new IntPtr(p), nameBytes, 0, nameBytes.Length);
          string name = Encoding.Unicode.GetString(nameBytes, 0, nameBytes.Length);
          int nullIndex = name.IndexOf('\0');
          if (nullIndex > 0)
          {
            name = name.Substring(0, nullIndex);
          }
          p += 256;

          // check the wireless capabilities
          try
          {
            NDISUIO.QueryOID(NDIS_OID.WEP_STATUS, name);

            // didn't throw, so it's wireless - determinine if it's WZC compatible
            INTF_ENTRY entry;
            if (WZC.QueryAdapter(name, out entry) == NativeMethods.NO_ERROR)
            {
              // this is a WZC wireless adapter
              interfaceList[i] = new WirelessZeroConfigNetworkInterface(index, name);
            }
            else
            {
              // this is a non-WZC wireless adapter
              interfaceList[i] = new WirelessNetworkInterface(index, name);
            }
            entry.Dispose();
          }
          catch
          {
            // if it's not wireless, it will throw and end up here
            interfaceList[i] = new NetworkInterface(index, name);
          }
        }
      }

      return interfaceList;
    }

    /// <summary>
    /// Gets the index of the loopback interface.  A return of -1 indicates that no loopback interface exists
    /// </summary>
    public unsafe static int LoopbackInterfaceIndex
    {
      get
      {
        int size = 0;
        byte[] data;

        NativeMethods.GetIfTable(IntPtr.Zero, ref size, true);
        data = new byte[size];
        int errorCode = NativeMethods.GetIfTable(data, ref size, true);

        if (errorCode != NativeMethods.NO_ERROR)
        {
          throw new NetworkInformationException(errorCode);
        }

        fixed (byte* pData = data)
        {
          byte* p = pData;
          int offset = 4;

          int entries = *(int*)p;

          for (int i = 0; i < entries; i++)
          {
            MibIfRow row = new MibIfRow(pData, offset);
            if (row.NetworkInterfaceType == NetworkInterfaceType.Loopback)
            {
              return row.Index;
            }
            offset += MibIfRow.Size;
          }
        }

        IP_ADAPTER_INFO info = GetAdapterInfo(-1);

        if (info == null)
        {
          return -1;
        }

        return info.Index;
      }
    }

    internal unsafe static IP_ADAPTER_INFO GetAdapterInfo(int adapterIndex)
    {
      return GetAdapterInfo(ref adapterIndex, null);
    }

    internal unsafe static IP_ADAPTER_INFO GetAdapterInfo(ref int adapterIndex, string adapterName)
    {
      uint size = 0;

      // get buffer size requirement
      NativeMethods.GetAdaptersInfo(null, ref size);

      byte[] info = new byte[size];

      // get the data
      int errorCode = NativeMethods.GetAdaptersInfo(info, ref size);
      if (errorCode != NativeMethods.NO_ERROR)
      {
        throw new NetworkInformationException(errorCode);
      }

      // walk the list looking fo the requested index
      fixed (byte* pInfo = info)
      {
        // get the index for this adapter
        byte* p = pInfo;
        uint pNext = 0;

        do
        {
          // get the pointer to the next adapter
          // C# is screwy - we have to cast the pointer type before getting the data
          pNext = (uint)*((uint*)p);
          byte* pThis = p;

          p += 8;
          string name = Marshal2.PtrToStringAnsi((IntPtr)p, 0, 256);

          p += 404;
          int index = (int)*((int*)p);

          if (index == adapterIndex)
          {
            return new IP_ADAPTER_INFO(pThis);
          }
          else if ((adapterName != null) && (name == adapterName))
          {
            // the index has changed (Rebind may do this) so update the index
            adapterIndex = index;
            return new IP_ADAPTER_INFO(pThis);
          }
          else if (adapterIndex == -1)
          {
            // looking for localhost
          }

          uint offset = pNext - (uint)p;
          p += offset;
        } while (pNext != 0);

        // if we get here, the index is not found
        return null;

        /*

        #define MAX_ADAPTER_DESCRIPTION_LENGTH  128 // arb.
        #define MAX_ADAPTER_NAME_LENGTH         256 // arb.
        #define MAX_ADAPTER_ADDRESS_LENGTH      8   // arb.

            typedef struct _IP_ADAPTER_INFO {
        000      struct _IP_ADAPTER_INFO* Next;
        004      DWORD ComboIndex;
        008      Char AdapterName[MAX_ADAPTER_NAME_LENGTH + 4];
        268      char Description[MAX_ADAPTER_DESCRIPTION_LENGTH + 4];
        400      UINT AddressLength;
        404      BYTE Address[MAX_ADAPTER_ADDRESS_LENGTH];
        412      DWORD Index;
        416      UINT Type;
        418      UINT DhcpEnabled;
              PIP_ADDR_STRING CurrentIpAddress;
              IP_ADDR_STRING IpAddressList;
              IP_ADDR_STRING GatewayList;
              IP_ADDR_STRING DhcpServer;
              BOOL HaveWins;
              IP_ADDR_STRING PrimaryWinsServer;
              IP_ADDR_STRING SecondaryWinsServer;
              time_t LeaseObtained;
              time_t LeaseExpires;
            } IP_ADAPTER_INFO, *PIP_ADAPTER_INFO;
        */
      }
    }

    internal static MibIfRow GetMibIfRow(int adapterIndex)
    {
      MibIfRow row = new MibIfRow();
      row.Index = adapterIndex;
      int errorCode = NativeMethods.GetIfEntry(row);

      if (errorCode != NativeMethods.NO_ERROR)
      {
        throw new NetworkInformationException(errorCode);
      }

      return row;
    }
  }
}
