using OpenNETCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Core.Test
{
  [TestClass()]
  public class FileInfoExtensionsTest : TestBase
  {
    [TestMethod()]
    [DeploymentItem("test.txt")]
    public void IsWritableTrueTest()
    {
      string path = Path.Combine(TestContext.TestDeploymentDir, "test.txt");

      FileInfo fi = new FileInfo(path);
      fi.Attributes &= ~FileAttributes.ReadOnly;
      Assert.IsTrue(fi.IsWritable());
    }

    [TestMethod()]
    [DeploymentItem("test.txt")]
    public void IsWritableFalseTest()
    {
      string path = Path.Combine(TestContext.TestDeploymentDir, "test.txt");

      FileInfo fi = new FileInfo(path);
      fi.Attributes |= FileAttributes.ReadOnly;

      Assert.IsFalse(fi.IsWritable());
      fi.Attributes &= ~FileAttributes.ReadOnly;
    }

    [TestMethod()]
    [DeploymentItem("test.txt")]
    public void MakeWritableTest()
    {
      string path = Path.Combine(TestContext.TestDeploymentDir, "test.txt");

      FileInfo fi = new FileInfo(path);
      fi.Attributes |= FileAttributes.ReadOnly;

      fi.MakeWritable();

      Assert.IsTrue((fi.Attributes & FileAttributes.ReadOnly) == 0);
      File.Delete(path);
    }
  }
}
