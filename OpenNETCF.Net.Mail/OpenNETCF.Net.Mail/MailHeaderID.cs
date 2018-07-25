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
    internal enum MailHeaderID
    {
        Bcc = 0,
        Cc = 1,
        Comments = 2,
        ContentDescription = 3,
        ContentDisposition = 4,
        ContentID = 5,
        ContentLocation = 6,
        ContentTransferEncoding = 7,
        ContentType = 8,
        Date = 9,
        From = 10,
        Importance = 11,
        InReplyTo = 12,
        Keywords = 13,
        Max = 14,
        MessageID = 15,
        MimeVersion = 0x10,
        Priority = 0x11,
        References = 0x12,
        ReplyTo = 0x13,
        ResentBcc = 20,
        ResentCc = 0x15,
        ResentDate = 0x16,
        ResentFrom = 0x17,
        ResentMessageID = 0x18,
        ResentSender = 0x19,
        ResentTo = 0x1a,
        Sender = 0x1b,
        Subject = 0x1c,
        To = 0x1d,
        Unknown = -1,
        XPriority = 30,
        XReceiver = 0x1f,
        XSender = 0x20,
        ZMaxEnumValue = 0x20
    }
}
