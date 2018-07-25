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

namespace OpenNETCF.Net
{
    [Flags]
    public enum QueryOptions : int
    {
        Standard = 0x00000000,
        AcceptTruncatedRespone = 0x00000001,
        UseTcpOnly = 0x00000002,
        NoRecursion = 0x00000004,
        BypassCache = 0x00000008,
        NoWireQuery = 0x00000010,
        NoLocalName = 0x00000020,
        NoHostsFile = 0x00000040,
        NoNetBT = 0x00000080,
        WireOnly = 0x00000100,
        ReturnMessage = 0x00000200,
        TreatAsFQDN = 0x00001000,
        DontResetTtleValues = 0x00100000

        //#define DNS_QUERY_STANDARD                  0x00000000
        //#define DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE 0x00000001
        //#define DNS_QUERY_USE_TCP_ONLY              0x00000002
        //#define DNS_QUERY_NO_RECURSION              0x00000004
        //#define DNS_QUERY_BYPASS_CACHE              0x00000008

        //#define DNS_QUERY_NO_WIRE_QUERY             0x00000010
        //#define DNS_QUERY_NO_LOCAL_NAME             0x00000020
        //#define DNS_QUERY_NO_HOSTS_FILE             0x00000040
        //#define DNS_QUERY_NO_NETBT                  0x00000080

        //#define DNS_QUERY_WIRE_ONLY                 0x00000100
        //#define DNS_QUERY_RETURN_MESSAGE            0x00000200

        //#define DNS_QUERY_TREAT_AS_FQDN             0x00001000
        //#define DNS_QUERY_DONT_RESET_TTL_VALUES     0x00100000
    }
}
