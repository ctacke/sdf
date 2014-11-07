using System;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Specifies the numbering plan used in the <see cref="PhonebookEntry.Address"/>.
	/// </summary>
	public enum NumberPlan : int
	{
		/// <summary>
		/// Unknown numbering.
		/// </summary>
		Unknown             =(0x00000000),
		/// <summary>
		/// ISDN/telephone numbering plan (E.164/E.163)
		/// </summary>
		Telephone           =(0x00000001),
		/// <summary>
		/// Data numbering plan (X.121)
		/// </summary>
		Data                =(0x00000002),
		/// <summary>
		/// Telex numbering plan
		/// </summary>
		Telex               =(0x00000003),
		/// <summary>
		/// National numbering plan
		/// </summary>
		National            =(0x00000004),
		/// <summary>
		/// Private numbering plan
		/// </summary>
		Private             =(0x00000005),
		/// <summary>
		/// ERMES numbering plan (ETSI DE/PS 3 01-3)
		/// </summary>
		Ermes               =(0x00000006),
	}
}
