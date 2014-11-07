using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Test
{
  [TestClass()]
  public class FileSystemMonitorTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures that a null path throws an ArgumentNullException")]
    public void PathNullTest()
    {
      ArgumentNullException expected = null;

      FileSystemMonitor target = new FileSystemMonitor();

      try
      {
        target.Path = null;
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }
      target.Dispose();

      Assert.IsTrue(expected != null);
    }

    [TestMethod()]
    [Description("Ensures that a null path throws an ArgumentNullException")]
    public void FileSystemWatcherConstructorNullPath()
    {
      ArgumentNullException expected = null;

      try
      {
        FileSystemMonitor target = new FileSystemMonitor(null);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsTrue(expected != null);
    }

    [TestMethod()]
    [Description("Ensures that a null path throws an ArgumentNullException")]
    public void FileSystemWatcherConstructorNullPath2()
    {
      ArgumentNullException expected = null;

      try
      {
        FileSystemMonitor target = new FileSystemMonitor(null, "*.*");
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsTrue(expected != null);
    }

    [TestMethod()]
    [Description("Ensures that a non-existent path throws an ArgumentException")]
    public void FileSystemWatcherConstructorInvalidPathTest()
    {
      ArgumentException expected = null;

      try
      {
        FileSystemMonitor target = new FileSystemMonitor("\\Path\\That\\Does\\Not\\Exist", "*.*");
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsTrue(expected != null);
    }
  }
}
