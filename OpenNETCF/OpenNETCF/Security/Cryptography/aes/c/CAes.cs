// wrapper around C implementation aesc.dll
// (c) 2004 by Vasian Cepa
//
// June 21, 2004
// http://fp.gladman.plus.com/cryptography_technology/rijndael/index.htm
// http://fp.gladman.plus.com/AES/index.htm 
// http://fp.gladman.plus.com/AES/aes.zip
// based on vb.txt

using sc;
using System.Runtime.InteropServices;

//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp09192002.asp
//DllImport calls LoadLibrary() to do its work. If a specific DLL has already been loaded into a process, LoadLibrary() will succeed, even if the specified path for the load is different.

namespace aes {

public sealed class CAes : IBlockCipher
{
	public enum Mode { Encrypt, Decrypt, Both };
	
	public const string aescDll = "aesc_evc.dll"; //MOD

	[DllImport(aescDll)] private static extern void aes_encrypt_key(byte[] key, int keyLength, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_decrypt_key(byte[] key, int keyLength, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_encrypt_key128(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_decrypt_key128(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_encrypt_key192(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_decrypt_key192(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_encrypt_key256(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_decrypt_key256(byte[] key, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_encrypt(byte[] inb, byte[] outb, long[] ctx);
	[DllImport(aescDll)] private static extern void aes_decrypt(byte[] inb, byte[] outb, long[] ctx);

	private long[] EnCtx = null;
	private long[] DeCtx = null;
	private Mode mode = Mode.Both;
	
	public CAes() : this(Mode.Both)
	{}
	
	public CAes(Mode mode)
	{
		this.mode = mode;
	}
	
	public void InitCipher(byte[] key)
	{
		//aes_encrypt_key(key, keyS, EnCtx);
		if((mode == Mode.Encrypt) || (mode == Mode.Both))
		{
			EnCtx = new long[64];
			switch(key.Length)
			{
				case 16:
					aes_encrypt_key128(key, EnCtx);
					break;
				case 24:
					aes_encrypt_key192(key, EnCtx);
					break;
				case 32:
					aes_encrypt_key256(key, EnCtx);
					break;
			}
		}
		if((mode == Mode.Decrypt) || (mode == Mode.Both))
		{
			DeCtx = new long[64];
			switch(key.Length)
			{
				case 16:
					aes_decrypt_key128(key, DeCtx);
					break;
				case 24:
					aes_decrypt_key192(key, DeCtx);
					break;
				case 32:
					aes_decrypt_key256(key, DeCtx);
					break;
			}
		}
	}
		
	//inb, outb 16 bytes
	public void Cipher(byte[] inb, byte[] outb)
	{
		aes_encrypt(inb, outb, EnCtx);
	}

	//inb, outb 16 bytes
	public void InvCipher(byte[] inb, byte[] outb)
	{
		aes_decrypt(inb, outb, DeCtx);
	}

	public int[] KeySizesInBytes()
	{
		return new int[] {16, 24, 32};
	}

	public int BlockSizeInBytes()
	{
		return 16;
	}

}//EOC

}