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
using System.Globalization;
using System.Collections.Specialized;
using OpenNETCF.Net.Mail;

namespace OpenNETCF.Net.Mime
{
    internal class HeaderCollection : NameValueCollection
    {
        // Fields
        //private MimeBasePart part;

        // Methods
        internal HeaderCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public override void Add(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
            }
            if (value == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "name");
            }
            MailBnfHelper.ValidateHeaderName(name);
            if (!MimeBasePart.IsAnsi(value, false))
            {
                throw new FormatException(SR.GetString("InvalidHeaderValue"));
            }
            name = MailHeaderInfo.NormalizeCase(name);
            //MailHeaderID iD = MailHeaderInfo.GetID(name);
            //if ((iD == MailHeaderID.ContentType) && (this.part != null))
            //{
            //    this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
            //}
            //else if ((iD == MailHeaderID.ContentDisposition) && (this.part is MimePart))
            //{
            //    ((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
            //}
            //else if (MailHeaderInfo.IsSingleton(name))
            //{
            //    base.Set(name, value);
            //}
            //else
            //{
                base.Add(name, value);
            //}
        }

        public override string Get(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
            }
            //MailHeaderID iD = MailHeaderInfo.GetID(name);
            //if ((iD == MailHeaderID.ContentType) && (this.part != null))
            //{
            //    this.part.ContentType.PersistIfNeeded(this, false);
            //}
            //else if ((iD == MailHeaderID.ContentDisposition) && (this.part is MimePart))
            //{
            //    ((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
            //}
            return base.Get(name);
        }

        public override string[] GetValues(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
            }
            //MailHeaderID iD = MailHeaderInfo.GetID(name);
            //if ((iD == MailHeaderID.ContentType) && (this.part != null))
            //{
            //    this.part.ContentType.PersistIfNeeded(this, false);
            //}
            //else if ((iD == MailHeaderID.ContentDisposition) && (this.part is MimePart))
            //{
            //    ((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
            //}
            return base.GetValues(name);
        }

        internal void InternalRemove(string name)
        {
            base.Remove(name);
        }

        internal void InternalSet(string name, string value)
        {
            base.Set(name, value);
        }

        public override void Remove(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
            }
            //MailHeaderID iD = MailHeaderInfo.GetID(name);
            //if ((iD == MailHeaderID.ContentType) && (this.part != null))
            //{
            //    this.part.ContentType = null;
            //}
            //else if ((iD == MailHeaderID.ContentDisposition) && (this.part is MimePart))
            //{
            //    ((MimePart)this.part).ContentDisposition = null;
            //}
            //base.Remove(name);
        }

        public override void Set(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (name == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
            }
            if (value == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "name");
            }
            if (!MimeBasePart.IsAscii(name, false))
            {
                throw new FormatException(SR.GetString("InvalidHeaderName"));
            }
            if (!MimeBasePart.IsAnsi(value, false))
            {
                throw new FormatException(SR.GetString("InvalidHeaderValue"));
            }
            name = MailHeaderInfo.NormalizeCase(name);
            //MailHeaderID iD = MailHeaderInfo.GetID(name);
            //if ((iD == MailHeaderID.ContentType) && (this.part != null))
            //{
            //    this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
            //}
            //else if ((iD == MailHeaderID.ContentDisposition) && (this.part is MimePart))
            //{
            //    ((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
            //}
            //else
            //{
                base.Set(name, value);
            //}
        }
    }
}