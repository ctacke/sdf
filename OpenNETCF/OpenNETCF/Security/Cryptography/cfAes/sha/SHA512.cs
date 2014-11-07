/*
 * ---------------------------------------------------------
 * NetSHA.SHA512 .Net Framework implementation of SHA-512
 * 
 * by Jarrod Nelson for ECE 575 Project  
 * --------------------------------------------------------- 
 * Comments:
 * This class contains static methods which
 * implement SHA-512 either on a given file or on
 * a message stored in a byte array.
 * ---------------------------------------------------------
 * 
 */

using System;
using System.IO;

namespace NetSHA
{
	/// <summary>
	/// SHA-512 implementation
	/// </summary>
	public class SHA512
	{
		//SHA-512 constants
		private const ulong _H00 = 0x6a09e667f3bcc908;
		private const ulong _H10 = 0xbb67ae8584caa73b;
		private const ulong _H20 = 0x3c6ef372fe94f82b;
		private const ulong _H30 = 0xa54ff53a5f1d36f1;
		private const ulong _H40 = 0x510e527fade682d1;
		private const ulong _H50 = 0x9b05688c2b3e6c1f;
		private const ulong _H60 = 0x1f83d9abfb41bd6b;
		private const ulong _H70 = 0x5be0cd19137e2179;

		/// <summary>
		/// This method take a byte array, pads it according to the 
		/// specifications in FIPS 180-2 and passes to the SHA-512
		/// hash function. It returns a byte array containing 
		/// the hash of the given message.
		/// </summary>
		/// <param name="message">Message to be hashed as byte array</param>
		/// <returns>Message hash value as byte array</returns>
		public static byte[] MessageSHA512( byte[] message )
		{
			int n, pBase;
			long l;
			byte[] m;
			byte[] pad;
			byte[] swap;
			byte[] temp;

			// Determine length of message in 512-bit blocks
			n = (int)(message.Length >> 7);

			// Determine space required for padding
			if( (message.Length & 0x7f) < 112 )
			{			
				n++;
				pBase = 112;
			}
			else
			{
				n+=2;
				pBase = 240;
			}

			// Determine total message length
			l = message.Length << 3;

			// Create message padding 
			pad = new byte[pBase-(message.Length & 0x7f)];
			pad[0] = 0x80;
			for( int i = 1; i < pad.Length; i++ )
				pad[i] = 0;
			m = new byte[n*128];

			// Assemble padded message
			Array.Copy( message, 0, m, 0, message.Length );
			Array.Copy( pad, 0, m, message.Length, pad.Length );
			temp = BitConverter.GetBytes( (long)l );
			swap = new byte[16];
			for( int i = 0; i < 16; i++ )
			{
				if( i < 8 )
					swap[i] = 0;
				else
					swap[i] = temp[15-i];
			}
			Array.Copy( swap, 0, m, message.Length+pad.Length, 16 );

			// Call Hash1 to hash message
			return Hash512( n, m );
		}

		/// <summary>
		/// This methods takes the name of a file containing
		/// a message to be hashed.  It loads the message and 
		/// calls MessageSHA512 to pad and hash the message.  This
		/// method returns a byte array containing the hash value
		/// for the given file.
		/// </summary>
		/// <param name="fileName">
		/// Name of file containing message to be hashed.
		/// </param>
		/// <returns>Message hash value as byte array</returns>
		public static byte[] FileSHA512( string fileName )
		{
			byte[] m;

			// Open File and read in message
			FileStream messageFile = new FileStream( fileName, 
				FileMode.Open, FileAccess.Read );

			// Check length of file to ensure it is within
			// currently supported range.
			if( Int32.MaxValue > messageFile.Length )
			{
				m = new byte[ (int)messageFile.Length-2 ];
				messageFile.Read( m, 0, (int)messageFile.Length-2 );
			}
			else
				throw new Exception( "File length exceeds current supported size!" );

			messageFile.Close();

			// Call MessageSHA1
			return MessageSHA512( m );
		}

