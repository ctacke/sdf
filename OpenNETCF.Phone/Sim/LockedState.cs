using System;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Describes the current state of the SIM card.
	/// </summary>
	public enum LockedState
	{
		/// <summary>
		/// Locking state is unknown.
		/// </summary>
		Unknown					= (0x00000000),
		/// <summary>
		/// Not awaiting a password (unlocked).
		/// </summary>
		Ready					= (0x00000001), 
		/// <summary>
		/// Awaiting the SIM PIN.
		/// </summary>
		SimPIN					= (0x00000002),
		/// <summary>
		/// Awaiting the SIM PUK.
		/// </summary>
		SimPUK					= (0x00000003),
		/// <summary>
		/// Awaiting the Phone to SIM Personalization PIN
		/// </summary>
		PhoneSimPIN				= (0x00000004),
		/// <summary>
		/// Awaiting the Phone to first SIM Personalization PIN
		/// </summary>
		PhoneFirstSimPIN		= (0x00000005),
		/// <summary>
		/// Awaiting the Phone to first SIM Personalization PUK
		/// </summary>
		PhoneFirstSimPUK		= (0x00000006),
		/// <summary>
		/// Awaiting the SIM PIN2
		/// </summary>
		SimPIN2					= (0x00000007),
		/// <summary>
		/// Awaiting the SIM PUK2
		/// </summary>
		SimPUK2					= (0x00000008),
		/// <summary>
		/// Awaiting the Network Personalization PIN
		/// </summary>
		NetworkPIN				= (0x00000009),
		/// <summary>
		/// Awaiting the Network Personalization PUK
		/// </summary>
		NetworkPUK				= (0x0000000a),
		/// <summary>
		/// Awaiting the Network Subset Personalization PIN
		/// </summary>
		NetworkSubsetPIN		= (0x0000000b),
		/// <summary>
		/// Awaiting the Network Subset Personalization PUK
		/// </summary>
		NetworkSubsetPUK		= (0x0000000c),
		/// <summary>
		/// Awaiting the Service Provider Personalization PIN
		/// </summary>
		ServiceProviderPIN      = (0x0000000d),
		/// <summary>
		/// Awaiting the Service Provider Personalization PUK
		/// </summary>
		ServiceProviderPUK      = (0x0000000e),
		/// <summary>
		/// Awaiting the Corporate Personalization PIN
		/// </summary>
		CorporatePIN			= (0x0000000f),
		/// <summary>
		/// Awaiting the Corporate Personalization PUK
		/// </summary>
		CorporatePUK			= (0x00000010),

	}
}
