using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    [Flags]
    public enum LVGF
    {
        NONE = 0x00000000,
        HEADER = 0x00000001,
        FOOTER = 0x00000002,
        STATE = 0x00000004,
        ALIGN = 0x00000008,
        GROUPID = 0x00000010,
    }
}
