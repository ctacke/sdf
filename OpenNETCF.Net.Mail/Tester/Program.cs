using System;

using System.Collections.Generic;
using System.Windows.Forms;
using OpenNETCF.Net.Mail;
using System.Text;

namespace Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            string to = "marteaga@opennetcf.com";
            string from = "support@opennetcf.com";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from, "OpenNETCF Support");
            message.To.Add(new MailAddress(to,"Mark Arteaga"));
            message.Subject = "Web Email";
            message.Body = Properties.Resources.Email;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.BodyEncoding = Encoding.UTF8;
            SmtpClient client = new SmtpClient("smtp.exchange.opennetcf.com",25);
            client.Credentials = new SmtpCredential("store@opennetcf.com", "st0repwd#", "opennetcf.com");
            client.Send(message);

        }
    }
}