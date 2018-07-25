#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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

        public ConnectionDetailCollection GetConnectionDetailItems()
        {
            if (m_connectionDetailColllection == null)
            {
                m_connectionDetailColllection = new ConnectionDetailCollection();

                m_statusChangeWnd = new StatusNotificationPump(m_connectionDetailColllection);
                SafeNativeMethods.ConnMgrRegisterForStatusChangeNotification(true, m_statusChangeWnd.Hwnd);

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
            return m_connectionDetailColllection;
        }
    }
}
