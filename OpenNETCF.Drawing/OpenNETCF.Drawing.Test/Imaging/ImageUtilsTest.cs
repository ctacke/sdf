using OpenNETCF.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Drawing;

namespace OpenNETCF.Drawing.Imaging.Test
{
  [TestClass()]
  public class ImageUtilsTest : TestBase
  {
    [TestMethod]
    [Description("Ensures that GetInstalledEncoders returns some encoders without exception")]
    public void GetInstalledEncodersTest()
    {
      ImageCodecInfo[] codecs = ImageUtils.GetInstalledEncoders();
      Assert.IsTrue(codecs.Length > 0);
    }

    [TestMethod]
    [Description("Ensures that GetInstalledDecoders returns some encoders without exception")]
    public void GetInstalledDecodersTest()
    {
      ImageCodecInfo[] codecs = ImageUtils.GetInstalledDecoders();
      Assert.IsTrue(codecs.Length > 0);
    }

    [TestMethod]
    [Description("Ensures that Rotate hows an ArgumentNullException when passed a null bitmap")]
    public void RotateNullTest()
    {
      ArgumentNullException expected = null;

      try
      {
        ImageUtils.Rotate(null, RotationAngle.Zero);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected);
    }

    [TestMethod]
    [Description("Ensures that Flip hows an ArgumentNullException when passed a null bitmap")]
    public void FlipNullTest()
    {
      ArgumentNullException expected = null;

      try
      {
        ImageUtils.Flip(null, FlipAxis.X | FlipAxis.Y);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected);
    }

    [TestMethod]
    [Description("Ensures that RotateFlip hows an ArgumentNullException when passed a null bitmap")]
    public void RotateFlipNullTest()
    {
      ArgumentNullException expected = null;

      try
      {
        ImageUtils.RotateFlip(null, RotationAngle.Zero, FlipAxis.X | FlipAxis.Y);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }
      Assert.IsNotNull(expected);
    }
  }
}