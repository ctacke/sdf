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

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides the Media Access Control (MAC) address for a network interface (adapter).
    /// </summary>
    public class PhysicalAddress
    {
        private byte[] m_address;

        /// <summary>
        /// Returns a new System.Net.NetworkInformation.PhysicalAddress instance with
        /// a zero length address. This field is read-only.
        /// </summary>
        public static readonly PhysicalAddress None;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.PhysicalAddress
        /// class.
        /// </summary>
        /// <param name="address">
        /// A System.Byte array containing the address.
        /// </param>
        public PhysicalAddress(byte[] address)
        {
            if ((address.Length != 6) && (address.Length != 0))
            {
                throw new ArgumentException("Invalid length address");
            }

            m_address = address;
        }

        /// <summary>
        /// Compares two System.Net.NetworkInformation.PhysicalAddress instances.
        /// </summary>
        /// <param name="comparand">
        /// The System.Net.NetworkInformation.PhysicalAddress to compare to the current
        /// instance.
        /// </param>
        /// <returns>
        /// true if this instance and the specified instance contain the same address;
        /// otherwise false.
        /// </returns>
        public override bool Equals(object comparand)
        {
            if((comparand == null) || (!(comparand is PhysicalAddress)))
            {
                return false;
            }

            return (string.Compare(((PhysicalAddress)comparand).ToString(), this.ToString()) == 0);
        }

        /// <summary>
        /// Returns the address of the current instance.
        /// </summary>
        /// <returns>A System.Byte array containing the address.</returns>
        public byte[] GetAddressBytes()
        {
            return (byte[])m_address.Clone();
        }

        /// <summary>
        /// Returns the hash value of a physical address.
        /// </summary>
        /// <returns>An integer hash value.</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (byte b in m_address)
            {
                hash |= b.GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// Returns the System.String representation of the address of this instance.
        /// </summary>
        /// <returns>
        /// A System.String containing the address contained in this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);

            for (int b = 0; b < m_address.Length; b++)
            {
                if (b < m_address.Length - 1)
                {
                    sb.Append(string.Format("{0:X2}:", m_address[b]));
                }
                else
                {
                    sb.Append(string.Format("{0:X2}", m_address[b]));
                }
            }
            return sb.ToString();
        }
    }
}
