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
        [TestMethod()]
        [Description("Checks to ensure LoadOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void LoadOpmlNullInputTest()
        {
            ArgumentNullException expected = null;

            try
            {
                Opml opml = FeedEngine.LoadOpml(null);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure LoadOpml throws ArgumentException when passed an empty string")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void LoadOpmlEmptyInputTest()
        {
            ArgumentException expected = null;

            try
            {
                Opml opml = FeedEngine.LoadOpml(string.Empty);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeOpmlNullObjectInputTest()
        {
            ArgumentNullException expected = null;

            try
            {
                Opml opml = FeedEngine.SubscribeOpml((Opml)null);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeOpmlNullStringInputTest()
        {
            ArgumentNullException expected = null;

            try
            {
                Opml opml = FeedEngine.SubscribeOpml((string)null);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentException when passed an empty string")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeOpmlEmptyStringInputTest()
        {
            ArgumentException expected = null;

            try
            {
                Opml opml = FeedEngine.SubscribeOpml(string.Empty);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeNullUriInputTest()
        {
            ArgumentNullException expected = null;
            Uri uri = null;

            try
            {
                FeedEngine.Subscribe(uri);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        public class TestReceiver : FeedReceiver
        {
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeNullUriWithReceiverTest()
        {
            ArgumentNullException expected = null;
            TestReceiver receiver = new TestReceiver();
            Uri uri = null;

            try
            {
                FeedEngine.Subscribe(uri, receiver);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Checks to ensure SubscribeOpml throws ArgumentNullException when passed a null")]
        [DeploymentItem("SmartDeviceTestHost.exe.config")]
        public void SubscribeNullReceiverTest()
        {
            ArgumentNullException expected = null;
            TestReceiver receiver = null;
            Uri uri = new Uri("http://blog.opennetcf.com");

            try
            {
                FeedEngine.Subscribe(uri, receiver);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }
    }
}
