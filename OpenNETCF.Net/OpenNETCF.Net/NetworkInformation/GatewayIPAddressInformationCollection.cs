using System;
using System.Collections;
using System.Text;
using Microsoft.Win32;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// Stores a set of System.Net.NetworkInformation.GatewayIPAddressInformation
  /// types.
  /// </summary>
  public class GatewayIPAddressInformationCollection : CollectionBase, IEnumerable
  {
    /// <summary>
    /// Initializes a new instance of the System.Net.NetworkInformation.GatewayIPAddressInformationCollection
    /// class.
    /// </summary>

    internal GatewayIPAddressInformationCollection(UnicastIPAddressInformationCollection c)
    {
      foreach (UnicastIPAddressInformation info in c)
      {
        GatewayIPAddressInformation gateway = new GatewayIPAddressInformation(info.Address);
        gateway.Changed += new AddressChangedHandler(gateway_Changed);
        this.List.Add(gateway);
      }
      this.m_adapterName = c.m_adapterName;
    }

    private string m_adapterName;
    /// <summary>
    /// Gets the System.Net.NetworkInformation.GatewayIPAddressInformation at the
    /// specific index of the collection.
    /// </summary>
    /// <param name="index">
    /// The index of interest.
    /// </param>
    /// <returns>
    /// The OpenNETCF.Net.NetworkInformation.GatewayIPAddressInformation at the specific
    /// index in the collection.
    /// </returns>
    public GatewayIPAddressInformation this[int index]
    {
      get { return (GatewayIPAddressInformation)List[index]; }
    }

    public void Add(IPAddress address)
    {
      GatewayIPAddressInformation info = new GatewayIPAddressInformation(address);
      List.Add(info);
      gateway_Changed();
    }

    /// <summary>
    /// Checks whether the collection contains the specified System.Net.NetworkInformation.GatewayIPAddressInformation
    /// object.
    /// </summary>
    /// <param name="address">
    /// The System.Net.NetworkInformation.GatewayIPAddressInformation object to be
    /// searched in the collection.
    /// </param>
    /// <returns>
    /// true if the System.Net.NetworkInformation.GatewayIPAddressInformation object
    /// exists in the collection; otherwise false.
    /// </returns>
    public virtual bool Contains(GatewayIPAddressInformation address)
    {
      return List.Contains(address);
    }

    /// <summary>
    /// Returns an object that can be used to iterate through this collection.
    /// </summary>
    /// <returns>
    /// An object that implements the System.Collections.IEnumerator interface and
    /// provides access to the System.Net.NetworkInformation.IPUnicastAddressInformation
    /// types in this collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return List.GetEnumerator();
    }

    void gateway_Changed()
    {
      // Modify the registry keys associated with this
      // adapter to set the new gateway.  Once that is done, we
      // have to rebind the network adapter to actually
      // make the change.  
      string regName = "\\comm\\" + this.m_adapterName + "\\Parms\\Tcpip";

      // Open the base key for the adapter.
      RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey(regName, true);

      // Make the change.
      tcpipkey.SetValue("DefaultGateway", this.ToStringArray(), RegistryValueKind.MultiString);

      tcpipkey.Close();
    }

    internal string[] ToStringArray()
    {
      string[] list = new string[this.Count];
      for (int i = 0; i < this.Count; i++)
      {
        list[i] = this[i].Address.ToString();
      }

      return list;
    }
  }
}
