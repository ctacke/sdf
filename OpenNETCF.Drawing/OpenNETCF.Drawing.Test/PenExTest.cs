using OpenNETCF.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Drawing;

namespace OpenNETCF.Drawing.Test
{
    [TestClass()]
    public class PenExTest : TestBase
    {
        [TestMethod]
        [Description("Ensures that constructing a PenEx with a width < 0 throws an ArgumentOPutOfRangeException")]
        public void PenExCTorNegativeWidthTest()
        {
            ArgumentOutOfRangeException expected = null;

            try
            {
                PenEx pen = new PenEx(Color.Black, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                expected = ex;
            }

            Assert.IsNotNull(expected);
        }
    }
}