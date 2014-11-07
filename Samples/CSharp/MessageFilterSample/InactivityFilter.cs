using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;

namespace MessageFilterSample
{
  public class InactivityFilter : IMessageFilter
  {
    public event MethodInvoker InactivityElapsed;

    private Timer m_inactivityTimer;

    public InactivityFilter(int timeoutMilliseconds)
    {
      m_inactivityTimer = new Timer();
      m_inactivityTimer.Interval = timeoutMilliseconds;

      m_inactivityTimer.Tick += new EventHandler(m_inactivityTimer_Tick);
      Reset();
    }

    void m_inactivityTimer_Tick(object sender, EventArgs e)
    {
      m_inactivityTimer.Enabled = false;
      Elapsed = true;

      if (InactivityElapsed != null) InactivityElapsed();
    }

    public bool PreFilterMessage(ref Microsoft.WindowsCE.Forms.Message m)
    {
      switch ((WM)m.Msg)
      {
        case WM.KEYUP:
        case WM.LBUTTONUP:
        case WM.MOUSEMOVE:
          // reset the timer
          m_inactivityTimer.Enabled = false;
          m_inactivityTimer.Enabled = true;
          break;
      }
      return false;
    }

    public int Timeout { get; set; }
    public bool Elapsed { get; private set; }

    public void Reset()
    {
      Elapsed = false;
      m_inactivityTimer.Enabled = true;
    }
  }
}
