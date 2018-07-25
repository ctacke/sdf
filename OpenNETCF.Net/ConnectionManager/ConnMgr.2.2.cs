using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        private static StatusNotificationPump StatusChangeWnd = null;
        #region Events 

        /// <summary>
        /// Occurs when the connection detail item list changes
        /// </summary>
        public static event EventHandler ConnectionDetailItemsChanged;
        #endregion

        #region Constructor/Destructor
        
        static ConnectionManager()
        {
            if (Environment.OSVersion.Version.Build >= 5)
            {
                StatusChangeWnd = new StatusNotificationPump();
                SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(true, StatusChangeWnd.Hwnd);
            }
        }
        
        ~ConnectionManager()
        {
            if (Environment.OSVersion.Version.Build >= 5)
                SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(false, StatusChangeWnd.Hwnd);

            RequestDisconnect();
        }

        #endregion

        #region Public Methods


        public static List<ConnectionDetail> ConnectionDetailItems
        {
            get
            {
                List<ConnectionDetail> items = new List<ConnectionDetail>();
                int cb = 0;
                uint ret = SafeNativeMethods.ConnMgrQueryDetailedStatus(IntPtr.Zero, ref cb);

                if (ret != SafeNativeMethods.INSUFFICIENT_BUFFER)
                {
                    throw new Exception("failed");
                }

                IntPtr pStat = Marshal.AllocHGlobal(cb);
                try
                {
                    ret = SafeNativeMethods.ConnMgrQueryDetailedStatus(pStat, ref cb);
                    if (ret == 0)
                    {
                        IntPtr pObj = pStat;
                        while (pObj != IntPtr.Zero)
                        {
                            CONNMGR_CONNECTION_DETAILED_STATUS stat = (CONNMGR_CONNECTION_DETAILED_STATUS)Marshal.PtrToStructure(pObj, typeof(CONNMGR_CONNECTION_DETAILED_STATUS));
                            items.Add(new ConnectionDetail(stat));
                            pObj = stat.pNext;
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pStat);
                }

                return items;
            }
        }
        #endregion
    }
}
