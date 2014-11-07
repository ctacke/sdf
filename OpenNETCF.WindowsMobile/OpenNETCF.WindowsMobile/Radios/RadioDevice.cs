using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsMobile
{
  internal sealed class RadioDevice
  {
    private const int DEV_NAME_PTR_OFFSET = 0;
    private const int DISP_NAME_PTR_OFFSET = 4;
    private const int STATE_OFFSET = 8;
    private const int DESIRED_OFFSET = 12;
    private const int TYPE_OFFSET = 16;
    private const int NEXT_OFFSET = 20;

    public RadioDevice(IntPtr pDeviceList, int offset)
    {
      Pointer = new IntPtr(pDeviceList.ToInt32() + offset);
    }

    public IntPtr Pointer { get; private set; }

    private int GetInt(int offset)
    {
      int[] data = new int[1];
      Marshal.Copy(new IntPtr(Pointer.ToInt32() + offset), data, 0, 1);
      return data[0];
    }

    public bool ActualState
    {
      get { return GetInt(STATE_OFFSET) != 0; }
    }

    public bool DesiredState
    {
      get { return GetInt(DESIRED_OFFSET) != 0; }
    }

    public RadioType RadioType
    {
      get { return (RadioType)GetInt(TYPE_OFFSET); }
    }

    public string DeviceName
    {
      get { return Marshal.PtrToStringUni(new IntPtr(GetInt(DEV_NAME_PTR_OFFSET)));  }
    }

    public string DisplayName
    {
      get { return Marshal.PtrToStringUni(new IntPtr(GetInt(DISP_NAME_PTR_OFFSET)));  }
    }

    public IntPtr Next
    {
      get { return new IntPtr(GetInt(NEXT_OFFSET)); }
    }

    [DllImport("coredll.dll", SetLastError = true)]
    private static extern int LocalFree(IntPtr hMem);
  }
}
