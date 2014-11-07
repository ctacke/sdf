using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public class NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public CDDS dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public uint dwItemSpec;  // this is control specific, but it's how to specify an item.  valid only with CDDS_ITEM bit set
        public CDIS uItemState;
        public IntPtr lItemlParam;
    }
}
