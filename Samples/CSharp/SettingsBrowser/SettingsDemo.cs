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
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.AppSettings;

namespace SettingsBrowser
{
	/// <summary>
	/// Demo application that shows how to write to and read from OpenNETCF.AppSettings
	/// </summary>
	public partial class SettingsDemo : Form
	{
		//Our handle to an AppSettings file
		private SettingsFile settingsFile;
		TreeNode tnParent;

		public SettingsDemo()
		{
			InitializeComponent();

			tnParent = new TreeNode("Settings File");
			tvAppSettings.Nodes.AddRange(new TreeNode[] { tnParent });

		}

		private void btnOpenXML_Click(object sender, EventArgs e)
		{
			//Launch File Picker
			OpenFileDialog sfd = new OpenFileDialog();
			sfd.Filter = "Setting Files (*.XML)|*.xml";

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				OpenOrCreateXML(sfd.FileName);
			}
		}

		private void btnCreateXML_Click(object sender, EventArgs e)
		{
			//Launch File Picker
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Setting Files (*.XML)|*.xml";

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				OpenOrCreateXML(sfd.FileName);
			}
		}

		private void OpenOrCreateXML(string fullPath)
		{
			//No need to catch exceptions - handle will be null if error occurs
			Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			settingsFile = new SettingsFile(fullPath);
			if (settingsFile != null)
			{
				status.Text = String.Format("Loaded {0} groups from XML", settingsFile.Groups.Count);
				this.tvAppSettings.Enabled = true;

				//Refresh tree
				if (settingsFile.Groups.Count != 0)
					RefreshTree();
			}
			else
			{
				status.Text = "Error loading file";
				this.txtValue.Enabled = false;
				this.btnAction.Enabled = false;

				tvAppSettings.Nodes.Clear();
				this.tvAppSettings.Enabled = false;
			}
			Cursor.Current = System.Windows.Forms.Cursors.Default;
		}

		/// <summary>
		/// Note - We do not recommend tearing down a data structure every time members change!
		/// This is just for illustratory purposes.
		/// </summary>
		private void RefreshTree()
		{
			tvAppSettings.Nodes.Clear();
			tnParent = new TreeNode("Settings File");
			tvAppSettings.Nodes.AddRange(new TreeNode[] { tnParent });

			//Look throughout the XML file
			foreach (SettingGroup sg in settingsFile.Groups)
			{
				//Create a new node for each group
				TreeNode tnGroup = new TreeNode(sg.Name);
				tnGroup.Tag = sg;
				tnParent.Nodes.AddRange(new TreeNode[] { tnGroup });

				foreach (Setting s in sg.Settings)
				{
					TreeNode tnSetting = new TreeNode(s.Name);
					tnSetting.Tag = s;

					tnGroup.Nodes.AddRange(new TreeNode[] { tnSetting });
				}
			}
			tnParent.Expand();
			tvAppSettings.SelectedNode = tnParent;
			tvAppSettings.Focus();
		}

		private void btnCreateGrp_Click(object sender, EventArgs e)
		{
            if (tvAppSettings.SelectedNode == null) return;

			if (!String.IsNullOrEmpty(txtValue.Text))
			{
				//We're in group create mode
				if (this.tvAppSettings.SelectedNode.Parent == null)
				{
					if (!settingsFile.Groups.Contains(txtValue.Text))
					{
						settingsFile.Groups.Add(txtValue.Text);
						settingsFile.Save();

						//Now read properties of Group
						try
						{
							SettingGroup loadedGroup = settingsFile.Groups[txtValue.Text];

							if (loadedGroup != null)
							{
								status.Text = String.Format("Got group {0} w/ {1} settings", loadedGroup.Name, loadedGroup.Settings.Count);
								RefreshTree();
							}
						}
						catch
						{
							//Did not load group!
							status.Text = "Problem creating group" + txtValue.Text;
						}
					}
					else
						status.Text = "Group already exists";
				}
				//Otherwise, we're in setting create/edit mode
				else
				{
					#region Goes Into other Form
					if (tvAppSettings.SelectedNode.Tag != null)
					{
						//We're going to create a new setting as we're sitting on a group
						if (tvAppSettings.SelectedNode.Tag.GetType() == typeof(SettingGroup))
						{
							//Redirect to create value
							SettingForm sf = new SettingForm();
							sf.CreateSetting();

							string newSettingName = sf.settingName;
							object newSettingValue = sf.settingValue;

							if (String.IsNullOrEmpty(sf.message))
							{
								Setting newSetting = new Setting(newSettingName, newSettingValue);
								SettingGroup groupSelected = (SettingGroup)tvAppSettings.SelectedNode.Tag;
								groupSelected.Settings.Add(newSetting);
								status.Text = "Created new setting";

								settingsFile.Save();
								RefreshTree();
							}
							else
								status.Text = sf.message;
						}
						else if (tvAppSettings.SelectedNode.Tag.GetType() == typeof(Setting))
						{
							//Redirect to edit value
							SettingForm sf = new SettingForm();
							sf.EditSetting(((Setting)tvAppSettings.SelectedNode.Tag).Name, ((Setting)tvAppSettings.SelectedNode.Tag).Value);
							object newSettingValue = sf.settingValue;

							if (newSettingValue != null && String.IsNullOrEmpty(sf.message))
							{
								//We're on an existing setting
								Setting selectedSetting = (Setting)tvAppSettings.SelectedNode.Tag;
								selectedSetting.Value = newSettingValue;
								status.Text = "Updated existing setting";

								settingsFile.Save();
								RefreshTree();
							}
							else
								status.Text = sf.message;

						}

					}
					#endregion
				}
			}
		}

		private void tvAppSettings_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.tvAppSettings.SelectedNode == null)
			{
				this.btnAction.Enabled = false;
				this.txtValue.Enabled = false;
				return;
			}
			else
			{
				this.btnAction.Enabled = true;
				this.txtValue.Enabled = true;
			}

			//We've selected the root node - allow group create
			if (e.Node == tnParent)
			{
				this.btnAction.Text = "Create Group";
				this.txtValue.Text = "GroupName";
				this.txtValue.Enabled = true;

				this.status.Text = String.Empty;
			}
			//We've selected a group node - allow setting create
			else if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(SettingGroup))
			{
				this.btnAction.Text = "Create Setting";
				this.txtValue.Enabled = false;

				this.status.Text = String.Format("Group {0} has {1} settings", ((SettingGroup)e.Node.Tag).Name, ((SettingGroup)e.Node.Tag).Settings.Count);
			}
			//We've selected an existing setting - allow setting modify
			else if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(Setting))
			{
				this.btnAction.Text = "Edit Setting";
				this.txtValue.Enabled = false;

				this.status.Text = String.Format("Value of {0}: {1}", ((Setting)e.Node.Tag).Name, ((Setting)e.Node.Tag).Value.ToString());
			}

		}


	}
}