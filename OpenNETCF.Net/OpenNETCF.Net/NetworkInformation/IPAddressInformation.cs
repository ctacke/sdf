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
    /// Provides information about a network interface address.
    /// </summary>
    public class IPAddressInformation
    {
        internal IPAddress m_address;
        internal PerAdapterFlags m_adapterFlags;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPAddressInformation
        /// class.
        /// </summary>
        protected internal IPAddressInformation() {}

        /// <summary>
        /// Gets the Internet Protocol (IP) address.
        /// </summary>
        public IPAddress Address
        {
            get { return m_address; }
            internal set { m_address = value; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether the Internet Protocol
        /// (IP) address is legal to appear in a Domain Name System (DNS) server database.
        /// </summary>
        public bool IsDnsEligible
        {
            get { return (m_adapterFlags | PerAdapterFlags.DnsEligible) == PerAdapterFlags.DnsEligible; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether the Internet Protocol
        /// (IP) address is transient (a cluster address).
        /// </summary>
        public bool IsTransient
        {
            get { return (m_adapterFlags | PerAdapterFlags.Transient) == PerAdapterFlags.Transient; }
        }

        internal PerAdapterFlags AdapterFlags
        {
            set { m_adapterFlags = value; }
        }

    }
}
