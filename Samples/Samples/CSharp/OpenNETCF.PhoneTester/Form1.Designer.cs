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



namespace OpenNETCF.PhoneTester
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
            this.textboxMessage = new OpenNETCF.Windows.Forms.TextBox2();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textboxNumber = new OpenNETCF.Windows.Forms.TextBox2();
            this.btnSend = new OpenNETCF.Windows.Forms.Button2();
            this.btnProperties = new OpenNETCF.Windows.Forms.Button2();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtdeviceNumber = new OpenNETCF.Windows.Forms.TextBox2();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.MaximumPhonebookAddressLength = new OpenNETCF.Windows.Forms.TextBox2();
            this.label7 = new System.Windows.Forms.Label();
            this.MaximumPhonebookIndex = new OpenNETCF.Windows.Forms.TextBox2();
            this.label6 = new System.Windows.Forms.Label();
            this.MaximumPhonebookTextLength = new OpenNETCF.Windows.Forms.TextBox2();
            this.label5 = new System.Windows.Forms.Label();
            this.MinimumPhonebookIndex = new OpenNETCF.Windows.Forms.TextBox2();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lbCallLog = new System.Windows.Forms.ListBox();
            this.btnCallLog = new OpenNETCF.Windows.Forms.Button2();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chkPrompt = new OpenNETCF.Windows.Forms.CheckBox2();
            this.txtCalledParty = new OpenNETCF.Windows.Forms.TextBox2();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCall = new OpenNETCF.Windows.Forms.Button2();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textboxMessage
            // 
            this.textboxMessage.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.textboxMessage.Location = new System.Drawing.Point(3, 26);
            this.textboxMessage.MaxLength = 100;
            this.textboxMessage.Multiline = true;
            this.textboxMessage.Name = "textboxMessage";
            this.textboxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textboxMessage.Size = new System.Drawing.Size(234, 48);
            this.textboxMessage.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 19);
            this.label1.Text = "Message";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.Text = "Phone Number";
            // 
            // textboxNumber
            // 
            this.textboxNumber.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.textboxNumber.Location = new System.Drawing.Point(4, 91);
            this.textboxNumber.Name = "textboxNumber";
            this.textboxNumber.Size = new System.Drawing.Size(233, 21);
            this.textboxNumber.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnSend.ImageIndex = -1;
            this.btnSend.ImageList = null;
            this.btnSend.Location = new System.Drawing.Point(167, 247);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(62, 20);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProperties.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnProperties.Image = ((System.Drawing.Image)(resources.GetObject("btnProperties.Image")));
            this.btnProperties.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnProperties.ImageIndex = -1;
            this.btnProperties.ImageList = null;
            this.btnProperties.Location = new System.Drawing.Point(113, 246);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(120, 22);
            this.btnProperties.TabIndex = 5;
            this.btnProperties.Text = "Get Properties";
            this.btnProperties.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(3, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(233, 72);
            this.listBox1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(240, 294);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtdeviceNumber);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textboxMessage);
            this.tabPage1.Controls.Add(this.btnSend);
            this.tabPage1.Controls.Add(this.textboxNumber);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 271);
            this.tabPage1.Text = "SMS";
            // 
            // txtdeviceNumber
            // 
            this.txtdeviceNumber.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.txtdeviceNumber.Location = new System.Drawing.Point(4, 132);
            this.txtdeviceNumber.Name = "txtdeviceNumber";
            this.txtdeviceNumber.ReadOnly = true;
            this.txtdeviceNumber.Size = new System.Drawing.Size(233, 21);
            this.txtdeviceNumber.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.Text = "Device Number:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.MaximumPhonebookAddressLength);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.MaximumPhonebookIndex);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.MaximumPhonebookTextLength);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.MinimumPhonebookIndex);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.btnProperties);
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(240, 271);
            this.tabPage2.Text = "SIM";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(7, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(200, 12);
            this.label8.Text = "MaximumPhonebookAddressLength";
            // 
            // MaximumPhonebookAddressLength
            // 
            this.MaximumPhonebookAddressLength.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.MaximumPhonebookAddressLength.Location = new System.Drawing.Point(7, 202);
            this.MaximumPhonebookAddressLength.Name = "MaximumPhonebookAddressLength";
            this.MaximumPhonebookAddressLength.ReadOnly = true;
            this.MaximumPhonebookAddressLength.Size = new System.Drawing.Size(100, 21);
            this.MaximumPhonebookAddressLength.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 12);
            this.label7.Text = "MaximumPhonebookIndex";
            // 
            // MaximumPhonebookIndex
            // 
            this.MaximumPhonebookIndex.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.MaximumPhonebookIndex.Location = new System.Drawing.Point(7, 160);
            this.MaximumPhonebookIndex.Name = "MaximumPhonebookIndex";
            this.MaximumPhonebookIndex.ReadOnly = true;
            this.MaximumPhonebookIndex.Size = new System.Drawing.Size(100, 21);
            this.MaximumPhonebookIndex.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 12);
            this.label6.Text = "MaximumPhonebookTextLength";
            // 
            // MaximumPhonebookTextLength
            // 
            this.MaximumPhonebookTextLength.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.MaximumPhonebookTextLength.Location = new System.Drawing.Point(7, 243);
            this.MaximumPhonebookTextLength.Name = "MaximumPhonebookTextLength";
            this.MaximumPhonebookTextLength.ReadOnly = true;
            this.MaximumPhonebookTextLength.Size = new System.Drawing.Size(100, 21);
            this.MaximumPhonebookTextLength.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 13);
            this.label5.Text = "MinimumPhonebookIndex";
            // 
            // MinimumPhonebookIndex
            // 
            this.MinimumPhonebookIndex.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.MinimumPhonebookIndex.Location = new System.Drawing.Point(7, 119);
            this.MinimumPhonebookIndex.Name = "MinimumPhonebookIndex";
            this.MinimumPhonebookIndex.ReadOnly = true;
            this.MinimumPhonebookIndex.Size = new System.Drawing.Size(100, 21);
            this.MinimumPhonebookIndex.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.Text = "Sim Numbers";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lbCallLog);
            this.tabPage3.Controls.Add(this.btnCallLog);
            this.tabPage3.Location = new System.Drawing.Point(0, 0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(240, 271);
            this.tabPage3.Text = "Call Log";
            // 
            // lbCallLog
            // 
            this.lbCallLog.Location = new System.Drawing.Point(3, 3);
            this.lbCallLog.Name = "lbCallLog";
            this.lbCallLog.Size = new System.Drawing.Size(230, 240);
            this.lbCallLog.TabIndex = 7;
            // 
            // btnCallLog
            // 
            this.btnCallLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCallLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCallLog.Image = ((System.Drawing.Image)(resources.GetObject("btnCallLog.Image")));
            this.btnCallLog.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnCallLog.ImageIndex = -1;
            this.btnCallLog.ImageList = null;
            this.btnCallLog.Location = new System.Drawing.Point(134, 246);
            this.btnCallLog.Name = "btnCallLog";
            this.btnCallLog.Size = new System.Drawing.Size(99, 22);
            this.btnCallLog.TabIndex = 6;
            this.btnCallLog.Text = "Get Call Log";
            this.btnCallLog.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnCallLog.Click += new System.EventHandler(this.btnCallLog_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chkPrompt);
            this.tabPage4.Controls.Add(this.txtCalledParty);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.btnCall);
            this.tabPage4.Location = new System.Drawing.Point(0, 0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(240, 271);
            this.tabPage4.Text = "Phone Call";
            // 
            // chkPrompt
            // 
            this.chkPrompt.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkPrompt.CheckBoxColor = System.Drawing.SystemColors.ControlText;
            this.chkPrompt.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.chkPrompt.Location = new System.Drawing.Point(6, 50);
            this.chkPrompt.Name = "chkPrompt";
            this.chkPrompt.Size = new System.Drawing.Size(197, 20);
            this.chkPrompt.TabIndex = 6;
            this.chkPrompt.Text = "Prompt";
            // 
            // txtCalledParty
            // 
            this.txtCalledParty.CharacterCasing = OpenNETCF.Windows.Forms.CharacterCasing.Normal;
            this.txtCalledParty.Location = new System.Drawing.Point(6, 23);
            this.txtCalledParty.Name = "txtCalledParty";
            this.txtCalledParty.Size = new System.Drawing.Size(197, 21);
            this.txtCalledParty.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(7, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.Text = "Called Party";
            // 
            // btnCall
            // 
            this.btnCall.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCall.Image = ((System.Drawing.Image)(resources.GetObject("btnCall.Image")));
            this.btnCall.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleRight;
            this.btnCall.ImageIndex = -1;
            this.btnCall.ImageList = null;
            this.btnCall.Location = new System.Drawing.Point(162, 239);
            this.btnCall.Name = "btnCall";
            this.btnCall.Size = new System.Drawing.Size(54, 20);
            this.btnCall.TabIndex = 2;
            this.btnCall.Text = "Call";
            this.btnCall.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleLeft;
            this.btnCall.Click += new System.EventHandler(this.btnCal_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.TextBox2 textboxMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private OpenNETCF.Windows.Forms.TextBox2 textboxNumber;
        private OpenNETCF.Windows.Forms.Button2 btnSend;
        private OpenNETCF.Windows.Forms.Button2 btnProperties;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private OpenNETCF.Windows.Forms.TextBox2 txtdeviceNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private OpenNETCF.Windows.Forms.TextBox2 MaximumPhonebookAddressLength;
        private System.Windows.Forms.Label label7;
        private OpenNETCF.Windows.Forms.TextBox2 MaximumPhonebookIndex;
        private System.Windows.Forms.Label label6;
        private OpenNETCF.Windows.Forms.TextBox2 MaximumPhonebookTextLength;
        private System.Windows.Forms.Label label5;
        private OpenNETCF.Windows.Forms.TextBox2 MinimumPhonebookIndex;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private OpenNETCF.Windows.Forms.Button2 btnCall;
        private OpenNETCF.Windows.Forms.TextBox2 txtCalledParty;
        private System.Windows.Forms.Label label10;
        private OpenNETCF.Windows.Forms.CheckBox2 chkPrompt;
        private System.Windows.Forms.ListBox lbCallLog;
        private OpenNETCF.Windows.Forms.Button2 btnCallLog;
    }
}

