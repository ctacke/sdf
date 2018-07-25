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
    internal sealed class NDIS : OpenNETCF.IO.StreamInterfaceDriver
    {
        public const uint IOCTL_NDIS_BIND_ADAPTER = 0x00170032;
        public const uint IOCTL_NDIS_REBIND_ADAPTER = 0x0017002e;
        public const uint IOCTL_NDIS_UNBIND_ADAPTER = 0x00170036;

        private NDIS()
            : base("NDS0:")
        {
        }

        public static void BindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_BIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static void UnbindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_UNBIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }

        public static void RebindInterface(string adapterName)
        {
            NDIS ndis = new NDIS();

            ndis.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

            try
            {
                byte[] nameBytes = Encoding.Unicode.GetBytes(adapterName);

                ndis.DeviceIoControl(IOCTL_NDIS_REBIND_ADAPTER, nameBytes, null);
            }
            finally
            {
                ndis.Dispose();
            }
        }
    }
}
