using System.Collections;
using System.Collections.Generic;

namespace OpenNETCF.Net
{
    /// <summary>
    /// Summary description for DestinationInfoCollection.
    /// </summary>
    public class DestinationInfoCollection : IEnumerable<DestinationInfo>
    {
        private List<DestinationInfo> m_destinations;

        internal DestinationInfoCollection()
        {
            m_destinations = new List<DestinationInfo>();
        }

        internal DestinationInfoCollection(DestinationInfo[] items)
        {
            m_destinations = new List<DestinationInfo>(items);
        }

        internal void Add(DestinationInfo value)
        {
            m_destinations.Add(value);
        }

        public bool Contains(DestinationInfo value)
        {
            return m_destinations.Contains(value);
        }

        public int IndexOf(DestinationInfo value)
        {
            return m_destinations.IndexOf(value);
        }

        public int Count
        {
            get { return m_destinations.Count; }
        }

        public DestinationInfo this[int index]
        {
            get { return (DestinationInfo) m_destinations[index]; }
            internal set { m_destinations[index] = value; }
        }

        public IEnumerator<DestinationInfo> GetEnumerator()
        {
            return m_destinations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_destinations.GetEnumerator();
        }
    }
}