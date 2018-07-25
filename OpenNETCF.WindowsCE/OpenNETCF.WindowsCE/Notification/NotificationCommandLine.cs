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
    /// Strings passed on the command line when an event occurs that the app has requested via CeRunAppAtEvent.  
    /// </summary>
    /// <remarks>Note that some of these strings will be used as the command line *prefix*, since the rest of the command line will be used as a parameter.</remarks>
    public enum NotificationCommandLine
    {
        /// <summary>
        /// String passed on the command line when an app is run as the result of a call to <see cref="M:OpenNETCF.Win32.Notify.Notify.RunAppAtTime"/>.
        /// </summary>
        AppRunAtTime,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
        /// </summary>
        AppRunToHandleNotification,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the system time settings are changed.
        /// </summary>
        AppRunAfterTimeChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after synchronisation.
        /// </summary>
        AppRunAfterSync,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device is connected to AC power.
        /// </summary>
        AppRunAtAcPowerOn,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the AC power is disconnected.
        /// </summary>
        AppRunAtAcPowerOff,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device connects to a LAN.
        /// </summary>
        AppRunAtNetConnect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device disconnects from a LAN.
        /// </summary>
        AppRunAtNetDisconnect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application that "owns" a notification.  It is followed by a space, and the stringized version of the notification handle.
        /// </summary>
        AppRunAtDeviceChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when another device is discovered using IR.
        /// </summary>
        AppRunAtIrDiscovery,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when a serial port connection is attempted.
        /// </summary>
        AppRunAtRs232Detect,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after a system restore.
        /// </summary>
        AppRunAfterRestore,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device wakes up from standby.
        /// </summary>
        AppRunAfterWakeup,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application when the device time-zone is changed.
        /// </summary>
        AppRunAfterTzChange,
        /// <summary>
        /// Prefix of the command line when the user requests to run the application after an extended event.
        /// </summary>
        AppRunAfterExtendedEvent,
    }
}
