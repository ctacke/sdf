using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Test
{
  [TestClass()]
  public class FileSystemWatcherTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures that a null path throws an ArgumentNullException")]
    public void PathNullTest()
    {
      ArgumentNullException expected = null;

      FileSystemWatcher target = new FileSystemWatcher();

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
        FileSystemWatcher target = new FileSystemWatcher(null);
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
        FileSystemWatcher target = new FileSystemWatcher(null, "*.*");
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
        FileSystemWatcher target = new FileSystemWatcher("\\Path\\That\\Does\\Not\\Exist", "*.*");
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsTrue(expected != null);
    }
  }
}
