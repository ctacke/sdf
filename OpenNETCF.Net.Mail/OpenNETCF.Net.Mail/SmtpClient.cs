using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace OpenNETCF.Net.Mail
{
    /// <summary>
    /// Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
    /// </summary>
    public class SmtpClient
    {
        private string m_host;
        private int m_port = 25;
        private SmtpCredential m_credentials;
        private bool m_inCall = false;

        private SmtpDeliveryMethod m_deliveryMethod = SmtpDeliveryMethod.Network;

        /// <summary>
        /// Occurs when an asynchronous e-mail send operation completes.
        /// </summary>
        public event SendCompletedEventHandler SendCompleted;
 
        /// <summary>
        /// Initializes a new instance of the SmtpClient class.
        /// </summary>
        public SmtpClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SmtpClient class that sends e-mail by using the specified SMTP server.
        /// </summary>
        /// <param name="host">A String that contains the name or IP address of the host used for SMTP transactions.</param>
        public SmtpClient(string host)
        {
            m_host = host;
        }

        /// <summary>
        /// Initializes a new instance of the SmtpClient class that sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="host">A String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An Int32 greater than zero that contains the port to be used on host</param>
        /// <exception cref="ArgumentOutOfRangeException">port cannot be less than zero.</exception>
        public SmtpClient(string host, int port)
            : this(host)
        {
            Port = port;
        }

        /// <summary>
        /// Gets or sets the name or IP address of the host used for SMTP transactions.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">The value specified for a set operation is equal to Empty ("").</exception>
        /// <exception cref="ArgumentNullException">The value specified for a set operation is null.</exception>
        public string Host
        {
            get { return m_host; }
            set
            {
                if (this.InCall)
                {
                    throw new InvalidOperationException(Resources.SmtpClientInvalidOperationDuringSend);
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value", Resources.SmtpClientHostValueNull);
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException(Resources.SmtpClientHostValueEmpty, "value");
                }
                this.m_host = value.Trim();
            }
        }

        /// <summary>
        /// An Int32 that contains the port number on the SMTP host. The default value is 25.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero.</exception>
        /// <exception cref="InvalidOperationException">You cannot change the value of this property when an email is being sent.</exception>
        public int Port
        {
            get { return m_port; }
            set
            {
                if (InCall)
                {
                    throw new InvalidOperationException(Resources.SmtpClientInvalidOperationDuringSend);
                }
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Port", Resources.SmtpClientInvalidPortNumber);
                }
                m_port = value;
            }
        }

        /// <summary>
        /// Gets or sets the credentials used to authenticate the sender.
        /// </summary>
        public SmtpCredential Credentials
        {
            get { return m_credentials; }
            set { m_credentials = value; }
        }

        
        /// <summary>
        /// Specifies how outgoing email messages will be handled.
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod
        {
            get { return m_deliveryMethod; }
            set { m_deliveryMethod = value; }
        }

        public void Send(MailMessage message)
        {
            if (this.InCall)
            {
                throw new InvalidOperationException(Resources.SmtpClientInAsyncCall);
            }

            if (message == null)
            {
                throw new ArgumentNullException("message", Resources.SmtpClientMessageNull);
            }

            if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
            {
                this.CheckHostAndPort();
            }

            MailAddressCollection recipients = new MailAddressCollection();
            if (message.From == null)
            {
                throw new InvalidOperationException(SR.GetString("SmtpFromRequired"));
            }
            if (message.To != null)
            {
                foreach (MailAddress address in message.To)
                {
                    recipients.Add(address);
                }
            }
            if (message.Bcc != null)
            {
                foreach (MailAddress address2 in message.Bcc)
                {
                    recipients.Add(address2);
                }
            }
            if (message.CC != null)
            {
                foreach (MailAddress address3 in message.CC)
                {
                    recipients.Add(address3);
                }
            }
            if (recipients.Count == 0)
            {
                throw new InvalidOperationException(SR.GetString("SmtpRecipientRequired"));
            }

            try
            {
                this.InCall = true;
                SendInternal(message);
            }
            catch (Exception exception)
            {
                throw new SmtpException(SR.GetString("SmtpSendMailFailure"), exception);
            }
            finally
            {
                this.InCall = false;
            }

        }

        private void SendInternal(MailMessage message)
        {
            if (Credentials == null)
            {
                throw new SmtpException("You must provide a Credential for the SMTP server");
            }

            SmtpCredential creds = this.Credentials;

            if (this.Host == null || this.Host.Length == 0)
            {
                throw new ArgumentException(Resources.SmtpClientHostValueInvalid);
            }

            if (message.From == null || message.From.Address == null || message.From.Address.Length == 0)
                throw new ArgumentException(Resources.SmtpClientFromNotValid);

            if (message.To.Count == 0)
                throw new ArgumentException(Resources.SmtpClientToNotValid);

            if (creds.Domain == null || creds.Domain.Length == 0)
                throw new ArgumentException(Resources.ResourceManager.FormatString(Resources.SmtpClientNetCredsDomainInvalid, creds.Domain));


            
            string sResponse = "";
            
            using (TcpClient client = new TcpClient(this.Host, this.Port))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    MailWriter mailWriter = new MailWriter(stream);
                    
                    //See if there is a response from the server on initial connection 
                    sResponse = this.ReadBuffer(stream);
                    int smtpResponse = this.GetSmtpResponse(sResponse);
                    if (smtpResponse != 220)
                    {
                        throw new SmtpException("Error connecting to Smtp server. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                    }

                    //TODO look at support HELO also
                    //Send an EHLO message 
                    this.WriteBuffer(stream, "ehlo " + creds.Domain + "\r\n");
                    sResponse = this.ReadBuffer(stream);
                    smtpResponse = this.GetSmtpResponse(sResponse);
                    if (smtpResponse != 250)
                    {
                        throw new SmtpException("Error initiating communication with Smtp server. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                    }
                    //Check if authorization is required
                    if ((sResponse.IndexOf("AUTH=LOGIN") >= 0) || (sResponse.IndexOf("AUTH LOGIN PLAIN") >= 0) || (sResponse.IndexOf("AUTH PLAIN LOGIN") >= 0))
                    {
                        this.WriteBuffer(stream, "auth login\r\n");
                        sResponse = this.ReadBuffer(stream);
                        smtpResponse = this.GetSmtpResponse(sResponse);
                        if (smtpResponse != 334)
                        {
                            throw new SmtpException("Error initiating Auth=Login. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                        }
                        this.WriteBuffer(stream, Convert.ToBase64String(Encoding.ASCII.GetBytes(creds.UserName)) + "\r\n");
                        sResponse = this.ReadBuffer(stream);
                        smtpResponse = this.GetSmtpResponse(sResponse);
                        if (smtpResponse != 334)
                        {
                            throw new SmtpException("Error setting Auth user name. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                        }
                        this.WriteBuffer(stream, Convert.ToBase64String(Encoding.ASCII.GetBytes(creds.Password)) + "\r\n");
                        sResponse = this.ReadBuffer(stream);
                        smtpResponse = this.GetSmtpResponse(sResponse);
                        if (smtpResponse != 235)
                        {
                            throw new SmtpException("Error setting Auth password. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                        }
                    }


                    //Set the from
                    this.WriteBuffer(stream, "mail from: <" + message.From.Address + ">\r\n");
                    sResponse = this.ReadBuffer(stream);
                    smtpResponse = this.GetSmtpResponse(sResponse);
                    if (smtpResponse != 250)
                    {
                        throw new SmtpException("Error setting sender email address. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                    }

                    //Set the to
                    foreach (MailAddress mailAddress in message.To)
                    {
                        this.WriteBuffer(stream, "rcpt to:<" + mailAddress.Address + ">\r\n");
                        sResponse = this.ReadBuffer(stream);
                        smtpResponse = this.GetSmtpResponse(sResponse);
                        if ((smtpResponse != 250) && (smtpResponse != 251))
                        {
                            throw new SmtpException("Error setting receipient email address. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                        }
                    }

                    //Begin writing the data
                    this.WriteBuffer(stream, "data\r\n");
                    sResponse = this.ReadBuffer(stream);
                    smtpResponse = this.GetSmtpResponse(sResponse);
                    if (smtpResponse != 354)
                    {
                        throw new SmtpException("Error starting email body. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                    }

                    //Write the content
                    message.Send(mailWriter, this.DeliveryMethod != SmtpDeliveryMethod.Network);

                    //this.WriteBuffer(stream, "from:<" + message.From.Address + ">\r\n");
                    //foreach (MailAddress mailAddress in message.To)
                    //{
                    //    this.WriteBuffer(stream, "to:<" + mailAddress.Address + ">\r\n");
                    //}
                    //this.WriteBuffer(stream, "Subject:" + message.Subject + "\r\n");
                    //if (message.IsBodyHtml)
                    //{
                    //    this.WriteBuffer(stream, "MIME-Version: 1.0\r\n");
                    //    this.WriteBuffer(stream, "Content-type: text/html\r\n");
                    //    this.WriteBuffer(stream, "\r\n" + message.Body + "\r\n.\r\n");
                    //}
                    //else
                    //{
                    //    this.WriteBuffer(stream, "\r\n" + message.Body + "\r\n.\r\n");
                    //}

                    //Complete writing the data
                    this.WriteBuffer(stream, "\r\n.\r\n");
                    sResponse = this.ReadBuffer(stream);
                    smtpResponse = this.GetSmtpResponse(sResponse);
                    if (smtpResponse != 250)
                    {
                        throw new SmtpException("Error setting email body. (code:" + smtpResponse.ToString() + " Message: " + sResponse + ")");
                    }

                    //Quit
                    this.WriteBuffer(stream, "quit\r\n");
                    sResponse = this.ReadBuffer(stream);
                }
            }

        }

        private string ReadBuffer(NetworkStream ns)
        {
            byte[] data = new byte[0x400];
            int num = 0;
            int tickCount = Environment.TickCount;
            try
            {
                //Get the response
TryAgain:
                if (ns.DataAvailable)
                {
                    int offset = 0;
                    int bytesLeft = data.Length;

                    do
                    {
                        offset += ns.Read(data, offset, bytesLeft);
                        bytesLeft = data.Length - offset;
                    }
                    while (ns.DataAvailable);
                }
                else
                {
                    if ((Environment.TickCount - tickCount) > 20000)
                    {
                        //If we reach here then no response was received from server
                        throw new SmtpException(Resources.ResourceManager.FormatString(Resources.SmtpClientHostConnectionTimeout, this.Host));
                    }
                    else
                        goto TryAgain;
                }
                
                //int num2;
                //while (ns.DataAvailable)
                //{
                //    Thread.Sleep(10);
                //    if (ns.DataAvailable)
                //    {
                //        if ((num < bytes.Length) && ns.DataAvailable)
                //        {
                //            num2 = ns.ReadByte();
                //            bytes[num++] = (byte)num2;
                //        }
                //    }

                
                //}
            }
            catch (IOException ex)
            {
                throw new SmtpException(Resources.ResourceManager.FormatString(Resources.SmtpClientHostErrorRxing,this.Host,ex.Message), ex);
            }

            string str = Encoding.ASCII.GetString(data, 0, data.Length);
            System.Diagnostics.Debug.WriteLine(str.Substring(0, str.IndexOf('\0', 0)));
            return str.Substring(0, str.IndexOf('\0', 0));
        }

        private void WriteBuffer(NetworkStream ns, string sBuffer)
        {
            try
            {
                Thread.Sleep(100);
                System.Diagnostics.Debug.WriteLine(sBuffer);
                byte[] bytes = Encoding.ASCII.GetBytes(sBuffer);
                ns.Write(bytes, 0, bytes.Length);
            }
            catch (IOException ex)
            {
                throw new SmtpException(Resources.ResourceManager.FormatString(Resources.SmtpClientHostErrorRxing, this.Host, ex.Message),ex);
            }
        }

        private int GetSmtpResponse(string sResponse)
        {
            Thread.Sleep(100);

            int num = 0;
            int index = sResponse.IndexOf(" ");
            int num3 = sResponse.IndexOf("-");
            if ((num3 > 0) && (num3 < index))
            {
                index = sResponse.IndexOf("-");
            }
            try
            {
                if (index > 0)
                {
                    num = int.Parse(sResponse.Substring(0, index));
                }
            }
            catch (Exception ex)
            {
                throw new SmtpException(Resources.ResourceManager.FormatString(Resources.SmtpClientHostErrorGetResponse, this.Host, ex.Message), ex);
            }
            return num;
        }

 

 

 

 
        /// <summary>
        /// Determines if a send operation is in progress.
        /// </summary>
        internal bool InCall
        {
            get
            {
                return this.m_inCall;
            }
            set
            {
                this.m_inCall = value;
            }
        }

        private void CheckHostAndPort()
        {
            if ((this.m_host == null) || (this.m_host.Length == 0))
            {
                throw new InvalidOperationException(Resources.SmtpClientHostValueInvalid);
            }
            if (this.Port == 0)
            {
                throw new InvalidOperationException(Resources.SmtpClientInvalidPortNumber);
            }
        }

 

 

    }

 
}
