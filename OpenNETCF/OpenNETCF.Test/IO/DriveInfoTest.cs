using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.IO.Test
{
  [TestClass()]
  public class DriveInfoTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures creating an DriveInfo instance with a null name throws an ArgumentNullException")]
    public void DriveInfoNullNameCtorTest()
    {
      ArgumentNullException expected = null;

      try
      {
        DriveInfo target = new DriveInfo(null);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "DriveInfo ctor did not throw ArgumentNullException for null name");
    }

    [TestMethod()]
    [Description("Ensures creating an DriveInfo instance with an empty name throws an ArgumentNullException")]
    public void DriveInfoEmptyNameCtorTest()
    {
      ArgumentNullException expected = null;

      try
      {
        DriveInfo target = new DriveInfo(string.Empty);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "DriveInfo ctor did not throw ArgumentNullException for an empty name");
    }

    [TestMethod()]
    [Description("Ensures creating an DriveInfo instance with a valid drive works")]
    public void DriveInfoConstructorTestPositive()
    {
      DriveInfo target = new DriveInfo("\\");
    }

    [TestMethod()]
    [Description("Ensures creating an DriveInfo instance with non-existent drive name throws")]
    public void DriveInfoConstructorMissingDirTest()
    {
      ArgumentException expected = null;

      try
      {
        DriveInfo target = new DriveInfo("\\MyNonExistentDir");
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "DriveInfo ctor did not throw ArgumentException for a non-existent folder");
    }

    [TestMethod()]
    [Description("Ensures the DriveInfo RootDirectory returns what was sent into the ctor")]
    public void DriveInfoRootDirectoryTest()
    {
      string root = "\\";
      DriveInfo target = new DriveInfo(root);
      Assert.AreEqual(root, target.RootDirectory.FullName, "RootDirectory not stored properly");
    }

    [TestMethod()]
    [Description("Ensures Space properties get set without throwing and that they return reasonable relative values")]
    public void DriveInfoSizeTest()
    {
      string root = "\\";
      DriveInfo target = new DriveInfo(root);
      Assert.IsTrue(target.AvailableFreeSpace > 0, "AvailableFreeSpace is not positive");
      Assert.IsTrue(target.TotalFreeSpace > 0, "TotalFreeSpace is not positive");
      Assert.IsTrue(target.TotalSize > 0, "TotalSize is not positive");
      Assert.IsTrue(target.AvailableFreeSpace <= target.TotalSize, "AvailableFreeSpace > TotalSize");
      Assert.IsTrue(target.TotalFreeSpace <= target.TotalSize, "TotalFreeSpace > TotalSize");
      Assert.IsTrue(target.TotalFreeSpace >= target.AvailableFreeSpace, string.Format("AvailableFreeSpace({0}) > TotalFreeSpace({1})", target.AvailableFreeSpace, target.TotalFreeSpace));
    }
  }
}
