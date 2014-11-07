using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OpenNETCF.Drawing.Imaging
{
  public class ColorPalette
  {
    // Methods
    internal ColorPalette()
      : this(0)
    {
    }
    internal ColorPalette(int count)
    {
      this.entries = new Color[count];
      flags = 0;
    }
    internal void ConvertFromMemory(IntPtr memory)
    {
      flags = Marshal.ReadInt32(memory);
      int num1 = Marshal.ReadInt32((IntPtr)(((long)memory) + 4));
      entries = new Color[num1];
      for (int num2 = 0; num2 < num1; num2++)
      {
        int num3 = Marshal.ReadInt32((IntPtr)((((long)memory) + 8) + (num2 * 4)));
        this.entries[num2] = Color.FromArgb(num3);
      }
    }
    internal IntPtr ConvertToMemory()
    {
      IntPtr ptr1 = Marshal.AllocCoTaskMem((int)(4 * (2 + this.entries.Length)));
      Marshal.WriteInt32(ptr1, 0, this.flags);
      Marshal.WriteInt32((IntPtr)(((long)ptr1) + 4), 0, this.entries.Length);
      for (int num1 = 0; num1 < this.entries.Length; num1++)
      {
        Marshal.WriteInt32((IntPtr)(((long)ptr1) + (4 * (num1 + 2))), 0, this.entries[num1].ToArgb());
      }
      return ptr1;
    }

    // Properties
    public Color[] Entries
    {
      get
      {
        return this.entries;
      }
    }


    public int Flags
    {
      get
      {
        return this.flags;
      }
    }



    // Fields
    private Color[] entries;
    private int flags;
  }
}
