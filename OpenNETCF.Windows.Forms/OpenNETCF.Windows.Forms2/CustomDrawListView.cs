using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using OpenNETCF.GDIPlus;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public class CustomDrawListView: BaseCustomDrawControl<ListView, NMLVCUSTOMDRAW>
    {
        public event ListViewQueryItemColorHandler QueryItemColor;
        public event ListViewPaintBackgroundEventHandler PaintBackground;
        public event ListViewPaintItemEventHandler PaintItem;

        protected int standardItemHeight;

        private List<Icon> m_icons;

        public List<Icon> Icons
        {
            get { return m_icons; }
        }

        public ListView.ColumnHeaderCollection Columns
        {
            get { return control.Columns; }
        }

        public ListView.ListViewItemCollection Items
        {
            get { return control.Items; }
        }

        public ImageList LargeImageList
        {
            get { return InnerControl.LargeImageList; }
            set { InnerControl.LargeImageList = value; }
        }

        public ImageList SmallImageList
        {
            get { return InnerControl.SmallImageList; }
            set { InnerControl.SmallImageList = value; }
        }

        public View View
        {
            get { return InnerControl.View; }
            set { InnerControl.View = value; }
        }

        public CustomDrawListView()
            : base()
        {
            m_icons = new List<Icon>();
            fullPaint = false;
        }

        protected override void SetControl(ListView ctl)
        {
            base.SetControl(ctl);
            //NativeMethods.SendMessage(ctl.Handle, (int)LVM.SETEXTENDEDLISTVIEWSTYLE, (int)LVS_EX.DOUBLEBUFFER, (int)LVS_EX.DOUBLEBUFFER);
        }


        protected override bool OnPrepaintItem(ref NMLVCUSTOMDRAW lvcd)
        {
            if (!fullPaint)
            {
                if (QueryItemColor == null)
                    return false;

                ListViewItem item = control.Items[(int)lvcd.dwItemSpec];
                ListViewItem.ListViewSubItem subItem = item.SubItems[lvcd.iSubItem];
                Color clrFore = item.ForeColor;
                Color clrBack = item.BackColor;
                Font font = Font;
                bool bRet = false;
                QueryItemColor(this, item, lvcd.iSubItem, ref clrFore, ref clrBack, ref font);
                if (clrFore != item.ForeColor || clrBack != item.BackColor)
                {
                    lvcd.clrText = ColorToRGB(clrFore);
                    lvcd.clrTextBk = ColorToRGB(clrBack);
                    bRet = true;
                }
                if (font != Font)
                {
                    NativeMethods.SelectObject(lvcd.hdc, font.ToHfont());
                    bRet = true;
                }
                return bRet;
            }

            if (PaintItem != null)
            {
                ListViewPaintItemEventArgs args = new ListViewPaintItemEventArgs();
                args.Hdc = lvcd.hdc;
                args.ItemIndex = (int)lvcd.dwItemSpec;
                args.SubitemIndex = lvcd.iSubItem;
                args.Selected = (lvcd.uItemState & CDIS.SELECTED) != 0;
                args.Focused = (lvcd.uItemState & CDIS.FOCUS) != 0;
                args.RectPaint = (Rectangle)lvcd.rc;
                PaintItem(this, args);
                return true;
            }

            return false;
        }

        protected override bool OnDrawBackground(IntPtr hdc, RECT rcView, RECT rcPaint)
        {
            if (PaintBackground != null)
            {
                ListViewPaintBackgroundEventArgs args = new ListViewPaintBackgroundEventArgs(hdc, (Rectangle)rcView, (Rectangle)rcPaint);
                PaintBackground(this, args);
                return true;
            }
            return false;
        }

        protected override RECT GetViewRect()
        {
            RECT rcView = new RECT();
            Win32Window.SendMessage(control.Handle, (int)LVM.GETVIEWRECT, 0, ref rcView);
            return rcView;
        }

        public void SetIconSpacing(Size spacing)
        {
            if (control.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Control has not been created");

            Win32Window.SendMessage(control.Handle, (int)LVM.SETICONSPACING, 0, (spacing.Width | (spacing.Height << 16)));

        }

        public void SetItemPosition(int item, Point position)
        {
            if (control.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Control has not been created");

            Win32Window.SendMessage(control.Handle, (int)LVM.SETITEMPOSITION, item, (position.X | (position.Y << 16)));
        }

        /// <summary>
        /// Sets listview background color
        /// Use Color.Transparent to avoid painting background
        /// </summary>
        /// <param name="clr"></param>
        public void SetBkColor(Color clr)
        {
            if (control.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Control has not been created");
            int colorRGB = clr == Color.Transparent? -1: ColorToRGB(clr);
            Win32Window.SendMessage(control.Handle, (int)LVM.SETBKCOLOR, 0, colorRGB);
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (control.Handle != IntPtr.Zero)
                    SetBkColor(value);
            }
        }
    }

    /// <summary>
    /// Arguments for custom-draw listbox background painting
    /// </summary>
    public class ListViewPaintBackgroundEventArgs : EventArgs
    {
        /// <summary>
        /// Display context
        /// </summary>
        public IntPtr Hdc;
        /// <summary>
        /// Bounding rectangle for all items in the listview
        /// </summary>
        public Rectangle RectView;
        /// <summary>
        /// Paint rectangle
        /// </summary>
        public Rectangle RectPaint;

        public ListViewPaintBackgroundEventArgs(IntPtr hdc, Rectangle rcView, Rectangle rcPaint)
        {
            Hdc = hdc;
            RectView = rcView;
            RectPaint = rcPaint;
        }
    }

    public class ListViewPaintItemEventArgs : EventArgs
    {
        /// <summary>
        /// Display context
        /// </summary>
        public IntPtr Hdc;
        /// <summary>
        /// 
        /// </summary>
        public Rectangle RectPaint;
        /// <summary>
        /// Item being painted
        /// </summary>
        public int ItemIndex;
        /// <summary>
        /// Subitem being painted (or null for the item itself)
        /// </summary>
        public int SubitemIndex;
        /// <summary>
        /// Paint selected state
        /// </summary>
        public bool Selected;
        /// <summary>
        /// Paint focused state
        /// </summary>
        public bool Focused;
    }

    public delegate void ListViewPaintBackgroundEventHandler(object sender, ListViewPaintBackgroundEventArgs e);
    public delegate void ListViewPaintItemEventHandler(object sender, ListViewPaintItemEventArgs e);
    public delegate void ListViewQueryItemColorHandler(object sender, ListViewItem item, int subItem, ref Color clrFore, ref Color clrBack, ref Font font);
}
