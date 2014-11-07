namespace OpenNETCF.Core.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenNETCF.Testing.Support.SmartDevice;
    using OpenNETCF;

    [TestClass]
    public class NumericExtensionsTests : TestBase
    {
        [TestMethod]
        public void ToBCDTest()
        {
            int value = 4023;
            int bcd = value.ToBCD();

            byte[] data = new byte[] { 3, 2, 0, 4 };

            int expected = BitConverter.ToInt32(data, 0);

            Assert.AreEqual(expected, bcd);
        }

        [TestMethod]
        public void FromBCDTest()
        {
            int bcd = 0x0A090803;
            int expected = 10983;
            int actual = bcd.FromBCD();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BCDRoundTripTest()
        {
            int expected = 12345;
            int bcd = expected.ToBCD();
            int actual = bcd.FromBCD();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordUIntTest()
        {
            uint i = 0x98765432;
            Assert.AreEqual(0x9876, i.HiWord(), "HiWord failed");
            Assert.AreEqual(0x5432, i.LoWord(), "LoWord failed");
        }

        [TestMethod]
        public void WordIntTest()
        {
            int i = 0x12345678;
            Assert.AreEqual(0x1234, i.HiWord(), "HiWord failed");
            Assert.AreEqual(0x5678, i.LoWord(), "LoWord failed");
        }

        [TestMethod]
        public void WordIntPtrTest()
        {
            unchecked
            {
                IntPtr i = new IntPtr((int)0xABCDEF91);
                Assert.AreEqual(0xABCD, i.HiWord(), "HiWord failed");
                Assert.AreEqual(0xEF91, i.LoWord(), "LoWord failed");
            }
        }
    }
}