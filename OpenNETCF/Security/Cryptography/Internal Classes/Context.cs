using System;
using System.Text;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Context
	{
		//static constructor
		static Context()
		{
			FindWoteInfo();
		}

		private static void FindWoteInfo()
		{
			try	
			{
				ProviderInfo [] pia = Prov.EnumProviders();
				isCryptoApi = true;
				foreach(ProviderInfo pi in pia)
				{
					if(pi.name == ProvName.MS_ENHANCED_PROV)
						isEnhanced = true;
					if(pi.type == ProvType.DSS_DH)
						isDsa = true;
				}
				if(isEnhanced == false)
					provName = ProvName.MS_DEF_PROV; //dont default to enhanced anymore

			}
			catch(MissingMethodException) //mme
			{
				//dll or method is missing
				//properties default to false;
			}
			//all other exceptions bubble up
		}

		private static bool isCryptoApi = false;
		public static bool IsCryptoApi
		{
			get
			{
				return isCryptoApi;
			}
		}

		private static bool isEnhanced = false;
		public static bool IsEnhanced
		{
			get
			{
				return isEnhanced;
			}
		}

		private static bool isDsa = false;
		public static bool IsDsa
		{
			get
			{
				return isDsa;
			}
		}

		private static IntPtr prov = IntPtr.Zero;
		public static IntPtr Provider
		{
			get{return prov;}
			set{prov=value;}
		}

		private static ProvType provType = ProvType.RSA_FULL;
		public static ProvType ProviderType
		{
			get{return provType;}
			set{provType=value;}
		}

		private static string provName = ProvName.MS_ENHANCED_PROV;
		public static string ProviderName
		{
			get{return provName;}
			set{provName=value;}
		}

		private static string container = "bNbContainer";
		public static string KeyContainer
		{
			get{return container;}
			set{container=value;}
		}

		public static void ResetKeySet()
		{
			IntPtr prov = AcquireContext(container, provName, provType, ContextFlag.DELETEKEYSET);
			Context.ReleaseContext(prov);
			prov = AcquireContext(container, provName, provType, ContextFlag.NEWKEYSET);
			Context.ReleaseContext(prov);
		}

		/// <summary>
		/// MissingMethodException. call AcquireContext instead
		/// </summary>
		public static IntPtr CpAcquireContext(string container, ContextFlag flag)
		{
			IntPtr prov;
			StringBuilder sb = new StringBuilder(container);
			byte[] vTable = new byte[0]; //VTableProvStruc with callbacks
			bool retVal = NativeMethods.CPAcquireContext(out prov, sb, (uint) flag, vTable);
			ErrCode ec = Error.HandleRetVal(retVal);
			return prov;
		}

		public static IntPtr AcquireContext()
		{
			return AcquireContext(container, provName, provType, ContextFlag.NONE);
		}

		public static IntPtr AcquireContext(string container)
		{
			return AcquireContext(container, provName, provType, ContextFlag.NONE);
		}

		public static IntPtr AcquireContext(ProvType provType)
		{
			return AcquireContext(null, null, provType, ContextFlag.NONE);
		}

		public static IntPtr AcquireContext(string provName, ProvType provType)
		{
			return AcquireContext(null, provName, provType, ContextFlag.NONE);
		}

		public static IntPtr AcquireContext(string provName, ProvType provType, ContextFlag conFlag)
		{
			return AcquireContext(null, provName, provType, conFlag);
		}

		public static IntPtr AcquireContext(string conName, string provName, ProvType provType)
		{
			return AcquireContext(conName, provName, provType, ContextFlag.NONE);
		}

		public static IntPtr AcquireContext(string conName, string provName, ProvType provType, ContextFlag conFlag)
		{
			IntPtr hProv;
			bool retVal = NativeMethods.CryptAcquireContext(out hProv, conName, provName, (uint) provType, (uint) conFlag);
			ErrCode ec = Error.HandleRetVal(retVal, ErrCode.NTE_BAD_KEYSET);
			if(ec == ErrCode.NTE_BAD_KEYSET) //try creating a new key container
			{
                retVal = NativeMethods.CryptAcquireContext(out hProv, conName, provName, (uint)provType, (uint)ContextFlag.NEWKEYSET);
				ec = Error.HandleRetVal(retVal);
			}
			if(hProv == IntPtr.Zero)
				throw new Exception("bNb.Sec: " + ec.ToString());
			return hProv;
		}

		public static void ReleaseContext(IntPtr prov)
		{
			uint reserved = 0;
			if(prov != IntPtr.Zero)
			{
                bool retVal = NativeMethods.CryptReleaseContext(prov, reserved);
				ErrCode ec = Error.HandleRetVal(retVal); //dont exception
			}
		}

		/// <summary>
		/// INVALID_PARAMETER. no need to ever call this
		/// </summary>
		public static void ContextAddRef(IntPtr prov)
		{
			uint reserved = 0;
			uint flags = 0;
            bool retVal = NativeMethods.CryptContextAddRef(prov, ref reserved, flags);
			ErrCode ec = Error.HandleRetVal(retVal);
		}
	}
}
