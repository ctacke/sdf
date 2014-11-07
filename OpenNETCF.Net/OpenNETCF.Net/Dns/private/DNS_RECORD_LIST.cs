using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    internal class DNS_RECORD_LIST : IEnumerable<DNS_RECORD>, IDisposable
    {
        private List<DNS_RECORD> m_records = new List<DNS_RECORD>();

        private DNS_RECORD_LIST()
        {
        }

        public static DNS_RECORD_LIST FromIntPtr(IntPtr ptr)
        {
            DNS_RECORD_LIST list = new DNS_RECORD_LIST();

            DNS_RECORD record = new DNS_RECORD(ptr);
            list.Add(record);
            return list;
        }

        private void Add(DNS_RECORD record)
        {
            m_records.Add(record);
        }

        public IEnumerator<DNS_RECORD> GetEnumerator()
        {
            return m_records.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_records.GetEnumerator();
        }

        public void Dispose()
        {
            if (m_records.Count > 0)
            {
                NativeMethods.DnsRecordListFree(m_records[0].m_address, DNS_FREE_TYPE.RecordList);
            }
        }
    }
}
