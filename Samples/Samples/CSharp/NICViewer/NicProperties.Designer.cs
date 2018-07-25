#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



namespace NICViewer
{
  partial class NicProperties
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
      this.nicName = new System.Windows.Forms.Label();
      this.nicDescription = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.dhcpEnabled = new System.Windows.Forms.CheckBox();
      this.dhcpRenew = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.currentIP = new System.Windows.Forms.TextBox();
      this.currentNetMask = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.currentGateway = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.nicSpeed = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.nicStatus = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.ok = new System.Windows.Forms.Button();
      this.nicMAC = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.refresh = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label1.Location = new System.Drawing.Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(85, 20);
      this.label1.Text = "Name:";
      // 
      // nicName
      // 
      this.nicName.Location = new System.Drawing.Point(95, 4);
      this.nicName.Name = "nicName";
      this.nicName.Size = new System.Drawing.Size(145, 20);
      this.nicName.Text = "<Unknown>";
      // 
      // nicDescription
      // 
      this.nicDescription.Location = new System.Drawing.Point(95, 24);
      this.nicDescription.Name = "nicDescription";
      this.nicDescription.Size = new System.Drawing.Size(145, 20);
      this.nicDescription.Text = "<Unknown>";
      // 
      // label4
      // 
      this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label4.Location = new System.Drawing.Point(4, 24);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(85, 20);
      this.label4.Text = "Description:";
      // 
      // label6
      // 
      this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label6.Location = new System.Drawing.Point(5, 110);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(85, 20);
      this.label6.Text = "DHCP:";
      // 
      // dhcpEnabled
      // 
      this.dhcpEnabled.Location = new System.Drawing.Point(52, 110);
      this.dhcpEnabled.Name = "dhcpEnabled";
      this.dhcpEnabled.Size = new System.Drawing.Size(38, 20);
      this.dhcpEnabled.TabIndex = 11;
      // 
      // dhcpRenew
      // 
      this.dhcpRenew.Location = new System.Drawing.Point(96, 110);
      this.dhcpRenew.Name = "dhcpRenew";
      this.dhcpRenew.Size = new System.Drawing.Size(90, 20);
      this.dhcpRenew.TabIndex = 12;
      this.dhcpRenew.Text = "Renew";
      // 
      // label5
      // 
      this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label5.Location = new System.Drawing.Point(5, 142);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(85, 20);
      this.label5.Text = "Current IP:";
      // 
      // currentIP
      // 
      this.currentIP.Location = new System.Drawing.Point(97, 142);
      this.currentIP.Name = "currentIP";
      this.currentIP.Size = new System.Drawing.Size(141, 21);
      this.currentIP.TabIndex = 15;
      // 
      // currentNetMask
      // 
      this.currentNetMask.Location = new System.Drawing.Point(97, 169);
      this.currentNetMask.Name = "currentNetMask";
      this.currentNetMask.Size = new System.Drawing.Size(141, 21);
      this.currentNetMask.TabIndex = 17;
      // 
      // label7
      // 
      this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label7.Location = new System.Drawing.Point(5, 169);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(96, 20);
      this.label7.Text = "Subnet Mask:";
      // 
      // currentGateway
      // 
      this.currentGateway.Location = new System.Drawing.Point(97, 196);
      this.currentGateway.Name = "currentGateway";
      this.currentGateway.Size = new System.Drawing.Size(141, 21);
      this.currentGateway.TabIndex = 20;
      // 
      // label8
      // 
      this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label8.Location = new System.Drawing.Point(5, 196);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(85, 20);
      this.label8.Text = "Gateway:";
      // 
      // nicSpeed
      // 
      this.nicSpeed.Location = new System.Drawing.Point(96, 64);
      this.nicSpeed.Name = "nicSpeed";
      this.nicSpeed.Size = new System.Drawing.Size(141, 20);
      this.nicSpeed.Text = "<Unknown>";
      // 
      // label10
      // 
      this.label10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label10.Location = new System.Drawing.Point(5, 64);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(85, 20);
      this.label10.Text = "Speed:";
      // 
      // nicStatus
      // 
      this.nicStatus.Location = new System.Drawing.Point(96, 44);
      this.nicStatus.Name = "nicStatus";
      this.nicStatus.Size = new System.Drawing.Size(141, 20);
      this.nicStatus.Text = "<Unknown>";
      // 
      // label12
      // 
      this.label12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label12.Location = new System.Drawing.Point(5, 44);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(85, 20);
      this.label12.Text = "Status:";
      // 
      // ok
      // 
      this.ok.Location = new System.Drawing.Point(166, 223);
      this.ok.Name = "ok";
      this.ok.Size = new System.Drawing.Size(72, 29);
      this.ok.TabIndex = 29;
      this.ok.Text = "OK";
      this.ok.Click += new System.EventHandler(this.ok_Click);
      // 
      // nicMAC
      // 
      this.nicMAC.Location = new System.Drawing.Point(96, 84);
      this.nicMAC.Name = "nicMAC";
      this.nicMAC.Size = new System.Drawing.Size(141, 20);
      this.nicMAC.Text = "<Unknown>";
      // 
      // label3
      // 
      this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
      this.label3.Location = new System.Drawing.Point(4, 84);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(85, 20);
      this.label3.Text = "MAC:";
      // 
      // refresh
      // 
      this.refresh.Location = new System.Drawing.Point(88, 223);
      this.refresh.Name = "refresh";
      this.refresh.Size = new System.Drawing.Size(72, 29);
      this.refresh.TabIndex = 34;
      this.refresh.Text = "Refresh";
      this.refresh.Click += new System.EventHandler(this.refresh_Click);
      // 
      // NicProperties
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.refresh);
      this.Controls.Add(this.nicMAC);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ok);
      this.Controls.Add(this.nicSpeed);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.nicStatus);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.currentGateway);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.currentNetMask);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.currentIP);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.dhcpRenew);
      this.Controls.Add(this.dhcpEnabled);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.nicDescription);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.nicName);
      this.Controls.Add(this.label1);
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "NicProperties";
      this.Text = "Properties";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label nicName;
    private System.Windows.Forms.Label nicDescription;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox dhcpEnabled;
    private System.Windows.Forms.Button dhcpRenew;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox currentIP;
    private System.Windows.Forms.TextBox currentNetMask;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox currentGateway;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label nicSpeed;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label nicStatus;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Button ok;
    private System.Windows.Forms.Label nicMAC;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button refresh;
  }
}