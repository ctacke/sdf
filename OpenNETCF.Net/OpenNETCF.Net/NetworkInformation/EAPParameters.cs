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

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// EAP Wireless LAN authentication descriptor
    /// </summary>
    public class EAPParameters
    {
        byte[] data;
        int baseOffset;

        internal EAPParameters(byte[] rd, int offset)
        {
            // The data here will normally be part of a
            // WZC_WLAN_CONFIG structure.  However, there's
            // another constructor if you need to build one
            // of these objects without referencing it to 
            // an existing data block.
            data = rd;
            baseOffset = offset;
        }

        /// <summary>
        /// Create an 'empty' instance of the EAP configuration
        /// parameter structure.
        /// </summary>
        public EAPParameters()
        {
            // Create a self-contained parameter structure.
            data = new byte[SizeOf];
            baseOffset = 0;
        }

        internal const int Enable8021xOffset = 0;
        internal const int EapFlagsOffset = 4;
        internal const int EapTypeOffset = 8;
        internal const int AuthDataLenOffset = 12;
        internal const int AuthDataOffset = 16;

        /// <summary>
        /// Enable 802.1x authentication
        /// </summary>
        public bool Enable8021x
        {
            get
            {
                return BitConverter.ToBoolean(data, baseOffset + Enable8021xOffset);
            }
            set
            {
                // This is a bit tricky.  When BOOL is used
                // in C, that takes up 4 bytes, but bool in
                // C# is one byte.  We need to set all four
                // bytes, so I check the value and build a
                // suitable array of bytes.
                byte[] bytes;
                if (value)
                {
                    bytes = new byte[] { 0xff, 0xff, 0xff, 0xff };
                }
                else
                {
                    bytes = new byte[] { 0, 0, 0, 0 };
                }
                Buffer.BlockCopy(bytes, 0, data, baseOffset + Enable8021xOffset, 4);
            }
        }

        /// <summary>
        /// EAP flags
        /// </summary>
        public EAPFlags EapFlags
        {
            get
            {
                return (EAPFlags)BitConverter.ToUInt32(data, baseOffset + EapFlagsOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((int)value);
                Buffer.BlockCopy(bytes, 0, data, baseOffset + EapFlagsOffset, 4);
            }
        }

        /// <summary>
        /// EAP type
        /// </summary>
        public EAPType EapType
        {
            get
            {
                return (EAPType)BitConverter.ToInt32(data, baseOffset + EapTypeOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((int)value);
                Buffer.BlockCopy(bytes, 0, data, baseOffset + EapTypeOffset, 4);
            }
        }

        /// <summary>
        /// Length of EAP provider-specific authentication data
        /// </summary>
        public int AuthDataLen
        {
            get
            {
                return BitConverter.ToInt32(data, baseOffset + AuthDataLenOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, data, baseOffset + AuthDataLenOffset, 4);
            }
        }

        /// <summary>
        /// The actual provider-specific authentication data
        /// </summary>
        public IntPtr AuthData
        {
            get
            {
                return (IntPtr)BitConverter.ToUInt32(data, baseOffset + AuthDataOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes((uint)value);
                Buffer.BlockCopy(bytes, 0, data, baseOffset + AuthDataOffset, 4);
            }
        }

        /// <summary>
        /// Return ***copy*** of data in the structure.
        /// </summary>
        public byte[] Data
        {
            get
            {
                byte[] d = new byte[SizeOf];
                Buffer.BlockCopy(data, baseOffset, d, 0, SizeOf);

                return d;
            }
        }

        /// <summary>
        /// The size of the 'structure' which this class
        /// represents, in bytes
        /// </summary>
        public const int SizeOf = 20;
    }
}