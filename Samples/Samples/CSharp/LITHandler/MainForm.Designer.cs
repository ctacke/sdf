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



namespace LITHandler
{
    partial class MainForm
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
            this.started = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.lastTick = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.firstTick = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tickInterval = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // started
            // 
            this.started.Location = new System.Drawing.Point(3, 151);
            this.started.Name = "started";
            this.started.Size = new System.Drawing.Size(234, 20);
            this.started.Text = "<not started>";
            this.started.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.Label3.Location = new System.Drawing.Point(3, 131);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(234, 20);
            this.Label3.Text = "Test Started At";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lastTick
            // 
            this.lastTick.Location = new System.Drawing.Point(3, 196);
            this.lastTick.Name = "lastTick";
            this.lastTick.Size = new System.Drawing.Size(234, 20);
            this.lastTick.Text = "<not fired>";
            this.lastTick.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.Label1.Location = new System.Drawing.Point(3, 176);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(234, 20);
            this.Label1.Text = "Last Tick Was At";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(47, 88);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(141, 31);
            this.start.TabIndex = 4;
            this.start.Text = "Start";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(37, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.Text = "First tick in";
            // 
            // firstTick
            // 
            this.firstTick.Location = new System.Drawing.Point(114, 12);
            this.firstTick.Name = "firstTick";
            this.firstTick.Size = new System.Drawing.Size(33, 21);
            this.firstTick.TabIndex = 10;
            this.firstTick.Text = "20";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(153, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.Text = "seconds";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(153, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.Text = "seconds";
            // 
            // tickInterval
            // 
            this.tickInterval.Location = new System.Drawing.Point(114, 39);
            this.tickInterval.Name = "tickInterval";
            this.tickInterval.Size = new System.Drawing.Size(33, 21);
            this.tickInterval.TabIndex = 15;
            this.tickInterval.Text = "30";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(37, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.Text = "Then every";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tickInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.firstTick);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.started);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.lastTick);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.start);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.Text = "LITHandler";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label started;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label lastTick;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox firstTick;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tickInterval;
        private System.Windows.Forms.Label label6;
    }
}

