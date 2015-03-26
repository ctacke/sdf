using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace OpenNETCF.Testing.Support.SmartDevice
{
    [TestClass]
    public class TestBase
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            CopyTestConfigFile();
        }

        private void CopyTestConfigFile()
        {
            // copy the config file to the test host folder
            string src = Path.Combine(TestContext.TestDeploymentDir, "SmartDeviceTestHost.exe.config");
            string dest = Path.Combine(TestHostFolder, "SmartDeviceTestHost.exe.config.unittest");
            if ((File.Exists(src)) && (!File.Exists(dest)))
            {
                File.Copy(src, dest);
            }
        }

        public string TestHostFolder
        {
            get
            {
                return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase)));
            }
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
        }
    }
}
