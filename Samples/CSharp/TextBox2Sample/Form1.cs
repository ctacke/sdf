using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Win32;
using Microsoft.WindowsCE.Forms;

namespace TextBox2Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = "lorem ipsum";
            textBox1.GotFocus += new EventHandler(textBox1_GotFocus);

            textBox2.Text = "lorem ipsum";
            textBox2.MouseDown += new EventHandler(textBox2_MouseDown);
        }

        void textBox2_MouseDown(object sender, EventArgs e)
        {
            textBox2.SelectAll();
        }

        void textBox1_GotFocus(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }
    }
}