namespace AudioRecorder
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
      this.record = new System.Windows.Forms.Button();
      this.play = new System.Windows.Forms.Button();
      this.stop = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // record
      // 
      this.record.Location = new System.Drawing.Point(15, 46);
      this.record.Name = "record";
      this.record.Size = new System.Drawing.Size(72, 33);
      this.record.TabIndex = 0;
      this.record.Text = "Record";
      this.record.Click += new System.EventHandler(this.record_Click);
      // 
      // play
      // 
      this.play.Location = new System.Drawing.Point(15, 94);
      this.play.Name = "play";
      this.play.Size = new System.Drawing.Size(72, 33);
      this.play.TabIndex = 1;
      this.play.Text = "Play";
      this.play.Click += new System.EventHandler(this.play_Click);
      // 
      // stop
      // 
      this.stop.Location = new System.Drawing.Point(15, 145);
      this.stop.Name = "stop";
      this.stop.Size = new System.Drawing.Size(72, 33);
      this.stop.TabIndex = 2;
      this.stop.Text = "Stop";
      this.stop.Click += new System.EventHandler(this.stop_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.stop);
      this.Controls.Add(this.play);
      this.Controls.Add(this.record);
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button record;
    private System.Windows.Forms.Button play;
    private System.Windows.Forms.Button stop;
  }
}

