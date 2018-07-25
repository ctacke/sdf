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
using OpenNETCF.Win32;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Represents a single Message item on the SIM card.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// Raw binary data
		/// </summary>
		private byte[] m_data;

		/// <summary>
		/// Length of the SimRecord data.
		/// </summary>
		private const int Length = 1116;

		/// <summary>
		/// Maximum length of an address in bytes.
		/// </summary>
		private const int MAX_LENGTH_ADDRESS = 512;
		/// <summary>
		/// Maximum length of message in bytes.
		/// </summary>
		private const int MAX_LENGTH_MESSAGE = 256;

		/// <summary>
		/// Create a new instance of SimMessage.
		/// </summary>
		public Message()
		{
			m_data = new byte[Length];
			//write size to first dword (used for versioning)
			BitConverter.GetBytes(Length).CopyTo(m_data, 0);
		}

		internal byte[] ToByteArray()
		{
			return m_data;
		}

		private MessageFlags Flags
		{
			get
			{
				return (MessageFlags)BitConverter.ToInt32(m_data, 4);
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
				if((Flags & MessageFlags.Address) == MessageFlags.Address)
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
		/// The numbering system used for this <see cref="Message"/>.
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
		/// The numbering system used for this <see cref="Message"/>.
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
		/// Gets or Sets the timestamp for the incoming message.
		/// </summary>
		public DateTime ReceiveTime
		{
			get
			{
				SystemTime st = new SystemTime(m_data, 528);
				return st.ToDateTime();
			}
			set
			{
				SystemTime st = SystemTime.FromDateTime(value);
				st.ToByteArray().CopyTo(m_data, 528);
			}
		}

		/// <summary>
		/// Gets or Sets the actual Header length in bytes.
		/// </summary>
		private int HeaderLength
		{
			get
			{
				return BitConverter.ToInt32(m_data, 544);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 544);
			}
		}

		/// <summary>
		/// Gets or Sets the Header data for this message.
		/// </summary>
		public byte[] Header
		{
			get
			{
				//create byte array of correct size
				byte[] header = new byte[HeaderLength];
				//copy the data across
				Buffer.BlockCopy(m_data, 548, header, 0, header.Length);
				//return header data
				return header;
			}
			set
			{
				if(value.Length <= 256)
				{
					//set actual length
					this.HeaderLength = value.Length;
					//copy data across
					Buffer.BlockCopy(value, 0, m_data, 548, value.Length);
				}
				else
				{
					throw new ArgumentException("Data supplied too long for field", "Header");
				}
			}
		}

		/// <summary>
		/// The message text.
		/// </summary>
		public string MessageText
		{
			get
			{
				if((Flags & MessageFlags.Message) == MessageFlags.Message)
				{
					string rawstring = System.Text.Encoding.Unicode.GetString(m_data, 804, MAX_LENGTH_MESSAGE);
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
				if(value.Length < 256)
				{
					//get bytes for value with trailing null
					byte[] stringbytes = System.Text.Encoding.Unicode.GetBytes(value + '\0');
					//copy to byte array
					Buffer.BlockCopy(stringbytes, 0, m_data, 804, stringbytes.Length);
				}
				else
				{
					throw new ArgumentException("Address must be no more that 255 Unicode characters", "Address");
				}
			}
		}

		/// <summary>
		/// Specifies which fields are valid.
		/// </summary>
		private enum MessageFlags : int
		{
			/// <summary>
			/// Address field is valid.
			/// </summary>
			Address           = 0x00000001,
			/// <summary>
			/// AddressType field is valid.
			/// </summary>
			AddressType      = 0x00000002,
			/// <summary>
			/// NumPlan field is valid.
			/// </summary>
			NumPlan           = 0x00000004,
			/// <summary>
			/// ReceiveTime field is valid.
			/// </summary>
			ReceiveTime      = 0x00000008,
			/// <summary>
			/// Header field is valid.
			/// </summary>
			Header            = 0x00000010,
			/// <summary>
			/// HdrLength field is valid.
			/// </summary>
			HeaderLength     = 0x00000020,
			/// <summary>
			/// Message field is valid.
			/// </summary>
			Message           = 0x00000040,
			/// <summary>
			/// All fields are valid.
			/// </summary>
			All               = 0x0000007f,
		}
	}
}
