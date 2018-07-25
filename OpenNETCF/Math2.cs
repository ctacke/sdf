using System;

namespace OpenNETCF
{
	/// <summary>
	/// Represents a complex number of the format real + (imaginary * sqrt(-1))
	/// </summary>
	public struct Complex
	{
        /// <summary>
        /// 
        /// </summary>
		public double real;

        /// <summary>
        /// 
        /// </summary>
		public double imaginary;
	}

	/// <summary>
	/// Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.
	/// </summary>
	/// <seealso cref="System.Math"/>
	[CLSCompliant(true)]
	public static class Math2
	{
		#region "inherited" contants
		/// <summary>
		/// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, p.
		/// </summary>
		public static double PI
		{
			get{ return Math.PI; }
		}

		/// <summary>
		/// Represents the natural logarithmic base, specified by the constant, e.
		/// </summary>
		public static double E
		{
			get{ return Math.E; }
		}
		#endregion

		#region "inherited" methods
		public static decimal Abs(decimal value)
		{ return Math.Abs(value); }

		public static double Acos(double d)
		{ return Math.Acos(d); }

		public static double Asin(double d)
		{ return Math.Asin(d); }

		public static double Atan(double d)
		{ return Math.Atan(d); }

		public static double Atan2(double y, double x)
		{ return Math.Atan2(y, x); }

		public static double Ceiling(double a)
		{ return Math.Ceiling(a); }

		public static double Cos(double d)
		{ return Math.Cos(d); }

		public static double Exp(double a)
		{ return Math.Exp(a); }

		public static double Floor(double a)
		{ return Math.Floor(a); }
		
		
		public static double IEEERemainder(double x, double y)
		{ return Math.IEEERemainder(x, y); }

		public static double Log(double a)
		{ return Math.Log(a); }

		public static double Log10(double a)
		{ return Math.Log10(a); }
		
		#region max
		public static decimal Max(decimal val1, decimal val2)
		{ return Math.Max(val1, val2); }

		public static double Max(double val1, double val2)
		{ return Math.Max(val1, val2); }

		public static float Max(float val1, float val2)
		{ return Math.Max(val1, val2); }

		[CLSCompliant(false)]
		public static UInt64 Max(UInt64 val1, UInt64 val2)
		{ return Math.Max(val1, val2); }
		
		[CLSCompliant(false)]
		public static UInt32 Max(UInt32 val1, UInt32 val2)
		{ return Math.Max(val1, val2); }
		
		[CLSCompliant(false)]
		public static UInt16 Max(UInt16 val1, UInt16 val2)
		{ return Math.Max(val1, val2); }
		
		[CLSCompliant(false)]
		public static SByte Max(SByte val1, SByte val2)
		{ return Math.Max(val1, val2); }

		public static Int64 Max(Int64 val1, Int64 val2)
		{ return Math.Max(val1, val2); }
		
		public static Int32 Max(Int32 val1, Int32 val2)
		{ return Math.Max(val1, val2); }
		
		public static Int16 Max(Int16 val1, Int16 val2)
		{ return Math.Max(val1, val2); }
		
		public static Byte Max(Byte val1, Byte val2)
		{ return Math.Max(val1, val2); }
		#endregion

		#region min
		public static decimal Min(decimal val1, decimal val2)
		{ return Math.Min(val1, val2); }

		public static double Min(double val1, double val2)
		{ return Math.Min(val1, val2); }

		public static float Min(float val1, float val2)
		{ return Math.Min(val1, val2); }

		[CLSCompliant(false)]
		public static UInt64 Min(UInt64 val1, UInt64 val2)
		{ return Math.Min(val1, val2); }
		
		[CLSCompliant(false)]
		public static UInt32 Min(UInt32 val1, UInt32 val2)
		{ return Math.Min(val1, val2); }
		
		[CLSCompliant(false)]
		public static UInt16 Min(UInt16 val1, UInt16 val2)
		{ return Math.Min(val1, val2); }
		
		[CLSCompliant(false)]
		public static SByte Min(SByte val1, SByte val2)
		{ return Math.Min(val1, val2); }

		public static Int64 Min(Int64 val1, Int64 val2)
		{ return Math.Min(val1, val2); }
		
		public static Int32 Min(Int32 val1, Int32 val2)
		{ return Math.Min(val1, val2); }
		
		public static Int16 Min(Int16 val1, Int16 val2)
		{ return Math.Min(val1, val2); }
		
