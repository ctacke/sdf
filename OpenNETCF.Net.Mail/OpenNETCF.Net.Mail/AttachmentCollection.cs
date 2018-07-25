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
    /// <summary>
    /// Stores attachments to be sent as part of an e-mail message.
    /// </summary>
    public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
    {
        // Fields
        private bool disposed;

        // Methods
        internal AttachmentCollection()
        {
        }

        protected override void ClearItems()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            base.ClearItems();
        }

        /// <summary>
        /// Releases all resources used by the AttachmentCollection. 
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                foreach (Attachment attachment in this.Items)
                {
                    attachment.Dispose();
                }
                base.Clear();
                this.disposed = true;
            }
        }

        protected override void InsertItem(int index, Attachment item)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Attachment item)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.SetItem(index, item);
        }
    }
}