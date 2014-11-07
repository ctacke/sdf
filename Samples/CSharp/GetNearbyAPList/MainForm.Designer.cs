namespace GetNearbyAPList
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
      this.apList = new System.Windows.Forms.ListBox();
      this.refresh = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.adapterName = new System.Windows.Forms.Label();
      this.adapterType = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // apList
      // 
      this.apList.Location = new System.Drawing.Point(3, 60);
      this.apList.Name = "apList";
      this.apList.Size = new System.Drawing.Size(234, 170);
      this.apList.TabIndex = 0;
      // 
      // refresh
      // 
      this.refresh.Enabled = false;
      this.refresh.Location = new System.Drawing.Point(156, 236);
      this.refresh.Name = "refresh";
      this.refresh.Size = new System.Drawing.Size(81, 29);
      this.refresh.TabIndex = 1;
      this.refresh.Text = "Refresh";
      this.refresh.Click += new System.EventHandler(this.refresh_Click);
      // 
      // label1
      // 
      this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 20);
      this.label1.Text = "Adapter:";
      // 
      // adapterName
      // 
      this.adapterName.Location = new System.Drawing.Point(62, 0);
      this.adapterName.Name = "adapterName";
      this.adapterName.Size = new System.Drawing.Size(178, 20);
      this.adapterName.Text = "<unknown>";
      // 
      // adapterType
      // 
      this.adapterType.Location = new System.Drawing.Point(62, 20);
      this.adapterType.Name = "adapterType";
      this.adapterType.Size = new System.Drawing.Size(178, 20);
      this.adapterType.Text = "<unknown>";
      // 
      // label4
      // 
      this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label4.Location = new System.Drawing.Point(3, 20);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(61, 20);
      this.label4.Text = "Type:";
      // 
      // label5
      // 
      this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label5.Location = new System.Drawing.Point(3, 40);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(150, 20);
      this.label5.Text = "Nearby Access Points:";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.adapterType);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.adapterName);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.refresh);
      this.Controls.Add(this.apList);
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "MainForm";
      this.Text = "Nearby APs";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox apList;
    private System.Windows.Forms.Button refresh;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label adapterName;
    private System.Windows.Forms.Label adapterType;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
  }
}

