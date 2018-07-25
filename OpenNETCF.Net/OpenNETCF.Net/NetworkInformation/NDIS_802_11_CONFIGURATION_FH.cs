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
    internal class NDIS_802_11_CONFIGURATION_FH
    {
        internal byte[] data;
        internal int offset;

        protected const int LengthOffset = 0;
        protected const int HopPatternOffset = 4;
        protected const int HopSetOffset = 8;
        protected const int DwellTimeOffset = 12;

        public NDIS_802_11_CONFIGURATION_FH(byte[] d, int o)
        {
            data = d;
            offset = o;
        }

        public uint Length
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + LengthOffset);
            }
        }

        public uint HopPattern
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + HopPatternOffset);
            }
        }

        public uint HopSet
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + HopSetOffset);
            }
        }

        /// <summary>
        /// Amount of time dwelling in each frequency (in kusec).
        /// </summary>
        public uint DwellTime
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + DwellTimeOffset);
            }
        }
    }
}
