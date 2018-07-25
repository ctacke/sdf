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

namespace OpenNETCF.Net.Mail
{
    internal static class SmtpCommands
    {
        // Fields
        internal static readonly byte[] Auth = Encoding.ASCII.GetBytes("AUTH ");
        internal static readonly byte[] CRLF = Encoding.ASCII.GetBytes("\r\n");
        internal static readonly byte[] Data = Encoding.ASCII.GetBytes("DATA\r\n");
        internal static readonly byte[] DataStop = Encoding.ASCII.GetBytes("\r\n.\r\n");
        internal static readonly byte[] EHello = Encoding.ASCII.GetBytes("EHLO ");
        internal static readonly byte[] Expand = Encoding.ASCII.GetBytes("EXPN ");
        internal static readonly byte[] Hello = Encoding.ASCII.GetBytes("HELO ");
        internal static readonly byte[] Help = Encoding.ASCII.GetBytes("HELP");
        internal static readonly byte[] Mail = Encoding.ASCII.GetBytes("MAIL FROM:");
        internal static readonly byte[] Noop = Encoding.ASCII.GetBytes("NOOP\r\n");
        internal static readonly byte[] Quit = Encoding.ASCII.GetBytes("QUIT\r\n");
        internal static readonly byte[] Recipient = Encoding.ASCII.GetBytes("RCPT TO:");
        internal static readonly byte[] Reset = Encoding.ASCII.GetBytes("RSET\r\n");
        internal static readonly byte[] Send = Encoding.ASCII.GetBytes("SEND FROM:");
        internal static readonly byte[] SendAndMail = Encoding.ASCII.GetBytes("SAML FROM:");
        internal static readonly byte[] SendOrMail = Encoding.ASCII.GetBytes("SOML FROM:");
        internal static readonly byte[] StartTls = Encoding.ASCII.GetBytes("STARTTLS");
        internal static readonly byte[] Turn = Encoding.ASCII.GetBytes("TURN\r\n");
        internal static readonly byte[] Verify = Encoding.ASCII.GetBytes("VRFY ");
    }
}
