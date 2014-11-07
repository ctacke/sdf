namespace SettingsBrowser
{
    partial class SettingForm
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
			this.txtSettingName = new System.Windows.Forms.TextBox();
			this.txtSettingVal = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new OpenNETCF.Windows.Forms.GroupBox();
			this.typeBool = new System.Windows.Forms.RadioButton();
			this.typeFloat = new System.Windows.Forms.RadioButton();
			this.typeString = new System.Windows.Forms.RadioButton();
			this.typeInt = new System.Windows.Forms.RadioButton();
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.Text = "Setting Name";
			// 
			// txtSettingName
			// 
			this.txtSettingName.Location = new System.Drawing.Point(93, 23);
			this.txtSettingName.MaxLength = 15;
			this.txtSettingName.Name = "txtSettingName";
			this.txtSettingName.Size = new System.Drawing.Size(100, 21);
			this.txtSettingName.TabIndex = 1;
			this.txtSettingName.Text = "Setting Name";
			this.txtSettingName.TextChanged += new System.EventHandler(this.txtSettingVal_TextChanged);
			// 
			// txtSettingVal
			// 
			this.txtSettingVal.Location = new System.Drawing.Point(93, 50);
			this.txtSettingVal.MaxLength = 15;
			this.txtSettingVal.Name = "txtSettingVal";
			this.txtSettingVal.Size = new System.Drawing.Size(100, 21);
			this.txtSettingVal.TabIndex = 3;
			this.txtSettingVal.Text = "Value";
			this.txtSettingVal.TextChanged += new System.EventHandler(this.txtSettingVal_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.Text = "Setting Value";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.typeBool);
			this.groupBox1.Controls.Add(this.typeFloat);
			this.groupBox1.Controls.Add(this.typeString);
			this.groupBox1.Controls.Add(this.typeInt);
			this.groupBox1.Location = new System.Drawing.Point(6, 77);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(234, 83);
			this.groupBox1.Text = "Setting Type";
			// 
			// typeBool
			// 
			this.typeBool.Location = new System.Drawing.Point(71, 47);
			this.typeBool.Name = "typeBool";
			this.typeBool.Size = new System.Drawing.Size(100, 20);
			this.typeBool.TabIndex = 7;
			this.typeBool.TabStop = false;
			this.typeBool.Text = "bool";
			// 
			// typeFloat
			// 
			this.typeFloat.Location = new System.Drawing.Point(71, 21);
			this.typeFloat.Name = "typeFloat";
			this.typeFloat.Size = new System.Drawing.Size(100, 20);
			this.typeFloat.TabIndex = 7;
			this.typeFloat.TabStop = false;
			this.typeFloat.Text = "float";
			// 
			// typeString
			// 
			this.typeString.Checked = true;
			this.typeString.Location = new System.Drawing.Point(3, 47);
			this.typeString.Name = "typeString";
			this.typeString.Size = new System.Drawing.Size(62, 20);
			this.typeString.TabIndex = 1;
			this.typeString.Text = "string";
			// 
			// typeInt
			// 
			this.typeInt.Location = new System.Drawing.Point(3, 21);
			this.typeInt.Name = "typeInt";
			this.typeInt.Size = new System.Drawing.Size(62, 20);
			this.typeInt.TabIndex = 0;
			this.typeInt.TabStop = false;
			this.typeInt.Text = "int";
			// 
			// ok
			// 
			this.ok.Enabled = false;
			this.ok.Location = new System.Drawing.Point(168, 166);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(72, 20);
			this.ok.TabIndex = 7;
			this.ok.Text = "OK";
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel
			// 
			this.cancel.Location = new System.Drawing.Point(90, 166);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 20);
			this.cancel.TabIndex = 8;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// SettingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtSettingVal);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtSettingName);
			this.Controls.Add(this.label1);
			this.KeyPreview = true;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "SettingForm";
			this.Text = "Create / Edit Setting";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSettingName;
        private System.Windows.Forms.TextBox txtSettingVal;
        private System.Windows.Forms.Label label2;
        private OpenNETCF.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton typeInt;
        private System.Windows.Forms.RadioButton typeBool;
        private System.Windows.Forms.RadioButton typeFloat;
        private System.Windows.Forms.RadioButton typeString;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
    }
}