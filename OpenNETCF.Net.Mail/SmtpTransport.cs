using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mail
{
    internal class SmtpTransport
    {
        // Fields
        private SmtpClient client;
        private SmtpConnection connection;
        private ICredentialsByHost credentials;
        internal const int DefaultPort = 0x19;
        private bool enableSsl;
        private ArrayList failedRecipientExceptions;
        private bool m_IdentityRequired;
        private int timeout;

        // Methods
        internal SmtpTransport(SmtpClient client)
            : this(client, SmtpAuthenticationManager.GetModules())
        {
        }

        internal SmtpTransport(SmtpClient client, ISmtpAuthenticationModule[] authenticationModules)
        {
            this.timeout = 0x186a0;
            this.failedRecipientExceptions = new ArrayList();
            this.client = client;
            if (authenticationModules == null)
            {
                throw new ArgumentNullException("authenticationModules");
            }
            this.authenticationModules = authenticationModules;
        }

        internal void Abort()
        {
            if (this.connection != null)
            {
                this.connection.Abort();
            }
        }

        internal void GetConnection(string host, int port)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if ((port < 0) || (port > 0xffff))
            {
                throw new ArgumentOutOfRangeException("port");
            }
            this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
            this.connection.Timeout = this.timeout;
            if (Logging.On)
            {
                Logging.Associate(Logging.Web, this, this.connection);
            }
            if (this.EnableSsl)
            {
                this.connection.EnableSsl = true;
                this.connection.ClientCertificates = this.ClientCertificates;
            }
            this.connection.GetConnection(host, port);
        }

        internal void ReleaseConnection()
        {
            if (this.connection != null)
            {
                this.connection.ReleaseConnection();
            }
        }

        internal MailWriter SendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, out SmtpFailedRecipientException exception)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }
            if (recipients == null)
            {
                throw new ArgumentNullException("recipients");
            }
            MailCommand.Send(this.connection, SmtpCommands.Mail, sender.SmtpAddress);
            this.failedRecipientExceptions.Clear();
            exception = null;
            foreach (MailAddress address in recipients)
            {
                string str;
                if (!RecipientCommand.Send(this.connection, this.connection.DSNEnabled ? (address.SmtpAddress + deliveryNotify) : address.SmtpAddress, out str))
                {
                    this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, address.SmtpAddress, str));
                }
            }
            if (this.failedRecipientExceptions.Count > 0)
            {
                if (this.failedRecipientExceptions.Count == 1)
                {
                    exception = (SmtpFailedRecipientException)this.failedRecipientExceptions[0];
                }
                else
                {
                    exception = new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == recipients.Count);
                }
                if (this.failedRecipientExceptions.Count == recipients.Count)
                {
                    exception.fatal = true;
                    throw exception;
                }
            }
            DataCommand.Send(this.connection);
            return new MailWriter(this.connection.GetClosableStream());
        }

        internal ICredentialsByHost Credentials
        {
            get
            {
                return this.credentials;
            }
            set
            {
                this.credentials = value;
            }
        }

        internal bool EnableSsl
        {
            get
            {
                return this.enableSsl;
            }
            set
            {
                this.enableSsl = value;
            }
        }

        internal bool IdentityRequired
        {
            get
            {
                return this.m_IdentityRequired;
            }
            set
            {
                this.m_IdentityRequired = value;
            }
        }

        internal bool IsConnected
        {
            get
            {
                return ((this.connection != null) && this.connection.IsConnected);
            }
        }

        internal int Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.timeout = value;
            }
        }
    }
}