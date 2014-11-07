using System;

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// Summary description for NotificationProviderSpecificData.
	/// </summary>
	public class NotificationProviderSpecificData : ProviderSpecificData
	{
		private byte[] m_data;

		internal const int Length = 24;

		public NotificationProviderSpecificData()
		{
			m_data = new byte[Length];
		}



		internal override byte[] ToByteArray()
		{
			return m_data;
		}

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

		/// <summary>
		/// The type of the notification message.
		/// </summary>
		public NotificationMessageWaitingType WaitingType
		{
			get
			{
				return (NotificationMessageWaitingType)BitConverter.ToInt32(m_data, 12);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 12);
			}
		}

		/// <summary>
		/// The number of waiting messages.
		/// </summary>
		public int NumberOfMessagesWaiting
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

		/// <summary>
		/// The cellular line that a notification is for.
		/// </summary>
		public NotificationIndicatorType IndicatorType
		{
			get
			{
				return (NotificationIndicatorType)BitConverter.ToInt32(m_data, 20);
			}
			set
			{
				BitConverter.GetBytes((int)value).CopyTo(m_data, 20);
			}
		}

	}
}
