using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.Mail;

namespace SendMailSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void send_Click(object sender, EventArgs e)
        {
            SendMessage("Test Message", "Test Body", "user@mydomain.com", "user@mydomain.com", string.Empty);
        }

        public void SendMessage(string subject, string messageBody, string fromAddress, string toAddress, string ccAddress)
        {

            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();

            MailAddress address = new MailAddress(fromAddress);

            // Set the sender's address
            message.From = address;

            // Allow multiple "To" addresses to be separated by a semi-colon
            if (toAddress.Trim().Length > 0)
            {
                foreach (string addr in toAddress.Split(';'))
                {
                    message.To.Add(new MailAddress(addr));
                }
            }

            // Allow multiple "Cc" addresses to be separated by a semi-colon
            if (ccAddress.Trim().Length > 0)
            {
                foreach (string addr in ccAddress.Split(';'))
                {
                    message.CC.Add(new MailAddress(addr));
                }
            }


            // Set the subject and message body text
            message.Subject = subject;
            message.Body = messageBody;

            // TODO: *** Modify for your SMTP server ***
            // Set the SMTP server to be used to send the message
            client.Host = "smtp.myserver.com";
            string domain = "mydomain";
            client.Credentials = new SmtpCredential("myusername", "mypassword", domain);

            // Send the e-mail message 
            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                string data = e.ToString();
            }
        }
    }
}