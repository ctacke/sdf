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

namespace OpenNETCF.Net
{
    /// <summary>
    /// The NdisNotificationType enumeration defines the types
    /// of notifications which can be requested from NDIS.
    /// </summary>
    public enum NdisNotificationType
    {
        /// <summary>
        /// NdisResetStart is set when an adapter is being
        /// reset.
        /// </summary>
        NdisResetStart = 0x00000001,

        /// <summary>
        /// NdisResetEnd is sent when the reset process on an
        /// adapter is complete and the adapter is ready to be
        /// rebound.
        /// </summary>
        NdisResetEnd = 0x00000002,

        /// <summary>
        /// NdisMediaConnect is set when the communications
        /// media, an Ethernet cable for example, is connected
        /// to the adapter.
        /// </summary>
        NdisMediaConnect = 0x00000004,

        /// <summary>
        /// NdisMediaDisconnect is set when the communciations
        /// media, an Ethernet cable for example, is disconnected
        /// from the adapter.
        /// </summary>
        NdisMediaDisconnect = 0x00000008,

        /// <summary>
        /// NdisBind is set when one or more protocols, TCP/IP
        /// typically, is bound to the adapter.
        /// </summary>
        NdisBind = 0x00000010,

        /// <summary>
        /// NdisUnbind is set when the adapter is unbound from
        /// one or more protocols.
        /// </summary>
        NdisUnbind = 0x00000020,

        /// <summary>
        /// NdisMediaSpecific is set when some notification not
        /// generally defined for all adapter types occurred.
        /// </summary>
        NdisMediaSpecific = 0x00000040,
    }
}
