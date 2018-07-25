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
    /// Specifies how an IP address host suffix was located.
    /// </summary>
    public enum SuffixOrigin
    {
        /// <summary>
        /// The suffix was located using an unspecified source.
        /// </summary>
        Other = 0,
        /// <summary>
        /// The suffix was manually configured.
        /// </summary>
        Manual = 1,
        /// <summary>
        /// The suffix is a well-known suffix. Well-known suffixes are specified in standard-track
        /// Request for Comments (RFC) documents and assigned by the Internet Assigned
        /// Numbers Authority (IANA) or an address registry. Such suffixes are reserved
        /// for special purposes.
        /// </summary>
        WellKnown = 2,
        /// <summary>
        /// The suffix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.
        /// </summary>
        OriginDhcp = 3,
        /// <summary>
        /// The suffix is a link-local suffix.
        /// </summary>
        LinkLayerAddress = 4,
        /// <summary>
        /// The suffix was randomly assigned.
        /// </summary>
        Random = 5,
    }
}
