/*
 * ---------------------------------------------------------
 * NetSHA.SHA1 .Net Framework implementation of SHA-1
 * 
 * by Jarrod Nelson for ECE 575 Project  
 * --------------------------------------------------------- 
 * Comments:
 * This class contains static methods which
 * implement SHA-1 either on a given file or on
 * a message stored in a byte array.
 * ---------------------------------------------------------
 * 
 */

using System;
using System.IO;

namespace NetSHA
{
	/// <summary>
	/// SHA-1 implementation
	/// </summary>
	public class SHA1
	{
		//SHA-1 constants
		private const uint _H00 = 0x67452301;
		private const uint _H10 = 0xefcdab89;
		private const uint _H20 = 0x98badcfe;
		private const uint _H30 = 0x10325476;
		private const uint _H40 = 0xc3d2e1f0;
	
		/// <summary>
		/// This method take a byte array, pads it according to the 
		/// specifications in FIPS 180-2 and passes to the SHA-1
		/// hash function. It returns a byte array containing 
		/// the hash of the given message.
		/// </summary>
		/// <param name="message">Message to be hashed as byte array</param>
		/// <returns>Message hash value as byte array</returns>
		public static byte[] MessageSHA1( byte[] message )
		{
			int n, pBase;
			long l;
			byte[] m;
			byte[] pad;
			byte[] swap;
			byte[] temp;

			// Determine length of message in 512-bit blocks
			n = (int)(message.Length >> 6);

			// Determine space required for padding
			if( (message.Length & 0x3f) < 56 )
			{			
				n++;
				pBase = 56;
			}
			else
			{
				n+=2;
				pBase = 120;
			}

			// Determine total message length
			l = message.Length << 3;

			// Create message padding 
			pad = new byte[pBase-(message.Length & 0x3f)];
			pad[0] = 0x80;
			for( int i = 1; i < pad.Length; i++ )
				pad[i] = 0;
			m = new byte[n*64];

			// Assemble padded message
			Array.Copy( message, 0, m, 0, message.Length );
			Array.Copy( pad, 0, m, message.Length, pad.Length );
			temp = BitConverter.GetBytes( (long)l );
			swap = new byte[8];
			for( int i = 0; i < 8; i++ )
				swap[i] = temp[7-i];
			Array.Copy( swap, 0, m, message.Length+pad.Length, 8 );

			// Call Hash1 to hash message
			return Hash1( n, m );
		}

		/// <summary>
		/// This methods takes the name of a file containing
		/// a message to be hashed.  It loads the message and 
		/// calls MessageSHA1 to pad and hash the message.  This
		/// method returns a byte array containing the hash value
		/// for the given file.
		/// </summary>
		/// <param name="fileName">
		/// Name of file containing message to be hashed.
		/// </param>
		/// <returns>Message hash value as byte array</returns>
		public static byte[] FileSHA1( string fileName )
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
			return MessageSHA1( m );
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
		private static byte[] Hash1( int n, byte[] m )
		{
			// K Constants
			uint[] K = { 0x5a827999, 0x6ed9eba1, 0x8f1bbcdc, 0xca62c1d6 };
			// Calculation variables
			uint a, b, c, d, e, temp;
			// Intermediate hash values
			uint[] h = new uint[5];
			// Scheduled W values
			uint[] w = new uint[80];
			// Final hash byte array
			byte[] hash = new byte[20];
			// Used to correct for endian
			byte[] swap = new byte[4];
			byte[] swap2 = new byte[4];

			// Initial hash values
			h[0] = _H00;
			h[1] = _H10;
			h[2] = _H20;
			h[3] = _H30;
			h[4] = _H40;

			// Perform hash operation
			for( int i = 0; i < n; i++ )
			{
				// Prepare the message schedule
				for( int t = 0; t < 80; t++ )
				{
					if( t < 16 )
					{	
						for( int j = 0; j < 4; j++ )
							swap[j] = m[i*64+t*4+3-j];
						w[t] = BitConverter.ToUInt32( swap, 0 );	
					}
					else
					{
						temp = (uint)( w[t-3] ^ w[t-8] ^ w[t-14] ^ w[t-16] );
						w[t] = (uint)( (temp>>31) | (temp<<1) );
					}
				}

				//Initialize the five working variables
				a = h[0];
				b = h[1];
				c = h[2];
				d = h[3];
				e = h[4];
				
				//Perform main hash loop
				for( int t = 0; t < 80; t++ )
				{
					int kt;
					if( t < 20 )
						kt = 0;
					else if( t < 40 )
						kt = 1;
					else if( t < 60 )
						kt = 2;
					else
						kt = 3;
					temp = (uint)( (uint)( (a<<5) | (a>>27) ) + Func(t, b, c, d) 
						+ e + K[kt] + w[t] );						
					e = d;
					d = c;
					c = (uint)( (b<<30) | (b>>2) );
					b = a;
					a = temp;
				}

				//Compute the intermediate hash values
				h[0] = (uint)( (a + h[0]) );
				h[1] = (uint)( (b + h[1]) );
				h[2] = (uint)( (c + h[2]) );
				h[3] = (uint)( (d + h[3]) );
				h[4] = (uint)( (e + h[4]) );
			}
	
			// Copy Intermediate results to final hash array
			for( int i = 0; i < 5; i++)
			{
				swap2 = BitConverter.GetBytes( (uint) h[i] );
				for( int j = 0; j < 4; j++ )
				{
					swap[j] = swap2[3-j];
				}
				Array.Copy( swap, 0, hash, i*4, 4 );
			}

			return hash;
		}

		/// <summary>
		/// Performs SHA-1 logical functions.  See FIPS 180-2 for
		/// complete description.  For internal use only!
		/// </summary>
		/// <param name="t">function number</param>
		/// <param name="x">first arguement</param>
		/// <param name="y">second arguement</param>
		/// <param name="z">third arguement</param>
		/// <returns>Function results</returns>
		private static uint Func( int t, uint x, uint y, uint z )
		{
			// Ch function
			if( t < 20 )
				return (uint)( (x & y) ^ ((~x) & z) );
			// Maj function
			else if( (t < 60) && (t >= 40) )
				return (uint)( (x & y) ^ (x & z) ^ (y & z) );
			// Parity function
			else			
				return (uint)( x ^ y ^ z );
		}
	}
}
