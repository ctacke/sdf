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
	/// Provides access to the SIM card and its data on Pocket PC Phone Edition and Smartphone 2003 devices.
	/// </summary>
	public class Sim
	{
		/// <summary>
		/// Handle to the SIM.
		/// </summary>
		private IntPtr hsim;

		/// <summary>
		/// Store raw capabilities data
		/// </summary>
		private byte[] m_data;

		/// <summary>
		/// Length of capabilties data in bytes
		/// </summary>
		private const int Length = 124;

		/// <summary>
		/// Number of supported locking facilities
		/// </summary>
		private const int SIM_NUMLOCKFACILITIES = 10;

		/// <summary>
		/// Used to retrieve all capabilities.
		/// </summary>
		private const int SIM_CAPSTYPE_ALL = 0x0000003F;

		/// <summary>
		/// Create a new instance of SIM.
		/// It is not recommended that you have more than one SIM object open in your application at one time.
		/// </summary>
		public Sim()
		{
			//init sim with no notifications
			int hresult = SimInitialize(0, IntPtr.Zero, 0, ref hsim);

			if(hresult != 0)
			{
				throw new ExternalException("Error initialising SIM");
			}

			//create buffer for capabilities
			m_data = new byte[Length];

			//write length to first dword
			BitConverter.GetBytes(Length).CopyTo(m_data, 0);

			//retrieve all SIM capabilities
			hresult = SimGetDevCaps(hsim, SIM_CAPSTYPE_ALL, m_data);

			if(hresult != 0)
			{
				throw new ExternalException("Error retrieving SIM capabilities");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		~Sim()
		{
			//free the hsim
			Close();
		}

		/// <summary>
		/// The Sim Handle from SimInitialize.
		/// </summary>
		internal IntPtr Handle
		{
			get
			{
				return hsim;
			}
		}

		/// <summary>
		/// Closes the connection to the SIM releasing the handle.
		/// </summary>
		public void Close()
		{
			if(hsim != IntPtr.Zero)
			{
				int hresult = SimDeinitialize(hsim);

				if(hresult != 0)
				{
					throw new ExternalException("Error deinitializing SIM");
				}

				hsim = IntPtr.Zero;
			}

		}

		public MessageCollection BroadcastMessages
		{
			get
			{
				return new MessageCollection(this, MessageCollection.SmsStorage.Broadcast);
			}
		}

		public MessageCollection SimMessages
		{
			get
			{
				return new MessageCollection(this, MessageCollection.SmsStorage.Sim);
			}
		}

		public Phonebook Phonebook
		{
			get
			{
				return new Phonebook(this, PhonebookStorage.SIM);
			}
		}

		public Phonebook EmergencyNumbers
		{
			get
			{
				return new Phonebook(this, PhonebookStorage.Emergency);
			}
		}

		public Phonebook OwnNumbers
		{
			get
			{
				return new Phonebook(this, PhonebookStorage.OwnNumbers);
			}
		}
		

		/// <summary>
		/// Gets a value indicating whether the SIM is currently awaiting a password.
		/// </summary>
		public LockedState LockedState
		{
			get
			{
				LockedState state = 0;

				int hresult = SimGetPhoneLockedState(hsim, ref state);

				return state;
			}
		}

		private CapabilityFlags Flags
		{
			get
			{
				return (CapabilityFlags)BitConverter.ToInt32(m_data, 4);
			}
		}

		/// <summary>
		/// Gets the number of supported phonebook storages.
		/// </summary>
		public PhonebookStorage PhonebookStorage
		{
			get
			{
				return (PhonebookStorage)BitConverter.ToInt32(m_data, 8);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int MinimumPhonebookIndex
		{
			get
			{
				return BitConverter.ToInt32(m_data, 12) - 1;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int MaximumPhonebookIndex
		{
			get
			{
				return BitConverter.ToInt32(m_data, 16) - 1;
			}
		}

		/// <summary>
		/// Gets the maximum address length of phonebook entries.
		/// </summary>
		public int MaximumPhonebookAddressLength
		{
			get
			{
				return BitConverter.ToInt32(m_data, 20);
			}
		}

		/// <summary>
		/// Gets the maximum text length of phonebook entries.
		/// </summary>
		public int MaximumPhonebookTextLength
		{
			get
			{
				return BitConverter.ToInt32(m_data, 24);
			}
		}

		/// <summary>
		/// Gets the number of supported locking facilities.
		/// </summary>
		public int LockFacilitiesCount
		{
			get
			{
				return BitConverter.ToInt32(m_data, 28);
			}
		}

		/// <summary>
		/// Gets the number of supported read message stores.
		/// </summary>
		public int ReadMessageStoresCount
		{
			get
			{
				return BitConverter.ToInt32(m_data, 32);
			}
		}

		/// <summary>
		/// Gets the number of supported write message stores.
		/// </summary>
		public int WriteMessageStoresCount
		{
			get
			{
				return BitConverter.ToInt32(m_data, 36);
			}
		}

		private int LockingPasswordCount
		{
			get
			{
				return BitConverter.ToInt32(m_data, 40);
			}
		}

		public LockingPassword[] LockingPasswords
		{
			get
			{
				int icount = 0;
				ArrayList al = new ArrayList();
				int LockingPasswordSize = Marshal.SizeOf(typeof(LockingPassword));

				for(icount = 0; icount < LockingPasswordCount; icount++)
				{
					LockingPassword lp = new LockingPassword();
					//set the values
					lp.Facility = (LockingFacility)BitConverter.ToInt32(m_data, 44 + (LockingPasswordSize * icount));
					lp.PasswordLength = BitConverter.ToInt32(m_data, 48 + (LockingPasswordSize * icount));
					//add this lockingpassword to the collection
					al.Add(lp);
				}

				return (LockingPassword[])al.ToArray(typeof(LockingPassword));
			}
		}

		/// <summary>
		/// Describes the minimum password length for the SIM.
		/// </summary>
		public struct LockingPassword
		{
			/// <summary>
			/// The locking facility.
			/// </summary>
			public LockingFacility Facility;
			/// <summary>
			/// The minimum password length.
			/// </summary>
			public int PasswordLength;
		}

		#region API Functions

		[DllImport("cellcore.dll")]
		private static extern int SimInitialize(
			int dwFlags,                          // @parm Indicates which notifications to receive
			IntPtr lpfnCallBack,               // @parm Function callback for notifications, may be NULL if notifications are not desired
			int dwParam,                          // @parm Parameter to pass on each notification function call, may be NULL
			ref IntPtr lphSim                           // @parm Points to a HSIM handle to use on subsequent function calls
			);

		[DllImport("cellcore.dll")]
		private static extern int SimDeinitialize(
			IntPtr hSim                               // @parm A valid HSIM handle to deinitialize
			);

		[DllImport("cellcore.dll")]
		private static extern int SimGetDevCaps(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwCapsType,                       // @parm Which device capabilities are we interested in?
			byte[] lpSimCaps                     // @parm Capabilities structure
			);

		

		[DllImport("cellcore.dll")]
		private static extern int SimGetPhoneLockedState (
			IntPtr hSim,
			ref LockedState lpdwLockedState );

		

		[DllImport("cellcore.dll")]
		private static extern int SimUnlockPhone(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			string lpszPassword,                    // @parm Points to password string
			string lpszNewPin                       // @parm Some locked states require a second password (e.g. PUK requires a new PIN to replace the old, presumably forgotten PIN)
			);

		[DllImport("cellcore.dll")]
		private static extern int SimGetLockingStatus(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwLockingFacility,                // @parm A SIMLOCKFACILITY_* constant
			string lpszPassword,                    // @parm Some facilities require a password
			ref int pfEnabled                         // @parm Enabled or diabled
			);

		[DllImport("cellcore.dll")]
		private static extern int SimSetLockingStatus(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwLockingFacility,                // @parm A SIMLOCKFACILITY_* constant
			string lpszPassword,                    // @parm Some facilities require a password
			int fEnabled                           // @parm Enable or diable
			);

		[DllImport("cellcore.dll")]
		private static extern int SimChangeLockingPassword(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwLockingFacility,                // @parm A SIMLOCKFACILITY_* constant
			string lpszOldPassword,                 // @parm The old password
			string lpszNewPassword                  // @parm The new password
			);



		[DllImport("cellcore.dll")]
		private static extern int SimGetSmsStorageStatus(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			ref int lpdwUsed,                       // @parm Nubmer of used locations
			ref int lpdwTotal                       // @parm Total number of locations
			);

		[DllImport("cellcore.dll")]
		private static extern int SimReadMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			int dwIndex,                          // @parm Index of the entry to retrieve
			byte[] lpSimMessage               // @parm Points to an SMS message structure
			);

		[DllImport("cellcore.dll")]
		private static extern int SimWriteMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			ref int lpdwIndex,                      // @parm Set to the index where the message was written
			byte[] lpSimMessage               // @parm Points to an SMS message structure
			);

		[DllImport("cellcore.dll")]
		private static extern int SimDeleteMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			int dwIndex                           // @parm Index of the entry to retrieve
			);



		[DllImport("cellcore.dll")]
		private static extern int SimReadRecord(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwAddress,                        // @parm SIM address
			int dwRecordType,                     // @parm A SIM_RECORDTYPE_* constant
			int dwIndex,                          // @parm Applies only to SIM_RECORDTYPE_CYCLIC and SIM_RECORDTYPE_LINEAR, otherwise ignored
			byte[] lpData,                          // @parm Data buffer
			int dwBufferSize,                     // @parm Size of data buffer
			ref int lpdwBytesRead                   // @parm Number of bytes read
			);

		[DllImport("cellcore.dll")]
		private static extern int SimWriteRecord(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwAddress,                        // @parm SIM address
			int dwRecordType,                     // @parm A SIM_RECORDTYPE_* constant
			int dwIndex,                          // @parm Applies only to SIM_RECORDTYPE_CYCLIC and SIM_RECORDTYPE_LINEAR, otherwise ignored
			byte[] lpData,                          // @parm Data to write
			int dwByteCount                       // @parm Number of bytes to write
			);

		[DllImport("cellcore.dll")]
		private static extern int SimGetRecordInfo(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			int dwAddress,                        // @parm SIM address
			byte[] lpSimRecordInfo         // @parm Points to a SIM record information structure
			);

		#endregion

		/// <summary>
		/// Specifies which capabilities are valid.
		/// </summary>
		[Flags()]
		private enum CapabilityFlags : int
		{
			PBSTORAGES           = (0x00000001),     // @paramdefine dwPBStorages field is valid
			PBEMAXADDRESSLENGTH  = (0x00000002),     // @paramdefine dwPBEMaxAddressLength field is valid
			PBEMAXTEXTLENGTH     = (0x00000004),     // @paramdefine dwPBEMaxTextLength field is valid
			PBEMININDEX          = (0x00000008),     // @paramdefine dwMinPBIndex field is valid
			PBEMAXINDEX          = (0x00000010),     // @paramdefine dwMaxPBIndex field is valid
			LOCKFACILITIES       = (0x00000020),     // @paramdefine dwLockFacilities field is valid
			LOCKINGPWDLENGTH     = (0x00000040),     // @paramdefine dwNumLockingPwdLengths and rgLockingPwdLengths fields are valid
			READMSGSTORAGES      = (0x00000080),     // @paramdefine dwReadMsgStorages field is valid
			WRITEMSGSTORAGES     = (0x00000100),     // @paramdefine dwWriteMsgStorages field is valid
			ALL                  = (0x000001ff),     // @paramdefine All fields are valid

		}
	}
}
