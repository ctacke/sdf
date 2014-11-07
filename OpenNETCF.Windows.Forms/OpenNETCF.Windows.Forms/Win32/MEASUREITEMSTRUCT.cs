using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MEASUREITEMSTRUCT
    {
        /// <summary>
        /// Control type
        /// </summary>
        public ODT CtlType;
        /// <summary>
        /// Control ID
        /// </summary>
        public uint CtlID;
        /// <summary>
        /// Item ID (for listview)
        /// </summary>
        public uint itemID;
        /// <summary>
        /// Item width (menu)
        /// </summary>
        public uint itemWidth;
        /// <summary>
        /// Item height (menu, tree, list)
        /// </summary>
        public uint itemHeight;
        /// <summary>
        /// Item data (custom value, app specific)
        /// </summary>
        public uint itemData;
    }
}
