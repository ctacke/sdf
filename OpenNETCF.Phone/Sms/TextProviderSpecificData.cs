using System;

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// Summary description for TextProviderSpecificData.
	/// </summary>
	public class TextProviderSpecificData : ProviderSpecificData
	{
		private const int SmsDatagramSize = 140;
		internal const int Length = 164;

		private byte[] m_data;
		
		public TextProviderSpecificData()
		{
			//byte array to contain structure
			m_data = new byte[Length];
		}
		public TextProviderSpecificData(byte[] data) : this()
		{
			if(data.Length == Length)
			{
				m_data = data;
			}
			else
			{
				throw new ArgumentException("Supplied data not of correct length to create TextProviderSpecificData instance");
			}

		}


		internal override byte[] ToByteArray()
		{
			return this.m_data;
		}

		/*public static implicit operator byte[](TextProviderSpecificData tpsd)
		{
			return tpsd.ToByteArray();
		}*/

		public MessageOptions MessageOptions
		{
			get
			{
				return (MessageOptions)BitConverter.ToInt32(m_data, 0);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 0);
			}
		}

		/// <summary>
		/// Sets the message class.
		/// </summary>
		public MessageClass MessageClass
		{
			get
			{
				return (MessageClass)BitConverter.ToInt32(m_data, 4);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 4);
			}
		}

		/// <summary>
		/// Text SMS messages with the appropriate flag can replace previously received notifications with a similar flag and originating address.
		/// </summary>
		public ReplaceOption ReplaceOption
		{
			get
			{
				return (ReplaceOption)BitConverter.ToInt32(m_data, 8);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 8);
			}
		}

		public int HeaderDataSize
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
		/// The information contained in the header.
		/// For multi-part messages, only the header from the first segment is returned.
		/// </summary>
		public byte[] HeaderData
		{
			get
			{
				byte[] headerdata = new byte[SmsDatagramSize];
				Buffer.BlockCopy(m_data, 16, headerdata, 0, 140);
				return headerdata;
			}
			set
			{
				Buffer.BlockCopy(value, 0, m_data, 16, 140);
			}
		}

		/// <summary>
		/// Flag that indicates that at least one segment of this message contains EMS headers.
		/// Only set this flag if the EMS handler installed.
		/// </summary>
		public bool ContainsEMSHeaders
		{
			get
			{
				return BitConverter.ToBoolean(m_data, 156);
			}
			set
			{
				BitConverter.GetBytes((int)(value ? -1 : 0)).CopyTo(m_data, 156);
			}
		}

		/// <summary>
		/// The Protocol Identifier (PID) of an invoming message, or the desired PID of an outgoing message.
		/// This applies to GSM only.
		/// </summary>
		public MessageProtocol ProtocolID
		{
			get
			{
				return (MessageProtocol)BitConverter.ToInt32(m_data, 160);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 160);
			}
		}
	}
}
