//----------------------------------------------------------------------------
//  This file is part of the OpenNETCF Smart Device Framework Code Samples.
// 
//  Copyright (C) OpenNETCF Consulting, LLC.  All rights reserved.
// 
//  This source code is intended only as a supplement to Smart Device 
//  Framework and/or on-line documentation.  
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using OpenNETCF.Media.WaveAudio;
using OpenNETCF.Windows.Forms;

namespace MobileVoiceNotes
{
    public partial class Main : Form
    {
        private OpenNETCF.Media.WaveAudio.Recorder recorder;
        private OpenNETCF.Media.WaveAudio.Player player;
        private FileStream recordingStream;

        private string tempVoicePath = "VoiceNote.wav";

        public Main()
        {
            using (new Cursor2())
            {
                InitializeComponent();

                //Load the data
                this.LoadData();

                //Bind the data
                this.listBox21.DrawItem += new OpenNETCF.Windows.Forms.DrawItemEventHandler(listBox21_DrawItem);
                this.UpdateList();
                this.listBox21.SelectedIndexChanged += new EventHandler(listBox21_SelectedIndexChanged);

                //Create the recorder object
                this.recorder = new OpenNETCF.Media.WaveAudio.Recorder();

                //Create the player
                this.player = new OpenNETCF.Media.WaveAudio.Player();

                //Set the temporary path for the voice note
                this.tempVoicePath = Path.Combine(Path.GetTempPath(), tempVoicePath);
            }
        }

