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
                private const int CONNMGR_STATUS_CONNECTED = 0x10;
                private const int CONNMGR_STATUS_DISCONNECTED = 0x20;
                private const int CONNMGR_STATUS_WAITINGCONNECTION = 0x40;
                private const int CONNMGR_STATUS_WAITINGDISCONNECTION = 0x80;
                private const int CONNMGR_STATUS_CONNECTIONFAILED = 0x21;

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
                                switch ((int)m.WParam)
                                {
                                    case CONNMGR_STATUS_CONNECTED:
                                        //if (connmgr.ConnectionStateChanged != null)
                                        //    connmgr.ConnectionStateChanged(connmgr, EventArgs.Empty);
                                        if (connmgr.Connected != null)
                                            connmgr.Connected(connmgr, EventArgs.Empty);
                                        break;
                                    case CONNMGR_STATUS_WAITINGCONNECTION:
                                        if (connmgr.ConnectionStateChanged != null)
                                            connmgr.ConnectionStateChanged(connmgr, EventArgs.Empty);
                                        break;
                                    // -----------------------------------------------------------------------
                                    // The WndProc never seems to receive CONNMGR_STATUS_DISCONNECTED, so let's remove it
                                    case CONNMGR_STATUS_DISCONNECTED:
                                        if (connmgr.Disconnected != null)
                                            connmgr.Disconnected(connmgr, EventArgs.Empty);
                                        break;
                                    // -----------------------------------------------------------------------
                                    case CONNMGR_STATUS_WAITINGDISCONNECTION:
                                        if (connmgr.ConnectionStateChanged != null)
                                            connmgr.ConnectionStateChanged(connmgr, EventArgs.Empty);
                                        break;
                                    case CONNMGR_STATUS_CONNECTIONFAILED:
                                        if (connmgr.ConnectionFailed != null)
                                            connmgr.ConnectionFailed(connmgr, EventArgs.Empty);
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
