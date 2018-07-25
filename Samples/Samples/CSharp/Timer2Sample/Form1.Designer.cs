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



namespace Timer2Sample
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
            this.eventBased = new System.Windows.Forms.RadioButton();
            this.callbackBased = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.interval = new System.Windows.Forms.TextBox();
            this.precision = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // eventBased
            // 
            this.eventBased.Checked = true;
            this.eventBased.Location = new System.Drawing.Point(6, 13);
            this.eventBased.Name = "eventBased";
            this.eventBased.Size = new System.Drawing.Size(100, 20);
            this.eventBased.TabIndex = 0;
            this.eventBased.Text = "Event";
            // 
            // callbackBased
            // 
            this.callbackBased.Location = new System.Drawing.Point(112, 13);
            this.callbackBased.Name = "callbackBased";
            this.callbackBased.Size = new System.Drawing.Size(100, 20);
            this.callbackBased.TabIndex = 1;
            this.callbackBased.TabStop = false;
            this.callbackBased.Text = "Callback";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.Text = "Interval";
            // 
            // interval
            // 
            this.interval.Location = new System.Drawing.Point(78, 47);
            this.interval.Name = "interval";
            this.interval.Size = new System.Drawing.Size(123, 21);
            this.interval.TabIndex = 3;
            this.interval.Text = "1000";
            // 
            // precision
            // 
            this.precision.Location = new System.Drawing.Point(78, 83);
            this.precision.Name = "precision";
            this.precision.Size = new System.Drawing.Size(123, 21);
            this.precision.TabIndex = 5;
            this.precision.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.Text = "Precision";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(60, 121);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(98, 31);
            this.start.TabIndex = 7;
            this.start.Text = "Start";
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(6, 180);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(231, 20);
            this.status.Text = "Ready";
            this.status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 300);
            this.Controls.Add(this.status);
            this.Controls.Add(this.start);
            this.Controls.Add(this.precision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.interval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.callbackBased);
            this.Controls.Add(this.eventBased);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Timer2 Sample";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton eventBased;
        private System.Windows.Forms.RadioButton callbackBased;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox interval;
        private System.Windows.Forms.TextBox precision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label status;

    }
}

