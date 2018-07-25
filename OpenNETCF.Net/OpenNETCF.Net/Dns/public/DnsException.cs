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
    public class DnsException : Exception
    {
        internal static DnsException FromErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case 1460:
                    return new DnsException("This operation returned because the timeout period expired.", errorCode);
                case 9001:
                    return new DnsException("DNS server unable to interpret format.", errorCode);
                case 9002:
                    return new DnsException("DNS server failure.", errorCode);
                case 9003:
                    return new DnsException("DNS name does not exist.", errorCode);
                case 9004:
                    return new DnsException("DNS request not supported by name server.", errorCode);
                case 9005:
                    return new DnsException("DNS operation refused.", errorCode);
                case 9006:
                    return new DnsException("DNS name that ought not exist, does exist.", errorCode);
                case 9007:
                    return new DnsException("DNS RR set that ought not exist, does exist.", errorCode);
                case 9008:
                    return new DnsException("DNS RR set that ought to exist, does not exist.", errorCode);
                case 9009:
                    return new DnsException("DNS server not authoritative for zone.", errorCode);
                case 9010:
                    return new DnsException("DNS name in update or prereq is not in zone.", errorCode);
                case 9016:
                    return new DnsException("DNS signature failed to verify.", errorCode);
                case 9017:
                    return new DnsException("DNS bad key.", errorCode);
                case 9018:
                    return new DnsException("DNS signature validity expired.", errorCode);
                case 9501:
                    return new DnsException("No records found for given DNS query.", errorCode);
                case 9502:
                    return new DnsException("Bad DNS packet.", errorCode);
                case 9503:
                    return new DnsException("No DNS packet.", errorCode);
                case 9504:
                    return new DnsException("DNS error, check rcode.", errorCode);
                case 9505:
                    return new DnsException("Unsecured DNS packet.", errorCode);
                case 9551:
                    return new DnsException("Invalid DNS type.", errorCode);
                case 9552:
                    return new DnsException("Invalid IP address.", errorCode);
                case 9553:
                    return new DnsException("Invalid property.", errorCode);
                case 9554:
                    return new DnsException("Try DNS operation again later.", errorCode);
                case 9555:
                    return new DnsException("Record for given name and type is not unique.", errorCode);
                case 9556:
                    return new DnsException("DNS name does not comply with RFC specifications.", errorCode);
                case 9557:
                    return new DnsException("DNS name is a fully-qualified DNS name.", errorCode);
                case 9558:
                    return new DnsException("DNS name is dotted (multi-label).", errorCode);
                case 9559:
                    return new DnsException("DNS name is a single-part name.", errorCode);
                case 9560:
                    return new DnsException("DNS name contains an invalid character.", errorCode);
                case 9561:
                    return new DnsException("DNS name is entirely numeric.", errorCode);
                case 9562:
                    return new DnsException("The operation requested is not permitted on a DNS root server.", errorCode);
                case 9563:
                    return new DnsException("The record could not be created because this part of the DNS namespace has been delegated to another server.", errorCode);
                case 9564:
                    return new DnsException("The DNS server could not find a set of root hints.", errorCode);
                case 9565:
                    return new DnsException("The DNS server found root hints but they were not consistent across all adapters.", errorCode);
                case 10049:
                    return new DnsException("The requested address is not valid in its context.", errorCode);
                default:
                    // TODO: more DNS errors are in winerror.h - fill these out at some point
                    return new DnsException("Unknown error code: " + errorCode.ToString() + ". See winerror.h for details.", errorCode);
            }
        }

        public DnsException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}
