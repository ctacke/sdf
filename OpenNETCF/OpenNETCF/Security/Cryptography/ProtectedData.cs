using System;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	/// <summary>
	/// Summary description for ProtectedData.
	/// </summary>
	public class ProtectedData
	{
		//http://blogs.msdn.com/shawnfa/archive/2004/05/05/126825.aspx
		//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnsecure/html/windataprotection-dpapi.asp
		//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secmod/html/secmod21.asp
		private ProtectedData(){}

		public static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			ProtectParam pf = ProtectParam.UI_FORBIDDEN;
			if(scope == DataProtectionScope.LocalMachine)
				pf = ProtectParam.UI_FORBIDDEN | ProtectParam.LOCAL_MACHINE;
			else
				pf = ProtectParam.UI_FORBIDDEN;

			//TODO handle entropy properly, ignore entropy
			string entropyDesc = String.Empty;
			if(optionalEntropy != null)
				entropyDesc = Format.GetString(optionalEntropy);

			return Cipher.ProtectData(userData, pf, entropyDesc);
		}
		
		public static byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			ProtectParam pf = ProtectParam.UI_FORBIDDEN;
			if(scope == DataProtectionScope.LocalMachine)
				pf = ProtectParam.UI_FORBIDDEN | ProtectParam.LOCAL_MACHINE;
			else
				pf = ProtectParam.UI_FORBIDDEN;

			//TODO handle entropy properly, ignore entropy
			string entropyDesc = String.Empty;
			if(optionalEntropy != null)
				entropyDesc = Format.GetString(optionalEntropy);
			
			string outDesc;
			byte [] clear = Cipher.UnprotectData(encryptedData, pf, out outDesc);
			if(outDesc != entropyDesc)
				throw new Exception("entropy failure");

			return clear;
		}
	}

	public enum DataProtectionScope
	{
		// Fields
		CurrentUser = 0,
		LocalMachine = 1
	}

}
