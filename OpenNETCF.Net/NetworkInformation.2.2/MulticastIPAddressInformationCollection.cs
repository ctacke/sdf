using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of OpenNETCF.Net.NetworkInformation.MulticastIPAddressInformation
    /// types.
    /// </summary>
    public class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
    {
        private List<MulticastIPAddressInformation> m_list;

        /// <summary>
        /// Initializes a new instance of the OpenNETCF.Net.NetworkInformation.MulticastIPAddressInformationCollection
        /// class.
        /// </summary>
        protected internal MulticastIPAddressInformationCollection()
        {
            m_list = new List<MulticastIPAddressInformation>();
        }

        /// <summary>
        /// Gets the number of OpenNETCF.Net.NetworkInformation.MulticastIPAddressInformation
        /// types in this collection.
        /// </summary>
        public virtual int Count
        {
            get { return m_list.Count; }
        }

        /// <summary>
        /// Gets a value that indicates whether access to this collection is read-only.
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the OpenNETCF.Net.NetworkInformation.MulticastIPAddressInformation instance
        /// at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>
        /// The OpenNETCF.Net.NetworkInformation.MulticastIPAddressInformation at the specified
        /// location.
        /// </returns>
        public virtual MulticastIPAddressInformation this[int index]
        {
            get { return m_list[index]; }
        }

        /// <summary>
        /// Throws a System.NotSupportedException because this operation is not supported
        /// for this collection.
        /// </summary>
        /// <param name="address">The object to be added to the collection.</param>
        public void Add(MulticastIPAddressInformation address)
        {
            throw new NotSupportedException("This collection is read-only");
        }
        internal void InternalAdd(MulticastIPAddressInformation address)
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
        /// Checks whether the collection contains the specified System.Net.NetworkInformation.MulticastIPAddressInformation
        /// object.
        /// </summary>
        /// <param name="address">
        /// The System.Net.NetworkInformation.MulticastIPAddressInformation object to be
        /// searched in the collection.
        /// </param>
        /// <returns>
        /// true if the System.Net.NetworkInformation.MulticastIPAddressInformation object
        /// exists in the collection; otherwise, false.
        /// </returns>
        public virtual bool Contains(MulticastIPAddressInformation address)
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
        /// of array.-or- The number of elements in this OpenNETCF.Net.NetworkInformation.IPAddressInformation
        /// is greater than the available space from offset to the end of the destination
        /// array.
        /// </exception>
        public virtual void CopyTo(MulticastIPAddressInformation[] array, int offset)
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
        public IEnumerator<MulticastIPAddressInformation> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        /// <summary>
        /// Throws a System.NotSupportedException because this operation is not supported
        /// for this collection.
        /// </summary>
        /// <param name="address">The object to be removed.</param>
        /// <returns>Always throws a System.NotSupportedException.</returns>
        public bool Remove(MulticastIPAddressInformation address)
        {
            throw new NotSupportedException("This collection is read-only");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }
    }
}
