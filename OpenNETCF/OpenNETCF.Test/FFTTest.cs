using OpenNETCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Test
{
  [TestClass()]
  public class FFTTest : TestBase
  {
    [TestMethod()]
    [DeploymentItem("OpenNETCF.dll")]
    [Description("Ensures passing null data to Calculate throws an ArgumentNullException")]
    public void CalculateNullDataTest()
    {
      ArgumentNullException expected = null;

      try
      {
        FFT.Calculate(null, false);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected, "Calculate did not throw ArgumentNullException");
    }

    [TestMethod()]
    [DeploymentItem("OpenNETCF.dll")]
    [Description("Ensures passing empty data to Calculate throws an ArgumentException")]
    public void CalculateEmptyDataTest()
    {
      ArgumentException expected = null;

      try
      {
        Complex[] data = new Complex[0];

        FFT.Calculate(data, false);
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "Calculate did not throw ArgumentException");
    }
  }
}
