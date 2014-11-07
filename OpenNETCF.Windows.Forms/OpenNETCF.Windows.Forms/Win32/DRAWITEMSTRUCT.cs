using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    /*
     *  DRAWITEMSTRUCT for ownerdraw
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct DRAWITEMSTRUCT
    {
        public ODT CtlType;
        public uint CtlID;
        public uint itemID;
        public ODA itemAction;
        public ODS itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public RECT rcItem;
        public uint itemData;
    }
}
