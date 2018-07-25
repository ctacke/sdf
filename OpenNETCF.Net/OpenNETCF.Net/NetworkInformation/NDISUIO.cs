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
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net.NetworkInformation
{
    internal sealed class NDISUIO : OpenNETCF.IO.StreamInterfaceDriver
    {
        public const uint IOCTL_NDISUIO_QUERY_OID_VALUE = 0x120804;
        public const uint IOCTL_NDISUIO_SET_OID_VALUE = 0x120814;

        private const uint IOCTL_NDISUIO_REQUEST_NOTIFICATION = 0x12081c;
        public const uint IOCTL_NDISUIO_CANCEL_NOTIFICATION = 0x120820;

        private const uint NDISUIO_NOTIFICATION_MEDIA_SPECIFIC_NOTIFICATION = 0x00000040;
        private const uint ALL_NOTIFICATIONS = NDISUIO_NOTIFICATION_MEDIA_SPECIFIC_NOTIFICATION | (uint)
            (InterfaceStatus.Bind |
            InterfaceStatus.MediaConnect |
            InterfaceStatus.MediaDisconnect |
            InterfaceStatus.ResetEnd |
            InterfaceStatus.ResetStart |
            InterfaceStatus.Unbind);

        private NDISUIO()
            : base("UIO1:")
        {
        }

        public static void RequestNotifications(P2PMessageQueue rxQueue)
        {
            NDISUIO ndis = new NDISUIO();

            try
            {
                NDISUIO_REQUEST_NOTIFICATION request = new NDISUIO_REQUEST_NOTIFICATION();
                request.hMsgQueue = rxQueue.Handle;
                request.dwNotificationTypes = ALL_NOTIFICATIONS;

                ndis.DeviceIoControl(IOCTL_NDISUIO_REQUEST_NOTIFICATION, request.GetBytes(), null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static void CancelNotifications()
        {
            NDISUIO ndis = new NDISUIO();
            try
            {
                ndis.DeviceIoControl(IOCTL_NDISUIO_CANCEL_NOTIFICATION, null, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static bool SetOID(NDIS_OID oid, string adapterName, byte[]data)
        {
            NDISQueryOid query = new NDISQueryOid(data.Length);
            query.Data = data;

            return SetOID(oid, adapterName, query);
        }

        public static bool SetOID(NDIS_OID oid, string adapterName)
        {
            return SetOID(oid, adapterName, null);
        }

        public unsafe static bool SetOID(NDIS_OID oid, string adapterName, NDISQueryOid queryOID)
        {
            NDISUIO ndis = new NDISUIO();

            if (queryOID == null)
            {
                queryOID = new NDISQueryOid(0);
            }

            byte[] nameBytes = System.Text.Encoding.Unicode.GetBytes(adapterName + '\0');
            fixed (byte* pName = &nameBytes[0])
            {
                queryOID.ptcDeviceName = pName;

                queryOID.Oid = (uint)oid;

                try
                {
                    ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                }
                catch
                {
                    throw new NetworkInformationException(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                }
                try
                {
                    ndis.DeviceIoControl(IOCTL_NDISUIO_SET_OID_VALUE, queryOID.getBytes(), queryOID.getBytes());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            ndis.Dispose();

            return true;
        }

        public unsafe static byte[] QueryOID(NDIS_OID oid, string adapterName)
        {
            if ((adapterName == null) || (adapterName == string.Empty))
            {
                // no point in even trying to call NDIS if this is the case
                return null;
            }

            NDISUIO ndis = new NDISUIO();

            NDISQueryOid queryOID;
            byte[] result;

            try
            {

                switch (oid)
                {
                    case NDIS_OID.RSSI:
                    case NDIS_OID.WEP_STATUS:
                    case NDIS_OID.BSSID_LIST_SCAN:
                    case NDIS_OID.AUTHENTICATION_MODE:
                    case NDIS_OID.INFRASTRUCTURE_MODE:
                    case NDIS_OID.NETWORK_TYPE_IN_USE:
                        queryOID = new NDISQueryOid(4);	    // The data is a four-byte signed int
                        break;
                    case NDIS_OID.BSSID:
                        queryOID = new NDISQueryOid(36);
                        break;
                    case NDIS_OID.SUPPORTED_RATES:
                        queryOID = new NDISQueryOid(8);
                        break;
                    case NDIS_OID.CONFIGURATION:
                        queryOID = new NDISQueryOid(32);
                        break;
                    case NDIS_OID.SSID:
                        queryOID = new NDISQueryOid(36);	// The data is a four-byte length plus 32-byte ASCII string
                        break;
                    case NDIS_OID.BSSID_LIST:
                        queryOID = new NDISQueryOid(2000);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("'NDIS_OID_{0}' Not supported", oid.ToString()));
                }

                byte[] nameBytes = System.Text.Encoding.Unicode.GetBytes(adapterName + '\0');

                fixed (byte* pName = &nameBytes[0])
                {
                    queryOID.ptcDeviceName = pName;
                    queryOID.Oid = (uint)oid;

                    try
                    {
                        ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                    }
                    catch
                    {
                        throw new NetworkInformationException(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                    }
                    try
                    {
                        ndis.DeviceIoControl(IOCTL_NDISUIO_QUERY_OID_VALUE, queryOID.getBytes(), queryOID.getBytes());
                    }
                    catch (Exception)
                    {
                        // if it's an attempt to get a list it might be an OOM
                        // are we trying to get a list?
                        if (oid != NDIS_OID.BSSID_LIST)
                        {
                            throw new NetworkInformationException();
                        }

                        // maybe just an OOM - try again
                        GC.Collect();
                        queryOID = new NDISQueryOid(8000);
                        queryOID.ptcDeviceName = pName;
                        queryOID.Oid = (uint)oid;
                        ndis.DeviceIoControl(IOCTL_NDISUIO_QUERY_OID_VALUE, queryOID.getBytes(), queryOID.getBytes());
                    }
                    finally
                    {
                        ndis.Close();
                    }

                    result = new byte[queryOID.Data.Length];
                    Buffer.BlockCopy(queryOID.Data, 0, result, 0, result.Length);
                }
            }
            finally
            {
                ndis.Dispose();
            }
            return result;
        }
    }
}
