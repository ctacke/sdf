using System;

namespace OpenNETCF.Windows.Forms
{
    partial class ImageViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;

            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            // 
            // hScrollBar
            // 
            this.hScrollBar.Location = new System.Drawing.Point(0, 154);
            this.hScrollBar.Maximum = 91;
            this.hScrollBar.Size = new System.Drawing.Size(162, 15);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Location = new System.Drawing.Point(152, 0);
            this.vScrollBar.Maximum = 91;
            this.vScrollBar.Size = new System.Drawing.Size(15, 168);

            this.hScrollBar.ValueChanged += new EventHandler(hScrollBar_ValueChanged);
            this.vScrollBar.ValueChanged += new EventHandler(vScrollBar_ValueChanged);

            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMap_KeyDown);
            //this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMap_KeyPress);
        }

        #endregion
    }
}
