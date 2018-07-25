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
    /// <summary>
    /// Describes the current status of the connection
    /// </summary>
    public enum ConnectionStatus : int
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// Connection is up
        /// </summary>
        Connected = 0x10,
        /// <summary>
        /// Connection is up but suspended
        /// </summary>
        Suspended = 0x11,
        /// <summary>
        /// Connection is disconnected
        /// </summary>
        Disconnected = 0x20,
        /// <summary>
        /// Connection failed and cannot not be re-established
        /// </summary>
        ConnectionFailed = 0x21,
        /// <summary>
        /// User aborted connection
        /// </summary>
        ConnectionCancelled = 0x22,
        /// <summary>
        /// Connection is ready to connect but disabled
        /// </summary>
        ConnectionDisabled = 0x23,
        /// <summary>
        /// No path could be found to destination
        /// </summary>
        NoPathToDestination = 0x24,
        /// <summary>
        /// Waiting for a path to the destination
        /// </summary>
        WaitingForPath = 0x25,
        /// <summary>
        /// Voice call is in progress
        /// </summary>
        WaitingForPhone = 0x26,
        /// <summary>
        /// Phone resource needed and phone is off
        /// </summary>
        PhoneOff = 0x27,
        /// <summary>
        /// the connection could not be established because it would multi-home an exclusive connection
        /// </summary>
        ExclusiveConflict = 0x28,
        /// <summary>
        /// Failed to allocate resources to make the connection.
        /// </summary>
        NoResources = 0x29,
        /// <summary>
        /// Connection link disconnected prematurely
        /// </summary>
        ConnectionLinkFailed = 0x2a,
        /// <summary>
        /// Failed to authenticate user.
        /// </summary>
        AuthenticationFailed = 0x2b,
        /// <summary>
        /// Attempting to connect
        /// </summary>
        WaitingConnection = 0x40,
        /// <summary>
        /// Resource is in use by another connection
        /// </summary>
        WaitingForResource = 0x41,
        /// <summary>
        /// No path could be found to destination
        /// </summary>
        WaitingForNetwork = 0x42,
        /// <summary>
        /// Connection is being brought down
        /// </summary>
        WaitingDisconnection = 0x80,
        /// <summary>
        /// Aborting connection attempt
        /// </summary>
        WaitingConnectionAbort = 0x81,
    };
}
