using OpenNETCF.AppSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.AppSettings.Test
{
    /// <summary>
    ///This is a test class for SettingsFileTest and is intended
    ///to contain all SettingsFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SettingsFileTest : TestBase
    {
        /// <summary>
        ///A test for SettingsFile Constructor
        ///</summary>
        [TestMethod()]
        [Description("Ensures attempting to create a SettingsFile with a null name throws an ArgumentNullException")]
        [WorkItem(-1)]
        public void SettingsFileCTorNullPathTest()
        {
            ArgumentNullException expected = null;

            try
            {
                SettingsFile sf = new SettingsFile(null);
            }
            catch (ArgumentNullException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures attempting to create a SettingsFile with an empty name throws an ArgumentException")]
        [WorkItem(-1)]
        public void SettingsFileCTorEmptyPathTest()
        {
            ArgumentException expected = null;

            try
            {
                SettingsFile sf = new SettingsFile(string.Empty);
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures attempting to create a SettingsFile with a non-existent filename throws a FileNotFoundException if create is false")]
        [WorkItem(-1)]
        public void SettingsFileCTorFileNotFoundTest()
        {
            FileNotFoundException expected = null;
            string filename = "\\settingfile.xml";

            // make sure the file really is missing
            if (File.Exists(filename)) File.Delete(filename);

            try
            {
                SettingsFile sf = new SettingsFile(filename, false);
            }
            catch (FileNotFoundException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures attempting to create a SettingsFile with a non-existent filename creates a file if create is true")]
        [WorkItem(-1)]
        public void SettingsFileCTorCreateFileIfNotFoundTest()
        {
            string filename = "\\settingfile.xml";

            // make sure the file really is missing
            if (File.Exists(filename)) File.Delete(filename);

            SettingsFile sf = new SettingsFile(filename);

            Assert.IsTrue(File.Exists(filename));
        }
    }
}
