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
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Summary description for SimCapabilities.
	/// </summary>
	public class SimCapabilities
	{
		private byte[] m_data;

		private const int Length = 124;

		private const int SIM_NUMLOCKFACILITIES = 10;
		
		/*DWORD cbSize;                           // @field Size of the structure in bytes
		DWORD dwParams;                         // @field Indicates valid parameter values
		DWORD dwPBStorages;                     // @field Supported phonebook storages
		DWORD dwMinPBIndex;                     // @field Minimum phonebook storages
		DWORD dwMaxPBIndex;                     // @field Maximum phonebook storages
		DWORD dwMaxPBEAddressLength;            // @field Maximum address length of phonebook entries
		DWORD dwMaxPBETextLength;               // @field Maximum text length of phonebook entries
		DWORD dwLockFacilities;                 // @field Supported locking facilities
		DWORD dwReadMsgStorages;                // @field Supported read message stores
		DWORD dwWriteMsgStorages;               // @field Supported write message stores
		DWORD dwNumLockingPwdLengths;           // @field Number of entries in rgLockingPwdLengths
		SIMLOCKINGPWDLENGTH rgLockingPwdLengths[SIM_NUMLOCKFACILITIES]; // @field Password lengths for each facility*/
		
		public SimCapabilities()
		{
			m_data = new byte[Length];

			//write length to first dword
			BitConverter.GetBytes(Length).CopyTo(m_data, 0);
		}

		internal byte[] ToByteArray()
		{
			return m_data;
		}

		

		
	}
}
