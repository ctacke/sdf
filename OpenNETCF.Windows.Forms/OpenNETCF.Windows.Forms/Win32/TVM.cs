using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum TVM
    {
        FIRST = 0x1100,
        INSERTITEMW = (FIRST + 50),
        DELETEITEM = (FIRST + 1),
        EXPAND = (FIRST + 2),
        GETITEMRECT = (FIRST + 4),
        GETCOUNT = (FIRST + 5),
        GETINDENT = (FIRST + 6),
        SETINDENT = (FIRST + 7),
        GETIMAGELIST = (FIRST + 8),
        SETIMAGELIST = (FIRST + 9),
        GETNEXTITEM = (FIRST + 10),
        SELECTITEM = (FIRST + 11),
        GETITEMW = (FIRST + 62),
        SETITEMW = (FIRST + 63),
        EDITLABELW = (FIRST + 65),
        GETEDITCONTROL = (FIRST + 15),
        GETVISIBLECOUNT = (FIRST + 16),
        HITTEST = (FIRST + 17),
        CREATEDRAGIMAGE = (FIRST + 18),
        SORTCHILDREN = (FIRST + 19),
        ENSUREVISIBLE = (FIRST + 20),
        SORTCHILDRENCB = (FIRST + 21),
        ENDEDITLABELNOW = (FIRST + 22),
        GETISEARCHSTRINGW = (FIRST + 64),
        SETITEMSPACING = (FIRST + 56),
    }
}
