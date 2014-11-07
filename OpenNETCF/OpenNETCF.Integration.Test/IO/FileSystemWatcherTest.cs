using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenNETCF.Integration.Test
{
  [TestClass()]
  public class FileSystemWatcherTest : TestBase
  {
    AutoResetEvent m_testEvent;

    [TestMethod()]
    [Description("Ensures that a FSW raised the Created event for file creation in the root watched folder")]
    public void FSWFileCreatedInWatchDirTestPositive()
    {
      FileSystemWatcher fsw = null;

      string folder = "\\Temp";
      string filename = Path.Combine(folder, "Test.txt");
      if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      if (File.Exists(filename)) File.Delete(filename);
      m_testEvent = new AutoResetEvent(false);

      try
      {
        fsw = new FileSystemWatcher(folder, "*.*");
        fsw.Created += new FileSystemEventHandler(
          delegate(object sender, OpenNETCF.IO.FileSystemEventArgs e)
          {
            m_testEvent.Set();
          });
        fsw.EnableRaisingEvents = true;

        File.CreateText(filename).Close();
        // give time for the message to get queued, and flush the queue a few times
        for (int i = 0; i < 10; i++)
        {
          Thread.Sleep(20);
          System.Windows.Forms.Application.DoEvents();
        }

        // check for an event
        Assert.IsTrue(m_testEvent.WaitOne(1000, false), "Create Event did not fire");
      }
      finally
      {
        // clean up
        File.Delete(filename);
        if( fsw != null) fsw.Dispose();
        m_testEvent.Close();
      }
    }

    [TestMethod]
    [Description("Ensures that a FSW raised the Created event for folder creation")]
    public void FSWFolderCreatedInWatchDirTestPositive()
    {
      FileSystemWatcher fsw = null;

      string folder = "\\Temp\\SubFolder";

      if (Directory.Exists(folder)) Directory.Delete(folder, true);

      try
      {
        m_testEvent = new AutoResetEvent(false); 
        fsw = new FileSystemWatcher("\\Temp", "*.*");
        fsw.Created += new FileSystemEventHandler(
          delegate(object sender, OpenNETCF.IO.FileSystemEventArgs e)
          {
            m_testEvent.Set();
          });
        fsw.EnableRaisingEvents = true;

        Directory.CreateDirectory(folder);

        // give time for the message to get queued, and flush the queue a few times
        for (int i = 0; i < 10; i++)
        {
          Thread.Sleep(20);
          System.Windows.Forms.Application.DoEvents();
        }

        // check for an event
        Assert.IsTrue(m_testEvent.WaitOne(1000, false), "Create Event did not fire");
      }
      finally
      {
        // clean up
        if (fsw != null) fsw.Dispose();
        m_testEvent.Close();
      }
    }

    [Ignore]
    // For some unknown reason, this test causes the entire test run to abort.
    [TestMethod()]
    [Description("Ensures that a FSW raised the Created event for file creation in a sub folder of the watched folder")]
    public void FSWFileCreatedInWatchSubDirTestPositive()
    {
      FileSystemWatcher fsw = null;
      string folder = "\\Temp";
      string subfolder = Path.Combine(folder, "Subfolder");
      string filename = Path.Combine(subfolder, "Test2.txt");

      if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      if (!Directory.Exists(subfolder)) Directory.CreateDirectory(subfolder);
      if (File.Exists(filename)) File.Delete(filename);

      try
      {
        AutoResetEvent testEvent = new AutoResetEvent(false);
        fsw = new FileSystemWatcher(folder, "*.*");
        fsw.Created += new FileSystemEventHandler(
          delegate(object sender, OpenNETCF.IO.FileSystemEventArgs e)
          {
            testEvent.Set();
          });
        fsw.IncludeSubdirectories = true;
        fsw.EnableRaisingEvents = true;

        File.CreateText(filename).Close();

        int i = 0;
        while (!testEvent.WaitOne(20, false))
        {
          System.Windows.Forms.Application.DoEvents();
          if (i++ > 10) Assert.Fail("Create Event did not fire");
        }
      }
      finally
      {
        // clean up
        File.Delete(filename);
        if (fsw != null) fsw.Dispose();
      }
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that a FSW does NOT raise the Created event for file creation in a sub folder of the watched folder when IncludeSubdirectories == false")]
    public void FSWFileCreatedInWatchSubDirTestNegative()
    {
      FileSystemWatcher fsw = null;

      string folder = "\\Temp";
      string subfolder = Path.Combine(folder, "Subfolder");
      string filename = Path.Combine(subfolder, "Test.txt");

      if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      if (!Directory.Exists(subfolder)) Directory.CreateDirectory(subfolder);
      if (File.Exists(filename)) File.Delete(filename);

      try
      {
        AutoResetEvent testEvent = new AutoResetEvent(false);
        fsw = new FileSystemWatcher(folder, "*.*");
        fsw.Created += new FileSystemEventHandler(
          delegate (object sender, OpenNETCF.IO.FileSystemEventArgs e)
          {
            testEvent.Set();
          });
        // do not watch subfolders
        fsw.IncludeSubdirectories = false;
        fsw.EnableRaisingEvents = true;

        File.CreateText(filename).Close();
        // give time for the message to get queued, and flush the queue a few times
        for (int i = 0; i < 10; i++)
        {
          Thread.Sleep(20);
          System.Windows.Forms.Application.DoEvents();
        }

        // check for an event
        Assert.IsFalse(testEvent.WaitOne(100, false), "Created Event fired when it should not");
      }
      finally
      {
        // clean up
        File.Delete(filename);
        if (fsw != null) fsw.Dispose();
      }
    }

    [TestMethod()]
    [Description("Ensures that a FSW raises the Deleted event for file deletion in the root watched folder")]
    public void FSWFileDeletedInWatchDirTestPositive()
    {
      FileSystemWatcher fsw = null;

      string folder = "\\Temp";
      string filename = Path.Combine(folder, "Test.txt");
      if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      if (!File.Exists(filename)) File.CreateText(filename).Close();

      try
      {
        m_testEvent = new AutoResetEvent(false);
        fsw = new FileSystemWatcher(folder, "*.*");
        fsw.Deleted += new FileSystemEventHandler(
          delegate(object sender, OpenNETCF.IO.FileSystemEventArgs e)
          {
            m_testEvent.Set();
          });
        fsw.EnableRaisingEvents = true;

        File.Delete(filename);
        // give time for the message to get queued, and flush the queue a few times
        for (int i = 0; i < 10; i++)
        {
          Thread.Sleep(20);
          System.Windows.Forms.Application.DoEvents();
        }

        // check for an event
        Assert.IsTrue(m_testEvent.WaitOne(1000, false), "Deleted Event did not fire");
      }
      finally
      {
        // clean up
        File.Delete(filename);
        if (fsw != null) fsw.Dispose();
        m_testEvent.Close();
      }
    }

    private string testDir = "\\test";
    private List<string> filesCreated = new List<string>();
    private string[] filesToCreate = new string[]
        {
          "\\test\\testFileA.txt",
          "\\test\\testFileB.txt",
          "\\test\\testFileC.txt",
          "\\test\\testFileD.txt"
        };
    private ManualResetEvent m_beginWriteEvent;

    [TestMethod()]
    [Description("Ensures that a FSW watching a folder raises unique events for 2 simultaneously created files in that folder")]
    public void FSWSimultaneousCreationTest()
    {
      m_beginWriteEvent = new ManualResetEvent(false);

      // test setup
      filesCreated.Clear();
      if (!Directory.Exists(testDir))
      {
        Directory.CreateDirectory(testDir);
      }
      foreach (string fn in filesToCreate)
      {
        if (File.Exists(fn)) File.Delete(fn);
      }

      FileSystemWatcher fsw = new OpenNETCF.IO.FileSystemWatcher(testDir, "*.*");
      fsw.IncludeSubdirectories = false;
      fsw.NotifyFilter = OpenNETCF.IO.NotifyFilters.CreationTime | OpenNETCF.IO.NotifyFilters.DirectoryName | OpenNETCF.IO.NotifyFilters.FileName | OpenNETCF.IO.NotifyFilters.LastAccess;
      fsw.EnableRaisingEvents = true;
      fsw.Created += new OpenNETCF.IO.FileSystemEventHandler( delegate (object sender, OpenNETCF.IO.FileSystemEventArgs e)
        {
          filesCreated.Add(e.FullPath);
        });

      // start up two threads
      foreach (string fn in filesToCreate)
      {
        ThreadPool.QueueUserWorkItem(CreateFileThreadProc, fn);
      }

      // make the write at the same time
      m_beginWriteEvent.Set();

      // since we're holding things up, let the system dispatch messages
      for (int i = 0; i < 10; i++)
      {
        Application.DoEvents();
        Thread.Sleep(10);
      }

      Assert.AreEqual(filesToCreate.Length, filesCreated.Count, "Incorrect number of files created");

      foreach (string fn in filesCreated)
      {
        Assert.IsTrue(filesCreated.Contains(fn), "Missing file " + fn);
      }

      // reset state and clean up
      m_beginWriteEvent.Close();
      fsw.Dispose();
    }

    void CreateFileThreadProc(object o)
    {
      string fileName = o as string;

      if (!m_beginWriteEvent.WaitOne(10000, false)) return;

      // create and close the file to fire off an FSW event
      File.CreateText(fileName).Close();
    }
  }
}
