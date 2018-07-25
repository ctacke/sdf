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

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case CONNMGR_STATUS_CHANGE_NOTIFICATION_MSG:
                        {
                            if (ConnectionManager.ConnectionDetailItemsChanged != null)
                                ConnectionManager.ConnectionDetailItemsChanged(null, EventArgs.Empty);

                            break;
                        }
                }

                base.WndProc(ref m);
            }
        }
    }
}
