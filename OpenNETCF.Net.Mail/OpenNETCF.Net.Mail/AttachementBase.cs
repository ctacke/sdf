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
using OpenNETCF.Net.Mime;

namespace OpenNETCF.Net.Mail
{
    public abstract class AttachmentBase : IDisposable
    {
        // Fields
        internal bool disposed;
        private MimePart part;

        // Methods
        internal AttachmentBase()
        {
            this.part = new MimePart();
        }

        protected AttachmentBase(Stream contentStream)
        {
            this.part = new MimePart();
            this.part.SetContent(contentStream);
        }

        protected AttachmentBase(string fileName)
        {
            this.part = new MimePart();
            this.SetContentFromFile(fileName, string.Empty);
        }

        protected AttachmentBase(Stream contentStream, ContentType contentType)
        {
            this.part = new MimePart();
            this.part.SetContent(contentStream, contentType);
        }

        protected AttachmentBase(Stream contentStream, string mediaType)
        {
            this.part = new MimePart();
            this.part.SetContent(contentStream, null, mediaType);
        }

        protected AttachmentBase(string fileName, ContentType contentType)
        {
            this.part = new MimePart();
            this.SetContentFromFile(fileName, contentType);
        }

        protected AttachmentBase(string fileName, string mediaType)
        {
            this.part = new MimePart();
            this.SetContentFromFile(fileName, mediaType);
        }

        internal AttachmentBase(Stream contentStream, string name, string mediaType)
        {
            this.part = new MimePart();
            this.part.SetContent(contentStream, name, mediaType);
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
                this.part.Dispose();
            }
        }

        internal virtual void PrepareForSending()
        {
            this.part.ResetStream();
        }

        internal void SetContentFromFile(string fileName, ContentType contentType)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
            }
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.part.SetContent(stream, contentType);
        }

        internal void SetContentFromFile(string fileName, string mediaType)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
            }
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.part.SetContent(stream, null, mediaType);
        }

        internal void SetContentFromString(string contentString, ContentType contentType)
        {
            Encoding aSCII;
            if (contentString == null)
            {
                throw new ArgumentNullException("content");
            }
            if (this.part.Stream != null)
            {
                this.part.Stream.Close();
            }
            if ((contentType != null) && (contentType.CharSet != null))
            {
                aSCII = Encoding.GetEncoding(contentType.CharSet);
            }
            else if (MimeBasePart.IsAscii(contentString, false))
            {
                aSCII = Encoding.ASCII;
            }
            else
            {
                aSCII = Encoding.GetEncoding("utf-8");
            }
            byte[] bytes = aSCII.GetBytes(contentString);
            this.part.SetContent(new MemoryStream(bytes), contentType);
            if (MimeBasePart.ShouldUseBase64Encoding(aSCII))
            {
                this.part.TransferEncoding = TransferEncoding.Base64;
            }
            else
            {
                this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
            }
        }

        internal void SetContentFromString(string contentString, Encoding encoding, string mediaType)
        {
            if (contentString == null)
            {
                throw new ArgumentNullException("content");
            }
            if (this.part.Stream != null)
            {
                this.part.Stream.Close();
            }
            if ((mediaType == null) || (mediaType == string.Empty))
            {
                mediaType = "text/plain";
            }
            int offset = 0;
            try
            {
                if (((MailBnfHelper.ReadToken(mediaType, ref offset, null).Length == 0) || (offset >= mediaType.Length)) || (mediaType[offset++] != '/'))
                {
                    throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
                }
                if ((MailBnfHelper.ReadToken(mediaType, ref offset, null).Length == 0) || (offset < mediaType.Length))
                {
                    throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
                }
            }
            catch (FormatException)
            {
                throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
            }
            ContentType contentType = new ContentType(mediaType);
            if (encoding == null)
            {
                if (MimeBasePart.IsAscii(contentString, false))
                {
                    encoding = Encoding.ASCII;
                }
                else
                {
                    encoding = Encoding.GetEncoding("utf-8");
                }
            }
            contentType.CharSet = encoding.BodyName();
            byte[] bytes = encoding.GetBytes(contentString);
            this.part.SetContent(new MemoryStream(bytes), contentType);
            if (MimeBasePart.ShouldUseBase64Encoding(encoding))
            {
                this.part.TransferEncoding = TransferEncoding.Base64;
            }
            else
            {
                this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
            }
        }

        internal static string ShortNameFromFile(string fileName)
        {
            int num = fileName.LastIndexOfAny(new char[] { '\\', ':' }, fileName.Length - 1, fileName.Length);
            if (num > 0)
            {
                return fileName.Substring(num + 1, (fileName.Length - num) - 1);
            }
            return fileName;
        }

        // Properties
        public string ContentId
        {
            get
            {
                string contentID = this.part.ContentID;
                if (string.IsNullOrEmpty(contentID))
                {
                    contentID = Guid.NewGuid().ToString();
                    this.ContentId = contentID;
                    return contentID;
                }
                if (((contentID.Length >= 2) && (contentID[0] == '<')) && (contentID[contentID.Length - 1] == '>'))
                {
                    return contentID.Substring(1, contentID.Length - 2);
                }
                return contentID;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.part.ContentID = null;
                }
                else
                {
                    if (value.IndexOfAny(new char[] { '<', '>' }) != -1)
                    {
                        throw new ArgumentException(SR.GetString("MailHeaderInvalidCID"), "value");
                    }
                    this.part.ContentID = "<" + value + ">";
                }
            }
        }

        internal Uri ContentLocation
        {
            get
            {
                Uri uri;
                if (!Uri.TryCreate(this.part.ContentLocation, UriKind.RelativeOrAbsolute, out uri))
                {
                    return null;
                }
                return uri;
            }
            set
            {
                this.part.ContentLocation = (value == null) ? null : (value.IsAbsoluteUri ? value.AbsoluteUri : value.OriginalString);
            }
        }

        public Stream ContentStream
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(base.GetType().FullName);
                }
                return this.part.Stream;
            }
        }

        public ContentType ContentType
        {
            get
            {
                return this.part.ContentType;
            }
            set
            {
                this.part.ContentType = value;
            }
        }

        internal MimePart MimePart
        {
            get
            {
                return this.part;
            }
        }

        public TransferEncoding TransferEncoding
        {
            get
            {
                return this.part.TransferEncoding;
            }
            set
            {
                this.part.TransferEncoding = value;
            }
        }
    }
}