using OpenNETCF.Rss.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.IO;

namespace OpenNETCF.Rss.Test
{
    [TestClass()]
    public class FeedConfigurationTest : TestBase
    {
        [TestMethod()]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void FeedConfigurationConstructorTest()
        {
            StorageConfiguration config = FeedConfiguration.StorageConfiguration;
            FeedFileStorage storage = config.GetProvider() as FeedFileStorage;


            Assert.IsNotNull(storage);

        }
    }
}
