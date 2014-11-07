namespace WiFinder
{
  partial class APView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APView));
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.interfaceName = new System.Windows.Forms.Label();
      this.nearbyAPList = new OpenNETCF.Windows.Forms.ListBox2();
      this.interfaceInfoBtn = new OpenNETCF.Windows.Forms.Button2();
      this.listItem1 = new OpenNETCF.Windows.Forms.ListItem();
      this.smartListItem1 = new OpenNETCF.Windows.Forms.SmartListItem();
      this.smartListItem2 = new OpenNETCF.Windows.Forms.SmartListItem();
      this.smartListItem3 = new OpenNETCF.Windows.Forms.SmartListItem();
      this.apContextMenu = new System.Windows.Forms.ContextMenu();
      this.apConnectMenuItem = new System.Windows.Forms.MenuItem();
      this.apPropetiesMenuItem = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.SuspendLayout();
      // 
      // interfaceName
      // 
      this.interfaceName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
      this.interfaceName.Location = new System.Drawing.Point(3, 2);
      this.interfaceName.Name = "interfaceName";
      this.interfaceName.Size = new System.Drawing.Size(195, 20);
      this.interfaceName.Text = "label1";
      // 
      // nearbyAPList
      // 
      this.nearbyAPList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.nearbyAPList.BackgroundImage = null;
      this.nearbyAPList.DataSource = null;
      this.nearbyAPList.DisplayMember = null;
      this.nearbyAPList.DrawMode = OpenNETCF.Windows.Forms.DrawMode.Normal;
      this.nearbyAPList.EvenItemColor = System.Drawing.SystemColors.Control;
      this.nearbyAPList.ImageList = null;
      this.nearbyAPList.ItemHeight = 15;
      this.nearbyAPList.LineColor = System.Drawing.SystemColors.ControlText;
      this.nearbyAPList.Location = new System.Drawing.Point(3, 25);
      this.nearbyAPList.Name = "nearbyAPList";
      this.nearbyAPList.SelectedIndex = -1;
      this.nearbyAPList.ShowLines = true;
      this.nearbyAPList.Size = new System.Drawing.Size(234, 180);
      this.nearbyAPList.TabIndex = 0;
      this.nearbyAPList.WrapText = false;
      // 
      // interfaceInfoBtn
      // 
      this.interfaceInfoBtn.ActiveBackColor = System.Drawing.SystemColors.Window;
      this.interfaceInfoBtn.ActiveBackgroundImage = ((System.Drawing.Image)(resources.GetObject("interfaceInfoBtn.ActiveBackgroundImage")));
      this.interfaceInfoBtn.ActiveBorderColor = System.Drawing.SystemColors.Window;
      this.interfaceInfoBtn.BackColor = System.Drawing.SystemColors.Window;
      this.interfaceInfoBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("interfaceInfoBtn.BackgroundImage")));
      this.interfaceInfoBtn.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.interfaceInfoBtn.ForeColor = System.Drawing.SystemColors.Window;
      this.interfaceInfoBtn.Location = new System.Drawing.Point(213, 0);
      this.interfaceInfoBtn.Name = "interfaceInfoBtn";
      this.interfaceInfoBtn.ShowFocusBorder = false;
      this.interfaceInfoBtn.Size = new System.Drawing.Size(24, 24);
      this.interfaceInfoBtn.TabIndex = 2;
      this.interfaceInfoBtn.TabStop = false;
      this.interfaceInfoBtn.Click += new System.EventHandler(this.interfaceInfoBtn_Click);
      // 
      // listItem1
      // 
      this.listItem1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
      this.listItem1.ForeColor = System.Drawing.Color.Black;
      this.listItem1.ImageIndex = -1;
      this.listItem1.Tag = null;
      this.listItem1.Text = "";
      // 
      // smartListItem1
      // 
      this.smartListItem1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
      this.smartListItem1.ForeColor = System.Drawing.Color.Black;
      this.smartListItem1.ImageIndex = -1;
      this.smartListItem1.Text = "";
      // 
      // smartListItem2
      // 
      this.smartListItem2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
      this.smartListItem2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.smartListItem2.ImageIndex = -1;
      this.smartListItem2.Text = "dsd";
      // 
      // smartListItem3
      // 
      this.smartListItem3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
      this.smartListItem3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.smartListItem3.ImageIndex = -1;
      this.smartListItem3.Text = "item2";
      // 
      // apContextMenu
      // 
      this.apContextMenu.MenuItems.Add(this.apConnectMenuItem);
      this.apContextMenu.MenuItems.Add(this.menuItem3);
      this.apContextMenu.MenuItems.Add(this.apPropetiesMenuItem);
      // 
      // apConnectMenuItem
      // 
      this.apConnectMenuItem.Text = "Connect";
      // 
      // apPropetiesMenuItem
      // 
      this.apPropetiesMenuItem.Text = "Properties";
      // 
      // menuItem3
      // 
      this.menuItem3.Text = "-";
      // 
      // APView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.interfaceName);
      this.Controls.Add(this.nearbyAPList);
      this.Controls.Add(this.interfaceInfoBtn);
      this.Menu = this.mainMenu1;
      this.Name = "APView";
      this.Text = "OpenNETCF WiFinder";
      this.ResumeLayout(false);

    }

    #endregion

    private OpenNETCF.Windows.Forms.ListItem listItem1;
    private OpenNETCF.Windows.Forms.SmartListItem smartListItem1;
    private OpenNETCF.Windows.Forms.SmartListItem smartListItem2;
    private OpenNETCF.Windows.Forms.SmartListItem smartListItem3;
    private OpenNETCF.Windows.Forms.ListBox2 nearbyAPList;
    private System.Windows.Forms.Label interfaceName;
    private OpenNETCF.Windows.Forms.Button2 interfaceInfoBtn;
    private System.Windows.Forms.ContextMenu apContextMenu;
    private System.Windows.Forms.MenuItem apConnectMenuItem;
    private System.Windows.Forms.MenuItem menuItem3;
    private System.Windows.Forms.MenuItem apPropetiesMenuItem;


  }
}

