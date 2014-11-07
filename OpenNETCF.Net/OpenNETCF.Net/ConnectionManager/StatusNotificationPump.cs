using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsCE.Forms;
using OpenNETCF.Win32;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {

        internal class StatusNotificationPump : MessageWindow
        {
			// jsm - Bug 363 - Registration wasn't being done correctly before
			private const string CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG = "CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG";

            private ConnectionDetailCollection m_detailsCollection;
			private int m_statusChangedMsg;

            public StatusNotificationPump(ConnectionDetailCollection detailsCollection)
            {
                m_detailsCollection = detailsCollection;
				m_statusChangedMsg = RegisterWindowMessage(CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG);
            }

            protected override void WndProc(ref Message m)
            {
				// Connection status chanaged?
				if (m.Msg == m_statusChangedMsg)
				{
					switch (((ConnectionStatus)((int)m.WParam)))
					{
						case ConnectionStatus.Connected:
						case ConnectionStatus.Suspended:
						case ConnectionStatus.Unknown:
						case ConnectionStatus.Disconnected:
						case ConnectionStatus.ConnectionFailed:
						case ConnectionStatus.ConnectionCancelled:
						case ConnectionStatus.ConnectionDisabled:
						case ConnectionStatus.NoPathToDestination:
						case ConnectionStatus.WaitingForPath:
						case ConnectionStatus.WaitingForPhone:
						case ConnectionStatus.PhoneOff:
						case ConnectionStatus.ExclusiveConflict:
						case ConnectionStatus.NoResources:
						case ConnectionStatus.ConnectionLinkFailed:
						case ConnectionStatus.AuthenticationFailed:
						case ConnectionStatus.WaitingConnection:
						case ConnectionStatus.WaitingForResource:
						case ConnectionStatus.WaitingForNetwork:
						case ConnectionStatus.WaitingDisconnection:
						case ConnectionStatus.WaitingConnectionAbort:
							m_detailsCollection.RaiseConnectionDetailItemsChanged((ConnectionStatus)m.WParam);
							break;
					}
                }

                base.WndProc(ref m);
            }

			// Register a windows message
			[DllImport("coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
			internal static extern int RegisterWindowMessage(string lpString);
        }
    }
}
