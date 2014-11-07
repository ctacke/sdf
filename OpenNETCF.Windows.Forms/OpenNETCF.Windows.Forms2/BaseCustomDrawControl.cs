using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{

    public class BaseCustomDrawControl<ControlType, CustomDrawType>: BaseControl 
        where ControlType : Control
        where CustomDrawType : NMCUSTOMDRAW
    {
        protected ControlType control; 

        public BaseCustomDrawControl()
        {
            control = Activator.CreateInstance(typeof(ControlType)) as ControlType;
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
            SetControl(control as ControlType);
        }

        public ControlType InnerControl
        {
            get { return (ControlType)control; }
        }

        protected bool fullPaint;

        public bool FullPaint
        {
            get { return fullPaint; }
            set { fullPaint = value; control.Invalidate(); }
        }

        protected virtual void SetControl(ControlType ctl)
        {
        }

        protected override IntPtr OnNotify(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            if (prevWndProc == IntPtr.Zero)
                return IntPtr.Zero;


            NMCUSTOMDRAW nmcd = (NMCUSTOMDRAW)Marshal.PtrToStructure(lParam, typeof(NMCUSTOMDRAW));

            //System.Diagnostics.Debug.WriteLine(string.Format("{0}", (NM)nmcd.hdr.code));

            if (nmcd.hdr.code == (int)NM.DBLCLK)
            {
                OnDoubleClick(EventArgs.Empty);
                return IntPtr.Zero;
            }

            if (nmcd.hdr.code != (int)NM.CUSTOMDRAW || nmcd.hdr.hwndFrom != control.Handle)
                return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);


            CustomDrawType cd = (CustomDrawType)Marshal.PtrToStructure(lParam, typeof(CustomDrawType));

            //System.Diagnostics.Debug.WriteLine(cd.dwDrawStage);
            try
            {
                switch (cd.dwDrawStage)
                {
                    case CDDS.PREPAINT:
                        if (FullPaint && !OnPrepaint(ref cd))
                            return (IntPtr)CDRF.DODEFAULT;
                        return (IntPtr)(CDRF.NOTIFYITEMDRAW | CDRF.NOTIFYPOSTERASE | CDRF.NOTIFYSUBITEMDRAW | CDRF.NOTIFYPOSTPAINT);
                    case CDDS.ITEMPREPAINT:
                        //if (listView.View == View.Details)
                        //    return (int)CDRF.NOTIFYSUBITEMDRAW;
                        if (!OnPrepaintItem(ref cd))
                            return (IntPtr)CDRF.DODEFAULT;
                        if (fullPaint)
                            return (IntPtr)(CDRF.SKIPDEFAULT);
                        else
                            return (IntPtr)(CDRF.NOTIFYSUBITEMDRAW);
                    case CDDS.SUBITEMPREPAINT:
                        if (!OnPrepaintItem(ref cd))
                            return (IntPtr)CDRF.DODEFAULT;
                        if (fullPaint)
                            return (IntPtr)(CDRF.SKIPDEFAULT);
                        else
                            return (IntPtr)(CDRF.NOTIFYITEMDRAW);
                    case CDDS.ITEMPOSTPAINT:
                        return (IntPtr)(CDRF.SKIPDEFAULT);
                    case CDDS.PREERASE:
                        if (fullPaint || !OnPreerase(ref cd))
                            return (IntPtr)CDRF.DODEFAULT;
                        return (IntPtr)(CDRF.NOTIFYPOSTERASE);
                }
            }
            finally
            {
                Marshal.StructureToPtr(cd, lParam, false);
            }

            return (IntPtr)CDRF.DODEFAULT;
        }


        protected virtual bool OnPreerase(ref CustomDrawType cd)
        {
            return true;
        }

        protected virtual bool OnPrepaintItem(ref CustomDrawType cd)
        {
            return false;
        }

        protected int ColorToRGB(Color clr)
        {
            return clr.R | (clr.G << 8) | (clr.B << 16);
        }

        protected Color ColorFromRGB(int rgb)
        {
            return Color.FromArgb(rgb & 0xff, (rgb >> 8) & 0xff, (rgb >> 16) & 0xff);
        }

        protected virtual RECT GetViewRect()
        {
            return (RECT)ClientRectangle;
        }

        protected virtual bool OnPrepaint(ref CustomDrawType cd)
        {
            IntPtr hdc = cd.hdc;
            return OnDrawBackground(hdc, GetViewRect(), cd.rc);
        }

        protected virtual bool OnDrawBackground(IntPtr hdc, RECT rcView, RECT rcPaint)
        {
            return false;
        }

        public int SendMessage(WM msg, int wParam, int lParam)
        {
            return Win32Window.SendMessage(InnerControl.Handle, msg, wParam, lParam);
        }



        protected override IntPtr OnMeasureItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        }

        protected override IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        }
    }
}
