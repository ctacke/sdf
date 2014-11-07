using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.ComponentModel;
using System.Threading;

namespace BackgroundWorkerSample
{
  public partial class Form1 : Form
  {
    private BackgroundWorker m_worker;

    public Form1()
    {
      InitializeComponent();

      m_worker = new BackgroundWorker();
      m_worker.RunWorkerCompleted += RunWorkerCompletedProc;
      m_worker.DoWork += DoWorkProc;
      m_worker.ProgressChanged += ProgressChangedProc;
      m_worker.WorkerReportsProgress = true;
      progressLabel.Text = "0%";
    }

    void ProgressChangedProc(object sender, ProgressChangedEventArgs e)
    {
      progress.Value = e.ProgressPercentage;
      progressLabel.Text = string.Format("{0}%", e.ProgressPercentage);
    }

    void DoWorkProc(object sender, DoWorkEventArgs e)
    {
      for(int i = 0 ; i < 100 ; i++)
      {
        m_worker.ReportProgress(i);
        Thread.Sleep(100);
      }
    }

    void RunWorkerCompletedProc(object sender, RunWorkerCompletedEventArgs e)
    {
      Thread.Sleep(400);
      start.Enabled = true;
      progress.Value = 0;
      progressLabel.Text = "0%";
    }

    private void start_Click(object sender, EventArgs e)
    {
      start.Enabled = false;
      m_worker.RunWorkerAsync();
    }
  }
}