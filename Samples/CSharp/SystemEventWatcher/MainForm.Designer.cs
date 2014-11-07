namespace SystemEventWatcher
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.deviceSerial = new System.Windows.Forms.CheckBox();
            this.devicePower = new System.Windows.Forms.CheckBox();
            this.deviceNetwork = new System.Windows.Forms.CheckBox();
            this.deviceTime = new System.Windows.Forms.CheckBox();
            this.deviceWake = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.eventList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.powerUp = new System.Windows.Forms.CheckBox();
            this.powerBoot = new System.Windows.Forms.CheckBox();
            this.powerDown = new System.Windows.Forms.CheckBox();
            this.powerCritical = new System.Windows.Forms.CheckBox();
            this.powerIdle = new System.Windows.Forms.CheckBox();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Controls.Add(this.tabPage2);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(240, 131);
            this.tabs.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.deviceSerial);
            this.tabPage1.Controls.Add(this.devicePower);
            this.tabPage1.Controls.Add(this.deviceNetwork);
            this.tabPage1.Controls.Add(this.deviceTime);
            this.tabPage1.Controls.Add(this.deviceWake);
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(240, 108);
            this.tabPage1.Text = "Device";
            // 
            // deviceSerial
            // 
            this.deviceSerial.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.deviceSerial.Location = new System.Drawing.Point(7, 74);
            this.deviceSerial.Name = "deviceSerial";
            this.deviceSerial.Size = new System.Drawing.Size(123, 20);
            this.deviceSerial.TabIndex = 4;
            this.deviceSerial.Text = "Serial";
            this.deviceSerial.CheckStateChanged += new System.EventHandler(this.deviceSerial_CheckStateChanged);
            // 
            // devicePower
            // 
            this.devicePower.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.devicePower.Location = new System.Drawing.Point(7, 57);
            this.devicePower.Name = "devicePower";
            this.devicePower.Size = new System.Drawing.Size(123, 20);
            this.devicePower.TabIndex = 3;
            this.devicePower.Text = "AC Power";
            this.devicePower.CheckStateChanged += new System.EventHandler(this.devicePower_CheckStateChanged);
            // 
            // deviceNetwork
            // 
            this.deviceNetwork.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.deviceNetwork.Location = new System.Drawing.Point(7, 40);
            this.deviceNetwork.Name = "deviceNetwork";
            this.deviceNetwork.Size = new System.Drawing.Size(123, 20);
            this.deviceNetwork.TabIndex = 2;
            this.deviceNetwork.Text = "Network Change";
            this.deviceNetwork.CheckStateChanged += new System.EventHandler(this.deviceNetwork_CheckStateChanged);
            // 
            // deviceTime
            // 
            this.deviceTime.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.deviceTime.Location = new System.Drawing.Point(7, 23);
            this.deviceTime.Name = "deviceTime";
            this.deviceTime.Size = new System.Drawing.Size(100, 20);
            this.deviceTime.TabIndex = 1;
            this.deviceTime.Text = "Time Change";
            this.deviceTime.CheckStateChanged += new System.EventHandler(this.deviceTime_CheckStateChanged);
            // 
            // deviceWake
            // 
            this.deviceWake.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.deviceWake.Location = new System.Drawing.Point(7, 6);
            this.deviceWake.Name = "deviceWake";
            this.deviceWake.Size = new System.Drawing.Size(100, 20);
            this.deviceWake.TabIndex = 0;
            this.deviceWake.Text = "Wake";
            this.deviceWake.CheckStateChanged += new System.EventHandler(this.deviceWake_CheckStateChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.powerIdle);
            this.tabPage2.Controls.Add(this.powerCritical);
            this.tabPage2.Controls.Add(this.powerDown);
            this.tabPage2.Controls.Add(this.powerUp);
            this.tabPage2.Controls.Add(this.powerBoot);
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(240, 108);
            this.tabPage2.Text = "Power";
            // 
            // eventList
            // 
            this.eventList.Location = new System.Drawing.Point(3, 23);
            this.eventList.Name = "eventList";
            this.eventList.Size = new System.Drawing.Size(234, 114);
            this.eventList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Events Received";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 137);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 131);
            // 
            // powerUp
            // 
            this.powerUp.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.powerUp.Location = new System.Drawing.Point(7, 24);
            this.powerUp.Name = "powerUp";
            this.powerUp.Size = new System.Drawing.Size(100, 20);
            this.powerUp.TabIndex = 3;
            this.powerUp.Text = "Power Up";
            this.powerUp.CheckStateChanged += new System.EventHandler(this.powerUp_CheckStateChanged);
            // 
            // powerBoot
            // 
            this.powerBoot.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.powerBoot.Location = new System.Drawing.Point(7, 7);
            this.powerBoot.Name = "powerBoot";
            this.powerBoot.Size = new System.Drawing.Size(100, 20);
            this.powerBoot.TabIndex = 2;
            this.powerBoot.Text = "Boot";
            this.powerBoot.CheckStateChanged += new System.EventHandler(this.powerBoot_CheckStateChanged);
            // 
            // powerDown
            // 
            this.powerDown.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.powerDown.Location = new System.Drawing.Point(7, 41);
            this.powerDown.Name = "powerDown";
            this.powerDown.Size = new System.Drawing.Size(100, 20);
            this.powerDown.TabIndex = 4;
            this.powerDown.Text = "Power Down";
            this.powerDown.CheckStateChanged += new System.EventHandler(this.powerDown_CheckStateChanged);
            // 
            // powerCritical
            // 
            this.powerCritical.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.powerCritical.Location = new System.Drawing.Point(7, 58);
            this.powerCritical.Name = "powerCritical";
            this.powerCritical.Size = new System.Drawing.Size(100, 20);
            this.powerCritical.TabIndex = 5;
            this.powerCritical.Text = "Critical";
            this.powerCritical.CheckStateChanged += new System.EventHandler(this.powerCritical_CheckStateChanged);
            // 
            // powerIdle
            // 
            this.powerIdle.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.powerIdle.Location = new System.Drawing.Point(7, 76);
            this.powerIdle.Name = "powerIdle";
            this.powerIdle.Size = new System.Drawing.Size(100, 20);
            this.powerIdle.TabIndex = 6;
            this.powerIdle.Text = "Idle";
            this.powerIdle.CheckStateChanged += new System.EventHandler(this.powerIdle_CheckStateChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eventList);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Event Watcher";
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox eventList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox deviceNetwork;
        private System.Windows.Forms.CheckBox deviceTime;
        private System.Windows.Forms.CheckBox deviceWake;
        private System.Windows.Forms.CheckBox devicePower;
        private System.Windows.Forms.CheckBox deviceSerial;
        private System.Windows.Forms.CheckBox powerUp;
        private System.Windows.Forms.CheckBox powerBoot;
        private System.Windows.Forms.CheckBox powerIdle;
        private System.Windows.Forms.CheckBox powerCritical;
        private System.Windows.Forms.CheckBox powerDown;
    }
}

