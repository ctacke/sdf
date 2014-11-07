namespace OpenNETCF.Samples.Imaging
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.imageViewer1 = new OpenNETCF.Windows.Forms.ImageViewer();
            this.lnkSelectImage = new System.Windows.Forms.LinkLabel();
            this.btnRotateLeft = new OpenNETCF.Windows.Forms.Button2();
            this.btnRotateRight = new OpenNETCF.Windows.Forms.Button2();
            this.pbThumbnail = new System.Windows.Forms.PictureBox();
            this.btnReload = new OpenNETCF.Windows.Forms.Button2();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lnkTakePicture = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // imageViewer1
            // 
            this.imageViewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewer1.Center = false;
            this.imageViewer1.Image = null;
            this.imageViewer1.Location = new System.Drawing.Point(3, 36);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.Size = new System.Drawing.Size(234, 166);
            this.imageViewer1.TabIndex = 0;
            // 
            // lnkSelectImage
            // 
            this.lnkSelectImage.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lnkSelectImage.Location = new System.Drawing.Point(2, 7);
            this.lnkSelectImage.Name = "lnkSelectImage";
            this.lnkSelectImage.Size = new System.Drawing.Size(100, 20);
            this.lnkSelectImage.TabIndex = 1;
            this.lnkSelectImage.Text = "Select Image";
            this.lnkSelectImage.Click += new System.EventHandler(this.lnkSelectImage_Click);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Enabled = false;
            this.btnRotateLeft.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
            this.btnRotateLeft.ImageIndex = -1;
            this.btnRotateLeft.ImageList = null;
            this.btnRotateLeft.Location = new System.Drawing.Point(29, 245);
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(24, 20);
            this.btnRotateLeft.TabIndex = 2;
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.Enabled = false;
            this.btnRotateRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRight.Image")));
            this.btnRotateRight.ImageIndex = -1;
            this.btnRotateRight.ImageList = null;
            this.btnRotateRight.Location = new System.Drawing.Point(55, 245);
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(24, 20);
            this.btnRotateRight.TabIndex = 3;
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // pbThumbnail
            // 
            this.pbThumbnail.Location = new System.Drawing.Point(212, 7);
            this.pbThumbnail.Name = "pbThumbnail";
            this.pbThumbnail.Size = new System.Drawing.Size(25, 23);
            // 
            // btnReload
            // 
            this.btnReload.Enabled = false;
            this.btnReload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
            this.btnReload.ImageIndex = -1;
            this.btnReload.ImageList = null;
            this.btnReload.Location = new System.Drawing.Point(3, 245);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(24, 20);
            this.btnReload.TabIndex = 4;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(2, 208);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(235, 31);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.Value = 5;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged_1);
            // 
            // lnkTakePicture
            // 
            this.lnkTakePicture.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lnkTakePicture.Location = new System.Drawing.Point(95, 7);
            this.lnkTakePicture.Name = "lnkTakePicture";
            this.lnkTakePicture.Size = new System.Drawing.Size(100, 17);
            this.lnkTakePicture.TabIndex = 10;
            this.lnkTakePicture.Text = "Take Picture";
            this.lnkTakePicture.Click += new System.EventHandler(this.lnkTakePicture_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lnkTakePicture);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.pbThumbnail);
            this.Controls.Add(this.btnRotateRight);
            this.Controls.Add(this.btnRotateLeft);
            this.Controls.Add(this.imageViewer1);
            this.Controls.Add(this.lnkSelectImage);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenNETCF.Windows.Forms.ImageViewer imageViewer1;
        private System.Windows.Forms.LinkLabel lnkSelectImage;
        private OpenNETCF.Windows.Forms.Button2 btnRotateLeft;
        private OpenNETCF.Windows.Forms.Button2 btnRotateRight;
        private System.Windows.Forms.PictureBox pbThumbnail;
        private OpenNETCF.Windows.Forms.Button2 btnReload;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.LinkLabel lnkTakePicture;

    }
}

