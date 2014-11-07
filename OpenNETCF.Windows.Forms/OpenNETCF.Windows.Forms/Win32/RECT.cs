using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public RECT(int l, int t, int r, int b)
        {
            this.left = l; this.top = t; this.right = r; this.bottom = b;
        }
        public static explicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
        }
        public static explicit operator RECT(Rectangle r)
        {
            return new RECT(r.Left, r.Top, r.Right, r.Bottom);
        }
    }
}