		/// <summary>
		/// SHA-1 Hash function - This method should only be used
		/// with properly padded messages.  To hash an unpadded 
		/// message use one of the other methods.  This method is
		/// called by other methods once the message is padded
		/// and loaded as a byte array.  For internal use only!
		/// </summary>
		/// <param name="n">Number of 512-bit message segments</param>
		/// <param name="m">Message to be hashed as byte array</param>
		/// <returns>Hash value as byte array</returns>
		private static byte[] Hash512( int n, byte[] m )
		{
			// K Constants			
			ulong[] K = { 0x428a2f98d728ae22, 0x7137449123ef65cd, 0xb5c0fbcfec4d3b2f, 0xe9b5dba58189dbbc,
							0x3956c25bf348b538, 0x59f111f1b605d019, 0x923f82a4af194f9b, 0xab1c5ed5da6d8118,
							0xd807aa98a3030242, 0x12835b0145706fbe, 0x243185be4ee4b28c, 0x550c7dc3d5ffb4e2, 
							0x72be5d74f27b896f, 0x80deb1fe3b1696b1, 0x9bdc06a725c71235, 0xc19bf174cf692694,
							0xe49b69c19ef14ad2, 0xefbe4786384f25e3, 0x0fc19dc68b8cd5b5, 0x240ca1cc77ac9c65,
							0x2de92c6f592b0275, 0x4a7484aa6ea6e483, 0x5cb0a9dcbd41fbd4, 0x76f988da831153b5,
							0x983e5152ee66dfab, 0xa831c66d2db43210, 0xb00327c898fb213f, 0xbf597fc7beef0ee4,
							0xc6e00bf33da88fc2, 0xd5a79147930aa725, 0x06ca6351e003826f, 0x142929670a0e6e70,
							0x27b70a8546d22ffc, 0x2e1b21385c26c926, 0x4d2c6dfc5ac42aed, 0x53380d139d95b3df,
							0x650a73548baf63de, 0x766a0abb3c77b2a8, 0x81c2c92e47edaee6, 0x92722c851482353b,
							0xa2bfe8a14cf10364, 0xa81a664bbc423001, 0xc24b8b70d0f89791, 0xc76c51a30654be30,
							0xd192e819d6ef5218, 0xd69906245565a910, 0xf40e35855771202a, 0x106aa07032bbd1b8,
							0x19a4c116b8d2d0c8, 0x1e376c085141ab53, 0x2748774cdf8eeb99, 0x34b0bcb5e19b48a8,
							0x391c0cb3c5c95a63, 0x4ed8aa4ae3418acb, 0x5b9cca4f7763e373, 0x682e6ff3d6b2b8a3,
							0x748f82ee5defb2fc, 0x78a5636f43172f60, 0x84c87814a1f0ab72, 0x8cc702081a6439ec,
							0x90befffa23631e28, 0xa4506cebde82bde9, 0xbef9a3f7b2c67915, 0xc67178f2e372532b,
							0xca273eceea26619c, 0xd186b8c721c0c207, 0xeada7dd6cde0eb1e, 0xf57d4f7fee6ed178,
							0x06f067aa72176fba, 0x0a637dc5a2c898a6, 0x113f9804bef90dae, 0x1b710b35131c471b,
							0x28db77f523047d84, 0x32caab7b40c72493, 0x3c9ebe0a15c9bebc, 0x431d67c49c100d4c,
							0x4cc5d4becb3e42b6, 0x597f299cfc657e2a, 0x5fcb6fab3ad6faec, 0x6c44198c4a475817 };

			// Calculation variables
			ulong a, b, c, d, e, f, g, h, temp1, temp2;
			// Intermediate hash values
			ulong[] interHash = new ulong[8];
			// Scheduled W values
			ulong[] w = new ulong[80];
			// Final hash byte array
			byte[] hash = new byte[64];
			// Used to correct for endian
			byte[] swap = new byte[8];
			byte[] swap2 = new byte[8];

			// Initial hash values
			interHash[0] = _H00;
			interHash[1] = _H10;
			interHash[2] = _H20;
			interHash[3] = _H30;
			interHash[4] = _H40;
			interHash[5] = _H50;
			interHash[6] = _H60;
			interHash[7] = _H70;

			// Perform hash operation
			for( int i = 0; i < n; i++ )
			{
				// Prepare the message schedule
				for( int t = 0; t < 80; t++ )
				{
					if( t < 16 )
					{	
						for( int j = 0; j < 8; j++ )
							swap[j] = m[i*128+t*8+7-j];
						w[t] = BitConverter.ToUInt64( swap, 0 );	
					}
					else
					{
						w[t] = (ulong)( Sigma( 3, w[t-2] ) + w[t-7] 
							+ Sigma( 2, w[t-15] ) + w[t-16] );
						
					}
				}

				//Initialize the five working variables
				a = interHash[0];
				b = interHash[1];
				c = interHash[2];
				d = interHash[3];
				e = interHash[4];
				f = interHash[5];
				g = interHash[6];
				h = interHash[7];
				
				//Perform main hash loop
				for( int t = 0; t < 80; t++ )
				{
					
					temp1 = (ulong)( h + Sigma( 1, e ) + Func(true, e, f, g) 
						+ K[t] + w[t] );
					temp2 = (ulong)( Sigma( 0, a ) + Func(false, a, b, c ) );
					h = g;
					g = f;
					f = e;
					e = (ulong)( d + temp1 );
					d = c;
					c = b;
					b = a;
					a = (ulong)( temp1 + temp2 );
				}

				//Compute the intermediate hash values
				interHash[0] = (ulong)( (a + interHash[0]) );
				interHash[1] = (ulong)( (b + interHash[1]) );
				interHash[2] = (ulong)( (c + interHash[2]) );
				interHash[3] = (ulong)( (d + interHash[3]) );
				interHash[4] = (ulong)( (e + interHash[4]) );
				interHash[5] = (ulong)( (f + interHash[5]) );
				interHash[6] = (ulong)( (g + interHash[6]) );
				interHash[7] = (ulong)( (h + interHash[7]) );
			}
	
			// Copy Intermediate results to final hash array
			for( int i = 0; i < 8; i++)
			{
				swap2 = BitConverter.GetBytes( (ulong) interHash[i] );
				for( int j = 0; j < 8; j++ )
				{
					swap[j] = swap2[7-j];
				}
				Array.Copy( swap, 0, hash, i*8, 8 );
			}

			return hash;
		}

