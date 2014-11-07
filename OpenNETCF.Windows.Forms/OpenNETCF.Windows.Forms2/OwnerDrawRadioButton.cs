using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public class OwnerDrawRadioButton: BaseOwnerDrawControl
    {
        public OwnerDrawRadioButton():this(0)
        {
        }

        public OwnerDrawRadioButton(int extraStyle)
            : base("BUTTON", (int)BS.OWNERDRAW | extraStyle)
        {
        }

        protected override IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            BN code = (BN)(wParam.ToInt32() >> 16);
            switch (code)
            {
                case BN.CLICKED:
                    OnClick(EventArgs.Empty);
                    Win32Window.SendMessage(m_hwndControl, (int)BM.SETCHECK, (int)BST.CHECKED, 0);
                    return IntPtr.Zero;
                case BN.KILLFOCUS:
                    OnLostFocus(EventArgs.Empty);
                    return IntPtr.Zero;
                case BN.SETFOCUS:
                    OnGotFocus(EventArgs.Empty);
                    return IntPtr.Zero;
                case BN.DBLCLK:
                    OnDoubleClick(EventArgs.Empty);
                    return IntPtr.Zero;
                default:
                    return Win32Window.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
            }
        }
    }
}
