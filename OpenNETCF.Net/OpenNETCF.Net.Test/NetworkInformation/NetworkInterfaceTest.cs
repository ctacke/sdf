using OpenNETCF.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Net;

namespace OpenNETCF.Net
{
  [TestClass()]
  public class NetworkInterfaceTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures that DHCP can be enabled/diabled and what is set is what gets read")]
    public void DhcpEnableDisableTest()
    {
      INetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

      Assert.IsTrue(interfaces.Length > 0, "No interfaces available to test");

      IPv4InterfaceProperties props = interfaces[0].GetIPProperties().GetIPv4Properties();

      // first ensure DHCP is disabled
      if (props.IsDhcpEnabled)
      {
        props.IsDhcpEnabled = false;
        props = interfaces[0].GetIPProperties().GetIPv4Properties();
        Assert.IsFalse(props.IsDhcpEnabled, "Set IsDhcpEnabled to false but read back true");
      }

      props.IsDhcpEnabled = true;
      props = interfaces[0].GetIPProperties().GetIPv4Properties();
      Assert.IsTrue(props.IsDhcpEnabled, "Set IsDhcpEnabled to true but read back false");

      props.IsDhcpEnabled = false;
      props = interfaces[0].GetIPProperties().GetIPv4Properties();
      Assert.IsFalse(props.IsDhcpEnabled, "Set IsDhcpEnabled to false but read back true (second attempt)");
    }
  }
}
