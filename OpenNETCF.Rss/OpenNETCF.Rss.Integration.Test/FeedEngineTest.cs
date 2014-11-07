using OpenNETCF.Rss;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Rss.Data;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.IO;

namespace OpenNETCF.Rss.Test
{
  [TestClass()]
  public class FeedEngineTest : TestBase
  {
    [Ignore]
    [TestMethod()]
    [Description("Checks to ensure FeedEngine.Receive correctly pulls a feed from a valid Uri")]
    [DeploymentItem("SmartDeviceTestHost.exe.config")]
    [Priority(5)]
    public void ReceiveTestPositive()
    {
      Uri uri = new Uri("http://blogs.msdn.com/mobiledev/rss.xml");
      Feed feed = FeedEngine.Receive(uri);

      Assert.IsNotNull(feed);
    }
  }
}
