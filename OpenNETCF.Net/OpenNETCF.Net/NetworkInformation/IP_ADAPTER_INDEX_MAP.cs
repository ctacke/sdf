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
    /*
        #define MAX_ADAPTER_NAME 128
     * 
       typedef struct _IP_ADAPTER_INDEX_MAP {
          ULONG Index; 
          WCHAR Name [MAX_ADAPTER_NAME]; 
        } IP_ADAPTER_INDEX_MAP, *PIP_ADAPTER_INDEX_MAP;
    */
    internal class IP_ADAPTER_INDEX_MAP
    {
        private const int NAME_OFFSET = 4;
        private const int NAME_LENGTH = (128 * 2); // WCHARs
        private const int SIZE = 4 + NAME_LENGTH; 

        private byte[] m_data;
        
        public static implicit operator byte[](IP_ADAPTER_INDEX_MAP map)
        {
            return map.m_data;
        }

        public IP_ADAPTER_INDEX_MAP()
        {
            m_data = new byte[SIZE];
        }

        public int Index
        {
            set { Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, 0, 4); }
            get { return BitConverter.ToInt32(m_data, 0); }
        }

        public string Name
        {
            set
            {
                byte[] data = Encoding.Unicode.GetBytes(value);
                Buffer.BlockCopy(data, 0, m_data, NAME_OFFSET, data.Length);
            }
            get
            {
                string name = Encoding.Unicode.GetString(m_data, NAME_OFFSET, NAME_LENGTH);
                int nullIndex = name.IndexOf('\0');
                if(nullIndex >=0)
                {
                    name = name.Substring(0, nullIndex);
                }
                return name;
            }
        }
    }
}
