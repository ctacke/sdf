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
using Microsoft.WindowsCE.Forms;
using OpenNETCF.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {

#if !NDOC
            internal class MessagePump : MessageWindow
            {
                private const int WM_APP_CONNMGR = 0x400 + 0;

                private ConnectionManager connmgr;

                public MessagePump(ConnectionManager obj)
                {
                    connmgr = obj;
                    connmgr.hWnd = Hwnd;
                }

                protected override void WndProc(ref Message m)
                {
                    switch (m.Msg)
                    {
                        
                        case WM_APP_CONNMGR:
                            {
                                switch ((ConnectionStatus)m.WParam)
                                {
                                    case ConnectionStatus.Connected:
                                        if (connmgr.Connected != null)
                                        {
                                            connmgr.Connected(connmgr, EventArgs.Empty);
                                        }
                                        goto case ConnectionStatus.AuthenticationFailed;
                                    case ConnectionStatus.Disconnected:
                                        if (connmgr.Disconnected != null)
                                        {
                                            connmgr.Disconnected(connmgr, EventArgs.Empty);
                                        }
                                        goto case ConnectionStatus.AuthenticationFailed;
                                    case ConnectionStatus.ConnectionFailed:
                                        if(connmgr.ConnectionFailed != null)
                                        {
                                            connmgr.ConnectionFailed(connmgr, EventArgs.Empty);
                                        }
                                        goto case ConnectionStatus.AuthenticationFailed;
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
                                        if (connmgr.ConnectionStateChanged != null)
                                        {
                                            connmgr.ConnectionStateChanged(connmgr, (ConnectionStatus)m.WParam);
                                        }
                                        break;
                                }
                            }
                            break;
                    }

                    base.WndProc(ref m);
                }
            }
#endif
        }
    }