		public static Byte Min(Byte val1, Byte val2)
		{ return Math.Min(val1, val2); }
		#endregion

		public static double Pow(double x, double y)
		{ return Math.Pow(x, y); }

		#region round
		public static decimal Round(decimal d, int decimals)
		{ return Math.Round(d, decimals); }

		public static decimal Round(decimal d)
		{ return Math.Round(d); }

		public static double Round(double d, int decimals)
		{ return Math.Round(d, decimals); }

		public static double Round(double d)
		{ return Math.Round(d); }
		#endregion

		#region Sign
		public static int Sign(decimal value)
		{ return Math.Sign(value); }

		public static int Sign(double value)
		{ return Math.Sign(value); }

		public static int Sign(float value)
		{ return Math.Sign(value); }

		public static int Sign(long value)
		{ return Math.Sign(value); }

		public static int Sign(int value)
		{ return Math.Sign(value); }
		
		public static int Sign(short value)
		{ return Math.Sign(value); }
		
		[CLSCompliant(false)]
		public static int Sign(SByte value)
		{ return Math.Sign(value); }
		#endregion

		public static double Sin(double a)
		{ return Math.Sin(a); }

		public static double Sqrt(double a)
		{ return Math.Sqrt(a); }
		
		public static double Tan(double a)
		{ return Math.Tan(a); }
		
		#endregion

		#region "new" methods
		/// <summary>
		/// Returns the hyperbolic sine of the specified angle.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static double Sinh(double angle)
		{
            return Math.Sinh(angle);
			//return (Math.Exp(angle) - Math.Exp(-angle)) / 2.0;
		}

		/// <summary>
		/// Returns the hyperbolic cosine of the specified angle.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static double Cosh(double angle)
		{
            return Math.Cosh(angle);
			//return (Math.Exp(angle) + Math.Exp(-angle)) / 2.0;
		}

		/// <summary>
		/// Returns the hyperbolic tangent of the specified angle.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static double Tanh(double angle)
		{
            return Math.Tanh(angle);
			//return (Math.Exp(angle) - Math.Exp(-angle)) / (Math.Exp(angle) + Math.Exp(-angle));
		}
		
		/// <summary>
		/// Produces the full product of two 32-bit numbers.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static long BigMul(int a, int b)
		{
			return a*b;
		}

		#region ---- IsPowerOf2 ----
		public static bool IsPowerOf2(Int16 number) 
		{
			return IsPowerOf2((object)number);
		}

		[CLSCompliant(false)]
		public static bool IsPowerOf2(UInt16 number) 
		{
			return IsPowerOf2((object)number);
		}

		public static bool IsPowerOf2(Int32 number) 
		{
			return IsPowerOf2((object)number);
		}

		[CLSCompliant(false)]
		public static bool IsPowerOf2(UInt32 number) 
		{
			return IsPowerOf2((object)number);
		}

		public static bool IsPowerOf2(Int64 number) 
		{
			return IsPowerOf2((object)number);
		}

		[CLSCompliant(false)]
		public static bool IsPowerOf2(UInt64 number) 
		{
			return IsPowerOf2((object)number);
		}

		private static bool IsPowerOf2(Object number) 
		{
			int length = 0;
			ulong lnumber = 0;

			if(number is UInt16) 
			{
				length = 16;
				lnumber = (ulong)((UInt16)number);
			} 
			else if(number is UInt32) 
			{
				length = 32;
				lnumber = (ulong)((UInt32)number);
			} 
			else if(number is UInt64) 
			{
				length = 64;
				lnumber = (ulong)((UInt64)number);
			} 
			else if(number is Int16) 
			{
				length = 15;
				lnumber = (ulong)((Int16)number);
			}
			else if(number is Int32) 
			{
				length = 31;
				lnumber = (ulong)((Int32)number);
			} 
			else if(number is Int64) 
			{
				length = 63;
				lnumber = (ulong)((Int64)number);
			}
			ulong test = 0;

			for(int i = 0 ; i <= length ; i++) 
			{
				test = 1uL << i;

				if(lnumber < test)
					return false;

				if(lnumber == test)
					return true;
			}

			return false;
		}
		#endregion
		#endregion

		/// <summary>
		/// Mathematical constants.
		/// </summary>
		public class Constants
		{
			/// <summary>
			/// Planck's Constant (Js)
			/// </summary>
			public double h
			{
				get{ return 6.6260693e-34; }
			}

