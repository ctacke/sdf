using OpenNETCF.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Configuration.Test
{
    
    
    /// <summary>
    ///This is a test class for AppSettingsReaderTest and is intended
    ///to contain all AppSettingsReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppSettingsReaderTest : TestBase
    {
        [TestMethod]
        public void TestCtor()
        {
            AppSettingsReader reader = new AppSettingsReader();
            Assert.IsNotNull(reader);
        }
    }
}
