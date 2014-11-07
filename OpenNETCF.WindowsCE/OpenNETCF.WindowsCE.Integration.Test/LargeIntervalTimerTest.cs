using OpenNETCF.WindowsCE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.WindowsCE.Test
{
    [TestClass()]
    public class LargeIntervalTimerTest : TestBase
    {
        [TestMethod()]
        [Description("Ensures that the Enabled property coupled with the Tick handler is thread safe")]
        [Priority(5)]
        public void EnabledThreadSafetyCheck()
        {
            LargeIntervalTimer lit = new LargeIntervalTimer();
            lit.Interval = TimeSpan.FromSeconds(25);
            lit.OneShot = true;
            lit.Tick += new EventHandler(EnabledThreadSafetyTick);
            // lit.TestCondition = -1;

            m_ticks = 0;
            lit.Enabled = true;

            // wait for 1 tick period to pass
            Thread.Sleep(30000);
            Assert.AreEqual(1, m_ticks, "LIT didn't tick the first time");

            // wait for 1 tick period to pass
            Thread.Sleep(30000);
            Assert.AreEqual(2, m_ticks, "LIT didn't tick the second time");
        }

        private volatile int m_ticks = 0;

        void EnabledThreadSafetyTick(object sender, EventArgs e)
        {
            LargeIntervalTimer lit = (LargeIntervalTimer)sender;
            m_ticks++;

            if (m_ticks == 1)
            {
                // re-enable the LIT once
                lit.Enabled = true;
            }
        }

        [TestMethod()]
        [Description("Ensures that only one worker thread is ever running")]
        [Priority(5)]
        public void ThreadCountTest()
        {
#if INTEGRATION_TEST
            for (int condition = 0; condition < 9; condition++)
            {
                // Create timer, set to some interval
                LargeIntervalTimer lit = new LargeIntervalTimer();
                lit.Interval = TimeSpan.FromSeconds(20);
                lit.OneShot = true;
                lit.TestCondition = condition;

                for (int i = 0; i < 10; ++i)
                {
                    lit.Enabled = false;
                    lit.Enabled = true;
                    Thread.Sleep(0);
                }

                Assert.IsTrue(lit.ThreadCount == 1);
                Assert.IsTrue(lit.OneShot);

                lit.Dispose();
            }
#else
            Assert.Inconclusive("INTEGRATION_TEST not defined - test not run");
#endif
        }

        [TestMethod()]
        [Description("Ensures that an old known race condition between one-shot and the worker thread doesn't reappear")]
        [Priority(5)]
        public void OneShotRaceTest()
        {
#if INTEGRATION_TEST
            for (int condition = 0; condition < 9; condition++)
            {
                // Create timer, set to some interval
                LargeIntervalTimer lit = new LargeIntervalTimer();
                lit.Interval = TimeSpan.FromSeconds(17);

                lit.OneShot = true;
                lit.Enabled = true;

                lit.Enabled = false;

                lit.OneShot = false;
                lit.Enabled = true;

                Thread.Sleep((int)lit.Interval.TotalMilliseconds + 3000);

                Assert.IsTrue(lit.Enabled);
                Assert.IsFalse(lit.OneShot);
                lit.Dispose();
            }
#else
            Assert.Inconclusive("INTEGRATION_TEST not defined - test not run");
#endif
        }
    }
}
