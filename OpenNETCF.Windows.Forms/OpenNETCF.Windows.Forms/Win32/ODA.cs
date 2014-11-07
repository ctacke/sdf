using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// owner draw action
    /// </summary>
    [Flags]
    public enum ODA
    {
        DRAWENTIRE = 0x0001,
        SELECT = 0x0002,
        FOCUS = 0x0004,
    }
}
