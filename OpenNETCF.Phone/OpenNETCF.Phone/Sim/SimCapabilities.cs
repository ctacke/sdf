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
