using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    [Flags]
    public enum LVGA
    {
        HEADER_LEFT = 0x00000001,
        HEADER_CENTER = 0x00000002,
        HEADER_RIGHT = 0x00000004,  // Don't forget to validate exclusivity
        FOOTER_LEFT = 0x00000008,
        FOOTER_CENTER = 0x00000010,
        FOOTER_RIGHT = 0x00000020,  // Don't forget to validate exclusivity
    }
}
