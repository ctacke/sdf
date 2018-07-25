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

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Represents a single entry in the SIM Phonebook.
	/// </summary>
	public class PhonebookEntry
	{
		//store data in byte array
		byte[] m_data;

		/// <summary>
		/// Length of the SimPhonebookEntry data.
		/// </summary>
		private const int length = 1040;
		/// <summary>
		/// Maximum length of an address
		/// </summary>
		private const int MAX_LENGTH_ADDRESS = 512;
		/// <summary>
		/// Maximum length of text in a phonebook entry
		/// </summary>
		private const int MAX_LENGTH_PHONEBOOKENTRYTEXT = 512;	

		//DWORD cbSize;                                   // @field Size of the structure in bytes
		//DWORD dwParams;                                 // @field Indicates valid parameter values
		//TCHAR lpszAddress[MAX_LENGTH_ADDRESS];          // @field The actual phone number
		//DWORD dwAddressType;                            // @field A SIM_ADDRTYPE_* constant
		//DWORD dwNumPlan;                                // @field A SIM_NUMPLAN_* constant
		//TCHAR lpszText[MAX_LENGTH_PHONEBOOKENTRYTEXT];  // @field Text assocaited with the entry

		/// <summary>
		/// Specifies which fields of the <see cref="PhonebookEntry"/> are valid.
		/// </summary>
		[Flags()]
		private enum PhonebookEntryFlags : int
		{
			/// <summary>
			/// Address field is valid.
			/// </summary>
			Address           = (0x00000001),
			/// <summary>
			/// AddressType field is valid.
			/// </summary>
			AddressType       = (0x00000002),
			/// <summary>
			/// Plan field is valid.
			/// </summary>
			Plan           = (0x00000004),
			/// <summary>
			/// Text field is valid.
			/// </summary>
			Text              = (0x00000008),
			/// <summary>
			/// All fields are valid.
			/// </summary>
			All               = (0x0000000f),

		}

		/// <summary>
		/// Creates a new <see cref="PhonebookEntry"/>.
		/// </summary>
		public PhonebookEntry()
		{
			m_data = new byte[length];

			//set length member
			BitConverter.GetBytes(m_data.Length).CopyTo(m_data, 0);
		}

		/// <summary>
		/// Returns the binary data held in this structure.
		/// </summary>
		/// <returns>A byte array for this <see cref="PhonebookEntry"/>.</returns>
		internal byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Indicates which fields are valid in this entry.
		/// </summary>
		private PhonebookEntryFlags Flags
		{
			get
			{
				return (PhonebookEntryFlags)BitConverter.ToInt32(m_data, 4);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 4);
			}
		}

		/// <summary>
		/// The actual phone number.
		/// </summary>
		public string Address
		{
			get
			{
				if((Flags & PhonebookEntryFlags.Address) == PhonebookEntryFlags.Address)
				{
					string rawstring = System.Text.Encoding.Unicode.GetString(m_data, 8, MAX_LENGTH_ADDRESS);
					//cut string at first null
					return rawstring.Substring(0, rawstring.IndexOf('\0'));
				}
				else
				{
					return "";
				}
			}
			set
			{
				if(value.Length < 255)
				{
				//get bytes for value with trailing null
				byte[] stringbytes = System.Text.Encoding.Unicode.GetBytes(value + '\0');
				//copy to byte array
				Buffer.BlockCopy(stringbytes, 0, m_data, 8, stringbytes.Length);
				}
				else
				{
					throw new ArgumentException("Address must be no more that 255 Unicode characters", "Address");
				}
			}
		}

		/// <summary>
		/// The numbering system used for this <see cref="PhonebookEntry"/>.
		/// </summary>
		public AddressType AddressType
		{
			get
			{
				return (AddressType)BitConverter.ToInt32(m_data, 520);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 520);
			}
		}

		/// <summary>
		/// The numbering system used for this <see cref="PhonebookEntry"/>.
		/// </summary>
		public NumberPlan Plan
		{
			get
			{
				return (NumberPlan)BitConverter.ToInt32(m_data, 524);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 524);
			}
		}

		/// <summary>
		/// The text associated.
		/// </summary>
		public string Text
		{
			get
			{
				if((Flags & PhonebookEntryFlags.Text) == PhonebookEntryFlags.Text)
				{
					string rawstring = System.Text.Encoding.Unicode.GetString(m_data, 528, MAX_LENGTH_PHONEBOOKENTRYTEXT).TrimEnd('\0');
					//trim out everything after first null
					return rawstring.Substring(0, rawstring.IndexOf('\0'));
				}
				else
				{
					return "";
				}
			}
			set
			{
				if(value.Length < 256)
				{
					//get bytes for value with trailing null
					byte[] stringbytes = System.Text.Encoding.Unicode.GetBytes(value + '\0');
					//copy to byte array
					Buffer.BlockCopy(stringbytes, 0, m_data, 528, stringbytes.Length);
				}
				else
				{
					throw new ArgumentException("Text must be no more that 255 Unicode characters", "Text");
				}
			}
		}
	}

	public enum PhonebookStorage : int
	{
		/// <summary>
		/// Emergency dial list
		/// </summary>
		Emergency         =(0x00000001),
		/// <summary>
		/// Fixed dialing list
		/// </summary>
		FixedDialing      =(0x00000002),
		/// <summary>
		/// Last dialing list
		/// </summary>
		LastDialing       =(0x00000004),
		/// <summary>
		/// Own numbers lists
		/// </summary>
		OwnNumbers        =(0x00000008),
		/// <summary>
		/// General SIM Storage
		/// </summary>
		SIM               =(0x00000010), 
		//SIM_NUMPBSTORAGES               =5 ,                // @constdefine Number of phonebook storages
	}
}