			/// <summary>
			/// Avogadro's Constant (1/mol)
			/// </summary>
			public static double L
			{
				get{ return 6.0221415e23; }
			}

			/// <summary>
			/// Boltzmann's Constant (J/K)
			/// </summary>
			public static double k
			{
				get{ return 1.3806505e-23; }
			}

			/// <summary>
			/// Atomic Mass Constant (kg)
			/// </summary>
			public static double m
			{
				get{ return 1.66053886e-27; }
			}

			/// <summary>
			/// Newtonian Constant of Gravitation (m3 kg^-1 s^-2)
			/// </summary>
			public static double G
			{
				get{ return 6.6742e-11; }
			}

			/// <summary>
			/// Electron mass (kg)
			/// </summary>
			public static double mSube
			{
				get{ return 9.1093826e-31; }
			}

			/// <summary>
			/// Proton mass (kg)
			/// </summary>
			public static double mSubp
			{
				get{ return 1.67262171e-27; }
			}

			/// <summary>
			/// Electron Volt (J)
			/// </summary>
			public static double eV
			{
				get{ return 1.60217653e-19; }
			}
		
			/// <summary>
			/// Faraday Constant (C/mol)
			/// </summary>
			public static double F
			{
				get{ return 96485.3383; }
			}

			/// <summary>
			/// Molar Gas Constant (J /(mol*K))
			/// </summary>
			public static double R
			{
				get{ return 8.314472; }
			}
			
			/// <summary>
			/// Rydberg Constant (1 / m)
			/// </summary>
			public static double Rinf
			{
				get{ return 10973731.568525; }
			}

			/// <summary>
			/// Stefan-Boltzman Constant (W m^-2 K^-4)
			/// </summary>
			public static double sigma
			{
				get{ return 5.670400e-8; }
			}

			/// <summary>
			/// Speed of Light in vacuum (m/s)
			/// </summary>
			public static double c
			{
				get{ return 299792458; }
			}
			
			/// <summary>
			/// Magnetic constant (N A^-2)
			/// </summary>
			public static double mu0
			{
				get{ return (4 * PI) * Math.Pow(10,-4); }
			}

			/// <summary>
			/// Elementaty Charge (C)
			/// </summary>
			public static double e
			{
				get{ return 1.60217653e-19; }
			}
		} // end class Constants


		/// <summary>
		/// An implementation of the Fast Fourier Transform based on the work of Laurent de Soras
		/// </summary>
		public class FFTReal
		{
			private int					m_length	= 0;
			private	int					m_bitcount	= 0;
			private BitReversedLUT		m_brlookup	= null;
			private TrigonometricLUT	m_tlookup	= null;
			private	double[]			m_data		= null;

			private double sqrt2_2 = System.Math.Sqrt(2) / 2;

			public FFTReal(int dataLength)
			{
				if(! Math2.IsPowerOf2(dataLength))
				{
					throw new Exception("Invalid length - must be power of 2");
				}

				// pre calculate the look up tables
				Prepare(dataLength);
			}

			private void Prepare(int length)
			{
				m_length = length;
				m_bitcount = (int)System.Math.Floor(System.Math.Log(length) / System.Math.Log(2) + 0.5);
				m_brlookup = new BitReversedLUT(m_bitcount);
				m_tlookup = new TrigonometricLUT(m_bitcount);

				if((1L << m_bitcount) != length)
				{
					throw new Exception("Invalid length - must be power of 2");
				}

				// allocate our buffer
				m_data = new double[length];
			}

