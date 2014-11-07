using System;

namespace OpenNETCF.Net.NetworkInformation
{
	#region NDIS Structure

	/// <summary>
	/// Managed partial representation of NIC_STATISTICS.  Definitions for NDIS properties 
	/// found at http://msdn.microsoft.com/en-us/library/ms901711.aspx
	/// </summary>
	public struct NDISProperties
	{
		/// <summary> Result of the NDISUIO query of OID_GEN_MEDIA_IN_USE </summary>
		public string deviceState;
		/// <summary>
		/// LinkSpeed is the speed of physical network connections, in 
		/// bits-per-second.  For example, 10BaseT Ethernet would return 
		/// 10,000,000.
		/// </summary>
		public int linkSpeed;
		/// <summary> Result of the NDISUIO query of OID_GEN_MEDIA_CONNECT_STATUS </summary>
		public string mediaState;
		/// <summary> NDISMediumxxx </summary>
		public string mediaType;
		/// <summary> Result of the NDISUIO query of OID_GEN_PHYSICAL_MEDIUM. </summary>
		public string physicalMediaType;
	}

	#endregion
}
