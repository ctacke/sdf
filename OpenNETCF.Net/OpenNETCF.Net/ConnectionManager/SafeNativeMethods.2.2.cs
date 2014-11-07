using System;
using System.Runtime.InteropServices;
using OpenNETCF.Reflection;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        private static void DoCellcoreCheck()
        {
            if (Device.Win32Library("cellcore.dll").Exists)
            {
                if (Device.Win32Library("cellcore.dll").HasMethod("ConnMgrReleaseConnection"))
                {
                    return;
                }
            }

            throw new PlatformNotSupportedException("Connection Manager API is not supported under this platform.");
        }

        partial class SafeNativeMethods
        {		
            internal const uint INSUFFICIENT_BUFFER = 0x8007007a;

            #region --------- API Prototypes ---------
            [DllImport("cellcore.dll", SetLastError = true)] //only available on WM5+
            internal static extern int ConnMgrRegisterForStatusChangeNotification(bool enable, IntPtr hWnd);

            [DllImport("cellcore.dll", SetLastError = true)]
            extern public static uint ConnMgrQueryDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS stat, ref int size);
         
		    #endregion
        }
    }
}
