using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;

namespace SendKeysSample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            input.Text = "1234567890!@#$%^&*()";
        }

        private void send_Click(object sender, EventArgs e)
        {
            try
            {
                output.Text = string.Empty;
                output.Focus();
                // The plus sign (+), caret (^), percent sign (%), tilde (~), and parentheses () have special 
                // meanings to SendKeys. To specify one of these characters, enclose it within braces ({}).
                string toSend = input.Text.Replace("^", "{^}");
                toSend = toSend.Replace("%", "{%}");
                toSend = toSend.Replace("(", "{(}");
                toSend = toSend.Replace(")", "{)}");

                SendKeys.Send(toSend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:\r\n" + ex.Message);
            }
        }
    }
}