using System;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Key
	{
		public static IntPtr ImportSessionKey(IntPtr prov, Calg algId, byte [] rawKey, bool reverse)
		{
			if(reverse == true)
				Array.Reverse(rawKey, 0, rawKey.Length);
			IntPtr nullKey = Key.ImportKey(prov, NullKey.PrivateKeyWithExponentOfOne, IntPtr.Zero, GenKeyParam.EXPORTABLE);
			IntPtr key = Key.GenKey(prov, algId, GenKeyParam.EXPORTABLE);
			byte [] baSessKey = Key.ExportKey(key, nullKey, KeyBlob.SIMPLEBLOB);
			Key.DestroyKey(key);
			Buffer.BlockCopy(rawKey, 0, baSessKey, 12, rawKey.Length);
			key = Key.ImportKey(prov, baSessKey, IntPtr.Zero, GenKeyParam.EXPORTABLE);
			Key.DestroyKey(nullKey);
			return key;
		}

		public static byte [] ExportSessionKey(IntPtr prov, IntPtr key, int length, bool reverse)
		{
			IntPtr nullKey = Key.ImportKey(prov, NullKey.PrivateKeyWithExponentOfOne, IntPtr.Zero, GenKeyParam.EXPORTABLE);
			byte [] baSessKey = Key.ExportKey(key, nullKey, KeyBlob.SIMPLEBLOB);
			//uint bitLen = BitConverter.ToUInt16(baSessKey, 9);
			//uint byteLen = bitLen / 8;
			byte [] baExcKey = new byte[length]; //byteLen
			Buffer.BlockCopy(baSessKey, 12, baExcKey, 0, baExcKey.Length);
			if(reverse == true)
				Array.Reverse(baExcKey, 0, baExcKey.Length);
			Key.DestroyKey(nullKey);
			return baExcKey;
		}

		public static void SetIv(IntPtr key, byte [] Iv)
		{
			SetKeyParam(key, KeyParam.IV, Iv);
		}

		/// <summary>
		/// BAD_DATA
		/// </summary>
		public static void SetPaddingMode(IntPtr key, PaddingMode pm)
		{
			uint iPm = (uint) pm;
			byte [] ba = BitConverter.GetBytes(iPm);
			SetKeyParam(key, KeyParam.PADDING, ba);
		}

		public static PaddingMode GetPaddingMode(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.PADDING);
			uint iPm = BitConverter.ToUInt32(ba, 0);
			PaddingMode pm = (PaddingMode) iPm;
			return pm;
		}

		public static CipherMode GetCipherMode(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.MODE);
			uint iCm = BitConverter.ToUInt32(ba, 0);
			CipherMode cm = (CipherMode) iCm;
			return cm;
		}

		public static int GetBlockSize(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.BLOCKLEN);
			return BitConverter.ToInt32(ba, 0);
		}

		public static byte [] GetSalt(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.SALT);
			return ba;
		}

		public static byte [] GetIv(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.IV);
			return ba;
		}

		public static int GetKeyLength(IntPtr key)
		{
			byte [] ba = GetKeyParam(key, KeyParam.KEYLEN);
			return BitConverter.ToInt32(ba, 0);
		}

		//algId can also be AT_KEYEXCHANGE = 1, AT_SIGNATURE = 2,
		public static IntPtr GenKey(IntPtr prov, Calg algId, GenKeyParam flags)
		{
			IntPtr key;
			bool retVal = NativeMethods.CryptGenKey(prov, (uint) algId, (uint) flags, out key);
			ErrCode ec = Error.HandleRetVal(retVal);
			return key;
		}

		public static IntPtr GetUserKey(IntPtr prov, KeySpec keySpec)
		{
			IntPtr key;
            bool retVal = NativeMethods.CryptGetUserKey(prov, (uint)keySpec, out key);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.NTE_NO_KEY);
			if(ec == ErrCode.NTE_NO_KEY) //2148073485
			{
                retVal = NativeMethods.CryptGenKey(prov, (uint)keySpec, (uint)GenKeyParam.EXPORTABLE, out key);
				ec = Error.HandleRetVal(retVal);
				//is this necessary? why not just use key from GenKey?
				//retVal = Crypto.CryptGetUserKey(prov, (uint) keySpec, out key);
			}
			if(key == IntPtr.Zero)
				throw new Exception(ec.ToString());
			return key;
		}

		public static IntPtr DeriveKey(IntPtr prov, Calg algId, IntPtr hash, GenKeyParam flags)
		{
			IntPtr key;
            bool retVal = NativeMethods.CryptDeriveKey(prov, (uint)algId, hash, (uint)flags, out key);
			ErrCode ec = Error.HandleRetVal(retVal);
			return key;
		}

		public static byte[] GetKeyParam(IntPtr key, KeyParam param)
		{
			byte[] data = new byte[0];
			uint dataLen = 0;
			uint flags = 0;
			//length
            bool retVal = NativeMethods.CryptGetKeyParam(key, (uint)param, data, ref dataLen, flags);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.MORE_DATA);
			if(ec == ErrCode.MORE_DATA)
			{
				//data
				data = new byte[dataLen];
                retVal = NativeMethods.CryptGetKeyParam(key, (uint)param, data, ref dataLen, flags);
				ec = Error.HandleRetVal(retVal);
			}
			return data;
		}

		public static void SetKeyParam(IntPtr key, KeyParam param, byte[] data)
		{
			uint flags = 0;
            bool retVal = NativeMethods.CryptSetKeyParam(key, (uint)param, data, flags);
			ErrCode ec = Error.HandleRetVal(retVal);
		}

		/// <summary>
		/// INVALID_PARAMETER
		/// </summary>
		public static IntPtr DuplicateKey(IntPtr key)
		{
			uint reserved = 0;
			uint flags = 0;
			IntPtr outKey;
            bool retVal = NativeMethods.CryptDuplicateKey(key, ref reserved, flags, out outKey);
			ErrCode ec = Error.HandleRetVal(retVal);
			return outKey;
		}

		public static void DestroyKey(IntPtr key)
		{
			if(key != IntPtr.Zero)
			{
                bool retVal = NativeMethods.CryptDestroyKey(key);
				ErrCode ec = Error.HandleRetVal(retVal); //dont exception
			}
		}

		public static byte [] ExportKey(IntPtr key, IntPtr pubKey, KeyBlob blobType)
		{
			uint flags = 0;
			//byte[] data = new byte[0]; //did not work for PROV_DSS_DH
			byte[] data = null;
			uint dataLen = 0;
			//length
            bool retVal = NativeMethods.CryptExportKey(key, pubKey, (uint)blobType, flags, data, ref dataLen);
			//ErrCode ec = Error.HandleRetVal(retVal, ErrCode.MORE_DATA);
			//if(ec == ErrCode.MORE_DATA)
			ErrCode ec = Error.HandleRetVal(retVal);
			if(dataLen != 0)
			{
				//data
				data = new byte[dataLen];
                retVal = NativeMethods.CryptExportKey(key, pubKey, (uint)blobType, flags, data, ref dataLen);
				ec = Error.HandleRetVal(retVal);
			}
			return data;
		}

		public static IntPtr ImportKey(IntPtr prov, byte[] keyBlob, IntPtr pubKey, GenKeyParam param)
		{
			uint keyLen = (uint) keyBlob.Length;
			IntPtr key;
            bool retVal = NativeMethods.CryptImportKey(prov, keyBlob, keyLen, pubKey, (uint)param, out key);
			ErrCode ec = Error.HandleRetVal(retVal);
			return key;
		}
	}
}
