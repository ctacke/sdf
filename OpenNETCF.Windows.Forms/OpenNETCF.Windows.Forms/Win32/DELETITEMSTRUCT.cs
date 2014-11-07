using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DELETITEMSTRUCT
    {
        public ODT CtlType;
        public uint CtlID;
        public uint itemID;
        public IntPtr hwndItem;
        public IntPtr itemData;
    }
}
