using System;

namespace OpenNETCF
{
	/// <summary>
	/// Trigonometric Look-up Table.
	/// </summary>
	internal class TrigonometricLUT
	{
		public double[] m_data = null;

		public unsafe TrigonometricLUT(int bits)
		{
			long	length = 0;

			if(bits > 3)
			{
				length = (1 << (bits - 1)) - 4;
				m_data = new double[length];

				fixed(double *pdata = m_data)
				{
					for (int level = 3; level < bits; ++level)
					{
						long	level_len = 1L << (level - 1);

						double* level_ptr = &pdata[level_len - 4];
						double	mul = System.Math.PI / (level_len << 1);

						for (long i = 0; i < level_len; ++ i)
						{
							level_ptr[i] = (double)System.Math.Cos(i * mul);
						}
					}
				}
			}
		}

	}
}
