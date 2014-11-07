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