        void listBox21_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox21.SelectedIndex < 0 || this.listBox21.SelectedIndex > this.listBox21.Items.Count)
                this.EnablePlayControls(PlayState.None);
            else
                this.EnablePlayControls(PlayState.EnablePlay);
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            this.Record();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.EnablePlayControls(PlayState.StartPlay);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            this.EnablePlayControls(PlayState.PausePlay);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.EnablePlayControls(PlayState.StopPlay);
        }

        /// <summary>
        /// Owner draw the list items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox21_DrawItem(object sender, OpenNETCF.Windows.Forms.DrawItemEventArgs e)
        {

            ////Draw the background 
            //e.DrawBackground();
            ////Draw the focus rectangle
            //e.DrawFocusRectangle();

            e.Graphics.Clip = new Region(e.Bounds);
            //Draw the data
            if (e.Index <= this.listBox21.Items.Count - 1)
            {
                VoiceNote.VoiceNoteRow row = ((VoiceListItem)this.listBox21.Items[e.Index]).VoiceRow;
                if (row != null)
                {
                    using (Brush b = new SolidBrush(e.State == DrawItemState.Selected ? SystemColors.ActiveCaptionText : e.ForeColor))
                    {
                        //get the length of the data
                        string length = this.GetVoiceNoteLength(row.Data);
                        SizeF size = e.Graphics.MeasureString(length, e.Font);
                        int y = (e.Bounds.Height / 2 - (int)size.Height / 2) + e.Bounds.Top;

                        //Draw the date
                        e.Graphics.DrawString(this.GetDate(row.Date), e.Font, b, 2, y);

                        //Draw the length
                        e.Graphics.DrawString(length, e.Font, b, e.Bounds.Right - (int)size.Width - 2, y);
                    }

                }
            }
            e.Graphics.ResetClip();
        }

        /// <summary>
        /// Enable the controls depending on the state
        /// </summary>
        /// <param name="state"></param>
        private void EnablePlayControls(PlayState state)
        {
            using (new Cursor2())
            {
                this.listBox21.Enabled = this.btnRecord.Enabled = this.btnPause.Enabled = this.btnStop.Enabled = this.btnPlay.Enabled = false;

                switch (state)
                {
                    case PlayState.EnablePlay:
                        this.listBox21.Enabled = this.btnRecord.Enabled = this.btnPlay.Enabled = true;
                        break;
                    case PlayState.StartPlay:
                        this.btnPause.Enabled = this.btnStop.Enabled = true;
                        this.PlayVoiceNote();
                        break;
                    case PlayState.PausePlay:
                        this.btnStop.Enabled = this.btnPlay.Enabled = true;
                        this.player.Pause();
                        break;
                    case PlayState.StopPlay:
                        this.listBox21.Enabled = this.btnRecord.Enabled = this.btnPlay.Enabled = true;
                        this.player.Stop();
                        break;
                    case PlayState.None:
                        this.listBox21.Enabled = this.btnRecord.Enabled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Playes a voicenote
        /// </summary>
        private void PlayVoiceNote()
        {
            if (this.player.Playing)
            {
                this.player.Restart();
            }
            else
            {
                //Get the voice row
                VoiceNote.VoiceNoteRow row = ((VoiceListItem)this.listBox21.Items[this.listBox21.SelectedIndex]).VoiceRow;

                //Get the voice data stream
                FileStream fs = new FileStream(this.tempVoicePath, FileMode.Create, FileAccess.ReadWrite);
                fs.Write(row.Data, 0, row.Data.Length);
                fs.Close();
                Stream s = File.Open(this.tempVoicePath, FileMode.Open, FileAccess.Read);

                //Wire up to the events
                this.player.DonePlaying += new WaveDoneHandler(player_DonePlaying);
                this.player.Play((Stream)s);
            }
        }

        /// <summary>
        /// Event handler when the playing is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        void player_DonePlaying(object sender, IntPtr wParam, IntPtr lParam)
        {
            this.EnablePlayControls(PlayState.StopPlay);
        }

        /// <summary>
        /// Starts a new recording
        /// </summary>
        private void Record()
        {
            using (new Cursor2())
            {
                if (recorder.Recording)
                {
                    //Stop the recording
                    this.recorder.Stop();
                }
                else
                {
                    //Create a new stream for the recording
                    this.recordingStream = new FileStream(this.tempVoicePath, FileMode.Create, FileAccess.ReadWrite);

                    //start a new recording
                    recorder.DoneRecording += new WaveFinishedHandler(rec_DoneRecording);
                    this.btnRecord.Text = "Stop";
                    recorder.RecordFor(this.recordingStream, 20);
                }
            }
        }

        /// <summary>
        /// Event handler when recording has stopped
        /// </summary>
        private void rec_DoneRecording()
        {
            using (new Cursor2())
            {
                //Unregister from the doneRecording event
                recorder.DoneRecording -= new WaveFinishedHandler(rec_DoneRecording);
                this.btnRecord.Text = "Record";

                //Make sure the temp file exists
                if (File.Exists(this.tempVoicePath))
                {
                    //Save the data to the database
                    VoiceNote.VoiceNoteRow row = this.voiceNote1._VoiceNote.NewVoiceNoteRow();
                    row.Date = DateTime.Now;

                    //Get the memory stream buffer to save the voice note
                    this.recordingStream = new FileStream(this.tempVoicePath, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[this.recordingStream.Length];
                    this.recordingStream.Read(buffer, 0, (int)this.recordingStream.Length);
                    this.recordingStream.Close();
                    this.recordingStream = null;
                    row.Data = buffer;

                    //Create a new key
                    row.Key = Guid.NewGuid();

                    //Save to the table
                    this.voiceNote1._VoiceNote.AddVoiceNoteRow(row);
                    this.voiceNoteTableAdapter1.Update(row);
                    this.UpdateList();
                }
            }
        }

        /// <summary>
        /// Loads the SqlMobile data
        /// </summary>
        private void LoadData()
        {
            this.voiceNoteTableAdapter1.Fill(this.voiceNote1._VoiceNote);
        }

        /// <summary>
        /// Updates the Listview
        /// </summary>
        private void UpdateList()
        {
            this.listBox21.BeginUpdate();
            this.listBox21.Items.Clear();
            VoiceListItem item;
            foreach (VoiceNote.VoiceNoteRow row in this.voiceNote1._VoiceNote.Rows)
            {
                item = new VoiceListItem(row);
                this.listBox21.Items.Add(new VoiceListItem(row));
            }
            this.listBox21.EndUpdate();
        }

        /// <summary>
        /// Gets the length of the voice note
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetVoiceNoteLength(byte[] data)
        {
            int seconds = 0;
            int milliseconds = 0;
            if (data.Length > 0)
            {
                //The fmt descrption of the wave data starts at byte 20
                byte[] buffer = new byte[data.Length - 20];
                for (int x = 20; x < data.Length; x++)
                    buffer[x - 20] = data[x];
                //Put a try/catch incase the data is corrupt
                try
                {
                    WaveFormat2 wave = new WaveFormat2(buffer);

                    //Calculate the length
                    seconds = buffer.Length / wave.AvgBytesPerSec;
                    milliseconds = buffer.Length % wave.AvgBytesPerSec;

                }
                catch { }
            }

            //Format to something nice
            int minute = (seconds / 60);
            int hour = minute / 60;

            return string.Format("{0}:{1}:{2}.{3}", hour.ToString("00"), minute.ToString("00"), seconds.ToString("00"), milliseconds.ToString("000"));
        }

        /// <summary>
        /// Gets a friendly date to display
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDate(DateTime date)
        {
            if (date.Date == DateTime.Now.Date)
                return "Today";
            if (date.Date == DateTime.Now.AddDays(-1).Date)
                return "Yesterday";
            if (date.Date == DateTime.Now.AddDays(-2).Date)
                return "2 days ago";
            else
                return date.ToShortDateString();
        }

        /// <summary>
        /// Voice List item that contains the voice row for later retreval
        /// </summary>
        private class VoiceListItem : ListItem
        {
            public VoiceNote.VoiceNoteRow VoiceRow = null;

            public VoiceListItem(VoiceNote.VoiceNoteRow row)
            {
                this.VoiceRow = row;
            }
        }

        /// <summary>
        /// Enum to specify the state of play
        /// </summary>
        private enum PlayState
        {
            EnablePlay,
            StartPlay,
            PausePlay,
            StopPlay,
            None
        }

    }
}