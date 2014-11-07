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
    public delegate void CustomDrawEventHandler(object sender, Graphics gx, Rectangle rcPaint, ODA action, ODS state);
    public partial class OwnerDrawButton : BaseOwnerDrawControl
    {

        //public new event EventHandler Click;
        //public new event EventHandler GotFocus;
        //public new event EventHandler LostFocus;
        //public new event EventHandler DoubleClick;

        public OwnerDrawButton()
            : base("BUTTON", (int)(BS.OWNERDRAW | BS.NOTIFY))
        {
            NativeMethods.SetWindowText(m_hwndControl, base.Text);
        }

        internal OwnerDrawButton(int style):base("BUTTON", style)
        {
        }

        public Button Button { get { return button; } }

        public override string Text
        {
            get
            {
                return Win32Window.GetWindowText(m_hwndControl);
            }
            set
            {
                if (m_hwndControl == IntPtr.Zero)
                    base.Text = value;
                else
                    NativeMethods.SetWindowText(m_hwndControl, value);
            }
        }

        public override bool Focused
        {
            get
            {
                return Win32Window.GetFocus() == m_hwndControl;
            }
        }

        //public CheckState CheckState
        //{
        //    get
        //    {
        //        int state = NativeMethods.SendMessage(m_hwndControl, (int)BM.GETSTATE, 0);

        //    }
        //}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Subclass();
        }

        void ExtraInit(object sender, EventArgs e)
        {
        }

        private void Subclass()
        {
            newWndProc = new WndProcDelegate(WndProc);
            prevWndProc = (IntPtr)Win32Window.GetWindowLong(Handle, GWL.WNDPROC);
            Win32Window.SetWindowLong(Handle, GWL.WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));
        }

        protected override int OnDrawItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            DRAWITEMSTRUCT ds = (DRAWITEMSTRUCT)Marshal.PtrToStructure(lParam, typeof(DRAWITEMSTRUCT));
            if (PaintDelegate != null)
            {
                using (Graphics g = Graphics.FromHdc(ds.hDC))
                {
                    Rectangle rcPaint = (Rectangle)ds.rcItem;
                    PaintDelegate(this, g, rcPaint, ds.itemAction, ds.itemState);
                    return 0;
                }
            }

            //Default paint

            //System.Diagnostics.Debug.WriteLine(ds.itemState);
            //SetTextMode(cd.hdc, TRANSPARENT);
            //cd.clrText = 0xffffff;
            return DoDefaultPaint(ref ds);
        }

        protected override int DoDefaultPaint(ref DRAWITEMSTRUCT ds)
        {
            using (Graphics g = Graphics.FromHdc(ds.hDC))
            using (Pen p = new Pen((ds.itemState & ODS.SELECTED) != 0 ? ColorInv(ForeColor) : ForeColor))
            using (SolidBrush br = new SolidBrush((ds.itemState & ODS.SELECTED) != 0 ? ColorInv(ForeColor) : ForeColor))
            {
                if ((ds.itemState & ODS.SELECTED) != 0)
                    g.Clear(Color.FromArgb(BackColor.ToArgb() ^ 0xffffff));
                else
                    g.Clear(BackColor);
                Rectangle rc = (Rectangle)ds.rcItem;
                StringFormat fmt = new StringFormat();
                fmt.Alignment = fmt.LineAlignment = System.Drawing.StringAlignment.Center;
                g.DrawString(Text, Font, br, rc, fmt);

                rc.Inflate(-1, -1);
                g.DrawRectangle(p, rc);
                return -1;
            }
        }


        protected override IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            BN code = (BN)(wParam.ToInt32() >> 16);
            switch (code)
            {
                case BN.CLICKED:
                    OnClick(EventArgs.Empty);
                    //if (Click != null)
                    //    Click(this, EventArgs.Empty);
                    return IntPtr.Zero;
                case BN.KILLFOCUS:
                    OnLostFocus(EventArgs.Empty);
                    //if (LostFocus != null)
                    //    LostFocus(this, EventArgs.Empty);
                    return IntPtr.Zero;
                case BN.SETFOCUS:
                    OnGotFocus(EventArgs.Empty);
                    //if (GotFocus != null)
                    //    GotFocus(this, EventArgs.Empty);
                    return IntPtr.Zero;
                case BN.DBLCLK:
                    OnDoubleClick(EventArgs.Empty);
                    //if (DoubleClick != null)
                    //    DoubleClick(this, EventArgs.Empty);
                    return IntPtr.Zero;
                default:
                    return base.OnCommand(hWnd, msg, wParam, lParam);
            }

        }
    }
}
