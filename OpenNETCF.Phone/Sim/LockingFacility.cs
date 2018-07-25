using System;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Specifies the phones locking behavior.
	/// </summary>
	public enum LockingFacility : int
	{
		/// <summary>
		/// Lock control surface
		/// </summary>
		Control							= (0x00000001),
		/// <summary>
		/// Lock phone to SIM card
		/// </summary>
		PhoneToSim						= (0x00000002),
		/// <summary>
		/// Lock phone to first SIM card
		/// </summary>
		PhoneToFirstSim					= (0x00000004), 
		/// <summary>
		/// Lock SIM card
		/// </summary>
		Sim								= (0x00000008),
		/// <summary>
		/// Lock SIM card
		/// </summary>
		SimPin2							= (0x00000010),
		/// <summary>
		/// SIM fixed dialing memory
		/// </summary>
		SimFixedDialing					= (0x00000020), 
		/// <summary>
		/// Network personalization
		/// </summary>
		NetworkPersonalization			= (0x00000040),
		/// <summary>
		/// Network subset personalization
		/// </summary>
		NetworkSubsetPersonalization    = (0x00000080),
		/// <summary>
		/// Service provider personalization
		/// </summary>
		ServiceProviderPersonalization  = (0x00000100),
		/// <summary>
		/// Corporate personalization
		/// </summary>
		CorporatePersonalization        = (0x00000200), 

		//#define SIM_NUMLOCKFACILITIES               10         // @constdefine Number of locking facilities
	}
}
