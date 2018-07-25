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
    partial class SearchResults
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
            this.miOpenLink = new System.Windows.Forms.MenuItem();
            this.miBack = new System.Windows.Forms.MenuItem();
            this.msnSearchResultsList1 = new MSNSearchMobile.MSNSearchResultsList();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.miOpenLink);
            this.mainMenu1.MenuItems.Add(this.miBack);
            // 
            // miOpenLink
            // 
            this.miOpenLink.Text = "Open";
            this.miOpenLink.Click += new System.EventHandler(this.miOpenLink_Click);
            // 
            // miBack
            // 
            this.miBack.Text = "Back";
            this.miBack.Click += new System.EventHandler(this.miBack_Click);
            // 
            // msnSearchResultsList1
            // 
            this.msnSearchResultsList1.BackgroundImage = null;
            this.msnSearchResultsList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msnSearchResultsList1.Location = new System.Drawing.Point(0, 0);
            this.msnSearchResultsList1.Name = "msnSearchResultsList1";
            this.msnSearchResultsList1.Size = new System.Drawing.Size(240, 268);
            this.msnSearchResultsList1.TabIndex = 3;
            this.msnSearchResultsList1.Text = "msnSearchResultsList1";
            // 
            // SearchResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.msnSearchResultsList1);
            this.Menu = this.mainMenu1;
            this.Name = "SearchResults";
            this.Text = "MSN Search Mobile";
            this.ResumeLayout(false);

        }

        #endregion

        private MSNSearchResultsList msnSearchResultsList1;
        private System.Windows.Forms.MenuItem miOpenLink;
        private System.Windows.Forms.MenuItem miBack;
    }
}

