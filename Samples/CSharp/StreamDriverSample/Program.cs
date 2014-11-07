using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace StreamDriverSample
{
  class Program
  {
    static void Main(string[] args)
    {
      Register();

      Load();

      try
      {
        FileDriver driver = new FileDriver();

        // open the driver
        driver.Open(FileAccess.ReadWrite, FileShare.ReadWrite);

        try
        {
          // create a test file
          string fileName = @"\TestFile.txt";
          byte[] nameBytes = Encoding.Unicode.GetBytes(fileName + "\0");
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_CREATE_FILE, nameBytes, null);

          // verify its creation
          if (!File.Exists(fileName))
          {
          }

          // get the filename back
          byte[] outNameBytes = new byte[260];
          int returned = 0;
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_OPEN_FILE_NAME, null, outNameBytes, out returned);

          //        Assert.AreEqual<int>(nameBytes.Length, returned);
          //        Assert.AreEqual<string>(fileName, Encoding.Unicode.GetString(outNameBytes, 0, returned).TrimEnd('\0'));

          byte[] attribBytes = new byte[4];
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_FILE_ATTRIBS, nameBytes, attribBytes, out returned);
          int returnedAttribs = BitConverter.ToInt32(attribBytes, 0);
          int expectedAttribs = GetFileAttributes(fileName);

          driver.DeviceIoControl(FileDriver.FIL_IOCTL_DO_NOTHING, null, null);
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
}
