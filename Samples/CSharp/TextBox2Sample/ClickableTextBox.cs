using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Win32;

namespace TextBox2Sample
{
    public class ClickableTextBox : TextBox2
    {
        public event EventHandler MouseDown;

        protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message m)
        {
            base.WndProc(ref m);

            switch ((WM)m.Msg)
            {
                // do this *after* the base so it can do the focus, etc. for us
                case WM.LBUTTONDOWN:
                    var handler = MouseDown;
                    if (handler != null) handler(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
