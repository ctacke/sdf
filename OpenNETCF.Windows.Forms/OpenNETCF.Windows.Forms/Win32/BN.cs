using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum BN
    {
        CLICKED = 0,
        PAINT = 1,
        HILITE = 2,
        UNHILITE = 3,
        DISABLE = 4,
        DOUBLECLICKED = 5,
        PUSHED = HILITE,
        UNPUSHED = UNHILITE,
        DBLCLK = DOUBLECLICKED,
        SETFOCUS = 6,
        KILLFOCUS = 7,
    }
}
