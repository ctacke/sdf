using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        partial class SafeNativeMethods
        {		
            #region --------- API Prototypes ---------
            //[DllImport("cellcore.dll", EntryPoint = "ConnMgrRegisterForStatusChangeNotification", SetLastError = true)] //only available on WM5+
            //internal static extern int ConnMgrRegisterForStatusChangeNotification(bool enable, IntPtr hWnd);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrReleaseConnection",SetLastError=true)]
            internal static extern int ConnMgrReleaseConnection(IntPtr hConnection, int bCache);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnection",SetLastError=true)]
            internal static extern int ConnMgrEstablishConnection(ref ConnectionInfo ConnInfo, out IntPtr phConnection);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnectionSync",SetLastError=true)]
            internal static extern int ConnMgrEstablishConnectionSync(ref ConnectionInfo ConnInfo, out IntPtr phConnection, uint dwTimeout, out uint pdwStatus);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEnumDestinations",SetLastError=true)]
		    internal static extern int ConnMgrEnumDestinations(int nIndex, IntPtr pDestinationInfo);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrMapURL",SetLastError=true)]
		    internal static extern int ConnMgrMapURL(string pwszUrl, ref Guid pguid, IntPtr pdwIndex);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrConnectionStatus",SetLastError=true)]
		    internal static extern int ConnMgrConnectionStatus(IntPtr hConnection, out uint pdwStatus);
            
            //[DllImport("cellcore")]
            //extern public static uint ConnMgrQueryDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS stat, ref int size);

            [DllImport("cellcore")]
            extern public static uint ConnMgrQueryDetailedStatus(IntPtr pStat, ref int size);

            [DllImport("cellcore.dll")]
            internal static extern IntPtr ConnMgrApiReadyEvent();

            [DllImport("coredll.dll")]
            internal static extern int CloseHandle(IntPtr hObject);

		    #endregion
        }
    }
}
