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

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef struct _MIB_IPSTATS {
      DWORD dwForwarding; 
      DWORD dwDefaultTTL; 
      DWORD dwInReceives; 
      DWORD dwInHdrErrors; 
      DWORD dwInAddrErrors; 
      DWORD dwForwDatagrams; 
      DWORD dwInUnknownProtos; 
      DWORD dwInDiscards; 
      DWORD dwInDelivers; 
      DWORD dwOutRequests; 
      DWORD dwRoutingDiscards; 
      DWORD dwOutDiscards; 
      DWORD dwOutNoRoutes; 
      DWORD dwReasmTimeout; 
      DWORD dwReasmReqds; 
      DWORD dwReasmOks; 
      DWORD dwReasmFails; 
      DWORD dwFragOks; 
      DWORD dwFragFails; 
      DWORD dwFragCreates; 
      DWORD dwNumIf; 
      DWORD dwNumAddr; 
      DWORD dwNumRoutes; 
    } MIB_IPSTATS, *PMIB_IPSTATS;
    */

    internal struct MibIpStats
    {
        public int dwForwarding;
        public int dwDefaultTTL;
        public int dwInReceives;
        public int dwInHdrErrors;
        public int dwInAddrErrors;
        public int dwForwDatagrams;
        public int dwInUnknownProtos;
        public int dwInDiscards;
        public int dwInDelivers;
        public int dwOutRequests;
        public int dwRoutingDiscards;
        public int dwOutDiscards;
        public int dwOutNoRoutes;
        public int dwReasmTimeout;
        public int dwReasmReqds;
        public int dwReasmOks;
        public int dwReasmFails;
        public int dwFragOks;
        public int dwFragFails;
        public int dwFragCreates;
        public int dwNumIf;
        public int dwNumAddr;
        public int dwNumRoutes;
    }
}
