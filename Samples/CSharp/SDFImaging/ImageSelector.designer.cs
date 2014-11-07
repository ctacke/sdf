namespace OpenNETCF.Samples.Imaging
{
    partial class ImageSelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.documentList1 = new Microsoft.WindowsCE.Forms.DocumentList();
            this.SuspendLayout();
            // 
            // documentList1
            // 
            this.documentList1.Filter = "Images|*.bmp;*.jpg;*.png;*.gif;*.tif";
            this.documentList1.Location = new System.Drawing.Point(0, 0);
            this.documentList1.Name = "documentList1";
            this.documentList1.Size = new System.Drawing.Size(240, 294);
            this.documentList1.TabIndex = 1;
            this.documentList1.DocumentActivated += new Microsoft.WindowsCE.Forms.DocumentListEventHandler(this.documentList1_DocumentActivated_1);
            // 
            // ImageSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.documentList1);
            this.Name = "ImageSelector";
            this.Text = "Image Selector";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ImageSelector_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.WindowsCE.Forms.DocumentList documentList1;
    }
}