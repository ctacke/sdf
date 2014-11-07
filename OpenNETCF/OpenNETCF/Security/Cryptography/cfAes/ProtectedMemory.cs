using System;
using OpenNETCF.Security.Cryptography.NativeMethods;
using System.Runtime.InteropServices;

using OpenNETCF.Win32; //for Registry
using OpenNETCF.Web.Services2.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography
{
	/// <summary>
	/// Summary description for ProtectedMemory.
	/// </summary>
	public class ProtectedMemory
	{
		private ProtectedMemory(){}

		private static byte[] protectedKey;
		private static byte[] protectedKeyCross;
		static ProtectedMemory()
		{
			byte [] randKey = Rand.GetRandomBytes(8); //for DES
			protectedKey = Cipher.ProtectData(randKey);

			//originally hardcoded
			//byte [] crossKey = new byte [] {0x4a, 0xdd, 0xa2, 0x2c, 0x79, 0xe8, 0x21, 0x05};

			//http://blogs.msdn.com/shawnfa/archive/2004/05/17/133650.aspx
			//i need some way to gen a key based off of the last boot
			//so the next time it reboots the key will be different
			//also so that another process can gen the same key
			int bootCount = 1;
			RegistryKey rkBootCount = Registry.LocalMachine.OpenSubKey("Comm", false);
			if(rkBootCount != null)
			{
				object oBootCount = rkBootCount.GetValue("BootCount", 1);
				bootCount = Int32.Parse(oBootCount.ToString());
			}

			string devId = Core.GetDeviceID();
			byte [] salt = new byte[8];
			for(int i=bootCount; i<bootCount+8; i++)
				salt[i-bootCount] = (byte) (i + 1);
			int iter = bootCount % 100;
			Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(devId, salt, iter);
			byte [] crossKey = rdb.GetBytes(8); //for DES key
			protectedKeyCross = Cipher.ProtectData(crossKey);
		}
		
		public static void Protect(byte[] userData, MemoryProtectionScope scope)
		{
			//IntPtr ip = Mem.CryptMemAlloc(memLen);
			//if(ip == IntPtr.Zero)
			//	throw new Exception("memory not allocated");
			//Marshal.Copy(userData, 0, ip, userData.Length);
			int memLen = userData.Length;
			if(memLen % 16 != 0)
				throw new Exception("message length must be divisible by 16");

			byte [] randKey;
			if(scope == MemoryProtectionScope.SameProcess)
				randKey = Cipher.UnprotectData(protectedKey);
			else //sameLong, crossProcess
				randKey = Cipher.UnprotectData(protectedKeyCross);

			DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
			dcsp.Key = randKey;
			TripleDesNoPadding tdnp = new TripleDesNoPadding(dcsp);
			byte [] cipher = tdnp.Encrypt(userData);

			for(int i=0; i<cipher.Length; i++)
				userData[i] = cipher[i];
			
			//userData = Cipher.ProtectData(userData);
			//byte [] retBa = Cipher.ProtectData(userData);
			//Array.Clear(userData, 0, userData.Length);
			//userData = retBa;
		}
		
		public static void Unprotect(byte[] encryptedData, MemoryProtectionScope scope)
		{
			//if(ip == IntPtr.Zero)
			//	throw new Exception("memory not re-allocated");
			//Mem.CryptMemFree(ip);
			int memLen = encryptedData.Length;
			if(memLen % 16 != 0)
				throw new Exception("message length must be divisible by 16");

			byte [] randKey;
			if(scope == MemoryProtectionScope.SameProcess)
				randKey = Cipher.UnprotectData(protectedKey);
			else //sameLong, crossProcess
				randKey = Cipher.UnprotectData(protectedKeyCross);

			DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
			dcsp.Key = randKey;
			TripleDesNoPadding tdnp = new TripleDesNoPadding(dcsp);
			byte [] clear = tdnp.Decrypt(encryptedData);

			for(int i=0; i<clear.Length; i++)
				encryptedData[i] = clear[i];

			//encryptedData = Cipher.UnprotectData(encryptedData);
			//byte [] retBa = Cipher.UnprotectData(encryptedData);
			//Array.Clear(encryptedData, 0, encryptedData.Length);
			//encryptedData = retBa;
		}

		//private static void VerifyScope(MemoryProtectionScope scope){}
	}

	public enum MemoryProtectionScope
	{
		// Fields
		CrossProcess = 1,
		SameLogon = 2,
		SameProcess = 0
	}
}
