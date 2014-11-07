using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public class OwnerDrawCheckBox: OwnerDrawButton
    {
        private bool m_fChecked;
        public OwnerDrawCheckBox()
            : base((int)(BS.CHECKBOX | BS.OWNERDRAW))
        {
            m_fChecked = false;
        }

        public CheckState CheckState
        {
            get 
            {
                return m_fChecked? CheckState.Checked: CheckState.Unchecked;
            }
            set
            {
                m_fChecked = value == CheckState.Checked;
                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            CheckState = CheckState == CheckState.Checked?CheckState.Unchecked: CheckState.Checked;
        }

        public bool AutoCheck
        {
            get { return ((int)Win32Window.GetWindowLong(m_hwndControl, GWL.STYLE) & (int)BS.AUTOCHECKBOX) != 0; }
            set 
            {
                int style = (int)Win32Window.GetWindowLong(m_hwndControl, GWL.STYLE);
                if (value)
                    style |= (int)BS.AUTOCHECKBOX;
                else
                    style &= ~(int)BS.AUTOCHECKBOX;
                Win32Window.SetWindowLong(m_hwndControl, GWL.STYLE, style); 
            }
        }

        protected override int DoDefaultPaint(ref DRAWITEMSTRUCT ds)
        {
            return base.DoDefaultPaint(ref ds);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
