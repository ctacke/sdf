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
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// List of interface names
    /// </summary>
    internal struct INTFS_KEY_TABLE : IDisposable
    {
        private uint dwNumIntfs;
        public IntPtr pData;

        public INTFS_KEY_TABLE(uint size)
        {
            dwNumIntfs = size;
            if (size != 0)
            {
                pData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * (int)size);
            }
            else
                pData = IntPtr.Zero;
        }

        public string this[uint i]
        {
            get
            {
                return Marshal.PtrToStringUni((IntPtr)(Marshal.ReadInt32(pData, (int)i * 4)));
            }
        }

        public uint Count
        {
            get { return dwNumIntfs; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (pData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        #endregion
    }
}
