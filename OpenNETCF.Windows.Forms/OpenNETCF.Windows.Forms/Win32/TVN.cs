using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    public enum TVN
    {
        FIRST = -400,
        SELCHANGINGA = (FIRST - 1),
        SELCHANGINGW = (FIRST - 50),
        SELCHANGEDA = (FIRST - 2),
        SELCHANGEDW = (FIRST - 51),


        GETDISPINFOA = (FIRST - 3),
        GETDISPINFOW = (FIRST - 52),
        SETDISPINFOA = (FIRST - 4),
        SETDISPINFOW = (FIRST - 53),
        ITEMEXPANDINGA = (FIRST - 5),
        ITEMEXPANDINGW = (FIRST - 54),
        ITEMEXPANDEDA = (FIRST - 6),
        ITEMEXPANDEDW = (FIRST - 55),
        BEGINDRAGA = (FIRST - 7),
        BEGINDRAGW = (FIRST - 56),
        BEGINRDRAGA = (FIRST - 8),
        BEGINRDRAGW = (FIRST - 57),
        DELETEITEMA = (FIRST - 9),
        DELETEITEMW = (FIRST - 58),
        BEGINLABELEDITA = (FIRST - 10),
        BEGINLABELEDITW = (FIRST - 59),
        ENDLABELEDITA = (FIRST - 11),
        ENDLABELEDITW = (FIRST - 60),
        KEYDOWN = (FIRST - 12),
    }
}
