using OpenNETCF.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Threading;
using System;
using System.Diagnostics;

namespace OpenNETCF.Integration.Test
{
  [TestClass()]
  public class BackgroundWorkerTest : TestBase
  {
    [TestMethod()]
    [Description("Ensures passing a null control to the ctor throws an ArgumentNullException")]
    public void CTorNullControlTest()
    {
      ArgumentNullException expected = null;

      try
      {
        BackgroundWorker worker = new BackgroundWorker(null);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected);
    }

    [Ignore]
    [TestMethod()]
    [Description("Tests to ensure progress event fire, in order, and only once per request")]
    public void RunWorkerAsyncProgressEventTest()
    {
      BackgroundWorker worker = new BackgroundWorker();

      worker.WorkerReportsProgress = true;
      worker.ProgressChanged += new ProgressChangedEventHandler(ProgressTestProgressChanged);
      worker.DoWork += new DoWorkEventHandler(ProgressTestDoWork);
      worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProgressTestRunWorkerCompleted);
      m_progressTestComplete = false;
      
      worker.RunWorkerAsync();

      // WaitCallback for completion
      int timeout = 0;
      while (worker.IsBusy)
      {
        Thread.Sleep(100);
        Assert.IsTrue(timeout++ <= 50, "Timed out waiting for worker to complete");
      }

      // make sure we have enough time for all progress events to get handled
      Thread.Sleep(1000);

      // test results
      Assert.IsTrue(m_lastProgress >= 0, "ProgressChanged did not fire");
      Assert.AreEqual(100, m_lastProgress, "Progress didn't run to 100");
      Assert.IsTrue(m_progressTestComplete, "RunWorkerCompleted did not fire");

      worker.Dispose();
    }

    void ProgressTestDoWork(object sender, DoWorkEventArgs e)
    {
      m_lastProgress = -1;
      BackgroundWorker worker = (BackgroundWorker)sender;
      for (int i = 0; i <= 100; i++)
      {
        worker.ReportProgress(i);
      }
    }

    void ProgressTestRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      m_progressTestComplete = true;
    }

    private int m_lastProgress;
    private bool m_progressTestComplete;

    void ProgressTestProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      Assert.IsTrue(e.ProgressPercentage == m_lastProgress + 1);
      m_lastProgress++;
    }

    private BackgroundWorker m_racetarget;
    [TestMethod()]
    [Description("Tests for a known race condition bug (Bugzilla 292)")]
    public void RunWorkerAsyncRaceTest()
    {
      m_racetarget = new BackgroundWorker();
      m_racetarget.RunWorkerCompleted += RunWorkerCompletedProc;
      m_racetarget.DoWork += DoWorkProc;
      m_threadException = null;
      m_racetarget.RunWorkerAsync();

      while (m_racetarget.IsBusy) Thread.Sleep(0);

      // simulate work to pre-empt the workre
      for (int i = 0; i < 1000; i++)
      {
        Thread.Sleep(1);
      }

      Assert.IsNull(m_threadException);

      m_racetarget.Dispose();
    }

    void DoWorkProc(object sender, DoWorkEventArgs e)
    {
    }

    private Exception m_threadException;

    void RunWorkerCompletedProc(object sender, RunWorkerCompletedEventArgs e)
    {
      try
      {
        m_racetarget.RunWorkerAsync();
      }
      catch(Exception ex)
      {
        m_threadException = ex;
      }
    }
  }
}
