using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using OpenNETCF;

namespace OpenNETCF.Core.Test
{
  [TestClass]
  public class CharExtensionsTests : TestBase
  {
    [TestMethod]
    public void TestIsDigitTrue()
    {
      char c = '1';
      Assert.IsTrue(c.IsDigit());
    }

    [TestMethod]
    public void TestIsDigitFalse()
    {
      char c = '-';
      Assert.IsFalse(c.IsDigit());
    }
  }
}