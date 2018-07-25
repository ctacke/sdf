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

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// Describes the status of an SMS message.
	/// </summary>
	/// <preliminary/>
	public class SmsMessageStatus
	{
		internal const int Length = 288;
		private byte[] m_data;

		public SmsMessageStatus()
		{
			m_data = new byte[Length];
		}

		#region Byte Array Support
		public SmsMessageStatus(byte[] data)
		{
			if(data.Length == Length)
			{
				m_data = data;
			}
			else
			{
				throw new ArgumentException("Cannot create SmsMessageStatus object from specified bytes, length is incorrect");
			}
		}

		public byte[] ToByteArray()
		{
			return m_data;
		}
		public static implicit operator byte[](SmsMessageStatus sms)
		{
			return sms.ToByteArray();
		}

		public static implicit operator SmsMessageStatus(byte[] b)
		{
			return new SmsMessageStatus(b);
		}
		#endregion


		/// <summary>
		/// A message identifier returned when calling SmsSendMessage.
		/// </summary>
		/// <preliminary/>
		public int MessageID
		{
			get
			{
				return BitConverter.ToInt32(m_data, 0);
			}
			set
			{
				BitConverter.GetBytes(value).CopyTo(m_data, 0);
			}
		}

		/// <summary>
		/// The status of the message.
		/// </summary>
		/// <preliminary/>
		[CLSCompliant(false)]
		public MessageStatus Status
		{
			get
			{
				return (MessageStatus)BitConverter.ToUInt32(m_data, 4);
			}
		}

		/// <summary>
		/// The destination address of the message.
		/// </summary>
		/// <preliminary/>
		public SmsAddress RecipientAddress
		{
			get
			{
				//copy subset of array to sms address object
				SmsAddress sa = new SmsAddress();
				Buffer.BlockCopy(m_data, 12, sa, 0, SmsAddress.Length);
				return sa;
			}
		}

		/// <summary>
		/// The time when the service center received the sent message.
		/// </summary>
		/// <remarks>For a status information response resulting from a multipart message, this field contains the most recent timestamp of all the multipart messages.</remarks>
		/// <preliminary/>
		public DateTime ServiceCenterTimeStamp
		{
			get
			{
				return DateTime.FromFileTimeUtc(BitConverter.ToInt64(m_data, 272));
			}
		}

		/// <summary>
		/// The time pertaining to the particular outcome defined in <see cref="P:OpenNETCF.Phone.Sms.SmsMessageStatus.MessageStatus">MessageStatus</see>.
		/// </summary>
		/// <remarks>For a status information response resulting from a multipart message, this field contains the most recent discharge timestamp of all the multipart messages.</remarks>
		/// <preliminary/>
		public DateTime DischargeTime
		{
			get
			{
				return DateTime.FromFileTimeUtc(BitConverter.ToInt64(m_data, 280));
			}
		}
	}
}
