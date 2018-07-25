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
    /// Provides information about network interfaces that support Internet Protocol
    /// version 6 (IPv6).
    /// </summary>
    public class IPv6InterfaceProperties
    {
        private IPInterfaceProperties m_props;
        private int m_mtu = 0;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPv6InterfaceProperties
        /// class.
        /// </summary>
        /// <param name="props"></param>
        /// <param name="mtu"></param>
        internal IPv6InterfaceProperties(IPInterfaceProperties props, int mtu)
        {
            m_props = props;
            m_mtu = mtu;
        }

        /// <summary>
        /// Gets the interface index for the Internet Protocol version 6 (IPv6) address.
        /// </summary>
        public int Index
        {
            get { return m_props.m_info.Index; }
        }

        /// <summary>
        /// Gets the maximum transmission unit (MTU) for this network interface.
        /// </summary>
        public int Mtu
        {
            get { return m_mtu; }
        }
    }
}
