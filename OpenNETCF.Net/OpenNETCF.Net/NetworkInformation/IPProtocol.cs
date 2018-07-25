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
    /*
    #define MIB_IPPROTO_OTHER		        1
    #define MIB_IPPROTO_LOCAL		        2
    #define MIB_IPPROTO_NETMGMT		        3
    #define MIB_IPPROTO_ICMP			    4
    #define MIB_IPPROTO_EGP			        5
    #define MIB_IPPROTO_GGP			        6
    #define MIB_IPPROTO_HELLO		        7
    #define MIB_IPPROTO_RIP			        8
    #define MIB_IPPROTO_IS_IS		        9
    #define MIB_IPPROTO_ES_IS		        10
    #define MIB_IPPROTO_CISCO		        11
    #define MIB_IPPROTO_BBN			        12
    #define MIB_IPPROTO_OSPF			    13
    #define MIB_IPPROTO_BGP			        14

    #define MIB_IPPROTO_NT_AUTOSTATIC       10002
    #define MIB_IPPROTO_NT_STATIC           10006
    #define MIB_IPPROTO_NT_STATIC_NON_DOD   10007
    */
    /// <summary>
    /// Protocol
    /// </summary>
    public enum IPProtocol
    {
        /// <summary>
        /// Other
        /// </summary>
        Other = 1,
        /// <summary>
        /// Local
        /// </summary>
        Local = 2,
        /// <summary>
        /// Network management
        /// </summary>
        NetworkManagement = 3,
        /// <summary>
        /// Internet Control Message
        /// </summary>
        ICMP = 4,
        /// <summary>
        /// Exterior Gateway
        /// </summary>
        EGP = 5,
        /// <summary>
        /// Gateway to Gateway
        /// </summary>
        GGP = 6,
        /// <summary>
        /// Local Network
        /// </summary>
        HELLO = 7,
        /// <summary>
        /// Routing Information
        /// </summary>
        RIP = 8,
        /// <summary>
        /// Intermediate System - Intermediate System
        /// </summary>
        IS_IS = 9,
        /// <summary>
        /// External System - Intermediate System
        /// </summary>
        ES_IS = 10,
        /// <summary>
        /// Cisco
        /// </summary>
        Cisco = 11,
        /// <summary>
        /// BBN
        /// </summary>
        BBN = 12,
        /// <summary>
        /// OSPF
        /// </summary>
        OSPF = 13,
        /// <summary>
        /// Border Gateway
        /// </summary>
        BGP = 14,
        /// <summary>
        /// Autostatic under WINNT
        /// </summary>
        NTAutoStatic = 10002,
        /// <summary>
        /// Static under WINNT
        /// </summary>
        NTStatic = 10006,
        /// <summary>
        /// Static Non-DoD
        /// </summary>
        NTStaticNonDOD = 10007
    }
}
