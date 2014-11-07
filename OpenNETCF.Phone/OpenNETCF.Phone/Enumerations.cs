using System;

namespace OpenNETCF.Phone
{
	#region Call Type Enumeration
	/// <summary>
	/// The type of call represented in the <see cref="T:OpenNETCF.Phone.CallLogEntry"/>.
	/// </summary>
	public enum CallType : int
	{
		/// <summary>
		/// An unanswered (missed) incoming call.
		/// </summary>
		Missed,
		/// <summary>
		/// An answered incoming call.
		/// </summary>
		Incoming,
		/// <summary>
		/// An outgoing call.
		/// </summary>
		Outgoing,
	}
	#endregion

	#region CallerID Type Enumeration
	/// <summary>
	/// Specifies the availability of Caller ID.
	/// </summary>
	public enum CallerIDType : int
	{
		/// <summary>
		/// The Caller ID is unavailable.
		/// </summary>
		Unavailable,
		/// <summary>
		/// The Caller ID is blocked.
		/// </summary>
		Blocked,
		/// <summary>
		/// The Caller ID is available.
		/// </summary>
		Available,
	}
	#endregion

	#region Call Log Seek Enumeration
	/// <summary>
	/// Specifies the location within the <see cref="T:OpenNETCF.Phone.CallLog"/> where a search will begin.
	/// </summary>
	public enum CallLogSeek : int
	{ 
		/// <summary>
		/// The search will begin at the start of the call log.
		/// </summary>
		Beginning = 2,
		/// <summary>
		/// The search will begin at the end of the call log.
		/// </summary>
		End = 4
	}   
	#endregion

}
 

 