			private unsafe void RunFFT(double[] x, double *sf, double*df)
			{

				/* Do the transformation in several pass */
				int		pass;
				long	nbr_coef;
				long	h_nbr_coef;
				long	d_nbr_coef;
				long	coef_index = 0;

				/* First and second pass at once */
				fixed(long *bit_rev_lut_ptr = m_brlookup.m_data)
				{
					do
					{
						long		rev_index_0 = bit_rev_lut_ptr [coef_index];
						long		rev_index_1 = bit_rev_lut_ptr [coef_index + 1];
						long		rev_index_2 = bit_rev_lut_ptr [coef_index + 2];
						long		rev_index_3 = bit_rev_lut_ptr [coef_index + 3];

						double *df2 = df + coef_index;
						df2 [1] = x[rev_index_0] - x[rev_index_1];
						df2 [3] = x[rev_index_2] - x[rev_index_3];

						double sf_0 = x[rev_index_0] + x[rev_index_1];
						double sf_2 = x[rev_index_2] + x[rev_index_3];

						df2 [0] = sf_0 + sf_2;
						df2 [2] = sf_0 - sf_2;
					
						coef_index += 4;
					}while (coef_index < m_length);

					/* Third pass */
					coef_index = 0;
					do
					{
						double v;
						sf [coef_index] = df [coef_index] + df [coef_index + 4];
						sf [coef_index + 4] = df [coef_index] - df [coef_index + 4];
						sf [coef_index + 2] = df [coef_index + 2];
						sf [coef_index + 6] = df [coef_index + 6];

						v = (df [coef_index + 5] - df [coef_index + 7]) * sqrt2_2;
						sf [coef_index + 1] = df [coef_index + 1] + v;
						sf [coef_index + 3] = df [coef_index + 1] - v;

						v = (df [coef_index + 5] + df [coef_index + 7]) * sqrt2_2;
						sf [coef_index + 5] = v + df [coef_index + 3];
						sf [coef_index + 7] = v - df [coef_index + 3];

						coef_index += 8;
					}while (coef_index < m_length);
		

					/* Next pass */
					for (pass = 3; pass < m_bitcount; ++pass)
					{
						coef_index = 0;
						nbr_coef = 1 << pass;
						h_nbr_coef = nbr_coef >> 1;
						d_nbr_coef = nbr_coef << 1;

						fixed(double *cos_ptr = &m_tlookup.m_data[(1L <<(pass-1))-4])
						{
							do
							{
								long				i;
								double *sf1r = sf + coef_index;
								double *sf2r = sf1r + nbr_coef;
								double *dfr = df + coef_index;
								double *dfi = dfr + nbr_coef;

								/* Extreme coefficients are always real */
								dfr [0] = sf1r [0] + sf2r [0];
								dfi [0] = sf1r [0] - sf2r [0];	// dfr [nbr_coef] =
								dfr [h_nbr_coef] = sf1r [h_nbr_coef];
								dfi [h_nbr_coef] = sf2r [h_nbr_coef];

								/* Others are conjugate complex numbers */
								double *sf1i = sf1r + h_nbr_coef;
								double *sf2i = sf1i + nbr_coef;
								for (i = 1; i < h_nbr_coef; ++ i)
								{
									double c = cos_ptr [i];					// cos (i*PI/nbr_coef);
									double s = cos_ptr [h_nbr_coef - i];	// sin (i*PI/nbr_coef);
									double v;

									v = sf2r [i] * c - sf2i [i] * s;
									dfr [i] = sf1r [i] + v;
									dfi [-i] = sf1r [i] - v;	// dfr [nbr_coef - i] =

									v = sf2r [i] * s + sf2i [i] * c;
									dfi [i] = v + sf1i [i];
									dfi [nbr_coef - i] = v - sf1i [i];
								}

								coef_index += d_nbr_coef;
							}while (coef_index < m_length);
						}

						/* Prepare to the next pass */
			
						double *temp_ptr = df;
						df = sf;
						sf = temp_ptr;
			
					}
				}
			}

			/// <summary>
			/// Compute an FFT on a 2^n-length data set
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			public Complex[] Compute(double[] input)
			{
				double[] output = new double[input.Length];

				Compute(output, input);

				Complex[] complex = new Complex[input.Length / 2];
				for(int c = 0 ; c < complex.Length ; c++)
				{
					complex[c].real = output[c];
					complex[c].imaginary = output[c + complex.Length];
				}
				
				return complex;
			}

			private unsafe void Compute(double[] output, double[] input)
			{
				if(input.Length != m_length)
					throw new Exception("data length must match prepared length");

				if((m_bitcount & 1) > 0)
				{
					fixed (double* df = m_data, sf = output)
					{
						RunFFT(input, sf, df);
					}
				}
				else
				{
					fixed (double* df = output, sf = m_data)
					{
						RunFFT(input, sf, df);
					}
				}
			}

