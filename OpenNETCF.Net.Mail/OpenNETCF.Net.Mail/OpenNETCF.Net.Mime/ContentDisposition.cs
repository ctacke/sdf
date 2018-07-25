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
using OpenNETCF.Net.Mail;
using System.Globalization;

namespace OpenNETCF.Net.Mime
{
    public class ContentDisposition
    {
        // Fields
        private string disposition;
        private string dispositionType;
        private bool isChanged;
        private bool isPersisted;
        private StringDictionary parameters;

        // Methods
        public ContentDisposition()
        {
            this.isChanged = true;
            this.disposition = "attachment";
            this.ParseValue();
        }

        public ContentDisposition(string disposition)
        {
            if (disposition == null)
            {
                throw new ArgumentNullException("disposition");
            }
            this.isChanged = true;
            this.disposition = disposition;
            this.ParseValue();
        }

        public override bool Equals(object rparam)
        {
            if (rparam == null)
            {
                return false;
            }
            return (string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        private void ParseValue()
        {
            int offset = 0;
            this.parameters = new StringDictionary();
            try
            {
                this.dispositionType = MailBnfHelper.ReadToken(this.disposition, ref offset, null);
                if ((this.dispositionType == null) || (this.dispositionType.Length == 0))
                {
                    throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
                }
                while (MailBnfHelper.SkipCFWS(this.disposition, ref offset))
                {
                    string str2;
                    if (this.disposition[offset++] != ';')
                    {
                        throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter"));
                    }
                    if (MailBnfHelper.SkipCFWS(this.disposition, ref offset))
                    {
                        string strA = MailBnfHelper.ReadParameterAttribute(this.disposition, ref offset, null);
                        if (this.disposition[offset++] != '=')
                        {
                            throw new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
                        }
                        if (!MailBnfHelper.SkipCFWS(this.disposition, ref offset))
                        {
                            str2 = string.Empty;
                        }
                        else if (this.disposition[offset] == '"')
                        {
                            str2 = MailBnfHelper.ReadQuotedString(this.disposition, ref offset, null);
                        }
                        else
                        {
                            str2 = MailBnfHelper.ReadToken(this.disposition, ref offset, null);
                        }
                        if (((strA == null) || (str2 == null)) || ((strA.Length == 0) || (str2.Length == 0)))
                        {
                            throw new FormatException(SR.GetString("ContentDispositionInvalid"));
                        }
                        if (((string.Compare(strA, "creation-date", StringComparison.OrdinalIgnoreCase) == 0) || (string.Compare(strA, "modification-date", StringComparison.OrdinalIgnoreCase) == 0)) || (string.Compare(strA, "read-date", StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            int num2 = 0;
                            MailBnfHelper.ReadDateTime(str2, ref num2);
                        }
                        this.parameters.Add(strA, str2);
                    }
                }
            }
            catch (FormatException)
            {
                throw new FormatException(SR.GetString("ContentDispositionInvalid"));
            }
        }

        internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
        {
            if ((this.IsChanged || !this.isPersisted) || forcePersist)
            {
                headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
                this.isPersisted = true;
            }
        }

        internal void Set(string contentDisposition, HeaderCollection headers)
        {
            this.disposition = contentDisposition;
            this.ParseValue();
            headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
            this.isPersisted = true;
        }

        public override string ToString()
        {
            if (((this.disposition == null) || this.isChanged) || (this.parameters != null))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.dispositionType);
                foreach (string str in this.Parameters.Keys)
                {
                    builder.Append("; ");
                    builder.Append(str);
                    builder.Append('=');
                    MailBnfHelper.GetTokenOrQuotedString(this.parameters[str], builder);
                }
                this.disposition = builder.ToString();
                this.isChanged = false;
                
                this.isPersisted = false;
            }
            return this.disposition;
        }

        // Properties
        public DateTime CreationDate
        {
            get
            {
                string data = this.Parameters["creation-date"];
                if (data == null)
                {
                    return DateTime.MinValue;
                }
                int offset = 0;
                return MailBnfHelper.ReadDateTime(data, ref offset);
            }
            set
            {
                this.Parameters["creation-date"] = MailBnfHelper.GetDateTimeString(value, null);
            }
        }

        public string DispositionType
        {
            get
            {
                return this.dispositionType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
                }
                this.isChanged = true;
                this.dispositionType = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.Parameters["filename"];
            }
            set
            {
                if ((value == null) || (value == string.Empty))
                {
                    this.Parameters.Remove("filename");
                }
                else
                {
                    this.Parameters["filename"] = value;
                }
            }
        }

        public bool Inline
        {
            get
            {
                return (this.dispositionType == "inline");
            }
            set
            {
                this.isChanged = true;
                if (value)
                {
                    this.dispositionType = "inline";
                }
                else
                {
                    this.dispositionType = "attachment";
                }
            }
        }

        internal bool IsChanged
        {
            get
            {
                return (this.isChanged || (this.parameters != null));
            }
        }

        public DateTime ModificationDate
        {
            get
            {
                string data = this.Parameters["modification-date"];
                if (data == null)
                {
                    return DateTime.MinValue;
                }
                int offset = 0;
                return MailBnfHelper.ReadDateTime(data, ref offset);
            }
            set
            {
                this.Parameters["modification-date"] = MailBnfHelper.GetDateTimeString(value, null);
            }
        }

        public StringDictionary Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new StringDictionary();
                }
                return this.parameters;
            }
        }

        public DateTime ReadDate
        {
            get
            {
                string data = this.Parameters["read-date"];
                if (data == null)
                {
                    return DateTime.MinValue;
                }
                int offset = 0;
                return MailBnfHelper.ReadDateTime(data, ref offset);
            }
            set
            {
                this.Parameters["read-date"] = MailBnfHelper.GetDateTimeString(value, null);
            }
        }

        public long Size
        {
            get
            {
                string s = this.Parameters["size"];
                if (s == null)
                {
                    return -1L;
                }
                return long.Parse(s);
            }
            set
            {
                this.Parameters["size"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}