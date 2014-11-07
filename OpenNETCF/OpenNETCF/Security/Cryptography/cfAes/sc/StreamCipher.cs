/**
 * (c) 2004 by Vasian Cepa
 * mailbox_200@yahoo.com
 * All rights reserved.
 * 
 * An ECB and CBC stream cipher in C# 
 * Can be use with any block cipher.
 */

using System;
using System.IO; // Stream, MemoryStream

namespace sc
{
	public class StreamCipher
	{
		#region constants
		
		// for use with MakeStreamCtx
		public enum Mode{ECB, CBC}
		// some predefined key sizes
		public static readonly int KeyBits128 = 16; // bytes
		public static readonly int KeyBits192 = 24; // bytes
		public static readonly int KeyBits256 = 32; // bytes
		// for use with Encode
		public static readonly bool ENCRYPT = true;
		public static readonly bool DECRYPT = false;

		#endregion constants

		private StreamCipher(){}

		#region context

		public static StreamCtx MakeStreamCtx(IBlockCipher ibc, byte[] key, byte[] iv)
		{
			return MakeStreamCtx(ibc, key, iv, StreamCipher.Mode.CBC);
		}

		/// <summary>
		/// creates the context to be used by the other methods
		/// once created the context is readonly
		/// use always this method (or its overwrites) to obtain a context
		/// </summary>
		/// <param name="ibc">this method will call InitCipher, you dot not need to call it before</param>
		/// <param name="key">make sure the length of the key is as required
		/// some ciphers have key size restrictions</param>
		/// <param name="iv">is ignored in EBC mode</param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static StreamCtx MakeStreamCtx(IBlockCipher ibc, byte[] key, byte[] iv, StreamCipher.Mode mode)
		{
			StreamCtx ctx = new StreamCtx();
			ctx.Ibc = ibc;
			ctx.Ibc.InitCipher(key);
			ctx.IV = iv;
			ctx.Mode = mode;
			ctx.MakeReadOnly();
			return ctx;
		}

		#endregion context

		#region encode

		/// <summary>
		/// encrypt or decrypt a byte[] array
		/// </summary>
		/// <param name="encrypt">ENCRYPT - encrypt (true), DECRYPT - decrypt (false)</param>
		public static byte[] Encode(StreamCtx ctx, byte[] data, bool encrypt)
		{
			MemoryStream instr = null, outstr = null;
			byte[] output = null;
			try
			{
				instr = new MemoryStream(data, false);
				outstr = new MemoryStream((data.Length - (data.Length % ctx.BlockSize())) + ctx.BlockSize());
				if(encrypt) Encrypt(ctx, instr, outstr);
				else Decrypt(ctx, instr, outstr);
				outstr.Seek(0, SeekOrigin.Begin);
				output = new byte[outstr.Length];
				outstr.Read(output, 0, output.Length);
			}
			finally
			{
				if(instr != null) instr.Close();
				if(outstr != null) outstr.Close();
			}
			return output;
		}

		/// <summary>
		/// just to make stream encryption have the same interface as for bytes
		/// buffer streams before passing them to this method
		/// </summary>
		public static void Encode(StreamCtx ctx, Stream instr, Stream outstr, bool encrypt)
		{
			if(encrypt) Encrypt(ctx, instr, outstr);
			else Decrypt(ctx, instr, outstr);
		}
		
		#endregion encode

		#region encrypt

