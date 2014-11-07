using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Media.WaveAudio;
using System.IO;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        Player m_player;
        Stream m_stream = null;

        public Form1()
        {
            InitializeComponent();
            m_player = new Player();
            m_player.AutoCloseStreamAfterPlaying = false;
            m_player.DonePlaying += new WaveDoneHandler(m_player_DonePlaying);
        }

        void m_player_DonePlaying(object sender, IntPtr wParam, IntPtr lParam)
        {
            if (m_stream == null) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(
                    delegate
                    {
                        m_stream.Seek(0, SeekOrigin.Begin);
                        m_player.Play(m_stream);
                    }));
            }
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (m_player.Playing)
            {
                m_player.Stop();
                m_stream.Close();
                m_stream = null;
                play.Text = "Play";
                return;
            }

            if (!File.Exists(filePath.Text))
            {
                MessageBox.Show("Invalid file name");
                return;
            }

            play.Text = "Stop";
            m_stream = File.OpenRead(filePath.Text);
            m_player.PlaybackRate = (float)(rate.Value) / 100f;
            m_player.Play(m_stream);
        }

        private void rate_ValueChanged(object sender, EventArgs e)
        {
            m_player.PlaybackRate = (float)(rate.Value) / 100f;
        }
    }
}