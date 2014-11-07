using OpenNETCF.Media.WaveAudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Threading;
using System.Runtime.InteropServices;
using System;

namespace OpenNETCF.Integration.Test
{
  [TestClass()]
  public class RecorderTest : TestBase
  {
    string wavPath;
    Recorder recorder;
    bool doneFired;
    bool streamClosed;

    private void DoEvents()
    {
      MSG msg;

      while (PeekMessage(out msg, IntPtr.Zero, 0, 0, 0))
      {
        if (GetMessage(out msg, IntPtr.Zero, 0, 0))
			  {
          TranslateMessage(out msg);
          DispatchMessage(ref msg);
				}
			}
    }

    internal struct MSG
    {
      public IntPtr hwnd;
      public int message;
      public IntPtr wParam;
      public IntPtr lParam;
      public int time;
      public int pt_x;
      public int pt_y;
    }

    [DllImport("coredll.dll", EntryPoint = "PeekMessage", SetLastError = true)]
    internal static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

    [DllImport("coredll.dll", EntryPoint = "GetMessageW", SetLastError = true)]
    internal static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("coredll.dll", EntryPoint = "TranslateMessage", SetLastError = true)]
    internal static extern bool TranslateMessage(out MSG lpMsg);

    [DllImport("coredll.dll", EntryPoint = "DispatchMessage", SetLastError = true)]
    internal static extern bool DispatchMessage(ref MSG lpMsg);

    [Ignore]
    [TestMethod()]
    public void RecordForTest()
    {     
      recorder = new Recorder();
      wavPath = Path.Combine(TestContext.TestDeploymentDir, "test.wav");
      using (FileStream recordingStream  = new FileStream(wavPath, FileMode.Create, FileAccess.ReadWrite))
      {
        //start a new recording
        recorder.DoneRecording += new WaveFinishedHandler(recorder_DoneRecording);
        doneFired = false;
        streamClosed = false;
        recorder.RecordFor(recordingStream, (short)(3));
      }

      for (int i = 0; i < 10; i++)
      {
        // simulate a message pump since the recorder uses windows messages for eventing
        Thread.Sleep(500);
        DoEvents();
      }

      Assert.IsTrue(doneFired, "DoneRecording never ran");
      Assert.IsTrue(streamClosed, "Target stream left open");
    }

    void recorder_DoneRecording()
    {
      doneFired = true;
      //Make sure the temp file exists
      if (File.Exists(this.wavPath))
      {
        if (recorder.Recording)
        {
          recorder.Stop();
        }

        //Get the memory stream buffer to save the voice note
        using (FileStream recordingStream = new FileStream(wavPath, FileMode.Open, FileAccess.Read))
        {
          streamClosed = true;
          byte[] buffer = new byte[recordingStream.Length];
          recordingStream.Read(buffer, 0, (int)recordingStream.Length);
          recordingStream.Close();
        }
      }
    }
  }
}
