using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        private static bool m_supportsNotifications;

        public bool SupportsStatusNotifications
        {
            get { return m_supportsNotifications; }
        }

        private static StatusNotificationPump m_statusChangeWnd = null;

        #region Constructor/Destructor
        
        static ConnectionManager()
        {
            DoCellcoreCheck();

            try
            {
                // just calling to see if the method is there
                SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(true, IntPtr.Zero);
            }
            catch (MissingMethodException)
            {
                m_supportsNotifications = false;
                return;
            }

            m_supportsNotifications = true;
        }
        
        ~ConnectionManager()
        {
            if (m_statusChangeWnd != null)
            {
                SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(false, m_statusChangeWnd.Hwnd);
                m_statusChangeWnd.Dispose();
            }

            RequestDisconnect();
        }

        #endregion

        private ConnectionDetailCollection m_connectionDetailColllection = null;

		/// <summary> Returns collection of detailed connection manager items </summary>
		/// <remarks>Does not refresh collection </remarks>
		public ConnectionDetailCollection GetConnectionDetailItems()
		{
			return GetConnectionDetailItems(false);
		}

		/// <summary> Returns collection of detailed connection manager items </summary>
        public ConnectionDetailCollection GetConnectionDetailItems(bool refresh)
        {
			bool newCollection = m_connectionDetailColllection == null;

			if (newCollection || refresh)
            {
				// jsm - Do this only on first-time initialization
				if (newCollection)
				{
					m_connectionDetailColllection = new ConnectionDetailCollection();

					m_statusChangeWnd = new StatusNotificationPump(m_connectionDetailColllection);
					SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(true, m_statusChangeWnd.Hwnd);
				}

				// jsm - Bug 363 - We're not refreshing 
				if (newCollection || refresh)
				{
					int cb = 0;
					uint ret = 0;

					try
					{
						ret = SafeNativeMethods.ConnMgrQueryDetailedStatus(IntPtr.Zero, ref cb);
					}
					catch (MissingMethodException)
					{
						throw new PlatformNotSupportedException("Detailed connection status is not supported on this platform");
					}

					if (ret != SafeNativeMethods.INSUFFICIENT_BUFFER)
					{
						throw new System.ComponentModel.Win32Exception();
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
								m_connectionDetailColllection.Add(new ConnectionDetail(stat));
								pObj = stat.pNext;
							}
						}
						else
						{
							throw new System.ComponentModel.Win32Exception();
						}
					}
					finally
					{
						Marshal.FreeHGlobal(pStat);
					}
				}
            }
            return m_connectionDetailColllection;
        }
    }
}
