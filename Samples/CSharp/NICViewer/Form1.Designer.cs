namespace NICViewer
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
      this.label1 = new System.Windows.Forms.Label();
      this.nicList = new System.Windows.Forms.ListView();
      this.nicContext = new System.Windows.Forms.ContextMenu();
      this.nicPropertiesMenuItem = new System.Windows.Forms.MenuItem();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(176, 20);
      this.label1.Text = "Detected Network Adapters";
      // 
      // nicList
      // 
      this.nicList.FullRowSelect = true;
      this.nicList.Location = new System.Drawing.Point(3, 23);
      this.nicList.Name = "nicList";
      this.nicList.Size = new System.Drawing.Size(234, 185);
      this.nicList.TabIndex = 1;
      this.nicList.View = System.Windows.Forms.View.Details;
      // 
      // nicContext
      // 
      this.nicContext.MenuItems.Add(this.nicPropertiesMenuItem);
      // 
      // nicPropertiesMenuItem
      // 
      this.nicPropertiesMenuItem.Text = "Properties";
      this.nicPropertiesMenuItem.Click += new System.EventHandler(this.nicPropertiesMenuItem_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.nicList);
      this.Controls.Add(this.label1);
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.Text = "NIC Viewer";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView nicList;
    private System.Windows.Forms.ContextMenu nicContext;
    private System.Windows.Forms.MenuItem nicPropertiesMenuItem;
  }
}

