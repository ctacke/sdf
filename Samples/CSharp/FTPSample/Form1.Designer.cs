namespace FTPSample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Button btnConnect;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.TextBox txtServerCtrlResp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServerDataResp;
        private System.Windows.Forms.TextBox txtCommand;

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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.txtServerCtrlResp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServerDataResp = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 32);
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.Text = "User";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(128, 8);
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.Text = "Pwd:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(48, 32);
            this.txtServer.Size = new System.Drawing.Size(104, 20);
            this.txtServer.Text = "ftp.suse.com";
            this.txtServer.GotFocus += new System.EventHandler(this.txtBoxes_GotFocus);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(48, 8);
            this.txtUsername.Size = new System.Drawing.Size(64, 20);
            this.txtUsername.Text = "ftp";
            this.txtUsername.GotFocus += new System.EventHandler(this.txtBoxes_GotFocus);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(168, 8);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(64, 20);
            this.txtPassword.Text = "testing@opennetcf.org";
            this.txtPassword.GotFocus += new System.EventHandler(this.txtBoxes_GotFocus);
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Enabled = false;
            this.btnSendCommand.Location = new System.Drawing.Point(160, 56);
            this.btnSendCommand.Text = "Send Cmd";
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // txtServerCtrlResp
            // 
            this.txtServerCtrlResp.Location = new System.Drawing.Point(8, 184);
            this.txtServerCtrlResp.Multiline = true;
            this.txtServerCtrlResp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerCtrlResp.Size = new System.Drawing.Size(224, 80);
            this.txtServerCtrlResp.Text = "";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 168);
            this.label4.Size = new System.Drawing.Size(152, 16);
            this.label4.Text = "Server Control Response";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(160, 32);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 87);
            this.label5.Size = new System.Drawing.Size(152, 16);
            this.label5.Text = "Server Data Response";
            // 
            // txtServerDataResp
            // 
            this.txtServerDataResp.Location = new System.Drawing.Point(8, 103);
            this.txtServerDataResp.Multiline = true;
            this.txtServerDataResp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerDataResp.Size = new System.Drawing.Size(224, 57);
            this.txtServerDataResp.Text = "";
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(8, 56);
            this.txtCommand.Size = new System.Drawing.Size(144, 20);
            this.txtCommand.Text = "";
            this.txtCommand.GotFocus += new System.EventHandler(this.txtBoxes_GotFocus);
            // 
            // Form1
            // 
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtServerDataResp);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtServerCtrlResp);
            this.Controls.Add(this.btnSendCommand);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Text = "Ftp Commander";

        }
        #endregion
    }
}

