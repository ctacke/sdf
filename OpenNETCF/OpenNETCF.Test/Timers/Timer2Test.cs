using OpenNETCF.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Test
{
    [TestClass()]
    public class Timer2Test : TestBase
    {
        [Ignore]
        [TestMethod()]
        [Description("Ensures that the Timer2 ctor fails on platforms without MMTimer support")]
        // TODO: this should be run on an unsupported platform
        public void UnsupportedPlatformTest()
        {
            PlatformNotSupportedException expected = null;

            try
            {
                Timer2 timer = new Timer2();
            }
            catch(PlatformNotSupportedException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that the Timer2 can be constructed on a supported platform")]
        public void ConstructorPositive()
        {
            Timer2 timer = new Timer2();
            // no error == success
        }

        [TestMethod()]
        [Description("Ensures that the Timer2 Interval can be set to a valid value ")]
        public void IntervalPositive()
        {
            Timer2 timer = new Timer2();
            timer.Interval = 100;

            // no error == success
        }

        [TestMethod()]
        [Description("Ensures that the Timer2 Resolution can be set to a valid value ")]
        public void ResolutionPositive()
        {
            Timer2 timer = new Timer2();
            timer.Resolution = 100;

            // no error == success
        }

        [TestMethod()]
        [Description("Ensures that consturcting a Timer2 with a zero interval will throw an ArgumentOutOfRangeException")]
        public void ConstructorIntervalZeroTest()
        {
            ArgumentOutOfRangeException expected = null;

            try
            {
                Timer2 timer = new Timer2(0);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that consturcting a Timer2 with < 0 interval will throw an ArgumentOutOfRangeException")]
        public void ConstructorIntervalLTZeroTest()
        {
            ArgumentOutOfRangeException expected = null;

            try
            {
                Timer2 timer = new Timer2(-1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that consturcting a Timer2 with a zero resolution is valid")]
        public void ConstructorResolutionZeroTest()
        {
            Timer2 timer = new Timer2(100, 0);
            Assert.IsNotNull(timer);

            // no error == success
        }

        [TestMethod()]
        [Description("Ensures that consturcting a Timer2 with < 0 resolution will throw an ArgumentOutOfRangeException")]
        public void ConstructorResolutionLTZeroTest()
        {
            ArgumentOutOfRangeException expected = null;

            try
            {
                Timer2 timer = new Timer2(1000, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that setting the Timer2 Interval property to zero throws an ArgumentOutOfRangeException")]
        public void InvalidIntervalZeroTest()
        {
            ArgumentOutOfRangeException expected = null;
            Timer2 timer = new Timer2();

            try
            {
                timer.Interval = 0;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that setting the Timer2 Interval property < 0 throws an ArgumentOutOfRangeException")]
        public void InvalidIntervalLTZeroTest()
        {
            ArgumentOutOfRangeException expected = null;
            Timer2 timer = new Timer2();

            try
            {
                timer.Interval = -1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that setting the Timer2 Resolution property to zero is acceptable")]
        public void ResolutionZeroTest()
        {
            Timer2 timer = new Timer2();
            timer.Resolution = 0;

            // no exception == success
        }

        [TestMethod()]
        [Description("Ensures that setting the Timer2 Resolution property < 0 throws an ArgumentOutOfRangeException")]
        public void InvalidResolutionLTZeroTest()
        {
            ArgumentOutOfRangeException expected = null;
            Timer2 timer = new Timer2();

            try
            {
                timer.Resolution = -1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that the Timer2 AutoRest property can be set to false and read back as the same value")]
        public void AutoResetFalseTest()
        {
            Timer2 timer = new Timer2();
            timer.AutoReset = false;

            Assert.IsFalse(timer.AutoReset);
        }

        [TestMethod()]
        [Description("Ensures that the Timer2 AutoRest property can be set to true and read back as the same value")]
        public void AutoResetTrueTest()
        {
            Timer2 timer = new Timer2();
            timer.AutoReset = true;

            Assert.IsTrue(timer.AutoReset);
        }

        [TestMethod()]
        [Description("Ensures that setting the Timer2 UseCallback property to true when no callback method exists will throw")]
        public void UseCallbackWithoutCallbackMethodTest()
        {
            NotImplementedException expected = null;

            Timer2 timer = new Timer2();

            try
            {
                timer.UseCallback = true;
            }
            catch (NotImplementedException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }

        [TestMethod()]
        [Description("Ensures that setting the UseCallback property to true on a Timer2-derived class succeeds")]
        public void UseCallbackPositiveTest()
        {
            Timer2Subclass timer = new Timer2Subclass();
            timer.UseCallback = true;

            Assert.IsNotNull(timer);
        }
    }

    internal class Timer2Subclass : Timer2
    {
        public bool CallbackHasBeenCalled { get; private set; }

        public override void TimerCallback()
        {
            CallbackHasBeenCalled = true;
        }
    }
}
