using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    public class ConnectionDetailCollection : IEnumerable<ConnectionDetail>
    {
        /// <summary>
        /// Occurs when the connection detail item list changes
        /// </summary>
        public event ConnectionStateChangedHandler ConnectionDetailItemsChanged;

        private List<ConnectionDetail> m_details;

        internal ConnectionDetailCollection()
        {
            m_details = new List<ConnectionDetail>();
        }

        public IEnumerator<ConnectionDetail> GetEnumerator()
        {
            return m_details.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_details.GetEnumerator();
        }

        internal void Add(ConnectionDetail detail)
        {
            m_details.Add(detail);
        }

        internal void RaiseConnectionDetailItemsChanged(ConnectionStatus status)
        {
            if (this.ConnectionDetailItemsChanged != null)
            {
                ConnectionDetailItemsChanged(this, status);
            }
        }

        public int Count
        {
            get { return m_details.Count; }
        }
    }
}
