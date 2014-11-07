using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OpenNETCF.Win32
{
    public struct NMLISTVIEW
    {
        public NMHDR hdr;
        public int iItem;
        public int iSubItem;
        public uint uNewState;
        public uint uOldState;
        public uint uChanged;
        public Point ptAction;
        public IntPtr lParam;
    }
}
