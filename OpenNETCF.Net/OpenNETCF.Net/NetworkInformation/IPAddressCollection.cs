using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of System.Net.IPAddress types.
    /// </summary>
    public class IPAddressCollection : ICollection<IPAddress>, IEnumerable<IPAddress>, IEnumerable
    {
        private List<IPAddress> m_list;
        internal event AddressChangedHandler Changed;

        internal IPAddressCollection()
        {
            m_list = new List<IPAddress>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPAddress this[int index]
        {
            get { return m_list[index]; }
            set 
            { 
              m_list[index] = value;
              if (Changed != null)
              {
                Changed();
              }
            }
        }

        /// <summary>
        /// Gets the number of OpenNETCF.Net.IPAddress types in this collection.
        /// </summary>
        public int Count
        {
            get { return m_list.Count; }
        }

        /// <summary>
        /// Gets a value that indicates whether access to this collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// </summary>
        /// <param name="address">The object to be added to the collection.</param>
        public void Add(IPAddress address)
        {
          InternalAdd(address);
          if (Changed != null)
          {
            Changed();
          }
        }

        internal void InternalAdd(IPAddress address)
        {
            m_list.Add(address);
        }

        /// <summary>
        /// Throws a System.NotSupportedException because this operation is not supported
        /// for this collection.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("This collection is read-only");
        }
        internal void InternalClear()
        {
            m_list.Clear();
        }

        /// <summary>
        /// Checks whether the collection contains the specified OpenNETCF.Net.NetworkInformation.IPAddress
        /// object.
        /// </summary>
        /// <param name="address">
        /// The OpenNETCF.Net.NetworkInformation.IPAddressInformation object to be searched
        /// in the collection.
        /// </param>
        /// <returns>
        /// true if the OpenNETCF.Net.NetworkInformation.IPAddressInformation object exists
        /// in the collection; otherwise. false.
        /// </returns>
        public virtual bool Contains(IPAddress address)
        {
            return m_list.Contains(address);
        }

        /// <summary>
        /// Copies the collection to the specified array.
        /// </summary>
        /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
        /// <param name="offset">The zero-based index in array at which the copy begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// array is multidimensional.-or- offset is equal to or greater than the length
        /// of array.-or- The number of elements in this OpenNETCF.Net.NetworkInformation.IPAddress
        /// is greater than the available space from offset to the end of the destination
        /// array.
        /// </exception>
        public virtual void CopyTo(IPAddress[] array, int offset)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if ((array.Rank > 1) || (array.Length > m_list.Count - offset))
            {
                throw new ArgumentException();
            }

            m_list.CopyTo(array, offset);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns>
        /// An object that implements the System.Collections.IEnumerator interface and
        /// provides access to the OpenNETCF.Net.NetworkInformation.IPAddressInformation
        /// types in this collection.
        /// </returns>
        public IEnumerator<IPAddress> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        /// <summary>
        /// Throws a System.NotSupportedException because this operation is not supported
        /// for this collection.
        /// </summary>
        /// <param name="address">The object to be removed.</param>
        /// <returns>Always throws a System.NotSupportedException.</returns>
        public bool Remove(IPAddress address)
        {
            throw new NotSupportedException("This collection is read-only");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

      internal string[] ToStringArray()
      {
        string[] list = new string[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
          list[i] = this[i].ToString();
        }

        return list;
      }
    }
}
