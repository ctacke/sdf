using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public delegate int MeasureItemHandler(BaseOwnerDrawControl sender, ref MEASUREITEMSTRUCT ms);
    public delegate int DrawItemHandler(BaseOwnerDrawControl sender, ref DRAWITEMSTRUCT ds);

    /// <summary>
    /// Base class for all XX_OWNERDRAW controls - 
    /// those that use WM_DRAWITEM and possibly WM_MEASUREITEM
    /// Examples are Button, Listbox, RadioButton
    /// </summary>
    public class BaseOwnerDrawControl : BaseControl
    {
        public CustomDrawEventHandler PaintDelegate;

        public BaseOwnerDrawControl(string className, int style)
        {
            //InitializeComponent();
            m_hwndControl = Win32Window.CreateWindowEx(0, className, base.Text, (int)WS.CHILD | (int)WS.VISIBLE | style, 0, 0, ClientRectangle.Width, ClientRectangle.Height,
                Handle, (IntPtr)IdProvider.NewId(Handle), IntPtr.Zero, IntPtr.Zero);
        }

        protected MeasureItemHandler MeasureItemDelegate;
        protected override IntPtr OnMeasureItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            MEASUREITEMSTRUCT ms = (MEASUREITEMSTRUCT)Marshal.PtrToStructure(lParam, typeof(MEASUREITEMSTRUCT));
            IntPtr ret = OnMeasureItem(ref ms);
            Marshal.StructureToPtr(ms, lParam, false);
            return ret;
        }

        protected virtual IntPtr OnMeasureItem(ref MEASUREITEMSTRUCT ms)
        {
            if (MeasureItemDelegate == null)
                return IntPtr.Zero;
            return (IntPtr)MeasureItemDelegate(this, ref ms);
        }

        public void SetMeasureItemDelegate(MeasureItemHandler func)
        {
            MeasureItemDelegate = func;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            MeasureItemDelegate = null;
            DrawItemDelegate = null;
            base.OnHandleDestroyed(e);
        }

        protected DrawItemHandler DrawItemDelegate;
        protected override int OnDrawItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            DRAWITEMSTRUCT ds = (DRAWITEMSTRUCT)Marshal.PtrToStructure(lParam, typeof(DRAWITEMSTRUCT));
            return OnDrawItem(ref ds);
        }

        protected virtual int OnDrawItem(ref DRAWITEMSTRUCT ds)
        {
            if (DrawItemDelegate == null)
                return 0;
            return DrawItemDelegate(this, ref ds);
        }

        public void SetDrawItemDelegate(DrawItemHandler func)
        {
            DrawItemDelegate = func;
        }

        protected override int DoDefaultPaint(ref DRAWITEMSTRUCT ds)
        {
            //Default paint

            using (Graphics g = Graphics.FromHdc(ds.hDC))
            using (Pen p = new Pen((ds.itemState & ODS.SELECTED) != 0 ? ColorInv(ForeColor) : ForeColor))
            using (SolidBrush br = new SolidBrush((ds.itemState & ODS.SELECTED) != 0 ? ColorInv(ForeColor) : ForeColor))
            {
                if ((ds.itemState & ODS.SELECTED) != 0)
                    g.Clear(Color.FromArgb(BackColor.ToArgb() ^ 0xffffff));
                else
                    g.Clear(BackColor);
                Rectangle rc = new Rectangle(ds.rcItem.left, ds.rcItem.top, ds.rcItem.right - ds.rcItem.left, ds.rcItem.bottom - ds.rcItem.top);
                StringFormat fmt = new StringFormat();
                fmt.Alignment = fmt.LineAlignment = System.Drawing.StringAlignment.Center;
                g.DrawString(Text, Font, br, rc, fmt);

                rc.Inflate(-1, -1);
                g.DrawRectangle(p, rc);
                return -1;
            }
        }

        protected override IntPtr OnNotify(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        }

        protected override IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        }
    }
}
