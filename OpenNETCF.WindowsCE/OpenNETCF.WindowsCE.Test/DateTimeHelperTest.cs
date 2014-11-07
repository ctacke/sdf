using OpenNETCF.WindowsCE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.WindowsCE.Test
{
    [TestClass()]
    public class DateTimeHelperTest : TestBase
    {
        [TestMethod()]
        [Description("Ensures that if we set the current TZI, that what we read back is what we set.")]
        public void TimzeZoneChangeTest()
        {
            TimeZoneInformation currentTzi = new TimeZoneInformation();
            DateTimeHelper.GetTimeZoneInformation(ref currentTzi);

            try
            {
                TimeZoneInformation expectedTzi = new TimeZoneInformation();
                expectedTzi.Bias = currentTzi.Bias + 60; // (add an hour)
                expectedTzi.StandardName = "Standard Lunar Time";
                expectedTzi.DaylightName = "Daylight Lunar Time";

                DateTimeHelper.SetTimeZoneInformation(expectedTzi);

                TimeZoneInformation retrievedTzi = new TimeZoneInformation();
                DateTimeHelper.GetTimeZoneInformation(ref retrievedTzi);

                Assert.AreEqual(expectedTzi.Bias, retrievedTzi.Bias);
                Assert.AreEqual(expectedTzi.StandardName, retrievedTzi.StandardName);
                Assert.AreEqual(expectedTzi.DaylightName, retrievedTzi.DaylightName);
            }
            finally
            {
                DateTimeHelper.SetTimeZoneInformation(currentTzi);
            }
        }
    }
}
