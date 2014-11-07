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
