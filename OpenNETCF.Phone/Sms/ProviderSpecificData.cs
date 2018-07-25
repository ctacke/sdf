using System;

namespace OpenNETCF.Phone.Sms
{
	/// <summary>
	/// Summary description for ProviderSpecificData.
	/// </summary>
	public abstract class ProviderSpecificData
	{
		internal abstract byte[] ToByteArray();

		public static implicit operator byte[](ProviderSpecificData psd)
		{
			return psd.ToByteArray();
		}
	}
}
