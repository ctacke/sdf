using OpenNETCF.WindowsCE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.WindowsCE.Test
{
    [TestClass()]
    public class LargeIntervalTimerTest : TestBase
    {
        [TestMethod()]
        [Description("Ensures that setting an Interval value < 15 seconds throws an exception")]
        [ExpectedException(typeof(ArgumentException))]
        public void IntervalLessThan15secondsTest()
        {
            LargeIntervalTimer lit = new LargeIntervalTimer();
            lit.Interval = new TimeSpan(0, 0, 10);
        }

        [TestMethod()]
        [Description("Ensures that setting an Interval to reasonable valid values succeeds (Bugzilla 365)")]
        public void IntervalValidValuesTest()
        {
            LargeIntervalTimer lit = new LargeIntervalTimer();

            // bugzilla 365
            TimeSpan expected = new TimeSpan(0, 45, 0);
            lit.Interval = expected;
            Assert.AreEqual(expected, lit.Interval);

            // minutes only
            expected = new TimeSpan(0, 1, 0);
            lit.Interval = expected;
            Assert.AreEqual(expected, lit.Interval);

            // minutes and secs < 15
            expected = new TimeSpan(0, 45, 10);
            lit.Interval = expected;
            Assert.AreEqual(expected, lit.Interval);

            // hours only
            expected = new TimeSpan(2, 0, 0);
            lit.Interval = expected;
            Assert.AreEqual(expected, lit.Interval);
        }
    }
}
