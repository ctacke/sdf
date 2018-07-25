using System;

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// SMS Addressing information.
	/// </summary>
	/// <remarks>Equivalent to native <b>sms_address_tag</b> structure</remarks>
	public class SmsAddress
	{
		// Length of fixed length address string
		internal const int SmsMaxLength = 256;

		/// <summary>
		/// Length, in Bytes,  of SmsAddress structure.
		/// </summary>
		public const int Length = 518;

		// Byte array containing structure data
		private byte[] m_data;

		/// <summary>
		/// Create a new instance of SmsAddress.
		/// </summary>
		public SmsAddress()
		{
			m_data = new byte[Length];
		}
		/// <summary>
		/// Create a new instance of SmsAddress from a Byte array.
		/// </summary>
		/// <param name="data">SmsAddress data in a Byte array.</param>
		public SmsAddress(byte[] data)
		{
			if(data.Length > Length)
			{
				m_data = data;
			}
		}
		/// <summary>
		/// Create a new instance of SmsAddress with a specified address.
		/// </summary>
		/// <param name="address">Address e.g. +447890123456</param>
		public SmsAddress(string address) : this()
		{
			this.Address = address;
		}
		/// <summary>
		/// Create a new instance of SmsAddress with a specified address and type.
		/// </summary>
		/// <param name="address">Address e.g. +447890123456</param>
		/// <param name="type">A member of the SmsAddressType Enumeration</param>
		public SmsAddress(string address, AddressType type) : this(address)
		{
			this.Type = type;
		}

		/// <summary>
		/// Returns a flat Byte Array of the Sms Address data
		/// </summary>
		/// <returns>Byte array containing SmsAddress data.</returns>
		public byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Cast SmsAddress object to a byte array
		/// </summary>
		/// <param name="sa">SmsAddress object</param>
		/// <returns>Byte array containing SmsAddress data.</returns>
		public static implicit operator byte[](SmsAddress sa)
		{
			return sa.ToByteArray();
		}

		/// <summary>
		/// Cast byte array to SmsAddress object.
		/// </summary>
		/// <param name="b">Byte array containing SmsAddress data</param>
		/// <returns>SmsAddress version of the data.</returns>
		public static implicit operator SmsAddress(byte[] b)
		{
			return new SmsAddress(b);
		}

		#region Type Property
		/// <summary>
		/// The address type.
		/// </summary>
		public AddressType Type
		{
			get
			{
				//return address type identifier
				return (AddressType)BitConverter.ToInt32(m_data, 0);
			}
			set
			{
				//copy value to data array
				BitConverter.GetBytes((int)value).CopyTo(m_data, 0);
			}
		}
		#endregion

		#region Address Property
		/// <summary>
		/// The address in string format. For example, "127.0.0.1" or "+1.800.123.4567".
		/// </summary>
		public string Address
		{
			get
			{
				//return the number portion of the address minus any trailing nulls
				string address = System.Text.Encoding.Unicode.GetString(m_data, 4, SmsMaxLength * 2);
				
				int nullindex = address.IndexOf('\0');

				if(nullindex > -1)
				{
					address = address.Substring(0, nullindex);
				}
				
				return address;
			}
			set
			{
				System.Text.Encoding.Unicode.GetBytes(value).CopyTo(m_data, 4);
			}
		}
		#endregion
	}
}
