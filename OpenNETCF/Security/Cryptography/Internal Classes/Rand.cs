using System;
/*
namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Rand
	{
		/// <summary>
		/// if crypto is not available
		/// </summary>
		/// <remarks>not on PPC 2002 device</remarks>
		public static byte [] CeGenRandom(int length)
		{
			byte [] baTemp = new byte[length];
            bool retVal = NativeMethods.CeGenRandom(baTemp.Length, baTemp);
			ErrCode ec = Error.HandleRetVal(retVal);
			return baTemp;
		}

		/// <summary>
		/// not seeded
		/// </summary>
		public static byte [] GetRandomBytes(int length)
		{
			byte[] randomBuf = new byte[length];
			return GetRandomBytes(randomBuf);
		}

		/// <summary>
		/// seeded, dont have to specify a provider
		/// </summary>
		public static byte [] GetRandomBytes(byte[] seed)
		{
			IntPtr prov = Context.AcquireContext(ProvType.RSA_FULL);
            bool retVal = NativeMethods.CryptGenRandom(prov, seed.Length, seed);
			ErrCode ec = Error.HandleRetVal(retVal);
			Context.ReleaseContext(prov);
			return seed;
		}

		public static byte [] GetNonZeroBytes(byte[] seed)
		{
			byte [] buffer = GetRandomBytes(seed);
			for(int i=0; i<buffer.Length; i++)
			{
				if(buffer[i] == 0)
					buffer[i] = 1;
			}
			return buffer;
		}
	}
}
*/