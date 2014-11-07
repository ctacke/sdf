using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// Custom Draw Item State
    /// </summary>
    [Flags]
    public enum CDIS
    {
        // itemState flags
        SELECTED = 0x0001,
        GRAYED = 0x0002,
        DISABLED = 0x0004,
        CHECKED = 0x0008,
        FOCUS = 0x0010,
        DEFAULT = 0x0020,
        HOT = 0x0040,
        NOCONTROLFOCUS = 0x8000,
    }
}
