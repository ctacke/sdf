using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public abstract class BaseControl: UserControl
    {
        protected IntPtr prevWndProc;
        protected WndProcDelegate newWndProc;
        protected IntPtr m_hwndControl;

        /// <summary>
        /// Constructor. Currently does nothing
        /// </summary>
        public BaseControl()
        {
            
        }

        /// <summary>
        /// Text property is overriden to work with the hosted control text
        /// </summary>
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
                    Win32Window.SetWindowText(m_hwndControl, value);
            }
        }

        /// <summary>
        /// Our control never gets focus. Instead the hosted control does
        /// </summary>
        public override bool Focused
        {
            get
            {
                return Win32Window.GetFocus() == m_hwndControl;
            }
        }

        /// <summary>
        /// Invoked when control handle is created. This is where it gets subclassed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Subclass();
        }

        /// <summary>
        /// Control is subclassed by substituting the GWL_WNDPROC DWORD with a pointer to 
        /// our replacement window proc
        /// </summary>
        private void Subclass()
        {
            newWndProc = new WndProcDelegate(WndProc);
            prevWndProc = (IntPtr)Win32Window.GetWindowLong(Handle, GWL.WNDPROC);
            Win32Window.SetWindowLong(Handle, GWL.WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));
        }

        /// <summary>
        /// Before control is destroyed, we want to restore the original window proc
        /// to avoid side effects
        /// </summary>
        private void Unsubclass()
        {
            if (prevWndProc == IntPtr.Zero)
                return;
            Win32Window.SetWindowLong(Handle, GWL.WNDPROC, (int)prevWndProc);
            prevWndProc = IntPtr.Zero;
        }

        /// <summary>
        /// Helper function to invert .Net Color
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        protected Color ColorInv(Color clr)
        {
            return Color.FromArgb(clr.ToArgb() ^ 0xffffff);
        }

        /// <summary>
        /// When control is disposed of we need to destroy the hosted control
        /// For managed hosted controls this needs to be overriden
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_hwndControl != IntPtr.Zero)
                    Win32Window.DestroyWindow(m_hwndControl);
                m_hwndControl = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Empty default DrawItem handler
        /// Handles WM_DRAWITEM
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        protected virtual int OnDrawItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            return 0;
        }

        /// <summary>
        /// Empty default paint routine
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        protected virtual int DoDefaultPaint(ref DRAWITEMSTRUCT ds)
        {
            return 0; // Not handled
        }

        /// <summary>
        /// By default we don't handle WM_NOTIFY
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        protected abstract IntPtr OnNotify(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

        protected abstract IntPtr OnMeasureItem(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

        protected abstract IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

        protected virtual IntPtr WndProc(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("{1:X} {0}", ((WM)msg).ToString(), hWnd));
            // For WM_DESTROY - unsubclass the control
            if ((WM)msg == WM.DESTROY)
            {
                IntPtr p = prevWndProc;
                Unsubclass();
                return Win32Window.CallWindowProc(p, hWnd, msg, wParam, lParam);
            }
            else if ((WM)msg == WM.DRAWITEM)
            {
                return (IntPtr)OnDrawItem(hWnd, msg, wParam, lParam);
            }
            else if ((WM)msg == WM.MEASUREITEM)
            {
                return (IntPtr)OnMeasureItem(hWnd, msg, wParam, lParam);
            }
            else if ((WM)msg == WM.NOTIFY)
            {
                return (IntPtr)OnNotify(hWnd, msg, wParam, lParam);
            }
            else if ((WM)msg == WM.COMMAND)
            {
                return (IntPtr)OnCommand(hWnd, msg, wParam, lParam);
            }
            else if ((WM)msg == WM.SIZE)
            {
                OnResize(EventArgs.Empty);
                return IntPtr.Zero;
            }
            else
                return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        }

        //When the parent control is resized, the hosted needs to be resized as well
        // We emulate dock.fill behavior
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Win32Window.SetWindowPos(m_hwndControl, 0, 0, 0, ClientRectangle.Width, ClientRectangle.Height, SWP.NOZORDER);
        }

    }

}
