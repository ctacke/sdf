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
using System.Collections.ObjectModel;

namespace OpenNETCF.Net.Mail
{
    public class MailAddressCollection : Collection<MailAddress>
    {
        /// <summary>
        /// Add a list of e-mail addresses to the collection.
        /// </summary>
        /// <param name="addresses">The e-mail addresses to add to the MailAddressCollection. Multiple e-mail addresses must be separated with a comma character (",").</param>
        public void Add(string addresses)
        {
            if (addresses == null)
            {
                throw new ArgumentNullException("addresses");
            }
            if (addresses == string.Empty)
            {
                throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "addresses" }), "addresses");
            }
            this.ParseValue(addresses);
        }

        /// <summary>
        /// Inserts an e-mail address into the MailAddressCollection, at the specified location.
        /// </summary>
        /// <param name="index">The e-mail address to be inserted into the collection.</param>
        /// <param name="item">The location at which to insert the e-mail address that is specified by item.</param>
        /// <exception cref="ArgumentNullException">The item parameter is null.</exception>
        protected override void InsertItem(int index, MailAddress item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.InsertItem(index, item);
        }

        internal void ParseValue(string addresses)
        {
            for (int i = 0; i < addresses.Length; i++)
            {
                MailAddress item = MailBnfHelper.ReadMailAddress(addresses, ref i);
                if (item == null)
                {
                    return;
                }
                base.Add(item);
                if (!MailBnfHelper.SkipCFWS(addresses, ref i))
                {
                    break;
                }
                if (addresses[i] != ',')
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index"An e-mail address that will replace the element in the collection.></param>
        /// <param name="item">The index of the e-mail address element to be replaced.</param>
        /// <exception cref="ArgumentNullException">The item parameter is null.</exception>
        protected override void SetItem(int index, MailAddress item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.SetItem(index, item);
        }

        internal string ToEncodedString()
        {
            bool flag = true;
            StringBuilder builder = new StringBuilder();
            foreach (MailAddress address in this.Items)
            {
                if (!flag)
                {
                    builder.Append(", ");
                }
                builder.Append(address.ToEncodedString());
                flag = false;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string representation of the e-mail addresses in this MailAddressCollection object.
        /// </summary>
        /// <returns>A String containing the e-mail addresses in this collection.</returns>
        public override string ToString()
        {
            bool flag = true;
            StringBuilder builder = new StringBuilder();
            foreach (MailAddress address in this.Items)
            {
                if (!flag)
                {
                    builder.Append(", ");
                }
                builder.Append(address.ToString());
                flag = false;
            }
            return builder.ToString();
        }
    }
}