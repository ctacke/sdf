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
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Services
{
    /*
        typedef struct _ServiceEnumInfo {
	        WCHAR   szPrefix[6];     
	        WCHAR   *szDllName;
	        HANDLE  hServiceHandle;
	        DWORD   dwServiceState;   // one of SERVICE_STATE_XXX values above.
        } ServiceEnumInfo;
    */

    internal class ServiceEnumInfo
    {
        private const int PREFIX_OFFSET = 0;
        private const int PREFIX_LENGTH = 12;
        private const int DLL_OFFSET = PREFIX_OFFSET + PREFIX_LENGTH;
        private const int DLL_LENGTH = 4;
        private const int HANDLE_OFFSET = DLL_OFFSET + DLL_LENGTH;
        private const int HANDLE_LENGTH = 4;
        private const int STATE_OFFSET = HANDLE_OFFSET + HANDLE_LENGTH;
        private const int STATE_LENGTH = 4;

        public const int SIZE = STATE_OFFSET + STATE_LENGTH;

        private byte[] m_data = new byte[SIZE];

        internal ServiceEnumInfo(byte[] data, int srcOffset)
        {
            Buffer.BlockCopy(data, srcOffset, m_data, 0, SIZE);
        }

        internal ServiceEnumInfo(byte[] data)
        {
            Buffer.BlockCopy(data, 0, m_data, 0, SIZE);
        }

        public static implicit operator byte[](ServiceEnumInfo sei) { return sei.m_data; }
        public static implicit operator ServiceEnumInfo(byte[] data) { return new ServiceEnumInfo(data); }

        public string Prefix
        {
            get 
            { 
                string prefix = Encoding.Unicode.GetString(m_data, PREFIX_OFFSET, PREFIX_LENGTH);
                return prefix.Substring(0, prefix.IndexOf('\0'));
            }
        }

        public unsafe string DLLName
        {
            get
            {
                int address = BitConverter.ToInt32(m_data, DLL_OFFSET);
                string name = Marshal.PtrToStringUni(new IntPtr(address));
                return name;
            }
        }

        public IntPtr Handle
        {
            get { return new IntPtr(BitConverter.ToInt32(m_data, HANDLE_OFFSET)); }
        }

        public ServiceState State
        {
            get
            {
                return (ServiceState)BitConverter.ToInt32(m_data, STATE_OFFSET);
            }
        }
    }
}
