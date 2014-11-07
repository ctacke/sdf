using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// owern draw state
    /// </summary>
    [Flags]
    public enum ODS
    {
        SELECTED = 0x0001,
        GRAYED = 0x0002,
        DISABLED = 0x0004,
        CHECKED = 0x0008,
        FOCUS = 0x0010,
    }
}
