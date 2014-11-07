using OpenNETCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Test
{

  [TestClass()]
  public class FFTRealTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures creating an FFTReal instance with a negative length throws an ArgumentException")]
    public void FFTRealNegativeLengthConstructorTest()
    {
      ArgumentException expected = null;

      try
      {
        FFTReal target = new FFTReal(-1);
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "FFTReal ctor did not throw ArgumentException for invalid length");
    }

    [TestMethod()]
    [Description("Ensures creating an FFTReal instance with a zero length throws an ArgumentException")]
    public void FFTRealZeroLengthConstructorTest()
    {
      ArgumentException expected = null;

      try
      {
        FFTReal target = new FFTReal(0);
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "FFTReal ctor did not throw ArgumentException for invalid length");
    }

    [TestMethod()]
    [Description("Ensures creating an FFTReal instance with a non power of 2 length throws an ArgumentException")]
    public void FFTRealNonPowerOf2LengthConstructorTest()
    {
      ArgumentException expected = null;

      try
      {
        FFTReal target = new FFTReal(1023);
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "FFTReal ctor did not throw ArgumentException for invalid length");
    }

    [TestMethod()]
    [Description("Ensures creating an FFTReal instance with a valid length works")]
    public void FFTRealConstructorTestPositive()
    {
      try
      {
        FFTReal target = new FFTReal(1024);
        Assert.IsNotNull(target);
      }
      catch (Exception ex)
      {
        Assert.Fail("FFTReal ctor threw for valid input");
      }
    }
  }
}
