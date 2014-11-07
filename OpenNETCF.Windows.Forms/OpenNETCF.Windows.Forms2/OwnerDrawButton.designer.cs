namespace OpenNETCF.Windows.Forms
{
    partial class OwnerDrawButton
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
                if (m_hwndControl != System.IntPtr.Zero)
                    NativeMethods.DestroyWindow(m_hwndControl);
                m_hwndControl = System.IntPtr.Zero;
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
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(136, 27);
            this.button.TabIndex = 0;
            this.button.Text = "button";
            // 
            // CustomDrawButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.button);
            this.Name = "CustomDrawButton";
            this.Size = new System.Drawing.Size(136, 27);
            this.ResumeLayout(false);
            this.Text = "Hugh";
        }

        #endregion

        private System.Windows.Forms.Button button;

    }
}
