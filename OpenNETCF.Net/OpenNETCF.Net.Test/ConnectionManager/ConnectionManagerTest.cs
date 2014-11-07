using OpenNETCF.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OpenNETCF.Net.Test
{
  [TestClass()]
  public class ConnectionManagerTest
  {
    [Ignore]
    [TestMethod()]
    [Description("Tests to ensure that a ConnectionManager class cannot be loaded on a platform without cellcore.dll")]
    // TODO: add platform deployment attributes to really check this
    // this should run on a generic CE device
    public void UnsupportedPlatformTest()
    {
      PlatformNotSupportedException expected = null;

      try
      {
        ConnectionManager target = new ConnectionManager();
      }
      catch (PlatformNotSupportedException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected);
    }

    [TestMethod()]
    [Description("Tests to ensure that a ConnectionManager class sets SupportsStatusNotifications to false of platforms that don't support it")]
    // TODO: add platform deployment attributes to really check this
    // this should run on a PPC 2003 emulator
    public void TestUnsupportedStatusNotifications()
    {
      ConnectionManager target = new ConnectionManager();
      Assert.IsFalse(target.SupportsStatusNotifications);
    }

    [Ignore]
    [TestMethod()]
    [Description("Tests to ensure that a ConnectionManager class sets SupportsStatusNotifications to true of platforms that do support it")]
    // TODO: add platform deployment attributes to really check this
    // this should run on a WM5 emulator
    public void TestSupportedStatusNotifications()
    {
      ConnectionManager target = new ConnectionManager();
      Assert.IsTrue(target.SupportsStatusNotifications);
    }

    [Ignore]
    [TestMethod()]
    [Description("Tests to ensure that a ConnectionManager class sets SupportsStatusNotifications to true of platforms that do support it")]
    // TODO: add platform deployment attributes to really check this
    // this should run on a PPC03 emulator
    public void TestUnsupportedConnectionDetailItems()
    {
      PlatformNotSupportedException expected = null;
      ConnectionManager manager = new ConnectionManager();

      try
      {
        manager.GetConnectionDetailItems();
      }
      catch (PlatformNotSupportedException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected);
    }
  }
}
