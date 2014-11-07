using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.IO;
namespace DriveInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.listBox1.DataSource = OpenNETCF.IO.DriveInfo.GetDrives();
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            //Get the drive details
            if (this.listBox1.SelectedIndex >= 0)
            {
                OpenNETCF.IO.DriveInfo di = (OpenNETCF.IO.DriveInfo)this.listBox1.SelectedValue;
                this.AvailableFreeSpace.Text = di.AvailableFreeSpace.ToString("N0");
                this.RootDirectory.Text = di.RootDirectory.ToString();
                this.TotalFreeSpace.Text = di.TotalFreeSpace.ToString("N0");
                this.TotalSize.Text = di.TotalSize.ToString("N0");
            }
        
        }
    }
}