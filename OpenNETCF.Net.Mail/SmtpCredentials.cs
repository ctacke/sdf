using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.Mail
{
    /// <summary>
    /// Credentials to use for SmtpClient
    /// </summary>
    public class SmtpCredential : NetworkCredential
    {
        /// <summary>
        /// Creates a new instance of the SmtpCredential class
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public SmtpCredential(string userName, string password, string domain) : base(userName, password, domain){}
        
    }
}
