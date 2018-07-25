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

namespace OpenNETCF.WindowsCE
{
    internal class DeviceDetail : Message
    {
        public const int MAX_DEVDETAIL_SIZE = 64 + 16 + 12;
        public const int MAX_DEVCLASS_NAMELEN = 64;

        internal byte[] data = new byte[MAX_DEVDETAIL_SIZE];

        public DeviceDetail()
        {            
           
        }

        public Guid guidDevClass
        {
            get
            {
                // Extract a 16-byte array from the structure and send that
                // to Guid to make a new Guid.
                byte[] b = new byte[16];
                Array.Copy(data, 0, b, 0, b.Length);

                return new Guid(b);
            }
        }

        public int dwReserved
        {
            get
            {
                return BitConverter.ToInt32(data, 16);
            }
        }

        public bool fAttached
        {
            get
            {
                return BitConverter.ToBoolean(data, 20);
            }
        }

        public int cbName
        {
            get
            {
                return BitConverter.ToInt32(data, 24);
            }
        }

        public string szName
        {
            get
            {
                String s = System.Text.Encoding.Unicode.GetString(data, 28, cbName);
                int l = s.IndexOf('\0');
                if (l != -1)
                    return s.Substring(0, l);
                return s;
            }
        }

        public override byte[] MessageBytes
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}
