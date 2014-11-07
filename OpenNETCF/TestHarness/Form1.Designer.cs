namespace TestHarness
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
            this.rate = new System.Windows.Forms.TrackBar();
            this.play = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.filePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rate
            // 
            this.rate.LargeChange = 25;
            this.rate.Location = new System.Drawing.Point(3, 99);
            this.rate.Maximum = 200;
            this.rate.Minimum = 25;
            this.rate.Name = "rate";
            this.rate.Size = new System.Drawing.Size(234, 45);
            this.rate.SmallChange = 25;
            this.rate.TabIndex = 0;
            this.rate.TickFrequency = 25;
            this.rate.Value = 100;
            this.rate.ValueChanged += new System.EventHandler(this.rate_ValueChanged);
            // 
            // play
            // 
            this.play.Location = new System.Drawing.Point(54, 167);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(136, 43);
            this.play.TabIndex = 1;
            this.play.Text = "Play";
            this.play.Click += new System.EventHandler(this.play_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.Text = "Audio File";
            // 
            // filePath
            // 
            this.filePath.Location = new System.Drawing.Point(3, 38);
            this.filePath.Name = "filePath";
            this.filePath.Size = new System.Drawing.Size(234, 21);
            this.filePath.TabIndex = 3;
            this.filePath.Text = "\\Windows\\infbeg.wav";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(98, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 20);
            this.label2.Text = "1x";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(215, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 20);
            this.label3.Text = "2x";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(156, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 20);
            this.label4.Text = "1.5x";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(32, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 20);
            this.label5.Text = "0.5x";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.play);
            this.Controls.Add(this.rate);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar rate;
        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

