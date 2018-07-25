using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        partial class SafeNativeMethods
        {		
            internal const uint INSUFFICIENT_BUFFER = 0x8007007a;

            #region --------- API Prototypes ---------
            [DllImport("cellcore.dll", EntryPoint = "ConnMgrRegisterForStatusChangeNotification", SetLastError = true)] //only available on WM5+
            internal static extern int ConnMgrRegisterForStatusChangeNotification(bool enable, IntPtr hWnd);

            [DllImport("cellcore")]
            extern public static uint ConnMgrQueryDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS stat, ref int size);

         
		    #endregion
        }
    }
}
