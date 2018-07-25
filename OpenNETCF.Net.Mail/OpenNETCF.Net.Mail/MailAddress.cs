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
using OpenNETCF.Net.Mime;

namespace OpenNETCF.Net.Mail
{
    public class MailAddress
    {
        // Fields
        private string address;
        private string displayName;
        private Encoding displayNameEncoding;
        private string encodedDisplayName;
        private string fullAddress;
        private string host;
        private string userName;

        // Methods
        public MailAddress(string address)
            : this(address, null, (Encoding)null)
        {
        }

        public MailAddress(string address, string displayName)
            : this(address, displayName, (Encoding)null)
        {
        }

        public MailAddress(string address, string displayName, Encoding displayNameEncoding)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (address == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "address" }), "address");
            }
            this.displayNameEncoding = displayNameEncoding;
            this.displayName = displayName;
            this.ParseValue(address);
            if ((this.displayName != null) && (this.displayName != string.Empty))
            {
                if ((this.displayName[0] == '"') && (this.displayName[this.displayName.Length - 1] == '"'))
                {
                    this.displayName = this.displayName.Substring(1, this.displayName.Length - 2);
                }
                this.displayName = this.displayName.Trim();
            }
            if ((this.displayName != null) && (this.displayName.Length > 0))
            {
                if (!MimeBasePart.IsAscii(this.displayName, false) || (this.displayNameEncoding != null))
                {
                    if (this.displayNameEncoding == null)
                    {
                        this.displayNameEncoding = Encoding.GetEncoding("utf-8");
                    }
                    this.encodedDisplayName = MimeBasePart.EncodeHeaderValue(this.displayName, this.displayNameEncoding, MimeBasePart.ShouldUseBase64Encoding(displayNameEncoding));
                    StringBuilder builder = new StringBuilder();
                    int offset = 0;
                    MailBnfHelper.ReadQuotedString(this.encodedDisplayName, ref offset, builder, true);
                    this.encodedDisplayName = builder.ToString();
                }
                else
                {
                    this.encodedDisplayName = this.displayName;
                }
            }
        }

        internal MailAddress(string address, string encodedDisplayName, uint bogusParam)
        {
            this.encodedDisplayName = encodedDisplayName;
            this.GetParts(address);
        }

        private void CombineParts()
        {
            if ((this.userName != null) && (this.host != null))
            {
                StringBuilder builder = new StringBuilder();
                MailBnfHelper.GetDotAtomOrQuotedString(this.User, builder);
                builder.Append('@');
                MailBnfHelper.GetDotAtomOrDomainLiteral(this.Host, builder);
                this.address = builder.ToString();
            }
        }

        public override bool Equals(object value)
        {
            if (value == null)
            {
                return false;
            }
            return this.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        private void GetParts(string address)
        {
            if (address != null)
            {
                int index = address.IndexOf('@');
                if (index < 0)
                {
                    throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
                }
                this.userName = address.Substring(0, index);
                this.host = address.Substring(index + 1);
            }
        }

        private void ParseValue(string address)
        {
            string str = null;
            int index = address.IndexOf('"');
            if (index > 0)
            {
                throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
            }
            if (index == 0)
            {
                index = address.IndexOf('"', 1);
                if (index < 0)
                {
                    throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
                }
                str = address.Substring(1, index - 1);
                if (address.Length == (index + 1))
                {
                    throw new FormatException(SR.GetString("MailAddressInvalidFormat"));
                }
                address = address.Substring(index + 1);
            }
            if (str == null)
            {
                index = address.IndexOf('<');
                if (index > 0)
                {
                    str = address.Substring(0, index);
                    address = address.Substring(index);
                }
            }
            if (this.displayName == null)
            {
                this.displayName = str;
            }
            index = 0;
            address = MailBnfHelper.ReadMailAddress(address, ref index, out this.encodedDisplayName);
            this.GetParts(address);
        }

        internal string ToEncodedString()
        {
            if (this.fullAddress == null)
            {
                if ((this.encodedDisplayName != null) && (this.encodedDisplayName != string.Empty))
                {
                    StringBuilder builder = new StringBuilder();
                    MailBnfHelper.GetDotAtomOrQuotedString(this.encodedDisplayName, builder);
                    builder.Append(" <");
                    builder.Append(this.Address);
                    builder.Append('>');
                    this.fullAddress = builder.ToString();
                }
                else
                {
                    this.fullAddress = this.Address;
                }
            }
            return this.fullAddress;
        }

        public override string ToString()
        {
            if (this.fullAddress == null)
            {
                if ((this.encodedDisplayName != null) && (this.encodedDisplayName != string.Empty))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append('"');
                    builder.Append(this.DisplayName);
                    builder.Append("\" <");
                    builder.Append(this.Address);
                    builder.Append('>');
                    this.fullAddress = builder.ToString();
                }
                else
                {
                    this.fullAddress = this.Address;
                }
            }
            return this.fullAddress;
        }

        // Properties
        public string Address
        {
            get
            {
                if (this.address == null)
                {
                    this.CombineParts();
                }
                return this.address;
            }
        }

        public string DisplayName
        {
            get
            {
                if (this.displayName == null)
                {
                    if ((this.encodedDisplayName != null) && (this.encodedDisplayName.Length > 0))
                    {
                        this.displayName = MimeBasePart.DecodeHeaderValue(this.encodedDisplayName);
                    }
                    else
                    {
                        this.displayName = string.Empty;
                    }
                }
                return this.displayName;
            }
        }

        public string Host
        {
            get
            {
                return this.host;
            }
        }

        internal string SmtpAddress
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append('<');
                builder.Append(this.Address);
                builder.Append('>');
                return builder.ToString();
            }
        }

        public string User
        {
            get
            {
                return this.userName;
            }
        }
    }
}

 
