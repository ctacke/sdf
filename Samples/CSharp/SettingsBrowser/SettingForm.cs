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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.AppSettings;

namespace SettingsBrowser
{
    public partial class SettingForm : Form
    {
		//Never expose public members like this!
		public string settingName;
		public object settingValue;
		public string message;

        public SettingForm()
        {
            InitializeComponent();
        }

		public void EditSetting(string settingName, object value)
		{
			this.txtSettingName.Text = settingName;
			this.txtSettingName.ReadOnly = true;
			this.txtSettingVal.Text = value.ToString();

			if (value.GetType() == typeof(bool))
				this.typeBool.Checked = true;
			else if (value.GetType() == typeof(float))
				this.typeFloat.Checked = true;
			else if (value.GetType() == typeof(string))
				this.typeString.Checked = true;
			else 
				this.typeInt.Checked = true;

			this.ShowDialog();
		}

		public void CreateSetting()
		{
			this.txtSettingVal.Text = String.Empty;
			this.ShowDialog();
		}

		private void ok_Click(object sender, EventArgs e)
		{
			//Process Setting Change
			if (String.IsNullOrEmpty(txtSettingName.Text))
				message = "No setting name chosen";
			else if (!String.IsNullOrEmpty(txtSettingVal.Text))
			{
				settingName = txtSettingName.Text;
				bool error = false;

				if (typeBool.Checked)
				{
					try
					{
						if (txtSettingVal.Text.ToUpper().StartsWith("T"))
							settingValue = true;
						else
							settingValue = false;
					}
					catch
					{
						error = true;
						settingValue = false;
					}
				}
				else if (typeFloat.Checked)
				{
					try
					{
						settingValue = System.Convert.ToDouble(txtSettingVal.Text);
					}
					catch
					{
						error = true;
						settingValue = (float)0;
					}
				}
				else if (typeInt.Checked)
				{
					try
					{
						settingValue = System.Convert.ToInt32(txtSettingVal.Text);
					}
					catch
					{
						error = true;
						settingValue = (Int32)0;
					}
				}
				else if (typeString.Checked)
				{
					settingValue = txtSettingVal.Text;
				}

				if (error)
					message = "Error parsing value.  Default value stored";
			}
			else
			{
				message = "No value entered";
			}

			this.Close();
		}

		private void cancel_Click(object sender, EventArgs e)
		{
			//Do nothing
			message = "Operation canceled";
			this.Close();
		}

		private void txtSettingVal_TextChanged(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(txtSettingVal.Text) || String.IsNullOrEmpty(txtSettingName.Text))
				this.ok.Enabled = false;
			else
				this.ok.Enabled = true;
		}

    }
}