		/// <summary>
		/// encrypts Stream instr to outStr
		/// buffer streams before passing them to this method
		/// </summary>		
		public static void Encrypt(StreamCtx ctx, Stream instr, Stream outstr)
		{
			if(instr.Length <= 0) return;
			
			byte[] buffer = new byte[ctx.BlockSize()];
			byte[] output = new byte[ctx.BlockSize()];
			bool ivInited = false;
			int i = 0;
			
			do
			{
				i = instr.Read(buffer, 0, ctx.BlockSize());
				if(i < 0) i = 0;
				if((i > 0) && (!ivInited)) // first block
				{
					ivInited = true;
					if(ctx.Mode == Mode.CBC)
					{
						Array.Copy(ctx.IV, 0, output, 0, output.Length);
					}
				}
				if((i <= 0) || (i < buffer.Length)) // lastBlock
				{
					PrepareLastBuffer(buffer, i);
					PrepareBuffer(ctx.Mode, buffer, output);
					ctx.Ibc.Cipher(buffer, output);
					outstr.Write(output, 0, output.Length);
					break;
				}
				PrepareBuffer(ctx.Mode, buffer, output);
				ctx.Ibc.Cipher(buffer, output);
				outstr.Write(output, 0, output.Length);
			} while(true);
			outstr.Flush();
		}

		private static void PrepareBuffer(Mode mode, byte[] inb, byte[] iv)
		{
			if(mode == Mode.ECB) return;
			if(mode == Mode.CBC)
			{
				//CBC Mode
				// inb = inb ^ iv; //iv previous cipher output buffer
				for(int i = 0; i < inb.Length; ++i)
				{
					inb[i] = (byte)((int)inb[i] ^ (int)iv[i]);
				}
			}
		}
		
		// padds the last buffer
		// ms-help://MS.VSCC/MS.MSDNVS/security/aboutcrypto_8jjb.htm
		// (PKCS), PKCS #5, section 6.2
		private static void PrepareLastBuffer(byte[] inb, int i)
		{
			// i < inb.Length
			byte val = (byte)(inb.Length - i);
			for(int j = i; j < inb.Length; ++j)
			{
				inb[j] = val;
			}
		}

		#endregion encrypt

		#region decrypt

		/// <summary>
		/// decrypts Stream instr to outStr
		/// buffer streams before passing them to this method
		/// </summary>
		public static void Decrypt(StreamCtx ctx, Stream instr, Stream outstr)
		{
			// in this method we fail silently
			
			if(instr.Length <= 0) return;
			byte[] buffer = new byte[ctx.BlockSize()];
			byte[] output = new byte[ctx.BlockSize()];
			int i = 0;
			bool ivInited = false;
			byte[] iv = new byte[ctx.BlockSize()];
			// to delay the output, required because we do not rely in
			// instr.Length to find the end of stream
			bool lastInited = false;
			byte[] last = new byte[ctx.BlockSize()];
			do
			{
				i = instr.Read(buffer, 0, buffer.Length);
				if(i < 0) i = 0;
				// must be a multiple of ctx.blockSize
				if((i > 0) && (i != buffer.Length)) return; //throw new Exception();
				if((i > 0) && (!ivInited))
				{
					ivInited = true;
					if(ctx.Mode == Mode.CBC)
					{
						Array.Copy(ctx.IV, 0, iv, 0, ctx.IV.Length);
					}
				}
				if(i <= 0) //lastblock
				{
					// buffer has no data
					if(!lastInited) return; // throw new Exception();
					Array.Copy(last, 0, output, 0, output.Length); //mod
					// remove padd data
					int k = (int)output[output.Length - 1];
					if(k > output.Length) return;
					outstr.Write(output, 0, output.Length - k);
					break;
				}
				ctx.Ibc.InvCipher(buffer, output);
				if(ctx.Mode == Mode.CBC)
				{
					PrepareBuffer(ctx.Mode, output, iv);
					Array.Copy(buffer, 0, iv, 0, buffer.Length); //mod
				}
				//delay output
				if(!lastInited)
				{
					Array.Copy(output, 0, last, 0, output.Length); //mod
					lastInited = true;
				}
				else
				{
					outstr.Write(last, 0, last.Length);
					Array.Copy(output, 0, last, 0, output.Length); //mod
				}
			} while(true);
			outstr.Flush();
		}

		#endregion decrypt

	}//EOC
}