			private unsafe void RunFFTI(double[] f, double *df, double *df_temp, double *x)
			{
				/* Do the transformation in several pass */
				fixed(double *psf = f)
				{
					double *sf = psf;

					int			pass;
					long			nbr_coef;
					long			h_nbr_coef;
					long			d_nbr_coef;
					long			coef_index = 0;

					/* First pass */
					for (pass = m_bitcount - 1; pass >= 3; --pass)
					{
						nbr_coef = 1 << pass;
						h_nbr_coef = nbr_coef >> 1;
						d_nbr_coef = nbr_coef << 1;
						fixed(double *cos_ptr = &m_tlookup.m_data[(1L <<(pass-1))-4])
						{
							do
							{
								long				i;
								double *sfr = sf + coef_index;
								double *sfi = sfr + nbr_coef;
								double *df1r = df + coef_index;
								double *df2r = df1r + nbr_coef;

								/* Extreme coefficients are always real */
								df1r [0] = sfr [0] + sfi [0];		// + sfr [nbr_coef]
								df2r [0] = sfr [0] - sfi [0];		// - sfr [nbr_coef]
								df1r [h_nbr_coef] = sfr [h_nbr_coef] * 2;
								df2r [h_nbr_coef] = sfi [h_nbr_coef] * 2;

								/* Others are conjugate complex numbers */
								double *df1i = df1r + h_nbr_coef;
								double *df2i = df1i + nbr_coef;
								for (i = 1; i < h_nbr_coef; ++ i)
								{
									df1r [i] = sfr [i] + sfi [-i];		// + sfr [nbr_coef - i]
									df1i [i] = sfi [i] - sfi [nbr_coef - i];

									double c = cos_ptr [i];					// cos (i*PI/nbr_coef);
									double s = cos_ptr [h_nbr_coef - i];	// sin (i*PI/nbr_coef);
									double vr = sfr [i] - sfi [-i];		// - sfr [nbr_coef - i]
									double vi = sfi [i] + sfi [nbr_coef - i];

									df2r [i] = vr * c + vi * s;
									df2i [i] = vi * c - vr * s;
								}

								coef_index += d_nbr_coef;
							}while (coef_index < m_length);
						}

						/* Prepare to the next pass */
						if (pass < m_bitcount - 1)
						{
							double *temp_ptr = df;
							df = sf;
							sf = temp_ptr;
						}
						else
						{
							sf = df;
							df = df_temp;
						}
			

						/* Antepenultimate pass */
						coef_index = 0;
						do
						{
							df [coef_index] = sf [coef_index] + sf [coef_index + 4];
							df [coef_index + 4] = sf [coef_index] - sf [coef_index + 4];
							df [coef_index + 2] = sf [coef_index + 2] * 2;
							df [coef_index + 6] = sf [coef_index + 6] * 2;

							df [coef_index + 1] = sf [coef_index + 1] + sf [coef_index + 3];
							df [coef_index + 3] = sf [coef_index + 5] - sf [coef_index + 7];

							double vr = sf [coef_index + 1] - sf [coef_index + 3];
							double vi = sf [coef_index + 5] + sf [coef_index + 7];

							df [coef_index + 5] = (vr + vi) * sqrt2_2;
							df [coef_index + 7] = (vi - vr) * sqrt2_2;

							coef_index += 8;
						}
						while (coef_index < m_length);


						/* Penultimate and last pass at once */
						coef_index = 0;
						fixed(long *bit_rev_lut_ptr = m_brlookup.m_data)
						{
							long *lut = bit_rev_lut_ptr;
							double *sf2 = df;
							do
							{
								double b_0 = sf2 [0] + sf2 [2];
								double b_2 = sf2 [0] - sf2 [2];
								double b_1 = sf2 [1] * 2;
								double b_3 = sf2 [3] * 2;

								x [lut [0]] = b_0 + b_1;
								x [lut [1]] = b_0 - b_1;
								x [lut [2]] = b_2 + b_3;
								x [lut [3]] = b_2 - b_3;

								b_0 = sf2 [4] + sf2 [6];
								b_2 = sf2 [4] - sf2 [6];
								b_1 = sf2 [5] * 2;
								b_3 = sf2 [7] * 2;

								x [lut [4]] = b_0 + b_1;
								x [lut [5]] = b_0 - b_1;
								x [lut [6]] = b_2 + b_3;
								x [lut [7]] = b_2 - b_3;

								sf2 += 8;
								coef_index += 8;
								lut += 8;

							}while (coef_index < m_length);
						}
					}
				}
			}
	
