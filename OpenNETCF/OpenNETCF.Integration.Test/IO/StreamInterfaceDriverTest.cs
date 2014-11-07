using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Integration.Test
{
  [TestClass()]
  public class StreamInterfaceDriverTest : TestBase
  {
    [TestInitialize]
    public override void TestInitialize()
    {
      Register();
    }

    [Ignore]
    [TestMethod()]
    [DeploymentItem("OpenNETCF.dll")]
    [DeploymentItem(@"FileDriver\FileDriver.dll")]
    public void IOCTLTests()
    {
      FileDriver driver = new FileDriver();
      Load();
      try
      {
        // open the driver
        driver.Open(FileAccess.ReadWrite, FileShare.ReadWrite);

        try
        {
          // TEST INPUT-ONLY IOCTL
          // create a test file
          string fileName = @"TestFile.txt";
          byte[] nameBytes = Encoding.Unicode.GetBytes(fileName + "\0");
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_CREATE_FILE, nameBytes, null);

          // verify its creation
          Assert.IsTrue(File.Exists(fileName), "File was not created");

          // TEST OUTPUT-ONLY IOCTL
          // get the filename back
          byte[] outNameBytes = new byte[260];
          int returned = 0;
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_OPEN_FILE_NAME, null, outNameBytes, out returned);

          // verify it's right
          Assert.AreEqual<int>(nameBytes.Length, returned, "Output IOCTL didn't return # of bytes expected");
          Assert.AreEqual<string>(fileName, Encoding.Unicode.GetString(outNameBytes, 0, returned).TrimEnd('\0'), "Output file name isn't expected");

          // TEST INPUT-OUTPUT IOCTL
          byte[] attribBytes = new byte[4];
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_FILE_ATTRIBS, nameBytes, attribBytes, out returned);
          int returnedAttribs = BitConverter.ToInt32(attribBytes, 0);
          int expectedAttribs = GetFileAttributes(fileName);

          Assert.AreEqual<int>(returnedAttribs, expectedAttribs, "Returned attributes don't match actuals");
          Assert.AreEqual<int>(attribBytes.Length, returned, "I/O IOCTL didn't return # of bytes expected");
          
          // TEST VOID IOCTL
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_DO_NOTHING, null, null);
          // no exception == pass on this one
        }
        finally
        {
          driver.Close();
          driver.Dispose();
        }
      }
      finally
      {
        Unload();
      }
    }

    [DllImport("FileDriver.dll")]
    private static extern void Register();
    [DllImport("FileDriver.dll")]
    private static extern void Load();
    [DllImport("FileDriver.dll")]
    private static extern void Unload();
    [DllImport("coredll.dll")]
    private static extern int GetFileAttributes(string fileName);
  }

  public class FileDriver : StreamInterfaceDriver
  {
    public const int FIL_IOCTL_CREATE_FILE = 1;
    public const int FIL_IOCTL_GET_OPEN_FILE_NAME = 2;
    public const int FIL_IOCTL_GET_FILE_ATTRIBS = 3;
    public const int FIL_IOCTL_DO_NOTHING = 4;

    public FileDriver()
      : base("FIL1:")
    {
    }

    public new void Open(System.IO.FileAccess access, System.IO.FileShare share)
    {
      base.Open(access, share);
    }

    public new void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData, out int bytesReturned)
    {
      base.DeviceIoControl(controlCode, inData, outData, out bytesReturned);
    }

    public new void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData)
    {
      base.DeviceIoControl(controlCode, inData, outData);
    }

    public new void Close()
    {
      base.Close();
    }
  }
}
