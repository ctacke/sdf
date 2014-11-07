using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public class NMTVCUSTOMDRAW : NMCUSTOMDRAW
    {
        //public NMCUSTOMDRAW nmcd;
        public int clrText;
        public int clrTextBk;
    }
}
