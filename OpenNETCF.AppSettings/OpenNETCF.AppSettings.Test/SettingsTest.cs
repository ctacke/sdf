using OpenNETCF.AppSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.AppSettings.Test
{
    [TestClass()]
    public class SettingsTest : TestBase
    {   
        [Ignore]
        [TestMethod()]
        [Description("Ensures that using the set accessor to the indexer required that the settingName match the incoming Setting.Name member")]
        public void IndexerSetWithWrongSettingNameTest()
        {
            /*
            Settings settings = new Settings();
            settings.Add("Setting 1", "value");

            ArgumentException expected = null;

            try
            {
                settings["Setting 1"] = new Setting("Setting 2", "value 2");
            }
            catch (ArgumentException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
            */
        }
    }
}
