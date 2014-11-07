using System;
using System.Text;
using System.Runtime.InteropServices;
/*
namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Cipher
	{
		public static byte [] Encrypt(IntPtr key, IntPtr hash, byte [] data)
		{
			bool final = true;
			uint flags = 0;

			uint dataLen = (uint) data.Length;
			//int blockSize = Key.GetBlockSize(key) / 8;
			//blockSize = Math.Min(blockSize, data.Length);
			uint bufLen = (uint) data.Length; //encrypted data could be larger - 1 block?
			//uint bufLen = (uint) (data.Length + blockSize);
			byte [] encData = new byte[bufLen];
			Array.Copy(data, 0, encData, 0, data.Length);

            bool retVal = NativeMethods.CryptEncrypt(key, hash, final, flags, encData, ref dataLen, bufLen);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.MORE_DATA);
			if(ec == ErrCode.MORE_DATA)
			{
				bufLen = dataLen;
				dataLen = (uint) data.Length;
				encData = new byte[bufLen];
				Buffer.BlockCopy(data, 0, encData, 0, data.Length);
                retVal = NativeMethods.CryptEncrypt(key, hash, final, flags, encData, ref dataLen, bufLen);	
				ec = Error.HandleRetVal(retVal);
			}
			retVal = Format.CompareBytes(data, encData);
			if(retVal == true) //same
				throw new Exception("data was not encrypted");
			//Format.HiddenBytes(data, encData); //catch nullKey encryption
			return encData;
		}

		public static byte[] Decrypt(IntPtr key, IntPtr hash, byte[] data)
		{
			byte [] ciphData = (byte []) data.Clone();
			byte [] clearData;
			bool final = true;
			uint flags = 0;
			uint dataLen = (uint) data.Length;

            bool retVal = NativeMethods.CryptDecrypt(key, hash, final, flags, ciphData, ref dataLen);
			ErrCode ec = Error.HandleRetVal(retVal);
			
			clearData = new byte[dataLen];
			Buffer.BlockCopy(ciphData, 0, clearData, 0, (int) dataLen);
			return clearData;
		}

		public static byte [] ProtectData(byte [] plainIn)
		{
			byte [] cipherBa = ProtectData(plainIn, ProtectParam.UI_FORBIDDEN, null);
			if(cipherBa.Length == plainIn.Length)
				throw new Exception("data was not protected");
			return cipherBa;
		}

		public static byte [] UnprotectData(byte [] cipherIn)
		{
			string outStr = null;
			byte [] plainBa = UnprotectData(cipherIn, ProtectParam.UI_FORBIDDEN, out outStr);
			if(plainBa.Length == cipherIn.Length)
				throw new Exception("data was not unprotected");
			return plainBa;
		}

		//http://www.obviex.com/samples/dpapi.aspx
		//http://msdn.microsoft.com/security/securecode/dotnet/default.aspx?pull=/library/en-us/dnnetsec/html/SecNetHT07.asp
		public static byte [] ProtectData(byte [] plainIn, ProtectParam flags, string desc)
		{
			StringBuilder sb = new StringBuilder(desc);
			CRYPTOAPI_BLOB blobIn = new CRYPTOAPI_BLOB();
			byte [] cipherOut;
			try
			{
				blobIn.cbData = plainIn.Length;
				//blobIn.pbData = plainIn; //byte[]
				//blobIn.pbData = Mem.AllocHGlobal(blobIn.cbData);
				blobIn.pbData = Mem.CryptMemAlloc(blobIn.cbData);
				Marshal.Copy(plainIn, 0, blobIn.pbData, blobIn.cbData);

				IntPtr optEntropy = IntPtr.Zero; //CRYPTOAPI_BLOB*
				IntPtr reserved = IntPtr.Zero; //PVOID
				IntPtr prompt = IntPtr.Zero; //CRYPTPROTECT_PROMPTSTRUCT*
				CRYPTOAPI_BLOB dataOut = new CRYPTOAPI_BLOB();

                bool retVal = NativeMethods.CryptProtectData(ref blobIn, sb, optEntropy, reserved, prompt, (uint)flags, ref dataOut);
				ErrCode ec = Error.HandleRetVal(retVal);
				
				cipherOut = new byte[dataOut.cbData];
				Marshal.Copy(dataOut.pbData, cipherOut, 0, dataOut.cbData);
				//Mem.FreeHGlobal(dataOut.pbData);
				Mem.CryptMemFree(dataOut.pbData);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (blobIn.pbData != IntPtr.Zero)
				{
					//Mem.FreeHGlobal(blobIn.pbData);
					Mem.CryptMemFree(blobIn.pbData);
				}
			}
			return cipherOut;
		}

		//http://www.obviex.com/samples/dpapi.aspx
		//http://msdn.microsoft.com/security/securecode/dotnet/default.aspx?pull=/library/en-us/dnnetsec/html/SecNetHT07.asp
		public static byte [] UnprotectData(byte [] cipherIn, ProtectParam flags, out string desc)
		{
			desc =null; 
			StringBuilder sb = new StringBuilder(); //TODO not returning properly
			CRYPTOAPI_BLOB blobIn = new CRYPTOAPI_BLOB();
			byte [] plainOut;
			try
			{
				blobIn.cbData = cipherIn.Length;
				//blobIn.pbData = cipherIn; //byte[]
				//blobIn.pbData = Mem.AllocHGlobal(blobIn.cbData);
				blobIn.pbData = Mem.CryptMemAlloc(blobIn.cbData);
				Marshal.Copy(cipherIn, 0, blobIn.pbData, blobIn.cbData);

				IntPtr optEntropy = IntPtr.Zero; //CRYPTOAPI_BLOB*
				IntPtr reserved = IntPtr.Zero; //PVOID
				IntPtr prompt = IntPtr.Zero; //CRYPTPROTECT_PROMPTSTRUCT*
				CRYPTOAPI_BLOB dataOut = new CRYPTOAPI_BLOB();
				
				//BUG
				//bool retVal = Crypto.CryptUnprotectData(ref blobIn, sb, optEntropy, reserved, prompt, (uint) flags, ref dataOut);
				//desc = sb.ToString();
				
				//Assuming a max size of 99 characters in the description null terminating character
				IntPtr ppszDescription = Mem.CryptMemAlloc(100);
                bool retVal = NativeMethods.CryptUnprotectData(ref blobIn, ref ppszDescription, optEntropy, reserved, prompt, (uint)flags, ref dataOut);
				desc = Marshal.PtrToStringUni(ppszDescription);
				Mem.CryptMemFree(ppszDescription);
				ErrCode ec = Error.HandleRetVal(retVal);
				
				plainOut = new byte[dataOut.cbData];
				Marshal.Copy(dataOut.pbData, plainOut, 0, dataOut.cbData);
				//Mem.FreeHGlobal(dataOut.pbData);
				Mem.CryptMemFree(dataOut.pbData);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (blobIn.pbData != IntPtr.Zero)
				{
					//Mem.FreeHGlobal(blobIn.pbData);
					Mem.CryptMemFree(blobIn.pbData);
				}
			}
			return plainOut;
		}
	}
}
*/