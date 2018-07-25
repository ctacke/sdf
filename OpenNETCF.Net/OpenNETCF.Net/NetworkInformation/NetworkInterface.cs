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
using System.Collections;
using System.Text;
using Microsoft.Win32;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{

  /// <summary>
  /// Provides configuration and statistical information for a network interface.
  /// </summary>
  public partial class NetworkInterface : INetworkInterface
  {
    private IP_ADAPTER_INFO m_adapterInfo;
    private MibIfRow m_mibifRow;

    /// <summary>
    /// Creates a NetworkInterface instance
    /// </summary>
    /// <param name="index"></param>
    /// <param name="interfaceName"></param>
    internal NetworkInterface(int index, string interfaceName)
    {
      Index = index;
      Name = interfaceName;
      m_adapterInfo = null;
    }

    internal int Index { get; private set; }

    /// <summary>
    /// Gets the name of the network adapter.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets a System.String that describes this interface.
    /// </summary>
    public string Description
    {
      get
      {
        // get the adapter info if it's not already been done
        if (m_adapterInfo == null)
        {
          m_adapterInfo = GetAdapterInfo(Index);
        }

        return m_adapterInfo.Description;
      }
    }

    /// <summary>
    /// Gets the identifier of the network adapter.
    /// </summary>
    public string Id
    {
      get { return Index.ToString(); }
    }

    /// <summary>
    /// Specifies the type of interface
    /// </summary>
    public NetworkInterfaceType NetworkInterfaceType
    {
      get
      {
        // get the adapter info if it's not already been done
        if (m_adapterInfo == null)
        {
          m_adapterInfo = GetAdapterInfo(Index);
        }

        return m_adapterInfo.Type;
      }
    }

    /// <summary>
    /// Returns the Media Access Control (MAC) address for this adapter.
    /// </summary>
    /// <returns>
    /// A System.Net.NetworkInformation.PhysicalAddress object that contains the
    /// physical address.
    /// </returns>
    public PhysicalAddress GetPhysicalAddress()
    {
      // get the adapter info if it's not already been done
      if (m_adapterInfo == null)
      {
        m_adapterInfo = GetAdapterInfo(Index);
      }
      return m_adapterInfo.PhysicalAddress;
    }

    /// <summary>
    /// The currently active IP address of the adapter.
    /// </summary>
    /// <remarks>After Setting this property, you must Rebind the adapter for it to take effect</remarks>
    public IPAddress CurrentIpAddress
    {
      get
      {
        // get the adapter info if it's not already been done
        if (m_adapterInfo == null)
        {
          m_adapterInfo = GetAdapterInfo(Index);
        }
        return m_adapterInfo.CurrentIpAddress;
      }
      set
      {
        // Modify the registry keys associated with this
        // adapter to set the new static IP.  Once that is done, we
        // have to rebind the network adapter to actually
        // make the change.  
        string regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

        // Open the base key for the adapter.
        RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey(regName, true);

        // Get the current value of the IpAddress value.  If
        // it already matches the value we're trying to set, we
        // don't have to change it.
        object oldIpAddress = tcpipkey.GetValue("IpAddress", "");

        if (oldIpAddress is string[])
        {
          if (((string[])oldIpAddress)[0] != value.ToString())
          {
            // Make the change.
            tcpipkey.SetValue("IpAddress", new string[] { value.ToString() });
          }
        }
        else
        {
          if ((string)oldIpAddress != value.ToString())
          {
            // Make the change.
            tcpipkey.SetValue("IpAddress", new string[] { value.ToString() });
          }
        }


        tcpipkey.Close();

        Rebind();
      }
    }

    /// <summary>
    /// The currently active subnet mask address of the 
    /// adapter.
    /// </summary>
    /// <remarks>After Setting this property, you must Rebind the adapter for it to take effect</remarks>
    public IPAddress CurrentSubnetMask
    {
      get
      {
        // get the adapter info if it's not already been done
        if (m_adapterInfo == null)
        {
          m_adapterInfo = GetAdapterInfo(Index);
        }
        return m_adapterInfo.CurrentSubnetMask;
      }
      set
      {
        // Modify the registry keys associated with this
        // adapter to set the new subnet mask.  Once that is done, we
        // have to rebind the network adapter to actually
        // make the change.  
        string regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

        // Open the base key for the adapter.
        RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey(regName, true);

        // Get the current value of the SubnetMask value.  If
        // it already matches the value we're trying to set, we
        // don't have to change it.
        object result = tcpipkey.GetValue("SubnetMask", string.Empty);
        string oldSubnet;
        if (result is string[])
        {
          oldSubnet = ((string[])result)[0];
        }
        else
        {
          oldSubnet = (string)result;
        }

        if (oldSubnet != value.ToString())
        {
          // Make the change.
          tcpipkey.SetValue("SubnetMask", new string[] { value.ToString() });
        }

        tcpipkey.Close();

        Rebind();
      }
    }

    /// <summary>
    /// Specifies if the interface is administratively enabled or disabled. 
    /// </summary>
    public OperationalStatus OperationalStatus
    {
      get
      {
        if (m_mibifRow == null)
        {
          m_mibifRow = GetMibIfRow(Index);
        }

        return m_mibifRow.OperationalStatus;
      }
    }

    /// <summary>
    /// Specifies the operational status of the interface
    /// </summary>
    public InterfaceOperationalStatus InterfaceOperationalStatus
    {
      get
      {
        if (m_mibifRow == null)
        {
          m_mibifRow = GetMibIfRow(Index);
        }

        return m_mibifRow.InterfaceOperationalStatus;
      }
    }

    /// <summary>
    /// Specifies the speed of the interface in bits per second
    /// </summary>
    public int Speed
    {
      get
      {
        if (m_mibifRow == null)
        {
          m_mibifRow = GetMibIfRow(Index);
        }

        return m_mibifRow.Speed;
      }
    }

    /// <summary>
    /// Returns the NetworkInterface's Name or Description field
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      string name = (this.Name.Length > 0) ? Name : Description;

      if (name == null)
      {
        return "";
      }

      return name;
    }

    /// <summary>
    /// Refreshes all statistics for the current NetworkInterface
    /// </summary>
    public void Refresh()
    {
      // just set the members to null so the next query will call the API again
      m_adapterInfo = null;
      m_mibifRow = null;
    }

    /// <summary>
    /// Method called on unbound adapter (maybe when handling
    /// changing *both* the IP/subnet/gateway *and* the wireless
    /// settings).  This method notifies NDIS to bind the 
    /// adapter to all protocols indicated in the registry, in 
    /// effect causing the current registry settings to be 
    /// applied rather than those which the adapter is currently
    /// using.  Since we are binding, not *re*-binding the
    /// protocols, we are implying that the adapter is not
    /// currently bound to anything.  When making this call,
    /// we must refresh any adapter list that we might have,
    /// to retrieve the current state of all adapters.  
    /// Changes to things like the IP address, subnet mask, 
    /// etc. are not immediately returned.
    /// </summary>
    public void Bind()
    {
      NDIS.BindInterface(this.Name);
    }

    /// <summary>
    /// Method called to unbind a given adapter.  You might
    /// perform this operation before attempting to change
    /// *both* the protocol configuration of an adapter (IP,
    /// subnet, gateway), *and* the wireless configuration of
    /// the same adapter (WEP, SSID, etc.)  To do that, first
    /// unbind the adapter, then change the settings, then
    /// bind the adapter (UnbindAdapter(), make changes,
    /// BindAdapter()).  Once you have bound/unbound an 
    /// adapter, to get its new configuration, you must 
    /// regenerate the list of adapters.  Changes to things
    /// like the IP address, subnet mask, etc. are not 
    /// immediately returned.
    /// </summary>
    public void Unbind()
    {
      NDIS.UnbindInterface(this.Name);
    }

    /// <summary>
    /// Method called after making some changes to the current
    /// IP address, subnet mask, etc.  This method notifies NDIS
    /// to rebind the adapter to all protocols, in effect causing
    /// the current registry settings to be applied rather than
    /// those which the current configuration represents.  Once you
    /// have rebound an adapter, to get its new configuration, you
    /// must regenerate the list of adapters.  Changes to things
    /// like the IP address, subnet mask, etc. are not immediately 
    /// returned.
    /// </summary>
    public void Rebind()
    {
      NDIS.RebindInterface(this.Name);
      // refresh our adapter info, expecting a possible index change
      int index = Index;
      m_adapterInfo = GetAdapterInfo(ref index, Name);
      Index = index;
    }

    /// <summary>
    /// Returns an object that describes the configuration of this network interface.
    /// </summary>
    /// <returns>
    /// An System.Net.NetworkInformation.IPInterfaceProperties object that describes
    /// this network interface.
    /// </returns>
    /// <remarks>Note that the information in the object returned by this method reflects the interfaces as of the time the array is created. This information is not updated dynamically.</remarks>
    public IPInterfaceProperties GetIPProperties()
    {
      m_adapterInfo = GetAdapterInfo(Index);
      m_mibifRow = NetworkInterface.GetMibIfRow(Index);

      IPInterfaceProperties props = new IPInterfaceProperties(m_adapterInfo, m_mibifRow);

      return props;
    }

    /// <summary>
    /// Gets the IPv4 statistics.
    /// </summary>
    /// <returns>An OpenNETCF.Net.NetworkInformation.IPv4InterfaceStatistics object.</returns>
    public IPv4InterfaceStatistics GetIPv4Statistics()
    {
      m_mibifRow = NetworkInterface.GetMibIfRow(Index);

      IPv4InterfaceStatistics stats = new IPv4InterfaceStatistics(m_mibifRow);

      return stats;
    }

    /// <summary>
    /// The date/time at which the IP address lease from
    /// the DHCP server will expire (at which time the
    /// adapter will have to contact the server to renew
    /// the lease or stop using the IP address).
    /// </summary>
    public DateTime DhcpLeaseExpires
    {
      get
      {
        return m_adapterInfo == null ? DateTime.MinValue : m_adapterInfo.LeaseExpires;
      }
    }

    /// <summary>
    /// The date/time at which the IP address lease was
    /// obtained from the DHCP server.
    /// </summary>
    public DateTime DhcpLeaseObtained
    {
      get { return m_adapterInfo == null ? DateTime.MinValue : m_adapterInfo.LeaseObtained; }
    }

    /// <summary>
    /// Releases the Adapter's DHCP lease
    /// </summary>
    public void DhcpRelease()
    {
      if (!GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
      {
        throw new NotSupportedException("DHCP is not enabled on this interface");
      }

      IP_ADAPTER_INDEX_MAP map = new IP_ADAPTER_INDEX_MAP();
      map.Name = this.Name;
      map.Index = this.Index;

      NativeMethods.IpReleaseAddress(map);
      Refresh();
    }

    /// <summary>
    /// Renews the Adapter's DHCP lease
    /// </summary>
    public void DhcpRenew()
    {
      if (!GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
      {
        throw new NotSupportedException("DHCP is not enabled on this interface");
      }

      IP_ADAPTER_INDEX_MAP map = new IP_ADAPTER_INDEX_MAP();
      map.Name = this.Name;
      map.Index = this.Index;

      NativeMethods.IpRenewAddress(map);
      Refresh();
    }
  }
}
