using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    internal class CurrentFormMessageFilter : IMessageFilter
    {
        private IntPtr m_currentActiveFormHandle = IntPtr.Zero;

        public bool PreFilterMessage(ref Microsoft.WindowsCE.Forms.Message m)
        {
            switch ((WM)m.Msg)
            {
                case WM.ACTIVATE:
                    if ((WA)NativeMethods.LOWORD(m.WParam.ToInt32()) != WA.INACTIVE)
                    {
                        m_currentActiveFormHandle = m.HWnd;

                        return false;
                    }
                    break;
            }
                    return true;
        }

        public void ActivateCurrentForm()
        {
            ((Win32Window)m_currentActiveFormHandle).ShowWindow(SW.RESTORE);
        }
    }
}
