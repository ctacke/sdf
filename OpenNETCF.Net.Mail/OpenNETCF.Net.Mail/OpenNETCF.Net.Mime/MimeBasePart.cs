using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using OpenNETCF.Net.Mail;

namespace OpenNETCF.Net.Mime
{
    internal class MimeBasePart
    {

        // Fields
        protected ContentDisposition contentDisposition;
        protected ContentType contentType;
        internal const string defaultCharSet = "utf-8";
        private HeaderCollection headers;

        // Methods
        internal MimeBasePart()
        {
        }

        internal virtual void Send(MailWriter writer)
        {
            throw new NotImplementedException();
        }

        // Properties
        internal string ContentID
        {
            get
            {
                return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentID));
                }
                else
                {
                    this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)] = value;
                }
            }
        }

        internal string ContentLocation
        {
            get
            {
                return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentLocation));
                }
                else
                {
                    this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)] = value;
                }
            }
        }

        internal ContentType ContentType
        {
            get
            {
                if (this.contentType == null)
                {
                    this.contentType = new ContentType();
                }
                return this.contentType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.contentType = value;
                this.contentType.PersistIfNeeded((HeaderCollection)this.Headers, true);
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
                if (this.contentType == null)
                {
                    this.contentType = new ContentType();
                }
                this.contentType.PersistIfNeeded(this.headers, false);
                if (this.contentDisposition != null)
                {
                    this.contentDisposition.PersistIfNeeded(this.headers, false);
                }
                return this.headers;
            }
        }

        //Static methods
        internal static Encoding DecodeEncoding(string value)
        {
            if ((value == null) || (value.Length == 0))
            {
                return null;
            }
            string[] strArray = value.Split(new char[] { '?' });
            if (((strArray.Length != 5) || (strArray[0] != "=")) || (strArray[4] != "="))
            {
                return null;
            }
            string name = strArray[1];
            return Encoding.GetEncoding(name);
        }

        internal static string DecodeHeaderValue(string value)
        {
            int num;
            if ((value == null) || (value.Length == 0))
            {
                return string.Empty;
            }
            string[] strArray = value.Split(new char[] { '?' });
            if (((strArray.Length != 5) || (strArray[0] != "=")) || (strArray[4] != "="))
            {
                return value;
            }
            string name = strArray[1];
            bool flag = strArray[2] == "B";
            byte[] bytes = Encoding.ASCII.GetBytes(strArray[3]);
            if (flag)
            {
                num = new Base64Stream().DecodeBytes(bytes, 0, bytes.Length);
            }
            else
            {
                num = new QuotedPrintableStream().DecodeBytes(bytes, 0, bytes.Length);
            }
            return Encoding.GetEncoding(name).GetString(bytes, 0, num);
        }

        internal static string EncodeHeaderValue(string value, Encoding encoding, bool base64Encoding)
        {
            StringBuilder builder = new StringBuilder();
            if ((encoding == null) && IsAscii(value, false))
            {
                return value;
            }
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding("utf-8");
            }
            string bodyName = encoding.BodyName();
            if (encoding == Encoding.BigEndianUnicode)
            {
                bodyName = "utf-16be";
            }
            builder.Append("=?");
            builder.Append(bodyName);
            builder.Append("?");
            builder.Append(base64Encoding ? "B" : "Q");
            builder.Append("?");
            byte[] bytes = encoding.GetBytes(value);
            if (base64Encoding)
            {
                Base64Stream stream = new Base64Stream(-1);
                stream.EncodeBytes(bytes, 0, bytes.Length, true);
                builder.Append(Encoding.ASCII.GetString(stream.WriteState.Buffer, 0, stream.WriteState.Length));
            }
            else
            {
                QuotedPrintableStream stream2 = new QuotedPrintableStream(-1);
                stream2.EncodeBytes(bytes, 0, bytes.Length);
                builder.Append(Encoding.ASCII.GetString(stream2.WriteState.Buffer, 0, stream2.WriteState.Length));
            }
            builder.Append("?=");
            return builder.ToString();
        }

        internal static bool IsAnsi(string value, bool permitCROrLF)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (char ch in value)
            {
                if (ch > '\x00ff')
                {
                    return false;
                }
                if (!permitCROrLF && ((ch == '\r') || (ch == '\n')))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool IsAscii(string value, bool permitCROrLF)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (char ch in value)
            {
                if (ch > '\x007f')
                {
                    return false;
                }
                if (!permitCROrLF && ((ch == '\r') || (ch == '\n')))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool ShouldUseBase64Encoding(Encoding encoding)
        {
            if (((encoding != Encoding.Unicode) && (encoding != Encoding.UTF8)) && (encoding != Encoding.BigEndianUnicode))
            {
                return false;
            }
            return true;
        }

    }
}