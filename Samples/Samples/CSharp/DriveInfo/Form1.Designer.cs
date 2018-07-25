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



namespace DriveInfo
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnGetInfo = new OpenNETCF.Windows.Forms.Button2();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RootDirectory = new OpenNETCF.Windows.Forms.TextBox2();
            this.AvailableFreeSpace = new OpenNETCF.Windows.Forms.TextBox2();
            this.TotalFreeSpace = new OpenNETCF.Windows.Forms.TextBox2();
            this.TotalSize = new OpenNETCF.Windows.Forms.TextBox2();
            this.SuspendLayout();
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGetInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnGetInfo.Image")));
            this.btnGetInfo.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnGetInfo.ImageIndex = -1;
            this.btnGetInfo.ImageList = null;
            this.btnGetInfo.Location = new System.Drawing.Point(159, 271);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(78, 20);
            this.btnGetInfo.TabIndex = 0;
            this.btnGetInfo.Text = "Get Info";
            this.btnGetInfo.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(234, 100);
            this.listBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.Text = "RootDirectory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 18);
            this.label2.Text = "AvailableFreeSpace";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 18);
            this.label3.Text = "TotalFreeSpace";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 226);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 18);
            this.label4.Text = "TotalSize";
            // 
            // RootDirectory
            // 
            this.RootDirectory.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.RootDirectory.Location = new System.Drawing.Point(4, 125);
            this.RootDirectory.Name = "RootDirectory";
            this.RootDirectory.ReadOnly = true;
            this.RootDirectory.Size = new System.Drawing.Size(233, 21);
            this.RootDirectory.TabIndex = 9;
            // 
            // AvailableFreeSpace
            // 
            this.AvailableFreeSpace.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.AvailableFreeSpace.Location = new System.Drawing.Point(4, 162);
            this.AvailableFreeSpace.Name = "AvailableFreeSpace";
            this.AvailableFreeSpace.ReadOnly = true;
            this.AvailableFreeSpace.Size = new System.Drawing.Size(233, 21);
            this.AvailableFreeSpace.TabIndex = 10;
            // 
            // TotalFreeSpace
            // 
            this.TotalFreeSpace.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.TotalFreeSpace.Location = new System.Drawing.Point(4, 202);
            this.TotalFreeSpace.Name = "TotalFreeSpace";
            this.TotalFreeSpace.ReadOnly = true;
            this.TotalFreeSpace.Size = new System.Drawing.Size(233, 21);
            this.TotalFreeSpace.TabIndex = 11;
            // 
            // TotalSize
            // 
            this.TotalSize.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.TotalSize.Location = new System.Drawing.Point(3, 244);
            this.TotalSize.Name = "TotalSize";
            this.TotalSize.ReadOnly = true;
            this.TotalSize.Size = new System.Drawing.Size(233, 21);
            this.TotalSize.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.TotalSize);
            this.Controls.Add(this.TotalFreeSpace);
            this.Controls.Add(this.AvailableFreeSpace);
            this.Controls.Add(this.RootDirectory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnGetInfo);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.Button2 btnGetInfo;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private OpenNETCF.Windows.Forms.TextBox2 RootDirectory;
        private OpenNETCF.Windows.Forms.TextBox2 AvailableFreeSpace;
        private OpenNETCF.Windows.Forms.TextBox2 TotalFreeSpace;
        private OpenNETCF.Windows.Forms.TextBox2 TotalSize;

    }
}

