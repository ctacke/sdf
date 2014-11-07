using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    [Flags]
    public enum CDRF
    {
        DODEFAULT = 0x00000000,
        NEWFONT = 0x00000002,
        SKIPDEFAULT = 0x00000004,
        NOVERTBAR = 0x00000008, //For TBSTYLE_DROPDOWN buttons, don't draw vertical separator bar
        NOTIFYPOSTPAINT = 0x00000010,
        NOTIFYITEMDRAW = 0x00000020,
        NOTIFYSUBITEMDRAW = 0x00000020,  // flags are the same, we can distinguish by context
        NOTIFYPOSTERASE = 0x00000040
    }
}
