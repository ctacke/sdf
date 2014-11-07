using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Media.WaveAudio;
using System.Diagnostics;
using System.IO;

namespace AudioRecorder
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    Recorder m_recorder;
    Player m_player;
    FileStream m_stream;

    private void record_Click(object sender, EventArgs e)
    {
      if (m_recorder == null)
      {
        m_recorder = new Recorder();
        m_recorder.DoneRecording += new WaveFinishedHandler(m_recorder_DoneRecording);
      }
      
      m_stream = new FileStream("\\Test.wav", FileMode.Create, FileAccess.ReadWrite);
      Debug.WriteLine("Begin Recording");
      m_recorder.RecordFor(m_stream, 5);
    }

    void m_recorder_DoneRecording()
    {
      Debug.WriteLine("Done Recording");
      m_stream.Close();
    }

    private void play_Click(object sender, EventArgs e)
    {
      if (m_player == null)
      {
        m_player = new Player();
        m_player.DonePlaying += new WaveDoneHandler(player_DonePlaying);
        m_player.PositionChanged += new EventHandler(m_player_PositionChanged);
      }

      m_stream = new FileStream("\\Test.wav", FileMode.Open, FileAccess.ReadWrite);
      Debug.WriteLine("Playing");
      m_player.Play(m_stream);
    }

    void m_player_PositionChanged(object sender, EventArgs e)
    {
      Debug.WriteLine("Player position changed");
    }

    void player_DonePlaying(object sender, IntPtr wParam, IntPtr lParam)
    {
      Debug.WriteLine("Done Playing");
    }

    private void stop_Click(object sender, EventArgs e)
    {
      if ((m_player != null) && (m_player.Playing))
      {
        m_player.Stop();
      }
      else if ((m_recorder != null) && (m_recorder.Recording))
      {
        m_recorder.Stop();
      }
    }
  }
}