		/// <summary>
		/// Performs SHA-512 logical functions.  See FIPS 180-2 for
		/// complete description.  For internal use only!
		/// </summary>
		/// <param name="b">
		/// Boolean value indicating whether the desired function is Ch
		/// </param>
		/// <param name="x">First arguement</param>
		/// <param name="y">Second arguement</param>
		/// <param name="z">Third arguement</param>
		/// <returns>Function results</returns>
		private static ulong Func( bool b, ulong x, ulong y, ulong z )
		{
			// Ch function
			if( b )
				return (ulong)( (x & y) ^ ((~x) & z) );
			// Maj function
			return (ulong)( (x & y) ^ (x & z) ^ (y & z) );
			
		}

		/// <summary>
		/// Performs the SHA-512 shifting and rotating functions.
		/// See FIPS 180-2 for complete details.  For internal
		/// use only!
		/// </summary>
		/// <param name="i">Integer indicating which function to perform</param>
		/// <param name="x">Operand</param>
		/// <returns>Result of rotation/shift</returns>
		private static ulong Sigma( int i, ulong x )
		{
			ulong temp;

			switch( i )
			{
				case 0:
					temp = (ulong)( (x>>28) | (x<<36) );
					temp = temp ^ (ulong)( (x>>34) | (x<<30) );
					temp = temp ^ (ulong)( (x>>39) | (x<<25) );
					break;
				case 1:
					temp = (ulong)( (x>>14) | (x<<50) );
					temp = temp ^ (ulong)( (x>>18) | (x<<46) );
					temp = temp ^ (ulong)( (x>>41) | (x<<23) );
					break;
				case 2:
					temp = (ulong)( (x>>1) | (x<<63) );
					temp = temp ^ (ulong)( (x>>8) | (x<<56) );
					temp = temp ^ (ulong)(x>>7);
					break;
				case 3:
					temp = (ulong)( (x>>19) | (x<<45) );
					temp = temp ^ (ulong)( (x>>61) | (x<<3) );
					temp = temp ^ (ulong)(x>>6);
					break;
				default:
					temp = x;
					break;
			}
			return temp;
		}
	}
}
