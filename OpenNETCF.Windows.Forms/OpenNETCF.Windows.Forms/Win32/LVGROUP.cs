using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LVGROUP
    {
        public uint cbSize;
        public LVGF mask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string pszHeader;
        public int cchHeader;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string pszFooter;
        public int cchFooter;

        public int iGroupId;

        public LVGS stateMask;
        public LVGS state;
        public LVGA uAlign;
    }
}
