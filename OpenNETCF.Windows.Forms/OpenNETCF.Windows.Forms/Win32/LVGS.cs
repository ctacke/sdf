using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    [Flags]
    public enum LVGS
    {
        NORMAL = 0x00000000,
        COLLAPSED = 0x00000001,
        HIDDEN = 0x00000002,
        NOHEADER = 0x00000004,
        COLLAPSIBLE = 0x00000008,
        FOCUSED = 0x00000010,
        SELECTED = 0x00000020,
    }
}
