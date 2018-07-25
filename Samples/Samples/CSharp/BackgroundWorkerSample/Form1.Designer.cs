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



namespace BackgroundWorkerSample
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MainMenu mainMenu1;

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
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.panel1 = new System.Windows.Forms.Panel();
      this.progress = new System.Windows.Forms.ProgressBar();
      this.progressLabel = new System.Windows.Forms.Label();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.start = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.tabControl1);
      this.panel1.Location = new System.Drawing.Point(0, 32);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(240, 207);
      // 
      // progress
      // 
      this.progress.Location = new System.Drawing.Point(3, 245);
      this.progress.Name = "progress";
      this.progress.Size = new System.Drawing.Size(186, 20);
      // 
      // progressLabel
      // 
      this.progressLabel.Location = new System.Drawing.Point(195, 246);
      this.progressLabel.Name = "progressLabel";
      this.progressLabel.Size = new System.Drawing.Size(45, 20);
      this.progressLabel.Text = "100%";
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(240, 207);
      this.tabControl1.TabIndex = 0;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Location = new System.Drawing.Point(0, 0);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Size = new System.Drawing.Size(240, 184);
      this.tabPage1.Text = "tabPage1";
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.label1);
      this.tabPage2.Controls.Add(this.label3);
      this.tabPage2.Location = new System.Drawing.Point(0, 0);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Size = new System.Drawing.Size(240, 184);
      this.tabPage2.Text = "tabPage2";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(3, 96);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(234, 20);
      this.label2.Text = "This is Tab 1";
      this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(3, 96);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(234, 20);
      this.label3.Text = "This is Tab 2";
      this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // start
      // 
      this.start.Location = new System.Drawing.Point(4, 4);
      this.start.Name = "start";
      this.start.Size = new System.Drawing.Size(233, 22);
      this.start.TabIndex = 3;
      this.start.Text = "Start Background Worker";
      this.start.Click += new System.EventHandler(this.start_Click);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(234, 64);
      this.label1.Text = "Note that you can change tabs while the BackgroundWorker is working and updating " +
          "the UI";
      this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(3, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(234, 64);
      this.label4.Text = "Note that you can change tabs while the BackgroundWorker is working and updating " +
          "the UI";
      this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.start);
      this.Controls.Add(this.progressLabel);
      this.Controls.Add(this.progress);
      this.Controls.Add(this.panel1);
      this.Menu = this.mainMenu1;
      this.Name = "Form1";
      this.Text = "Form1";
      this.panel1.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ProgressBar progress;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Label progressLabel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button start;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label1;
  }
}

