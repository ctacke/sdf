using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Win32;

namespace MessageFilterSample
{
  public partial class Form1 : Form
  {
    InactivityFilter m_filter;
    public Form1()
    {
      InitializeComponent();

      // set a filter for 5 seconds
      m_filter = new InactivityFilter(5000);
      m_filter.InactivityElapsed += new MethodInvoker(m_filter_InactivityElapsed);
      Application2.AddMessageFilter(m_filter);
    }

    void m_filter_InactivityElapsed()
    {
      MessageBox.Show("Inactivity timer fired");
    }

    private void reset_Click(object sender, EventArgs e)
    {
      m_filter.Reset();
    }
  }
}