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



namespace KeyRecorder
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
      this.keys = new System.Windows.Forms.TextBox();
      this.clear = new System.Windows.Forms.Button();
      this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel();
      this.hideShow = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // keys
      // 
      this.keys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.keys.ForeColor = System.Drawing.SystemColors.MenuText;
      this.keys.Location = new System.Drawing.Point(4, 4);
      this.keys.Name = "keys";
      this.keys.Size = new System.Drawing.Size(208, 21);
      this.keys.TabIndex = 0;
      // 
      // clear
      // 
      this.clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.clear.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
      this.clear.Location = new System.Drawing.Point(216, 14);
      this.clear.Name = "clear";
      this.clear.Size = new System.Drawing.Size(21, 10);
      this.clear.TabIndex = 1;
      this.clear.Text = "CLR";
      // 
      // hideShow
      // 
      this.hideShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.hideShow.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
      this.hideShow.Location = new System.Drawing.Point(216, 4);
      this.hideShow.Name = "hideShow";
      this.hideShow.Size = new System.Drawing.Size(21, 10);
      this.hideShow.TabIndex = 2;
      this.hideShow.Text = "_";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.hideShow);
      this.Controls.Add(this.clear);
      this.Controls.Add(this.keys);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.TopMost = true;
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox keys;
    private System.Windows.Forms.Button clear;
    private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
    private System.Windows.Forms.Button hideShow;
  }
}

