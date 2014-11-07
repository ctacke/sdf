using OpenNETCF.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Net;
using System.Collections.Generic;
using NI = OpenNETCF.Net.NetworkInformation;

namespace OpenNETCF.Net
{
  [TestClass()]
  public class NetworkInterfaceTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures that Unbind/Rebind does not throw an exception (bugzilla bug 240)")]
    public void UnbindRebindTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      foreach (INetworkInterface intf in interfaces)
      {
        intf.Unbind();
        intf.Rebind();
      }

      // no exception == pass
    }

    [TestMethod()]
    [Description("Ensures that DHCP can be enabled/diabled, can be refreshed when enable and throws when not enabled")]
    public void DhcpTest()
    {
      NotSupportedException expected = null;

      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      // first ensure DHCP is disabled
      if (interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = false;
        interfaces[0].Rebind();
      }

      try
      {
        interfaces[0].DhcpRelease();
      }
      catch (NotSupportedException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected, "DhcpRelease on a non-DHCP enabled interface did not throw a NotSupportedException");

      expected = null;
      try
      {
        interfaces[0].DhcpRenew();
      }
      catch (NotSupportedException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected, "DhcpRenew on a non-DHCP enabled interface did not throw a NotSupportedException");

      // now enable DHCP and ensure that it happened
      interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = true;
      Assert.IsTrue(interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled, "Set IsDhcpEnabled to true but read back false");

      interfaces[0].DhcpRelease();
      interfaces[0].DhcpRenew();

      // no exception == pass
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that the CurrentSubnetMask can be set and read")]
    public void SubnetMaskTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      // make sure DHCP is not enabled
      bool wasDhcpEnabled = interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
      interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = false;
      interfaces[0].Rebind();

      // read
      IPAddress originalSubnet = interfaces[0].CurrentSubnetMask;

      IPAddress newSubnet = IPAddress.Parse("255.255.255.254");

      // make sure it's new
      if (newSubnet.Equals(originalSubnet))
      {
        newSubnet = IPAddress.Parse("255.255.255.0");
      }

      interfaces[0].CurrentSubnetMask = newSubnet;
      interfaces[0].Rebind();

      // no exception == pass

      // read
      IPAddress readSubnet = interfaces[0].CurrentSubnetMask;

      Assert.IsTrue(newSubnet.Equals(readSubnet), "Subnet read back did not match what we wrote in");

      // restore
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = true;
      }
      else
      {
        interfaces[0].CurrentSubnetMask = originalSubnet;
        interfaces[0].Rebind();
      }
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that the gateway can be set and read")]
    public void GatewayTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      // make sure DHCP is not enabled
      bool wasDhcpEnabled = interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = false;
        interfaces[0].Rebind();
      }

      GatewayIPAddressInformationCollection gateways = interfaces[0].GetIPProperties().GatewayAddresses;

      if (gateways.Count == 0)
      {
        gateways.Add(IPAddress.Parse("10.1.1.1"));
        interfaces[0].Rebind();
      }

      IPAddress oldGateway = gateways[0].Address;
      IPAddress newGateway = IPAddress.Parse("10.0.0.0");
      // make sure it's new
      if (newGateway.Equals(oldGateway))
      {
        newGateway = IPAddress.Parse("10.1.1.1");
      }

      gateways[0].Address = newGateway;
      interfaces[0].Rebind();

      // no exception == pass

      // read
      gateways = interfaces[0].GetIPProperties().GatewayAddresses;

      Assert.IsTrue(newGateway.Equals(gateways[0].Address), string.Format("Gateway read back ({0}) did not match what we wrote in ({1})", gateways[0].Address, newGateway.Address));

      // restore
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = true;
      }
      else
      {
        gateways[0].Address = oldGateway;
        interfaces[0].Rebind();
      }
    }

    [TestMethod()]
    [Description("Ensures that the DNS can be set and read")]
    public void DNSTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      // make sure DHCP is not enabled
      bool wasDhcpEnabled = interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = false;
        interfaces[0].Rebind();
      }

      if (interfaces[0].GetIPProperties().DnsAddresses.Count == 0)
      {
        interfaces[0].GetIPProperties().DnsAddresses.Add(IPAddress.Parse("30.30.30.3"));
        interfaces[0].Rebind();
        Assert.AreEqual<int>(1, interfaces[0].GetIPProperties().DnsAddresses.Count, "Add did not increment DNS count");
      }

      IPAddress oldDns = interfaces[0].GetIPProperties().DnsAddresses[0];
      IPAddress newDns = IPAddress.Parse("30.30.30.4");

      if (oldDns.Equals(newDns))
      {
        newDns = IPAddress.Parse("30.30.30.5");
      }

      interfaces[0].GetIPProperties().DnsAddresses[0] = newDns;
      interfaces[0].Rebind();

      IPAddress setDns = interfaces[0].GetIPProperties().DnsAddresses[0];

      Assert.IsTrue(setDns.Equals(newDns), string.Format("DNS read back ({0}) did not match what we wrote in ({1})", setDns.Address, newDns.Address));

      // restore
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = true;
      }
      interfaces[0].GetIPProperties().DnsAddresses[0] = oldDns;
      interfaces[0].Rebind();
    }

    [TestMethod()]
    [Description("Ensures that the current IPAddress can bet set")]
    public void IPAddressTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      // make sure DHCP is not enabled
      bool wasDhcpEnabled = interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = false;
        interfaces[0].Rebind();
      }

      IPAddress oldAddress = interfaces[0].CurrentIpAddress;
      IPAddress newAddress = IPAddress.Parse("192.168.200.20");

      if (oldAddress.Equals(newAddress))
      {
        newAddress = IPAddress.Parse("192.168.200.40");
      }

      interfaces[0].CurrentIpAddress = newAddress;
      interfaces[0].Rebind();

      IPAddress setAddress = interfaces[0].CurrentIpAddress;

      Assert.IsTrue(setAddress.Equals(newAddress), string.Format("IP read back ({0}) did not match what we wrote in ({1})", setAddress.Address, newAddress.Address));

      // restore
      if (wasDhcpEnabled)
      {
        interfaces[0].GetIPProperties().GetIPv4Properties().IsDhcpEnabled = true;
      }
      interfaces[0].CurrentIpAddress = oldAddress;
      interfaces[0].Rebind();
    }

    [Ignore]
    // This can only be run against a physical device with real APs - it ras as of 10/18/08, but we don't currently have an automated mechanism for testing
    [TestMethod]
    [Description("Verifies that an Available, Open, Infrastructure Access Point can be added to the preferred list and connected to")]
    public void PreferredOpenInfrastructureAPTest(WirelessZeroConfigNetworkInterface wzc)
    {
      List<string> preferredAPs = new List<string>();
      // get a list of available adapters
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      if (interfaces.Length <= 0)
      {
        Assert.Fail("No interfaces available to test");
      }

      bool wzcFound = false;

      // find the WZC adapter
      foreach (INetworkInterface intf in interfaces)
      {
        if (intf is WirelessZeroConfigNetworkInterface)
        {
          // run the test
          wzcFound = true;

          // get a list of preferred APs
          foreach (NI.AccessPoint ap in wzc.PreferredAccessPoints)
          {
            preferredAPs.Add(ap.Name);
          }

          // remove them all
          foreach (string ssid in preferredAPs)
          {
            if (!wzc.RemovePreferredNetwork(ssid))
            {
              Assert.Fail("Failed to remove AP " + ssid);
            }
          }

          // make sure the list really got emptied
          if (wzc.PreferredAccessPoints.Count > 0)
          {
            Assert.Fail("Failed to drop all preferred APs");
          }

          // make sure we have a nearby AP
          NI.AccessPointCollection nearbyAPs = wzc.NearbyAccessPoints;
          if (nearbyAPs.Count <= 0)
          {
            // try twice, in case the adapter was not initialized
            System.Threading.Thread.Sleep(100);
            nearbyAPs = wzc.NearbyAccessPoints;
            if (nearbyAPs.Count <= 0)
            {
              Assert.Fail("No nearby APs found");
            }
          }

          bool nearbyFound = false;
          List<string> nearbyNames = new List<string>();

          foreach (NI.AccessPoint ap in nearbyAPs)
          {
            if (nearbyNames.Contains(ap.Name))
            {
              // we can't add two APs with the same name (and different physical addresses)
              continue;
            }
            nearbyNames.Add(ap.Name);

            if (ap.InfrastructureMode == NI.InfrastructureMode.Infrastructure)
            {
              if (ap.Privacy == 0)
              {
                // we have found an open, infrastructure AP
                nearbyFound = true;

                if (!wzc.AddPreferredNetwork(ap.Name, false, null, 1, NI.AuthenticationMode.Open, NI.WEPStatus.WEPDisabled, null))
                {
                  Assert.Fail("Failed to add AP named " + ap.Name);
                }
              }
            }
          }

          if (!nearbyFound)
          {
            Assert.Fail("No nearby APs found");
          }

          // now remove them all again (to ensure we've actually removed something - we could have started with none)
          preferredAPs.Clear();

          // get a list of preferred APs
          foreach (NI.AccessPoint ap in wzc.PreferredAccessPoints)
          {
            preferredAPs.Add(ap.Name);
          }

          // remove them all
          foreach (string ssid in preferredAPs)
          {
            if (!wzc.RemovePreferredNetwork(ssid))
            {
              Assert.Fail("Failed to remove AP " + ssid);
            }
          }

          Assert.IsTrue(wzc.PreferredAccessPoints.Count == 0, "Failed to remove all preferred APs");
          break;
        }
      }

      if (!wzcFound)
      {
        Assert.Fail("No WZC Adapter found");
      }
    }
  }
}
