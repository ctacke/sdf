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



namespace MSNSearchMobile
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.searchTermBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmbSearchTerm = new System.Windows.Forms.ComboBox();
            this.btnSearch = new OpenNETCF.Windows.Forms.Button2();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.animateCtl1 = new OpenNETCF.Windows.Forms.AnimateCtl();
            this.searchTerms1 = new MSNSearchMobile.SearchTerms();
            this.SuspendLayout();
            // 
            // searchTermBindingSource
            // 
            this.searchTermBindingSource.DataMember = "SearchTerm";
            this.searchTermBindingSource.DataSource = this.searchTerms1;
            // 
            // cmbSearchTerm
            // 
            this.cmbSearchTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearchTerm.DataSource = this.searchTermBindingSource;
            this.cmbSearchTerm.DisplayMember = "Value";
            this.cmbSearchTerm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbSearchTerm.Location = new System.Drawing.Point(3, 121);
            this.cmbSearchTerm.Name = "cmbSearchTerm";
            this.cmbSearchTerm.Size = new System.Drawing.Size(156, 22);
            this.cmbSearchTerm.TabIndex = 0;
            this.cmbSearchTerm.ValueMember = "SearchTermKey";
            // 
            // btnSearch
            // 
            this.btnSearch.ActiveForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSearch.ImageIndex = -1;
            this.btnSearch.ImageList = null;
            this.btnSearch.Location = new System.Drawing.Point(165, 121);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 20);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(234, 36);
            // 
            // lblResponse
            // 
            this.lblResponse.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblResponse.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblResponse.Location = new System.Drawing.Point(3, 144);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(234, 40);
            this.lblResponse.Text = "[Response/Errors]";
            this.lblResponse.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblResponse.Visible = false;
            // 
            // animateCtl1
            // 
            this.animateCtl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.animateCtl1.DelayInterval = 50;
            this.animateCtl1.FrameHeight = 11;
            this.animateCtl1.FrameWidth = 67;
            this.animateCtl1.Height = 11;
            this.animateCtl1.Image = ((System.Drawing.Image)(resources.GetObject("animateCtl1.Image")));
            this.animateCtl1.Location = new System.Drawing.Point(87, 187);
            this.animateCtl1.LoopCount = 0;
            this.animateCtl1.Name = "animateCtl1";
            this.animateCtl1.Size = new System.Drawing.Size(67, 11);
            this.animateCtl1.TabIndex = 4;
            this.animateCtl1.Visible = false;
            this.animateCtl1.Width = 67;
            // 
            // searchTerms1
            // 
            this.searchTerms1.DataSetName = "SearchTerms";
            this.searchTerms1.Prefix = "";
            this.searchTerms1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.animateCtl1);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.cmbSearchTerm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "MSN Search Mobile";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSearchTerm;
        private OpenNETCF.Windows.Forms.Button2 btnSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblResponse;
        private OpenNETCF.Windows.Forms.AnimateCtl animateCtl1;
        private System.Windows.Forms.BindingSource searchTermBindingSource;
        private SearchTerms searchTerms1;
    }
}