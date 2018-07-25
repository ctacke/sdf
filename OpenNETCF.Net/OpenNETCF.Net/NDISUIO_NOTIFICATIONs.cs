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
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.Net
{
    internal class NDISUIO_REQUEST_NOTIFICATION
    {
        public const int Size = 8;
        byte[] data = new byte[Size];

        public IntPtr hMsgQueue
        {
            get
            {
                return (IntPtr)BitConverter.ToUInt32(data, 0);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((UInt32)value);
                Buffer.BlockCopy(bytes, 0, data, 0, 4);
            }
        }

        public uint dwNotificationTypes
        {
            get
            {
                return BitConverter.ToUInt32(data, 4);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, data, 4, 4);
            }
        }

        public byte[] getBytes()
        {
            return data;
        }
    }
}
