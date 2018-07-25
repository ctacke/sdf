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
    /// Encapsulates generic data blob
    /// </summary>
    internal struct RAW_DATA : IDisposable
    {
        //byte[] m_data;
        private uint m_cbData;
        private IntPtr m_lpData;

        public RAW_DATA(byte[] data)
        {
            //m_data = new byte[0];
            m_lpData = IntPtr.Zero;
            m_cbData = (uint)data.Length;
            lpData = data;
        }
        public uint cbData { get { return m_cbData; } }
        public byte[] lpData
        {
            get
            {
                if (m_lpData == IntPtr.Zero)
                    return null;
                byte[] data = new byte[m_cbData];
                Marshal.Copy(m_lpData, data, 0, (int)m_cbData);
                return data;
            }
            set
            {
                FreeMemory();
                m_lpData = Marshal.AllocHGlobal(value.Length);
                Marshal.Copy(value, 0, m_lpData, value.Length);
            }
        }

        public IntPtr lpDataDirect
        {
            get
            {
                return m_lpData;
            }
        }

        internal void Clear()
        {
            m_lpData = IntPtr.Zero;
            m_cbData = 0;
        }
        #region IDisposable Members

        public void Dispose()
        {
            FreeMemory();
        }

        private void FreeMemory()
        {
            if (m_lpData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_lpData);
                m_lpData = IntPtr.Zero;
            }
        }
        #endregion
    }
}
