namespace NetUI
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
            this.adaptersMenu = new System.Windows.Forms.MenuItem();
            this.wirelessNetworkMenu = new System.Windows.Forms.MenuItem();
            this.itemList = new OpenNETCF.Windows.Forms.ListBox2();
            this.adapterMenu = new System.Windows.Forms.ContextMenu();
            this.releaseMenu = new System.Windows.Forms.MenuItem();
            this.renewMenu = new System.Windows.Forms.MenuItem();
            this.titleLabel = new System.Windows.Forms.Label();
            this.propertiesFrame = new OpenNETCF.Windows.Forms.GroupBox();
            this.gatewayLabel = new System.Windows.Forms.Label();
            this.subnetLabel = new System.Windows.Forms.Label();
            this.dhcpLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.macLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.wzcMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.propertiesFrame.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.adaptersMenu);
            this.mainMenu1.MenuItems.Add(this.wirelessNetworkMenu);
            // 
            // adaptersMenu
            // 
            this.adaptersMenu.Text = "Adapters";
            // 
            // wirelessNetworkMenu
            // 
            this.wirelessNetworkMenu.Enabled = false;
            this.wirelessNetworkMenu.Text = "Wireless Networks";
            // 
            // itemList
            // 
            this.itemList.BackColor = System.Drawing.SystemColors.Window;
            this.itemList.DataSource = null;
            this.itemList.DisplayMember = null;
            this.itemList.EvenItemColor = System.Drawing.SystemColors.Control;
            this.itemList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
            this.itemList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.itemList.ImageList = null;
            this.itemList.ItemHeight = 60;
            this.itemList.LineColor = System.Drawing.SystemColors.ControlText;
            this.itemList.Location = new System.Drawing.Point(3, 23);
            this.itemList.Name = "itemList";
            this.itemList.SelectedIndex = -1;
            this.itemList.ShowLines = true;
            this.itemList.ShowScrollbar = true;
            this.itemList.Size = new System.Drawing.Size(234, 120);
            this.itemList.TabIndex = 0;
            this.itemList.Text = "listBox21";
            this.itemList.TopIndex = 0;
            this.itemList.WrapText = false;
            // 
            // adapterMenu
            // 
            this.adapterMenu.MenuItems.Add(this.releaseMenu);
            this.adapterMenu.MenuItems.Add(this.renewMenu);
            // 
            // releaseMenu
            // 
            this.releaseMenu.Text = "DHCP Release";
            // 
            // renewMenu
            // 
            this.renewMenu.Text = "DHCP Renew";
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(3, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(138, 20);
            this.titleLabel.Text = "Network Adapters";
            // 
            // propertiesFrame
            // 
            this.propertiesFrame.Controls.Add(this.gatewayLabel);
            this.propertiesFrame.Controls.Add(this.subnetLabel);
            this.propertiesFrame.Controls.Add(this.dhcpLabel);
            this.propertiesFrame.Controls.Add(this.label3);
            this.propertiesFrame.Controls.Add(this.label2);
            this.propertiesFrame.Controls.Add(this.label1);
            this.propertiesFrame.Controls.Add(this.macLabel);
            this.propertiesFrame.Controls.Add(this.label5);
            this.propertiesFrame.Location = new System.Drawing.Point(3, 188);
            this.propertiesFrame.Name = "propertiesFrame";
            this.propertiesFrame.Size = new System.Drawing.Size(234, 77);
            // 
            // gatewayLabel
            // 
            this.gatewayLabel.Location = new System.Drawing.Point(93, 55);
            this.gatewayLabel.Name = "gatewayLabel";
            this.gatewayLabel.Size = new System.Drawing.Size(138, 20);
            this.gatewayLabel.Text = "N/A";
            // 
            // subnetLabel
            // 
            this.subnetLabel.Location = new System.Drawing.Point(93, 38);
            this.subnetLabel.Name = "subnetLabel";
            this.subnetLabel.Size = new System.Drawing.Size(138, 20);
            this.subnetLabel.Text = "N/A";
            // 
            // dhcpLabel
            // 
            this.dhcpLabel.Location = new System.Drawing.Point(93, 22);
            this.dhcpLabel.Name = "dhcpLabel";
            this.dhcpLabel.Size = new System.Drawing.Size(138, 20);
            this.dhcpLabel.Text = "N/A";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.Text = "Gateway:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.Text = "Subnet Mask:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.Text = "DHCP Server:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // macLabel
            // 
            this.macLabel.Location = new System.Drawing.Point(93, 6);
            this.macLabel.Name = "macLabel";
            this.macLabel.Size = new System.Drawing.Size(138, 20);
            this.macLabel.Text = "N/A";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.Text = "MAC Address:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // wzcMenu
            // 
            this.wzcMenu.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Connect";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.propertiesFrame);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.itemList);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "OpenNETCF NetUI";
            this.propertiesFrame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.ListBox2 itemList;
        private System.Windows.Forms.MenuItem adaptersMenu;
        private System.Windows.Forms.ContextMenu adapterMenu;
        private System.Windows.Forms.MenuItem renewMenu;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.MenuItem wirelessNetworkMenu;
        private OpenNETCF.Windows.Forms.GroupBox propertiesFrame;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label gatewayLabel;
        private System.Windows.Forms.Label subnetLabel;
        private System.Windows.Forms.Label dhcpLabel;
        private System.Windows.Forms.Label macLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MenuItem releaseMenu;
        private System.Windows.Forms.ContextMenu wzcMenu;
        private System.Windows.Forms.MenuItem menuItem1;

    }
}

