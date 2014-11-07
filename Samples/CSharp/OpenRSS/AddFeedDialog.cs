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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenRSS
{
	/// <summary>
	/// Summary description for AddFeedDialog.
	/// </summary>
	public class AddFeedDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private string url;
	
		public AddFeedDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		public string Url
		{
			get
			{
				return url;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			// 
			// txtUrl
			// 
			this.txtUrl.Location = new System.Drawing.Point(20, 117);
			this.txtUrl.Size = new System.Drawing.Size(201, 22);
			this.txtUrl.Text = "http://";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 57);
			this.label1.Size = new System.Drawing.Size(199, 35);
			this.label1.Text = "Enter the URL of the feed you wish to add:";
			// 
			// AddFeedDialog
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtUrl);
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Text = "Add Feed";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AddFeedDialog_Closing);

		}
		#endregion

		private void AddFeedDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			url = txtUrl.Text;
		}
	}
}