			/// <summary>
			/// Compute an inverse FFT on a 2^n-length data set of complex numbers
			/// </summary>
			/// <param name="input"></param>
			/// <returns></returns>
			public double[] ComputeInverse(Complex[] input)
			{
				double[] output = new double[input.Length * 2];
				double[] indata = new double[input.Length * 2];

				for(int c = 0 ; c < input.Length ; c++)
				{
					indata[c * 2] = input[c].real;
					indata[(c * 2) + 1] = input[c].imaginary;
				}

				ComputeInverse(indata, output);
				
				return output;
			}

			private unsafe void ComputeInverse(double[] input, double[] output)
			{
				if(input.Length != m_length)
					throw new Exception("data length must match prepared length");

				if((m_bitcount & 1) > 0)
				{
					fixed (double* df = m_data, dftemp = output, px = output)
					{
						RunFFTI(input, df, dftemp, px);
					}
				}
				else
				{
					fixed (double* dftemp = m_data, df = output, px = output)
					{
						RunFFTI(input, df, dftemp, px);
					}
				}
			
			}
		} // end class FFTReal

		public class FFT
		{
			public FFT()
			{
			}

			private int ReverseBits (int index, int NumBits)
			{
				int i;
				int rev;

				for (i = rev = 0 ; i < NumBits; i++ )
				{
					rev = (rev << 1) | (index & 1);
					index >>= 1;
				}

				return (int)rev;
			}

			private int NumberOfBitsNeeded(int PowerOfTwo)
			{
				for (int i = 0 ; ; i++)
				{
					if((PowerOfTwo & (1 << i)) > 0)
						return i;
				}
			}

			public unsafe Complex[] Calculate(Complex[] inData, bool Inverse)
			{
				int NumSamples = (int)inData.Length;
				Complex[] outData = new Complex[NumSamples];
				//			Complex *outData = stackalloc Complex[NumSamples];

				int NumBits = 0;
				int i;
				int j;
				int k;
				int n;
				int BlockSize;
				int BlockEnd;

				double angle_numerator = 2.0 * System.Math.PI;
				double tr, ti;     // temp real, temp imaginary

				// check power of 2
				// check we have >2 data points

				if(Inverse)
					angle_numerator = angle_numerator * -1;

				NumBits = NumberOfBitsNeeded (NumSamples);

				/*
				**   Do simultaneous data copy and bit-reversal ordering into outputs...
				*/

				for (i = 0 ; i < NumSamples ; i++ )
				{
					j = ReverseBits (i, NumBits);
					outData[j].real = inData[i].real;
					outData[j].imaginary = inData[i].imaginary;
				}

				/*
				**   Do the FFT itself...
				*/

				BlockEnd = 1;
				double[] ar = new double[3];
				double[] ai = new double[3];

				for (BlockSize = 2 ; BlockSize <= NumSamples ; BlockSize <<= 1 )
				{
					double delta_angle = angle_numerator / (double)BlockSize;
					double sm2 = System.Math.Sin ( -2 * delta_angle );
					double sm1 = System.Math.Sin ( -delta_angle );
					double cm2 = System.Math.Cos ( -2 * delta_angle );
					double cm1 = System.Math.Cos ( -delta_angle );
					double w = 2 * cm1;
					// double temp;

					for (i = 0 ; i < NumSamples ; i += BlockSize)
					{
						ar[2] = cm2;
						ar[1] = cm1;

						ai[2] = sm2;
						ai[1] = sm1;

						for (j = i, n = 0; n < BlockEnd; j++, n++ )
						{
							ar[0] = w*ar[1] - ar[2];
							ar[2] = ar[1];
							ar[1] = ar[0];

							ai[0] = w*ai[1] - ai[2];
							ai[2] = ai[1];
							ai[1] = ai[0];

							k = j + BlockEnd;
							tr = ar[0]*outData[k].real - ai[0]*outData[k].imaginary;
							ti = ar[0]*outData[k].imaginary + ai[0]*outData[k].real;

							outData[k].real = outData[j].real - tr;
							outData[k].imaginary = outData[j].imaginary - ti;

							outData[j].real += tr;
							outData[j].imaginary += ti;
						}
					}

					BlockEnd = BlockSize;
				}
		

				/*
				**   Need to normalize if inverse transform...
				*/

				if(Inverse)
				{
					double denom = (double)NumSamples;

					for (i = 0 ; i < NumSamples ; i++ )
					{
						outData[i].real /= denom;
						outData[i].imaginary /= denom;
					}
				}
				return outData;
			}
		} // end class FFT
	}
}
