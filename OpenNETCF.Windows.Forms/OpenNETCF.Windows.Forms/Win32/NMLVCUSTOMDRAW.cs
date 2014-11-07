using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public class NMLVCUSTOMDRAW : NMCUSTOMDRAW
    {
        //public NMCUSTOMDRAW nmcd;
        public int clrText;
        public int clrTextBk;
        public int iSubItem;
        public LVCDI dwItemType;

        // Group Custom Draw
        public RECT rcText;
        public LVGA uAlign;      // Alignment. Use LVGA_HEADER_CENTER, LVGA_HEADER_RIGHT, LVGA_HEADER_LEFT
    }
}
