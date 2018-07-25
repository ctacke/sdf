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

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        private static void DoCellcoreCheck()
        {
            IntPtr hLib = SafeNativeMethods.LoadLibrary("cellcore.dll");

            if (hLib != IntPtr.Zero)
            {
                SafeNativeMethods.FreeLibrary(hLib);
                return;
            }

            throw new PlatformNotSupportedException("cellcore.dll not found on platform.");
        }

        partial class SafeNativeMethods
        {		
            internal const uint INSUFFICIENT_BUFFER = 0x8007007a;

            #region --------- API Prototypes ---------
            [DllImport("cellcore.dll", SetLastError = true)] //only available on WM5+
            internal static extern int ConnMgrRegisterForStatusChangeNotification(bool enable, IntPtr hWnd);

            [DllImport("cellcore.dll", SetLastError = true)]
            extern public static uint ConnMgrQueryDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS stat, ref int size);

            [DllImport("coredll.dll", SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("coredll.dll", SetLastError = true)]
            public static extern int FreeLibrary(IntPtr hModule);
         
		    #endregion
        }
    }
}
