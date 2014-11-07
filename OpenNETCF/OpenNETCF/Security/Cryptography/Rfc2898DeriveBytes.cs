using System;
using OpenNETCF.Web.Services2.Security.Cryptography;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	/// <summary>
	/// Summary description for Rfc2898DeriveBytes.
	/// </summary>
	public class Rfc2898DeriveBytes : DeriveBytes
	{
		private int internalCount = 1;
		private string password;
		private byte [] salt;
		int iterations;

		//public Rfc2898DeriveBytes(string password, byte [] salt) : this(password, salt, 1000)
		//{
		//}

		public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
		{
			this.password = password;
			if(salt.Length < 8)
				throw new Exception("salt must be at least 8 bytes");
			this.salt = salt;
			this.iterations = iterations;
		}

		public int IterationCount
		{
			get{return iterations;}
			set{iterations = value;}
		}
		public byte [] Salt
		{
			get{return salt;}
			set{salt = value;}
		}

		/// <summary>
		/// Returns pseudo-random key bytes.
		/// </summary>
		/// <param name="cb">The number of pseudo-random key bytes to generate.</param>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		public override byte[] GetBytes(int cb)
		{
			//http://www.rsasecurity.com/rsalabs/node.asp?id=2127
			byte [] P = Format.GetBytes(password);
			HashAlgorithm ha = new HMACSHA1(P);

			int hLen = 20; //HMACSHA1 out is 20 bytes
			int L = cb / hLen; 
			int rem = cb % hLen;
			if(rem != 0)
				L = L + 1; //round up
			int r = cb - (L - 1) * hLen;

			byte [] outL = new byte[hLen * L];
			int offsetL = 0;
			for(int k=0; k<L; k++)
			{
				byte[] icb = BitConverter.GetBytes(internalCount);
				Array.Reverse(icb, 0, 4);
				byte [] salt_icb = PSHA1.ConcatBa(salt, icb);
				byte [] U = ha.ComputeHash(salt_icb);
				byte[] T = U;
				for (int i = 2; i <= iterations; i++)
				{
					U = ha.ComputeHash(U);
					for (int j = 0; j < hLen; j++)
					{
						byte b1 = T[j];
						byte b2 = U[j];
						T[j] = (byte) (b1 ^ b2);
					}
				}
				Array.Copy(T, 0, outL, offsetL, hLen);
				offsetL = offsetL + hLen;
				internalCount = internalCount + 1;
			}			

			byte[] key = new byte[cb];
			Array.Copy(outL, 0, key, 0, cb);
			return key;
		}

		/// <summary>
		/// Resets the state of the operation.
		/// </summary>
		public override void Reset()
		{
			internalCount = 0;
			//this.salt = null;
		}
	}
}
