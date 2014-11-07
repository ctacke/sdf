using System;
using System.Text;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Hash
	{
		public static byte [] ComputeHashSsl3Shamd5(byte [] data)
		{
			IntPtr prov = Context.AcquireContext();
			byte [] md5 = ComputeHash(prov, CalgHash.MD5, data);
			byte [] sha = ComputeHash(prov, CalgHash.SHA1, data);
			byte [] md5Sha = new byte[md5.Length + sha.Length];
			Buffer.BlockCopy(md5, 0, md5Sha, 0, md5.Length);
			Buffer.BlockCopy(sha, 0, md5Sha, md5.Length, sha.Length);

			IntPtr hash = Hash.CreateHash(prov, CalgHash.SSL3_SHAMD5);
			Hash.SetHashParam(hash, HashParam.HASHVAL, md5Sha);
			byte [] sig = Hash.SignHash(hash, KeySpec.KEYEXCHANGE); //64 bytes
			Hash.DestroyHash(hash);
			Context.ReleaseContext(prov);
			return sig;
		}

		public static byte [] ComputeHash(CalgHash algId, byte [] data)
		{
			IntPtr key = IntPtr.Zero;
			return ComputeKeyedHash(algId, data, key);
		}

		public static byte [] ComputeKeyedHash(CalgHash algId, byte [] data, IntPtr key)
		{
			//dont have to specify enhanced provider,
			//because hash sizes i care about dont change
			IntPtr prov = Context.AcquireContext(ProvType.RSA_FULL);
			byte [] baHash = ComputeKeyedHash(prov, algId, data, key);
			Context.ReleaseContext(prov);
			return baHash;
		}

		public static byte [] ComputeHash(IntPtr prov, CalgHash algId, byte [] data)
		{
			IntPtr key = IntPtr.Zero;
			return ComputeKeyedHash(prov, algId, data, key);
		}

		public static byte [] ComputeKeyedHash(IntPtr prov, CalgHash algId, byte [] data, IntPtr key)
		{
			IntPtr hash = CreateHash(prov, algId, key);
			HashData(hash, data);
			byte [] baHash = GetHashParam(hash);
			DestroyHash(hash);
			return baHash;
		}

		public static IntPtr CreateHash(IntPtr prov, CalgHash algId)
		{
			IntPtr key = IntPtr.Zero; //non-keyed hash
			return CreateHash(prov, algId, key);
		}

		public static IntPtr CreateHash(IntPtr prov, CalgHash algId, IntPtr key)
		{
			uint flags = 0;
			IntPtr hash;
			bool retVal = NativeMethods.CryptCreateHash(prov, (uint) algId, key, flags, out hash);
			ErrCode ec = Error.HandleRetVal(retVal);
			return hash;
		}

		public static void HashData(IntPtr hash, byte[] data)
		{
			uint flags = 0;
			byte [] tempData = (byte[]) data.Clone();
            bool retVal = NativeMethods.CryptHashData(hash, tempData, tempData.Length, flags);
			ErrCode ec = Error.HandleRetVal(retVal);
		}

		public static byte[] GetHashParam(IntPtr hash)
		{
			byte [] data = new byte[0];
			uint dataLen = 0;
			uint flags = 0;
			//size
            bool retVal = NativeMethods.CryptGetHashParam(hash, (uint)HashParam.HASHVAL, data, ref dataLen, flags);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.MORE_DATA);
			if(ec == ErrCode.MORE_DATA)
			{
				//data
				data = new byte[dataLen];
                retVal = NativeMethods.CryptGetHashParam(hash, (uint)HashParam.HASHVAL, data, ref dataLen, flags);
				ec = Error.HandleRetVal(retVal);
			}
			return data;
		}

		public static void DestroyHash(IntPtr hash)
		{
			if(hash != IntPtr.Zero)
			{
                bool retVal = NativeMethods.CryptDestroyHash(hash);
				ErrCode ec = Error.HandleRetVal(retVal); //dont exception
			}
		}

		public static void SetHashParam(IntPtr hash, HashParam param, byte[] data)
		{
			uint flags = 0;
            bool retVal = NativeMethods.CryptSetHashParam(hash, (uint)param, data, flags);
			ErrCode ec = Error.HandleRetVal(retVal);
		}

		/// <summary>
		/// INVALID_PARAMETER
		/// </summary>
		public static IntPtr DuplicateHash(IntPtr hash)
		{
			uint reserved = 0;
			uint flags = 0;
			IntPtr outHash = IntPtr.Zero;
            bool retVal = NativeMethods.CryptDuplicateHash(hash, ref reserved, flags, out outHash);
			ErrCode ec = Error.HandleRetVal(retVal);
			return outHash;
		}

		public static byte[] SignHash(IntPtr hash, KeySpec keySpec)
		{
			string desc = null; //String.Empty
			uint flags = 0;
			byte [] sig = new byte[0];
			uint sigLen = 0;
			//length
            bool retVal = NativeMethods.CryptSignHash(hash, (uint)keySpec, desc, flags, sig, ref sigLen);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.MORE_DATA);
			if(ec == ErrCode.MORE_DATA)
			{
				//sign
				sig = new byte[sigLen];
                retVal = NativeMethods.CryptSignHash(hash, (uint)keySpec, desc, flags, sig, ref sigLen);
				ec = Error.HandleRetVal(retVal);
			}
			return sig;
		}

		public static void VerifySignature(IntPtr hash, byte[] sig, IntPtr pubKey)
		{
			string desc = null; //String.Empty
			uint flags = 0;
            bool retVal = NativeMethods.CryptVerifySignature(hash, sig, (uint)sig.Length, pubKey, desc, flags);
			ErrCode ec = Error.HandleRetVal(retVal); //2148073478 if doesnt verify
		}

		public static void HashSessionKey(IntPtr hash, IntPtr key)
		{
			uint flags = 0;
            bool retVal = NativeMethods.CryptHashSessionKey(hash, key, flags);
			ErrCode ec = Error.HandleRetVal(retVal);
		}
	}
}
