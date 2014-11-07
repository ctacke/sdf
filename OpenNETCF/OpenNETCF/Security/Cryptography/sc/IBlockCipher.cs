using System;

namespace sc
{
	// any block cipher wrapper must implement this interface
	// see CAes.cs for an example
	public interface IBlockCipher
	{
		void InitCipher(byte[] key); // key.Length is keysize in bytes
		void Cipher(byte[] inb, byte[] outb);
		void InvCipher(byte[] inb, byte[] outb);
		int[] KeySizesInBytes();
		// iv length will/must be the same as BlockSizeInBytes
		int BlockSizeInBytes();
	}//EOC
}
