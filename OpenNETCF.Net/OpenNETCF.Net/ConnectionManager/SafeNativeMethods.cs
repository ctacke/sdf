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
        partial class SafeNativeMethods
        {		
            #region --------- API Prototypes ---------
            //[DllImport("cellcore.dll", EntryPoint = "ConnMgrRegisterForStatusChangeNotification", SetLastError = true)] //only available on WM5+
            //internal static extern int ConnMgrRegisterForStatusChangeNotification(bool enable, IntPtr hWnd);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrReleaseConnection",SetLastError=true)]
            internal static extern int ConnMgrReleaseConnection(IntPtr hConnection, int bCache);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnection",SetLastError=true)]
            internal static extern int ConnMgrEstablishConnection(ref ConnectionInfo ConnInfo, out IntPtr phConnection);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEstablishConnectionSync",SetLastError=true)]
            internal static extern int ConnMgrEstablishConnectionSync(ref ConnectionInfo ConnInfo, out IntPtr phConnection, uint dwTimeout, out uint pdwStatus);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrEnumDestinations",SetLastError=true)]
		    internal static extern int ConnMgrEnumDestinations(int nIndex, IntPtr pDestinationInfo);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrMapURL",SetLastError=true)]
		    internal static extern int ConnMgrMapURL(string pwszUrl, ref Guid pguid, IntPtr pdwIndex);

		    [DllImport("cellcore.dll",EntryPoint="ConnMgrConnectionStatus",SetLastError=true)]
		    internal static extern int ConnMgrConnectionStatus(IntPtr hConnection, out uint pdwStatus);
            
            //[DllImport("cellcore")]
            //extern public static uint ConnMgrQueryDetailedStatus(CONNMGR_CONNECTION_DETAILED_STATUS stat, ref int size);

            [DllImport("cellcore")]
            extern public static uint ConnMgrQueryDetailedStatus(IntPtr pStat, ref int size);

            [DllImport("cellcore.dll")]
            internal static extern IntPtr ConnMgrApiReadyEvent();

            [DllImport("coredll.dll")]
            internal static extern int CloseHandle(IntPtr hObject);

		    #endregion
        }
    }
}
