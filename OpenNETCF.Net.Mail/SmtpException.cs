using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mail
{
    public class SmtpException : ApplicationException
    {
        // Methods
        public SmtpException(string message)
            : base(message)
        {
        }
          public SmtpException(string message, Exception innerexception)
            : base(message,innerexception)
        {
        }
    }

 

}
