using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Represents the IP address of the network gateway. This class cannot be instantiated.
    /// </summary>
    public class GatewayIPAddressInformation
    {
        private IPAddress m_gateway;

        /// <summary>
        /// Initializes the members of this class.
        /// </summary>
        protected GatewayIPAddressInformation() { }

        internal GatewayIPAddressInformation(IPAddress gatewayAddress)
        {
            m_gateway = gatewayAddress;
        }

        /// <summary>
        /// Get the IP address of the gateway.
        /// </summary>
        public IPAddress Address 
        {
            get { return m_gateway; }
            internal set { m_gateway = value; }
        }


        /// <summary>
        /// Determines if this address is equivalent to the address of another GatewayIPAddressInformation instance
        /// </summary>
        /// <param name="obj">A GatewayIPAddressInformation instance</param>
        /// <returns>true if equivalent, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GatewayIPAddressInformation ipi = (GatewayIPAddressInformation)obj;

            return ipi.Address.Equals(this.Address);
        }

        /// <summary>
        /// Returns a hash value for a GatewayIPAddressInformation
        /// </summary>
        /// <returns>A hash value</returns>
        public override int GetHashCode()
        {
            return this.Address.GetHashCode();
        }

    }
}
