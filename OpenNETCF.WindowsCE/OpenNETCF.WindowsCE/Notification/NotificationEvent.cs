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

namespace OpenNETCF.WindowsCE.Notification
{
    /// <summary>   
    /// System Event Flags   
    /// </summary>   
    public enum NotificationEvent : int
    {
        /// <summary>   
        /// No events�remove all event registrations for this application.   
        /// </summary>   
        None = 0x00,
        /// <summary>   
        /// When the system time is changed.   
        /// </summary>   
        TimeChange = 0x01,
        /// <summary>   
        /// When data synchronization finishes.   
        /// </summary>   
        SyncEnd = 0x02,
        /// <summary>
        /// The user turned the AC power on.
        /// </summary>
        OnACPower = 0x03,
        /// <summary>
        /// The user turned the alternating current (AC) power off.
        /// </summary>
        OffACPower = 0x04,
        /// <summary>
        /// The device connected to a network.
        /// </summary>
        NetConnect = 0x05,
        /// <summary>
        /// The device disconnected from a network.
        /// </summary>
        NetDisconnect = 0x06,
        /// <summary>   
        /// When a PC Card device is changed.   
        /// </summary>   
        DeviceChange = 0x07,
        /// <summary>
        /// The device discovered a server by using infrared communications.
        /// </summary>
        IRDiscovered = 0x08,
        /// <summary>   
        /// When an RS232 connection is made.   
        /// </summary>   
        RS232Detected = 0x09,
        /// <summary>   
        /// When a full device data restore completes.   
        /// </summary>   
        RestoreEnd = 0x0A,
        /// <summary>   
        /// When the device wakes up.   
        /// </summary>   
        Wakeup = 0x0B,
        /// <summary>   
        /// When the time zone is changed.   
        /// </summary>   
        TimeZoneChange = 0x0C,
        /// <summary>
        /// When the machines name changes.
        /// Requires Windows CE.NET 4.2.
        /// </summary>
        MachineNameChange = 0x0D,
        /// <summary>
        /// RNDISFN interface is instantiated.
        /// Requires Windows CE 5.0.
        /// </summary>
        RndisFNDetected = 0x0E,
        /// <summary>
        /// The Internet Proxy used by the device has changed.
        /// Requires Windows CE 5.0.
        /// </summary>
        InternetProxyChange = 0x0f,
    } 
}
