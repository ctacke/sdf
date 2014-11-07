using OpenNETCF.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;
using System.Threading;

namespace OpenNETCF.Integration.Test
{
    [TestClass()]
    public class Timer2Test : TestBase
    {
        private Timer2 m_timer;
        private bool m_hasFired;

        [TestMethod()]
        [Description("Ensures that a Timer2 with AutoReset set to false does not fire twice")]
        [Priority(5)]
        public void AutoResetFalseTest()
        {
            int interval = 500;
            m_timer = new Timer2(interval);
            m_timer.AutoReset = false;
            m_timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            m_hasFired = false;
            m_timer.Start();

            Thread.Sleep(interval + 1);

            Assert.IsTrue(m_hasFired, "Timer didn't fire");

            m_hasFired = false;
            Thread.Sleep(interval * 2);

            Assert.IsFalse(m_hasFired, "Timer fired twice");
        }

        [TestMethod()]
        [Description("Ensures that a Timer2 with AutoReset set to true does fire twice")]
        [Priority(5)]
        public void AutoResetTrueTest()
        {
            int interval = 500;
            m_timer = new Timer2(interval);
            m_timer.AutoReset = true;
            m_timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            m_hasFired = false;
            m_timer.Start();

            Thread.Sleep(interval + 1);

            Assert.IsTrue(m_hasFired, "Timer didn't fire first time");

            m_hasFired = false;
            Thread.Sleep(interval + 1);

            Assert.IsTrue(m_hasFired, "Timer didn't fire second time");
        }

        [TestMethod()]
        [Description("Ensures setting Enabled to true starts and false stops Timer2 events")]
        [Priority(5)]
        public void EnabledTimerTest()
        {
            int interval = 500;
            m_timer = new Timer2(interval);
            m_timer.AutoReset = true;
            m_timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            m_hasFired = false;
            m_timer.Enabled = true;

            Assert.IsTrue(m_timer.Enabled);

            Thread.Sleep(interval + 1);

            Assert.IsTrue(m_hasFired, "Timer didn't fire first time");

            m_hasFired = false;
            m_timer.Enabled = false;

            Assert.IsFalse(m_timer.Enabled);

            Thread.Sleep(interval + 1);

            Assert.IsFalse(m_hasFired, "Timer fired after diabling");
        }

        [TestMethod()]
        [Description("Ensures calling Stop on a running Timer2 stops events")]
        [Priority(5)]
        public void StopRunningTimerTest()
        {
            int interval = 500;
            m_timer = new Timer2(interval);
            m_timer.AutoReset = true;
            m_timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            m_hasFired = false;
            m_timer.Start();

            Thread.Sleep(interval + 1);

            Assert.IsTrue(m_hasFired, "Timer didn't fire first time");

            m_hasFired = false;
            m_timer.Stop();

            Thread.Sleep(interval + 1);

            Assert.IsFalse(m_hasFired, "Timer fired after diabling");
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_hasFired = true;
        }

        [TestMethod()]
        [Description("Ensures that a Timer2-derived class with AutoReset set to false does not call its callback twice")]
        [Priority(5)]
        public void TimerCallbackAutoResetFalseTest()
        {
            int interval = 500;
            Timer2Subclass timer = new Timer2Subclass(interval);
            timer.AutoReset = false;
            timer.UseCallback = true;
            timer.CallbackHasBeenCalled = false;
            timer.Start();

            Thread.Sleep(interval + 1);

            Assert.IsTrue(timer.CallbackHasBeenCalled, "Callback wasn't called");

            timer.CallbackHasBeenCalled = false;
            Thread.Sleep(interval * 2);

            Assert.IsFalse(timer.CallbackHasBeenCalled, "Callback called twice");
        }

        [TestMethod()]
        [Description("Ensures that a Timer2-derived class with AutoReset set to true calls its callback periodically")]
        [Priority(5)]
        public void TimerCallbackAutoResetTrueTest()
        {
            int interval = 500;
            Timer2Subclass timer = new Timer2Subclass(interval);
            timer.AutoReset = true;
            timer.UseCallback = true;
            timer.CallbackHasBeenCalled = false;
            timer.Start();

            Thread.Sleep(interval + 1);

            Assert.IsTrue(timer.CallbackHasBeenCalled, "Callback wasn't called once");

            timer.CallbackHasBeenCalled = false;
            Thread.Sleep(interval + 1);

            Assert.IsTrue(timer.CallbackHasBeenCalled, "Callback wasn't called twice");

            timer.Dispose();
        }
    }
}
