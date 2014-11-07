using System;

namespace OpenNETCF
{
	internal class BitReversedLUT
	{
		public long[] m_data;

		public BitReversedLUT (int bits)
		{
			long length;
			long br_index;
			long bit;

			length = 1 << bits;
			m_data = new long[length];

			br_index = 0;
			m_data[0] = 0;
			for (int i = 1; i < length; ++i)
			{
				/* ++br_index (bit reversed) */
				bit = length >> 1;
				while (((br_index ^= bit) & bit) == 0)
				{
					bit >>= 1;
				}

				m_data[i] = br_index;
			}
		}
	}
}
