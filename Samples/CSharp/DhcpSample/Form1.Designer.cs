namespace DhcpSample
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
            this.enabled = new System.Windows.Forms.CheckBox();
            this.eventList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // enabled
            // 
            this.enabled.Location = new System.Drawing.Point(3, 53);
            this.enabled.Name = "enabled";
            this.enabled.Size = new System.Drawing.Size(147, 20);
            this.enabled.TabIndex = 2;
            this.enabled.Text = "DHCP Enabled";
            this.enabled.CheckStateChanged += new System.EventHandler(this.enabled_CheckStateChanged);
            // 
            // eventList
            // 
            this.eventList.Location = new System.Drawing.Point(3, 81);
            this.eventList.Name = "eventList";
            this.eventList.Size = new System.Drawing.Size(267, 210);
            this.eventList.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(274, 311);
            this.Controls.Add(this.eventList);
            this.Controls.Add(this.enabled);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "DHCP Sample";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox enabled;
        private System.Windows.Forms.ListBox eventList;
    }
}

