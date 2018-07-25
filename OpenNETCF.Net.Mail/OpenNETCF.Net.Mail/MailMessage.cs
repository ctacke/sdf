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



using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using OpenNETCF.Net.Mime;
using System.IO;

namespace OpenNETCF.Net.Mail
{
    public class MailMessage : IDisposable
    {
        //TODO Support for attachments
        //TODO Support for alternateView (don't know if this is neeeded)
        // Fields
        private AttachmentCollection attachments;
        private string body;
        private Encoding bodyEncoding;
        private AlternateView bodyView;
        private DeliveryNotificationOptions deliveryStatusNotification;
        private bool disposed;
        private bool isBodyHtml;
        private Message message;
        //private AlternateViewCollection views;

        // Methods
        public MailMessage()
        {
            this.body = string.Empty;
            this.message = new Message();
        }

        public MailMessage(MailAddress from, MailAddress to)
        {
            this.body = string.Empty;
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }
            this.message = new Message(from, to);
        }

        public MailMessage(string from, string to)
        {
            this.body = string.Empty;
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }
            if (from == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "from" }), "from");
            }
            if (to == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "to" }), "to");
            }
            this.message = new Message(from, to);
        }

        public MailMessage(string from, string to, string subject, string body)
            : this(from, to)
        {
            this.Subject = subject;
            this.Body = body;
        }

        //internal IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
        //{
        //    this.SetContent();
        //    return this.message.BeginSend(writer, sendEnvelope, callback, state);
        //}

        internal string BuildDeliveryStatusNotificationString()
        {
            if (this.deliveryStatusNotification == DeliveryNotificationOptions.None)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder(" NOTIFY=");
            bool flag = false;
            if (this.deliveryStatusNotification == DeliveryNotificationOptions.Never)
            {
                builder.Append("NEVER");
                return builder.ToString();
            }
            if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnSuccess) > DeliveryNotificationOptions.None)
            {
                builder.Append("SUCCESS");
                flag = true;
            }
            if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnFailure) > DeliveryNotificationOptions.None)
            {
                if (flag)
                {
                    builder.Append(",");
                }
                builder.Append("FAILURE");
                flag = true;
            }
            if ((this.deliveryStatusNotification & DeliveryNotificationOptions.Delay) > DeliveryNotificationOptions.None)
            {
                if (flag)
                {
                    builder.Append(",");
                }
                builder.Append("DELAY");
            }
            return builder.ToString();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.disposed = true;
                //if (this.views != null)
                //{
                //    this.views.Dispose();
                //}
                if (this.attachments != null)
                {
                    this.attachments.Dispose();
                }
                //if (this.bodyView != null)
                //{
                //    this.bodyView.Dispose();
                //}
            }
        }

        //internal void EndSend(IAsyncResult asyncResult)
        //{
        //    this.message.EndSend(asyncResult);
        //}

        internal void Send(MailWriter writer, bool sendEnvelope)
        {
            this.SetContent();
            this.message.Send(writer, sendEnvelope);
        }

        private void SetContent()
        {
            if (this.bodyView != null)
            {
                this.bodyView.Dispose();
                this.bodyView = null;
            }
            //TODO AlternateViews and Attachments are not supported currently
            //if ((this.AlternateViews.Count == 0) && (this.Attachments.Count == 0))
            //{
            if ((this.body != null) && (this.body != string.Empty))
            {
                this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
                this.message.Content = this.bodyView.MimePart;
            }
            //}
            //else if ((this.AlternateViews.Count == 0) && (this.Attachments.Count > 0))
            //{
            //    MimeMultiPart part = new MimeMultiPart(MimeMultiPartType.Mixed);
            //    if ((this.body != null) && (this.body != string.Empty))
            //    {
            //        this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
            //    }
            //    else
            //    {
            //        this.bodyView = AlternateView.CreateAlternateViewFromString(string.Empty);
            //    }
            //    part.Parts.Add(this.bodyView.MimePart);
            //    foreach (Attachment attachment in this.Attachments)
            //    {
            //        if (attachment != null)
            //        {
            //            attachment.PrepareForSending();
            //            part.Parts.Add(attachment.MimePart);
            //        }
            //    }
            //    this.message.Content = part;
            //}
            //else
            //{
            //    MimeMultiPart part2 = null;
            //    MimeMultiPart item = new MimeMultiPart(MimeMultiPartType.Alternative);
            //    if ((this.body != null) && (this.body != string.Empty))
            //    {
            //        this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, null);
            //        item.Parts.Add(this.bodyView.MimePart);
            //    }
            //    foreach (AlternateView view in this.AlternateViews)
            //    {
            //        if (view == null)
            //        {
            //            continue;
            //        }
            //        view.PrepareForSending();
            //        if (view.LinkedResources.Count > 0)
            //        {
            //            MimeMultiPart part4 = new MimeMultiPart(MimeMultiPartType.Related);
            //            part4.ContentType.Parameters["type"] = view.ContentType.MediaType;
            //            part4.ContentLocation = view.MimePart.ContentLocation;
            //            part4.Parts.Add(view.MimePart);
            //            foreach (LinkedResource resource in view.LinkedResources)
            //            {
            //                resource.PrepareForSending();
            //                part4.Parts.Add(resource.MimePart);
            //            }
            //            item.Parts.Add(part4);
            //            continue;
            //        }
            //        item.Parts.Add(view.MimePart);
            //    }
            //    if (this.Attachments.Count > 0)
            //    {
            //        part2 = new MimeMultiPart(MimeMultiPartType.Mixed);
            //        part2.Parts.Add(item);
            //        MimeMultiPart part5 = new MimeMultiPart(MimeMultiPartType.Mixed);
            //        foreach (Attachment attachment2 in this.Attachments)
            //        {
            //            if (attachment2 != null)
            //            {
            //                attachment2.PrepareForSending();
            //                part5.Parts.Add(attachment2.MimePart);
            //            }
            //        }
            //        part2.Parts.Add(part5);
            //        this.message.Content = part2;
            //    }
            //    else if ((item.Parts.Count == 1) && ((this.body == null) || (this.body == string.Empty)))
            //    {
            //        this.message.Content = item.Parts[0];
            //    }
            //    else
            //    {
            //        this.message.Content = item;
            //    }
            //}
        }

        // Properties
        //public AlternateViewCollection AlternateViews
        //{
        //    get
        //    {
        //        if (this.disposed)
        //        {
        //            throw new ObjectDisposedException(base.GetType().FullName);
        //        }
        //        if (this.views == null)
        //        {
        //            this.views = new AlternateViewCollection();
        //        }
        //        return this.views;
        //    }
        //}

        public AttachmentCollection Attachments
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(base.GetType().FullName);
                }
                if (this.attachments == null)
                {
                    this.attachments = new AttachmentCollection();
                }
                return this.attachments;
            }
        }

        public MailAddressCollection Bcc
        {
            get
            {
                return this.message.Bcc;
            }
        }

        public string Body
        {
            get
            {
                if (this.body == null)
                {
                    return string.Empty;
                }
                return this.body;
            }
            set
            {
                this.body = value;
                if ((this.bodyEncoding == null) && (this.body != null))
                {
                    if (MimeBasePart.IsAscii(this.body, true))
                    {
                        this.bodyEncoding = Encoding.ASCII;
                    }
                    else
                    {
                        this.bodyEncoding = Encoding.GetEncoding("utf-8");
                    }
                }
            }
        }

        public Encoding BodyEncoding
        {
            get
            {
                return this.bodyEncoding;
            }
            set
            {
                this.bodyEncoding = value;
            }
        }

        public MailAddressCollection CC
        {
            get
            {
                return this.message.CC;
            }
        }

        public DeliveryNotificationOptions DeliveryNotificationOptions
        {
            get
            {
                return this.deliveryStatusNotification;
            }
            set
            {
                if (((DeliveryNotificationOptions.Delay | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess) < value) && (value != DeliveryNotificationOptions.Never))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.deliveryStatusNotification = value;
            }
        }

        public MailAddress From
        {
            get
            {
                return this.message.From;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.message.From = value;
            }
        }

        public NameValueCollection Headers
        {
            get
            {
                return this.message.Headers;
            }
        }

        public bool IsBodyHtml
        {
            get
            {
                return this.isBodyHtml;
            }
            set
            {
                this.isBodyHtml = value;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return this.message.Priority;
            }
            set
            {
                this.message.Priority = value;
            }
        }

        public MailAddress ReplyTo
        {
            get
            {
                return this.message.ReplyTo;
            }
            set
            {
                this.message.ReplyTo = value;
            }
        }

        public MailAddress Sender
        {
            get
            {
                return this.message.Sender;
            }
            set
            {
                this.message.Sender = value;
            }
        }

        public string Subject
        {
            get
            {
                if (this.message.Subject == null)
                {
                    return string.Empty;
                }
                return this.message.Subject;
            }
            set
            {
                this.message.Subject = value;
            }
        }

        public Encoding SubjectEncoding
        {
            get
            {
                return this.message.SubjectEncoding;
            }
            set
            {
                this.message.SubjectEncoding = value;
            }
        }

        public MailAddressCollection To
        {
            get
            {
                return this.message.To;
            }
        }
    }

    internal class Message
    {
        // Fields
        private MailAddressCollection bcc;
        private MailAddressCollection cc;
        private MimeBasePart content;
        private HeaderCollection envelopeHeaders;
        private MailAddress from;
        private HeaderCollection headers;
        private MailPriority priority;
        private MailAddress replyTo;
        private MailAddress sender;
        private string subject;
        private Encoding subjectEncoding;
        private MailAddressCollection to;

        // Methods
        internal Message()
        {
            this.priority = ~MailPriority.Normal;
        }

        internal Message(MailAddress from, MailAddress to)
            : this()
        {
            this.from = from;
            this.To.Add(to);
        }

        internal Message(string from, string to)
            : this()
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }
            if (from == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "from" }), "from");
            }
            if (to == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "to" }), "to");
            }
            this.from = new MailAddress(from);
            MailAddressCollection addresss = new MailAddressCollection();
            addresss.Add(to);
            this.to = addresss;
        }

        //internal virtual IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
        //{
        //    this.PrepareHeaders(sendEnvelope);
        //    writer.WriteHeaders(this.Headers);
        //    if (this.Content != null)
        //    {
        //        return this.Content.BeginSend(writer, callback, state);
        //    }
        //    LazyAsyncResult result = new LazyAsyncResult(this, state, callback);
        //    IAsyncResult result2 = writer.BeginGetContentStream(new AsyncCallback(this.EmptySendCallback), new EmptySendContext(writer, result));
        //    if (result2.CompletedSynchronously)
        //    {
        //        writer.EndGetContentStream(result2).Close();
        //    }
        //    return result;
        //}

        //internal void EmptySendCallback(IAsyncResult result)
        //{
        //    Exception exception = null;
        //    if (!result.CompletedSynchronously)
        //    {
        //        EmptySendContext asyncState = (EmptySendContext)result.AsyncState;
        //        try
        //        {
        //            asyncState.writer.EndGetContentStream(result).Close();
        //        }
        //        catch (Exception exception2)
        //        {
        //            exception = exception2;
        //        }
        //        catch
        //        {
        //            exception = new Exception(SR.GetString("net_nonClsCompliantException"));
        //        }
        //        asyncState.result.InvokeCallback(exception);
        //    }
        //}

        //internal virtual void EndSend(IAsyncResult asyncResult)
        //{
        //    if (asyncResult == null)
        //    {
        //        throw new ArgumentNullException("asyncResult");
        //    }
        //    if (this.Content != null)
        //    {
        //        this.Content.EndSend(asyncResult);
        //    }
        //    else
        //    {
        //        LazyAsyncResult result = asyncResult as LazyAsyncResult;
        //        if ((result == null) || (result.AsyncObject != this))
        //        {
        //            throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"));
        //        }
        //        if (result.EndCalled)
        //        {
        //            throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndSend" }));
        //        }
        //        result.InternalWaitForCompletion();
        //        result.EndCalled = true;
        //        if (result.Result is Exception)
        //        {
        //            throw ((Exception)result.Result);
        //        }
        //    }
        //}

        internal void PrepareEnvelopeHeaders(bool sendEnvelope)
        {
            this.EnvelopeHeaders[MailHeaderInfo.GetString(MailHeaderID.XSender)] = this.From.ToEncodedString();
            this.EnvelopeHeaders.Remove(MailHeaderInfo.GetString(MailHeaderID.XReceiver));
            foreach (MailAddress address in this.To)
            {
                this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), address.ToEncodedString());
            }
            foreach (MailAddress address2 in this.CC)
            {
                this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), address2.ToEncodedString());
            }
            foreach (MailAddress address3 in this.Bcc)
            {
                this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), address3.ToEncodedString());
            }
        }

        internal void PrepareHeaders(bool sendEnvelope)
        {
            this.Headers[MailHeaderInfo.GetString(MailHeaderID.MimeVersion)] = "1.0";
            this.Headers[MailHeaderInfo.GetString(MailHeaderID.From)] = this.From.ToEncodedString();
            if (this.Sender != null)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Sender)] = this.Sender.ToEncodedString();
            }
            else
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Sender));
            }
            if (this.To.Count > 0)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.To)] = this.To.ToEncodedString();
            }
            else
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.To));
            }
            if (this.CC.Count > 0)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Cc)] = this.CC.ToEncodedString();
            }
            else
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Cc));
            }
            if (this.replyTo != null)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.ReplyTo)] = this.ReplyTo.ToEncodedString();
            }
            else
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ReplyTo));
            }
            if (this.priority == MailPriority.High)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "1";
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "urgent";
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "high";
            }
            else if (this.priority == MailPriority.Low)
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "5";
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "non-urgent";
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "low";
            }
            else if (this.priority != ~MailPriority.Normal)
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.XPriority));
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Priority));
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Importance));
            }
            this.Headers[MailHeaderInfo.GetString(MailHeaderID.Date)] = MailBnfHelper.GetDateTimeString(DateTime.Now, null);
            if ((this.subject != null) && (this.subject != string.Empty))
            {
                this.Headers[MailHeaderInfo.GetString(MailHeaderID.Subject)] = MimeBasePart.EncodeHeaderValue(this.subject, this.subjectEncoding, MimeBasePart.ShouldUseBase64Encoding(this.subjectEncoding));
            }
            else
            {
                this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Subject));
            }
        }

        internal virtual void Send(MailWriter writer, bool sendEnvelope)
        {
            if (sendEnvelope)
            {
                this.PrepareEnvelopeHeaders(sendEnvelope);
                writer.WriteHeaders(this.EnvelopeHeaders);
            }
            this.PrepareHeaders(sendEnvelope);
            writer.WriteHeaders(this.Headers);
            if (this.Content != null)
            {
                this.Content.Send(writer);
            }
            else
            {
                writer.GetContentStream().Close();
            }
        }

        internal void WriteHeaders(NameValueCollection headers)
        {

        }

        // Properties
        internal MailAddressCollection Bcc
        {
            get
            {
                if (this.bcc == null)
                {
                    this.bcc = new MailAddressCollection();
                }
                return this.bcc;
            }
        }

        internal MailAddressCollection CC
        {
            get
            {
                if (this.cc == null)
                {
                    this.cc = new MailAddressCollection();
                }
                return this.cc;
            }
        }

        internal virtual MimeBasePart Content
        {
            get
            {
                return this.content;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.content = value;
            }
        }

        internal NameValueCollection EnvelopeHeaders
        {
            get
            {
                if (this.envelopeHeaders == null)
                {
                    this.envelopeHeaders = new HeaderCollection();
                }
                return this.envelopeHeaders;
            }
        }

        internal MailAddress From
        {
            get
            {
                return this.from;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.from = value;
            }
        }

        internal NameValueCollection Headers
        {
            get
            {
                if (this.headers == null)
                {
                    this.headers = new HeaderCollection();
                }
                return this.headers;
            }
        }

        public MailPriority Priority
        {
            get
            {
                if (this.priority != ~MailPriority.Normal)
                {
                    return this.priority;
                }
                return MailPriority.Normal;
            }
            set
            {
                this.priority = value;
            }
        }

        internal MailAddress ReplyTo
        {
            get
            {
                return this.replyTo;
            }
            set
            {
                this.replyTo = value;
            }
        }

        internal MailAddress Sender
        {
            get
            {
                return this.sender;
            }
            set
            {
                this.sender = value;
            }
        }

        internal string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                if ((value != null) && MailBnfHelper.HasCROrLF(value))
                {
                    throw new ArgumentException(SR.GetString("MailSubjectInvalidFormat"));
                }
                this.subject = value;
                if (((this.subject != null) && (this.subjectEncoding == null)) && !MimeBasePart.IsAscii(this.subject, false))
                {
                    this.subjectEncoding = Encoding.GetEncoding("utf-8");
                }
            }
        }

        internal Encoding SubjectEncoding
        {
            get
            {
                return this.subjectEncoding;
            }
            set
            {
                this.subjectEncoding = value;
            }
        }

        internal MailAddressCollection To
        {
            get
            {
                if (this.to == null)
                {
                    this.to = new MailAddressCollection();
                }
                return this.to;
            }
        }

        // Nested Types
        //internal class EmptySendContext
        //{
        //    // Fields
        //    internal LazyAsyncResult result;
        //    internal BaseWriter writer;

        //    // Methods
        //    internal EmptySendContext(BaseWriter writer, LazyAsyncResult result)
        //    {
        //        this.writer = writer;
        //        this.result = result;
        //    }
        //}
    }
}
