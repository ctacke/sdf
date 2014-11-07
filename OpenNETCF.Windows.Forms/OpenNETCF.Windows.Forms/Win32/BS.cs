using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum BS
    {
        PUSHBUTTON = 0x00000000,
        DEFPUSHBUTTON = 0x00000001,
        CHECKBOX = 0x00000002,
        AUTOCHECKBOX = 0x00000003,
        RADIOBUTTON = 0x00000004,
        THREESTATE = 0x00000005,
        AUTO3STATE = 0x00000006,
        GROUPBOX = 0x00000007,
        AUTORADIOBUTTON = 0x00000009,
        OWNERDRAW = 0x0000000B,
        LEFTTEXT = 0x00000020,
        TEXT = 0x00000000,
        LEFT = 0x00000100,
        RIGHT = 0x00000200,
        CENTER = 0x00000300,
        TOP = 0x00000400,
        BOTTOM = 0x00000800,
        VCENTER = 0x00000C00,
        PUSHLIKE = 0x00001000,
        MULTILINE = 0x00002000,
        NOTIFY = 0x00004000,
    }
}
