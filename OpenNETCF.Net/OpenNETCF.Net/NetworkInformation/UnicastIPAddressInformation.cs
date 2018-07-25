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
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about a network interface's unicast address.
    /// </summary>
    public class UnicastIPAddressInformation : IPAddressInformation
    {
        private IPAddress m_subnetMask;
        private int m_nteContext;
        internal uint m_preferredLifetime;
        internal uint m_validLifetime;
        internal uint m_leaseLifetime;
        internal DuplicateAddressDetectionState m_dadState = DuplicateAddressDetectionState.Invalid;
        internal PrefixOrigin m_prefixOrigin = PrefixOrigin.Other;
        internal SuffixOrigin m_suffixOrigin = SuffixOrigin.Other;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.UnicastIPAddressInformation
        /// class.
        /// </summary>
        protected internal UnicastIPAddressInformation()
        {
        }

        /// <summary>
        /// Gets the IPv4 subnet mask.
        /// </summary>
        public IPAddress IPv4Mask 
        {
            get { return m_subnetMask; }
            internal set { m_subnetMask = value; }
        }

        internal int NetworkTableEntryContext
        {
            set { m_nteContext = value; }
            get { return m_nteContext; }
        }

        /// <summary>
        /// Gets the number of seconds remaining during which this address is the preferred
        /// address.
        /// </summary>
        [CLSCompliant(false)]
        public uint AddressPreferredLifetime
        {
            get { return m_preferredLifetime; }
            internal set { m_preferredLifetime = value; }
        }

        /// <summary>
        /// Gets the number of seconds remaining during which this address is valid.
        /// </summary>
        [CLSCompliant(false)]
        public uint AddressValidLifetime
        {
            get { return m_validLifetime; }
            internal set { m_validLifetime = value; }
        }

        /// <summary>
        /// Specifies the amount of time remaining on the Dynamic Host Configuration
        /// Protocol (DHCP) lease for this IP address.
        /// </summary>
        [CLSCompliant(false)]
        public uint DhcpLeaseLifetime
        {
            get { return m_leaseLifetime; }
            internal set { m_leaseLifetime = value; }
        }

        /// <summary>
        /// Gets a value that indicates the state of the duplicate address detection
        /// algorithm.
        /// </summary>
        public DuplicateAddressDetectionState DuplicateAddressDetectionState
        {
            get { return m_dadState; }
            internal set { m_dadState = value; }
        }

        /// <summary>
        /// Gets a value that identifies the source of a unicast Internet Protocol (IP)
        /// address prefix.
        /// </summary>
        public PrefixOrigin PrefixOrigin
        {
            get { return m_prefixOrigin; }
            internal set { m_prefixOrigin = value; }
        }

        /// <summary>
        /// Gets a value that identifies the source of a unicast Internet Protocol (IP)
        /// address suffix.
        /// </summary>
        public SuffixOrigin SuffixOrigin
        {
            get { return m_suffixOrigin; }
            internal set { m_suffixOrigin = value; }
        }

        /// <summary>
        /// Determines if this instance is equivalent to another UnicastIPAddressInformation
        /// </summary>
        /// <param name="obj">An UnicastIPAddressInformationinstance</param>
        /// <returns>true if equivalent, otherwise false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            UnicastIPAddressInformation ipi = (UnicastIPAddressInformation)obj;

            if (ipi.Address != this.Address)
            {
                return false;
            }

            if (ipi.IPv4Mask != this.IPv4Mask)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash for a UnicastIPAddressInformation
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Address.GetHashCode() | this.IPv4Mask.GetHashCode();
        }

    }
}
