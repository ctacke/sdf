using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.IO;

namespace StreamDriverSample
{
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
