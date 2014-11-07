using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// Summary description for AccessPointCollectionTest
  /// </summary>
  [TestClass]
  public class AccessPointCollectionTest : TestBase
  {
    [TestMethod()]
    [WorkItem(175)]
    [Description("Proves that AccessPointCollection throws ArgumentNullException when null WirelessZeroConfigNetworkInterface is passed")]
    public void ConstructorWithNullBSSID()
    {
      ArgumentNullException caughtException = null;
      try
      {
        AccessPointCollection target = new AccessPointCollection(null);
      }
      catch (ArgumentNullException ex)
      {
        caughtException = ex;
      }

      Assert.IsNotNull(caughtException, "AccessPointCollection did not throw ArgumentNullException when null WirelessZeroConfigNetworkInterface was passed");
    }

    [Ignore]
    [TestMethod()]
    [WorkItem(175)]
    [Description("Proves that AccessPointCollection's constructor does not throw exception when valid WirelessZeroConfigNetworkInterface is passed")]
    public void ConstructorValidWirelessZeroConfigNetworkInterface()
    {
      WirelessZeroConfigNetworkInterface wirelessZeroConfigNI = new WirelessZeroConfigNetworkInterface(0, "Config1");

      AccessPointCollection target = null;

      Exception caughtException = null;
      try
      {
        target = new AccessPointCollection(wirelessZeroConfigNI);
      }
      catch (Exception ex)
      {
        caughtException = ex;
      }

      Assert.IsNull(caughtException, "Constructor threw exception. AccessPointCollection was not constructed successfully");
    }

    [Ignore]
    [TestMethod()]
    [WorkItem(175)]
    [Description("Proves that AccessPointCollection's constructor sets properties when valid WirelessZeroConfigNetworkInterface is passed")]
    public void ConstructorSetsProperties()
    {
      WirelessZeroConfigNetworkInterface wirelessZeroConfigNI = new WirelessZeroConfigNetworkInterface(0, "Config1");
      AccessPointCollection target = new AccessPointCollection(wirelessZeroConfigNI, false);
      Assert.AreEqual<WirelessZeroConfigNetworkInterface>(wirelessZeroConfigNI, target.AssociatedAdapter, "Constructor did not set AssociatedAdapter property correctly");
    }
  }
}
