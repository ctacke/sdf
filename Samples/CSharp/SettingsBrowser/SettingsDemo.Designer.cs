namespace SettingsBrowser
{
    partial class SettingsDemo
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
			this.btnCreateXML = new System.Windows.Forms.Button();
			this.status = new System.Windows.Forms.StatusBar();
			this.btnAction = new System.Windows.Forms.Button();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.tvAppSettings = new System.Windows.Forms.TreeView();
			this.btnOpen = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnCreateXML
			// 
			this.btnCreateXML.Location = new System.Drawing.Point(26, 28);
			this.btnCreateXML.Name = "btnCreateXML";
			this.btnCreateXML.Size = new System.Drawing.Size(79, 21);
			this.btnCreateXML.TabIndex = 0;
			this.btnCreateXML.Text = "Create";
			this.btnCreateXML.Click += new System.EventHandler(this.btnCreateXML_Click);
			// 
			// status
			// 
			this.status.Location = new System.Drawing.Point(0, 246);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(240, 22);
			// 
			// btnAction
			// 
			this.btnAction.Enabled = false;
			this.btnAction.Location = new System.Drawing.Point(15, 73);
			this.btnAction.Name = "btnAction";
			this.btnAction.Size = new System.Drawing.Size(102, 20);
			this.btnAction.TabIndex = 3;
			this.btnAction.Text = "Create Group";
			this.btnAction.Click += new System.EventHandler(this.btnCreateGrp_Click);
			// 
			// txtValue
			// 
			this.txtValue.Enabled = false;
			this.txtValue.Location = new System.Drawing.Point(140, 72);
			this.txtValue.MaxLength = 12;
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(86, 21);
			this.txtValue.TabIndex = 4;
			this.txtValue.Text = "GroupName";
			// 
			// tvAppSettings
			// 
			this.tvAppSettings.Enabled = false;
			this.tvAppSettings.Location = new System.Drawing.Point(15, 113);
			this.tvAppSettings.Name = "tvAppSettings";
			this.tvAppSettings.Size = new System.Drawing.Size(209, 117);
			this.tvAppSettings.TabIndex = 6;
			this.tvAppSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvAppSettings_AfterSelect);
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(129, 28);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(79, 21);
			this.btnOpen.TabIndex = 8;
			this.btnOpen.Text = "Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpenXML_Click);
			// 
			// SettingsDemo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.tvAppSettings);
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.btnAction);
			this.Controls.Add(this.status);
			this.Controls.Add(this.btnCreateXML);
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "SettingsDemo";
			this.Text = "Settings Demo";
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button btnCreateXML;
		private System.Windows.Forms.StatusBar status;
		private System.Windows.Forms.Button btnAction;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.TreeView tvAppSettings;
		private System.Windows.Forms.Button btnOpen;
    }
}

