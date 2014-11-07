using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Win32
{
    /// <summary>
    /// Notification header returned in WM_NOTIFY lParam
    /// </summary>
    public struct NMHDR
    {
        /// <summary>
        /// Window that has sent WM_NOTIFY
        /// </summary>
        public IntPtr hwndFrom;
        /// <summary>
        /// Control ID of the window that sent the notification
        /// </summary>
        public int idFrom;
        /// <summary>
        /// Notification code
        /// </summary>
        public int code;         // NM_ code
    }
}
