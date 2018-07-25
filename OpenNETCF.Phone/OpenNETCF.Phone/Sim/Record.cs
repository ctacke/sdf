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
	/// Describes a single record in the SIM card storage.
	/// </summary>
	public class Record
	{
		/// <summary>
		/// Raw binary data
		/// </summary>
		private byte[] m_data;

		/// <summary>
		/// Length of the SimRecord data.
		/// </summary>
		private const int Length = 20;

		/// <summary>
		/// Creates a new <see cref="Record"/>.
		/// </summary>
		public Record()
		{
			m_data = new byte[Length];

			//write size to first four bytes
			BitConverter.GetBytes(Length).CopyTo(m_data, 0);
		}

		internal byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Defines which fields are valid.
		/// </summary>
		private RecordFlags Flags
		{
			get
			{
				return (RecordFlags)BitConverter.ToInt32(m_data, 4);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 4);
			}
		}

		/// <summary>
		/// Gets or Sets the type of record.
		/// </summary>
		/// <seealso cref="RecordType"/>
		public RecordType Type
		{
			get
			{
				return (RecordType)BitConverter.ToInt32(m_data, 8);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 8);
			}
		}

		/// <summary>
		/// Gets or Sets the number of items contained in the record.
		/// </summary>
		public int ItemCount
		{
			get
			{
				return BitConverter.ToInt32(m_data, 12);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 12);
			}
		}

		/// <summary>
		/// Gets or Sets the size, in bytes, of each item.
		/// </summary>
		public int Size
		{
			get
			{
				return BitConverter.ToInt32(m_data, 16);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 16);
			}
		}

		private enum RecordFlags :int
		{
			/// <summary>
			/// RecordType field is valid.
			/// </summary>
			RecordType = 0x00000001,
			/// <summary>
			/// ItemCount field is valid.
			/// </summary>
			ItemCount  = 0x00000002,
			/// <summary>
			/// Size field is valid.
			/// </summary>
			Size       = 0x00000004,
			/// <summary>
			/// All fields are valid.
			/// </summary>
			All        = 0x00000007,
		}
	}
}
