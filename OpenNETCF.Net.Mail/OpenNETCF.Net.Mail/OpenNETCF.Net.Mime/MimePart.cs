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
using System.IO;
using OpenNETCF.Net.Mail;
using System.Net.Sockets;

namespace OpenNETCF.Net.Mime
{
    internal class MimePart : MimeBasePart, IDisposable
    {
        // Fields
        private const int maxBufferSize = 0x4400;
        private Stream stream;
        private bool streamSet;
        private bool streamUsedOnce;

        // Methods
        internal MimePart()
        {
        }

        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Close();
            }
        }

        internal Stream GetEncodedStream(Stream stream)
        {
            Stream stream2 = stream;
            if (this.TransferEncoding == TransferEncoding.Base64)
            {
                return new Base64Stream(stream2);
            }
            if (this.TransferEncoding == TransferEncoding.QuotedPrintable)
            {
                return new QuotedPrintableStream(stream2, true);
            }
            if (this.TransferEncoding == TransferEncoding.SevenBit)
            {
                stream2 = new SevenBitStream(stream2);
            }
            return stream2;
        }

       

        internal void ResetStream()
        {
            if (this.streamUsedOnce)
            {
                if (!this.Stream.CanSeek)
                {
                    throw new InvalidOperationException(SR.GetString("MimePartCantResetStream"));
                }
                this.Stream.Seek(0L, SeekOrigin.Begin);
                this.streamUsedOnce = false;
            }
        }

        internal override void Send(MailWriter writer)
        {

            if (this.Stream != null)
            {
                int num;
                byte[] buffer = new byte[0x4400];
                writer.WriteHeaders(base.Headers);
                Stream contentStream = writer.GetContentStream();
                contentStream = this.GetEncodedStream(contentStream);
                this.ResetStream();
                this.streamUsedOnce = true;
                while ((num = this.Stream.Read(buffer, 0, 0x4400)) > 0)
                {
                    contentStream.Write(buffer, 0, num);
                }
                contentStream.Close();
            }
        }



        internal void SetContent(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (this.streamSet)
            {
                this.stream.Close();
                this.stream = null;
                this.streamSet = false;
            }
            this.stream = stream;
            this.streamSet = true;
            this.streamUsedOnce = false;
            this.TransferEncoding = TransferEncoding.Base64;
        }

        internal void SetContent(Stream stream, ContentType contentType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            base.contentType = contentType;
            this.SetContent(stream);
        }

        internal void SetContent(Stream stream, string name, string mimeType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if ((mimeType != null) && (mimeType != string.Empty))
            {
                base.contentType = new ContentType(mimeType);
            }
            if ((name != null) && (name != string.Empty))
            {
                base.ContentType.Name = name;
            }
            this.SetContent(stream);
        }

        // Properties
        internal ContentDisposition ContentDisposition
        {
            get
            {
                return base.contentDisposition;
            }
            set
            {
                base.contentDisposition = value;
                if (value == null)
                {
                    ((HeaderCollection)base.Headers).InternalRemove(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition));
                }
                else
                {
                    base.contentDisposition.PersistIfNeeded((HeaderCollection)base.Headers, true);
                }
            }
        }

        internal Stream Stream
        {
            get
            {
                return this.stream;
            }
        }

        internal TransferEncoding TransferEncoding
        {
            get
            {
                if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("base64", StringComparison.OrdinalIgnoreCase))
                {
                    return TransferEncoding.Base64;
                }
                if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("quoted-printable", StringComparison.OrdinalIgnoreCase))
                {
                    return TransferEncoding.QuotedPrintable;
                }
                if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("7bit", StringComparison.OrdinalIgnoreCase))
                {
                    return TransferEncoding.SevenBit;
                }
                return TransferEncoding.Unknown;
            }
            set
            {
                if (value == TransferEncoding.Base64)
                {
                    base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "base64";
                }
                else if (value == TransferEncoding.QuotedPrintable)
                {
                    base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "quoted-printable";
                }
                else
                {
                    if (value != TransferEncoding.SevenBit)
                    {
                        throw new NotSupportedException(SR.GetString("MimeTransferEncodingNotSupported", new object[] { value }));
                    }
                    base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "7bit";
                }
            }
        }
    }
}