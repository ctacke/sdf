using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// Custom draw draw stage
    /// </summary>
    [Flags]
    public enum CDDS
    {
        PREPAINT = 0x00000001,
        POSTPAINT = 0x00000002,
        PREERASE = 0x00000003,
        POSTERASE = 0x00000004,
        // the 0x000010000 bit means it's individual item specific
        ITEM = 0x00010000,
        ITEMPREPAINT = (ITEM | PREPAINT),
        ITEMPOSTPAINT = (ITEM | POSTPAINT),
        ITEMPREERASE = (ITEM | PREERASE),
        ITEMPOSTERASE = (ITEM | POSTERASE),
        SUBITEM = 0x00020000,
        SUBITEMPREPAINT = SUBITEM | ITEMPREPAINT,
        SUBITEMPOSTPAINT = SUBITEM | ITEMPOSTPAINT,
    }
}
