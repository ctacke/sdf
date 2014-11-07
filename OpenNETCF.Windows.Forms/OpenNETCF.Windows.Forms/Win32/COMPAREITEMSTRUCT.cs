using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    struct COMPAREITEMSTRUCT
    {
        public ODT CtlType;
        public uint CtlID;
        public IntPtr hwndItem;
        public uint itemID1;
        public uint itemData1;
        public uint itemID2;
        public uint itemData2;
        public uint dwLocaleId;
    }
}
