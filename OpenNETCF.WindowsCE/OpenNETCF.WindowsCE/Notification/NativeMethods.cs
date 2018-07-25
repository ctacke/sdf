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
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
    internal static class NativeMethods
    {
        //Notify APIs
        [DllImport("coredll.dll", EntryPoint = "CeRunAppAtEvent", SetLastError = true)]
        internal static extern bool CeRunAppAtEvent(string pwszAppName, int lWhichEvent);

        [DllImport("coredll.dll", EntryPoint = "CeRunAppAtTime", SetLastError = true)]
        internal static extern bool CeRunAppAtTime(string pwszAppName, ref SYSTEMTIME lpTime);

		[DllImport("coredll.dll", EntryPoint = "CeRunAppAtTime", SetLastError = true)]
		internal static extern bool CeRunAppAtTimeCancel(string pwszAppName, byte[] lpTime);

        [DllImport("coredll.dll", EntryPoint = "CeSetUserNotificationEx", SetLastError = true)]
        internal static extern int CeSetUserNotificationEx(int hNotification, UserNotificationTrigger lpTrigger, UserNotification lpUserNotification);

        [DllImport("coredll.dll", EntryPoint = "CeSetUserNotification", SetLastError = true)]
        internal static extern int CeSetUserNotification(int hNotification, string pwszAppName, ref SYSTEMTIME lpTime, UserNotification lpUserNotification);

        [DllImport("coredll.dll", EntryPoint = "CeClearUserNotification", SetLastError = true)]
        internal static extern bool CeClearUserNotification(int hNotification);

        [DllImport("coredll.dll", EntryPoint = "CeGetUserNotification", SetLastError = true)]
        internal static extern bool CeGetUserNotification(int hNotification, uint cBufferSize, ref int pcBytesNeeded, IntPtr pBuffer);

        [DllImport("coredll.dll", EntryPoint = "CeGetUserNotificationHandles", SetLastError = true)]
        internal static extern bool CeGetUserNotificationHandles(int[] rghNotifications, int cHandles, ref int pcHandlesNeeded);

        [DllImport("coredll.dll", EntryPoint = "CeGetUserNotificationPreferences", SetLastError = true)]
        internal static extern bool CeGetUserNotificationPreferences(IntPtr hWndParent, UserNotification lpNotification);

        [DllImport("coredll.dll", EntryPoint = "CeHandleAppNotifications", SetLastError = true)]
        internal static extern bool CeHandleAppNotifications(string appName);

        //NLed APIs

        [DllImport("coredll.dll", EntryPoint = "NLedGetDeviceInfo", SetLastError = true)]
        internal extern static bool NLedGetDeviceCount(short nID, ref OpenNETCF.WindowsCE.Notification.Led.NLED_COUNT_INFO pOutput);

        [DllImport("coredll.dll", EntryPoint = "NLedGetDeviceInfo", SetLastError = true)]
        internal extern static bool NLedGetDeviceSupports(short nID, ref Led.NLED_SUPPORTS_INFO pOutput);


        [DllImport("coredll.dll", EntryPoint = "NLedSetDevice", SetLastError = true)]
        internal extern static bool NLedSetDevice(short nID, ref Led.NLED_SETTINGS_INFO pOutput);

    }
}
