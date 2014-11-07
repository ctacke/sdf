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
	}
}
