using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// wParams for the WM.ACTIVATE message
    /// </summary>
    public enum WA
    {
        /// <summary>
        /// Deactivated
        /// </summary>
        INACTIVE = 0,
        /// <summary>
        /// Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow function or by use of the keyboard interface to select the window).
        /// </summary>
        ACTIVE = 1,
        /// <summary>
        /// Activated by a mouse click
        /// </summary>
        CLICKACTIVE = 2
    }
}
