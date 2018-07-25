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
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Time-Zone
    /// </summary>
    internal class TZREG
    {
        // Declare array of bytes to represent the structure
        // contents in a form which can be marshalled.
        private byte[] flatStruct = new byte[4 + 4 + 4 + 16 + 16];

        #region Flat structure offset constants
        private const int biasOffset = 0;
        private const int standardBiasOffset = 4;
        private const int daylightBiasOffset = 4 + 4;
        private const int standardDateOffset = 4 + 4 + 4;
        private const int daylightDateOffset = 4 + 4 + 4 + 16 /* sizeof( SYSTEMTIME ) */;
        #endregion

        public TZREG()
        {
        }

        public TZREG(byte[] bytes)
            :
            this(bytes, 0)
        {
        }

        public TZREG(byte[] bytes, int offset)
        {
            // Dump the byte array into our array.
            Buffer.BlockCopy(bytes, offset, flatStruct, 0, flatStruct.Length);
        }

        public byte[] ToByteArray()
        {
            return flatStruct;
        }

        public static implicit operator byte[](TZREG tzr)
        {
            return tzr.ToByteArray();
        }

        public int Bias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, biasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, biasOffset, 4);
            }
        }
        public int StandardBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, standardBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, standardBiasOffset, 4);
            }
        }
        public int DaylightBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, daylightBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightBiasOffset, 4);
            }
        }
        public SystemTime StandardDate
        {
            get
            {
                return new SystemTime(flatStruct, standardDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, standardDateOffset, 16);
            }
        }
        public SystemTime DaylightDate
        {
            get
            {
                return new SystemTime(flatStruct, daylightDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightDateOffset, 16);
            }
        }
    }
}
