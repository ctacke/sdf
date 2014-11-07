using System;

namespace OpenNETCF.Phone
{
	/// <summary>
	/// Identifies the phone number type specified in the <see cref="Sms.SmsAddress"/> structure.
	/// </summary>
	public enum AddressType 
	{
		/// <summary>
		/// Unknown phone number type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Number is expressed in full with country code.
		/// </summary>
		International,
		/// <summary>
		/// Number is expressed without country code.
		/// </summary>
		National,
		/// <summary>
		/// 
		/// </summary>
		NetworkSpecific,
		/// <summary>
		/// 
		/// </summary>
		Subscriber,
		/// <summary>
		/// 
		/// </summary>
		Alphanumeric,
		/// <summary>
		/// 
		/// </summary>
		Abbreviated,
	}
}
