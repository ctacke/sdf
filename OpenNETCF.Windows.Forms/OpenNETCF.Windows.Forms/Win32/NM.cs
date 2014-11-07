using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum NM
    {
        FIRST = (0),
        OUTOFMEMORY = (FIRST - 1),
        CLICK = (FIRST - 2),
        DBLCLK = (FIRST - 3),
        RETURN = (FIRST - 4),
        RCLICK = (FIRST - 5),
        RDBLCLK = (FIRST - 6),
        SETFOCUS = (FIRST - 7),
        KILLFOCUS = (FIRST - 8),
        CUSTOMDRAW = (FIRST - 12),
        HOVER = (FIRST - 13),
        NCHITTEST = (FIRST - 14),
        KEYDOWN = (FIRST - 15),
        RECOGNIZEGESTURE = (FIRST - 50)
    }
}
