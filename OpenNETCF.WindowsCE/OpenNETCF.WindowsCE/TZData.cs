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

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Timezone Data
    /// </summary>
    public struct TZData
    {
        /// <summary>
        /// Creates a TZData instance
        /// </summary>
        /// <param name="pData"></param>
        public TZData(IntPtr pData)
        {
            Name = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            ShortName = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            DSTName = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            GMTOffset = Marshal.ReadInt32(pData);
            pData = (IntPtr)(pData.ToInt32() + 8);
            DSTOffset = Marshal.ReadInt32(pData);
        }

        /// <summary>
        /// Timezone's full name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Timezone's DST name
        /// </summary>
        public readonly string DSTName;
        /// <summary>
        /// Timezone's short name
        /// </summary>
        public readonly string ShortName;
        /// <summary>
        /// Timezone's offset from GMT (in minutes)
        /// </summary>
        public readonly int GMTOffset;
        /// <summary>
        /// Timezone's DST offset (in minutes)
        /// </summary>
        public readonly int DSTOffset;

        /// <summary>
        /// Returns the timezone's short name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ShortName;
        }
    }
}
