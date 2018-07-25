#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



namespace MobileVoiceNotes
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.listBox21 = new OpenNETCF.Windows.Forms.ListBox2();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.btnPlay = new OpenNETCF.Windows.Forms.Button2();
            this.btnRecord = new OpenNETCF.Windows.Forms.Button2();
            this.btnPause = new OpenNETCF.Windows.Forms.Button2();
            this.btnStop = new OpenNETCF.Windows.Forms.Button2();
            this.voiceNote1 = new MobileVoiceNotes.VoiceNote();
            this.voiceNoteTableAdapter1 = new MobileVoiceNotes.VoiceNoteTableAdapters.VoiceNoteTableAdapter();
            this.SuspendLayout();
            // 
            // listBox21
            // 
            this.listBox21.BackColor = System.Drawing.SystemColors.Window;
            this.listBox21.DataSource = null;
            this.listBox21.DisplayMember = null;
            this.listBox21.EvenItemColor = System.Drawing.SystemColors.Control;
            this.listBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
            this.listBox21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listBox21.ImageList = null;
            this.listBox21.ItemHeight = 40;
            this.listBox21.LineColor = System.Drawing.SystemColors.ControlText;
            this.listBox21.Location = new System.Drawing.Point(0, 0);
            this.listBox21.Name = "listBox21";
            this.listBox21.SelectedIndex = -1;
            this.listBox21.ShowLines = true;
            this.listBox21.ShowScrollbar = true;
            this.listBox21.Size = new System.Drawing.Size(240, 265);
            this.listBox21.TabIndex = 0;
            this.listBox21.Text = "listBox21";
            this.listBox21.TopIndex = 0;
            this.listBox21.WrapText = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnPlay.ImageIndex = -1;
            this.btnPlay.ImageList = null;
            this.btnPlay.Location = new System.Drawing.Point(3, 271);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(20, 20);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnRecord.Image")));
            this.btnRecord.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnRecord.ImageIndex = -1;
            this.btnRecord.ImageList = null;
            this.btnRecord.Location = new System.Drawing.Point(170, 271);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(67, 20);
            this.btnRecord.TabIndex = 2;
            this.btnRecord.Text = "Record";
            this.btnRecord.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnPause.ImageIndex = -1;
            this.btnPause.ImageList = null;
            this.btnPause.Location = new System.Drawing.Point(29, 271);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(20, 20);
            this.btnPause.TabIndex = 4;
            this.btnPause.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnStop.ImageIndex = -1;
            this.btnStop.ImageList = null;
            this.btnStop.Location = new System.Drawing.Point(55, 271);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(20, 20);
            this.btnStop.TabIndex = 5;
            this.btnStop.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // voiceNote1
            // 
            this.voiceNote1.DataSetName = "VoiceNote";
            this.voiceNote1.Prefix = "";
            this.voiceNote1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // voiceNoteTableAdapter1
            // 
            this.voiceNoteTableAdapter1.ClearBeforeFill = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.listBox21);
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Mobile Voice Notes";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.ListBox2 listBox21;
        private System.Windows.Forms.ImageList imageList1;
        private OpenNETCF.Windows.Forms.Button2 btnPlay;
        private OpenNETCF.Windows.Forms.Button2 btnRecord;
        private OpenNETCF.Windows.Forms.Button2 btnPause;
        private OpenNETCF.Windows.Forms.Button2 btnStop;
        private VoiceNote voiceNote1;
        private MobileVoiceNotes.VoiceNoteTableAdapters.VoiceNoteTableAdapter voiceNoteTableAdapter1;

    }
}

