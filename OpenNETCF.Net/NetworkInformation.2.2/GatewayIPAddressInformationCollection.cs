using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of System.Net.NetworkInformation.GatewayIPAddressInformation
    /// types.
    /// </summary>
    public class GatewayIPAddressInformationCollection : CollectionBase, IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.GatewayIPAddressInformationCollection
        /// class.
        /// </summary>
        protected internal GatewayIPAddressInformationCollection() { }

        /// <summary>
        /// Gets the System.Net.NetworkInformation.GatewayIPAddressInformation at the
        /// specific index of the collection.
        /// </summary>
        /// <param name="index">
        /// The index of interest.
        /// </param>
        /// <returns>
        /// The OpenNETCF.Net.NetworkInformation.GatewayIPAddressInformation at the specific
        /// index in the collection.
        /// </returns>
        public GatewayIPAddressInformation this[int index]
        {
            get { return (GatewayIPAddressInformation)List[index]; }
            internal set { List[index] = value; }
        }

        internal int Add(GatewayIPAddressInformation address)
        {
            return List.Add(address);
        }

        /// <summary>
        /// Checks whether the collection contains the specified System.Net.NetworkInformation.GatewayIPAddressInformation
        /// object.
        /// </summary>
        /// <param name="address">
        /// The System.Net.NetworkInformation.GatewayIPAddressInformation object to be
        /// searched in the collection.
        /// </param>
        /// <returns>
        /// true if the System.Net.NetworkInformation.GatewayIPAddressInformation object
        /// exists in the collection; otherwise false.
        /// </returns>
        public virtual bool Contains(GatewayIPAddressInformation address)
        {
            return List.Contains(address);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns>
        /// An object that implements the System.Collections.IEnumerator interface and
        /// provides access to the System.Net.NetworkInformation.IPUnicastAddressInformation
        /// types in this collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static explicit operator GatewayIPAddressInformationCollection(UnicastIPAddressInformationCollection c)
        {
            GatewayIPAddressInformationCollection coll = new GatewayIPAddressInformationCollection();

            foreach (UnicastIPAddressInformation info in c)
            {
                coll.Add(new GatewayIPAddressInformation(info.Address));
            }

            return coll;
        }
    }
}
