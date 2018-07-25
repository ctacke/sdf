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
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsCE.Forms;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        internal class StatusNotificationPump : MessageWindow
        {
            private const int CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG = 0x1A; //it always sends 26.  Need to test on other devices.

            private ConnectionDetailCollection m_detailsCollection;

            public StatusNotificationPump(ConnectionDetailCollection detailsCollection)
            {
                m_detailsCollection = detailsCollection;
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG:
                        switch ((ConnectionStatus)m.WParam)
                        {
                            case ConnectionStatus.Connected:
                            case ConnectionStatus.Disconnected:
                            case ConnectionStatus.ConnectionFailed:
                            case ConnectionStatus.AuthenticationFailed:
                            case ConnectionStatus.ConnectionCancelled:
                            case ConnectionStatus.ConnectionDisabled:
                            case ConnectionStatus.ConnectionLinkFailed:
                            case ConnectionStatus.ExclusiveConflict:
                            case ConnectionStatus.NoPathToDestination:
                            case ConnectionStatus.NoResources:
                            case ConnectionStatus.PhoneOff:
                            case ConnectionStatus.Suspended:
                            case ConnectionStatus.Unknown:
                            case ConnectionStatus.WaitingConnection:
                            case ConnectionStatus.WaitingConnectionAbort:
                            case ConnectionStatus.WaitingDisconnection:
                            case ConnectionStatus.WaitingForNetwork:
                            case ConnectionStatus.WaitingForPath:
                            case ConnectionStatus.WaitingForPhone:
                            case ConnectionStatus.WaitingForResource:
                                m_detailsCollection.RaiseConnectionDetailItemsChanged((ConnectionStatus)m.WParam);
                                break;
                        }
                        break;
                }

                base.WndProc(ref m);
            }
        }
    }
}
