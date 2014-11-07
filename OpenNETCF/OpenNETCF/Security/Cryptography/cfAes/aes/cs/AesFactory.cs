using System;
using sc;

namespace aes
{
	public sealed class AesFactory
	{
		private AesFactory()
		{}

		public static IBlockCipher GetAes()
		{
			return new Aes();
		}

		public static IBlockCipher GetAes(bool managed)
		{
			if(managed == true)
				return new Aes();
			else //native
				return new CAes();
		}
	}
}
