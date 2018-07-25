using System;
using System.Runtime.InteropServices;
using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class WoTe
	{
		//static constructor
		static WoTe()
		{
			OperatingSystem os = System.Environment.OSVersion;
			PlatformID pid = os.Platform;
			Version osV = os.Version;
			string strOsV = osV.ToString(3);
			Version clrV = System.Environment.Version;
			string strClrV = clrV.ToString(3);
			if(pid == PlatformID.WinCE) //WinCE
			{
				_CLR = Clr.Cf10; //assume no SP 1.0.2268
				if(strClrV == "1.0.3111") //SP1
					_CLR = Clr.Cf11;
				if(strClrV == "1.0.3226") //SP2 Recall
					_CLR = Clr.Cf12Recall;
				if(strClrV == "1.0.3227") //SP2 Beta
					_CLR = Clr.Cf12Beta;
				if(strClrV == "1.0.3316") //SP2 Release
					_CLR = Clr.Cf12;
				if(strClrV.CompareTo("1.0.3316") > 0) //SP2 release of greater
					_CLR = Clr.CfNewer;

				_OS = Os.WinCE; //assume WinCE
				_DEVICE = Device.Ce; //assume CE
				//if(strOsV.CompareTo("3.0.0") >= 0)
				//	OS = Os.Ce30;
				if(strOsV.CompareTo("3.0.11171") < 0)
				{
					_OS = Os.Ppc2000;
					_DEVICE = Device.PocketPc;
				}
				if(strOsV == "3.0.11171")
				{
					_OS = Os.Ppc2002;
					_DEVICE = Device.PocketPc;
				}
				if(strOsV.CompareTo("4.10.0") >= 0)
				{
					_OS = Os.Ce41;
					_DEVICE = Device.Ce;
				}
				if(strOsV.CompareTo("4.20.0") >= 0)
				{
					_OS = Os.Ce42;
					_DEVICE = Device.Ce;
				}
				if(strOsV == "4.20.1081")
				{
					_OS = Os.Ppc2003;
					_DEVICE = Device.PocketPc;
				}
				if(strOsV == "4.20.1088")
				{
					_OS = Os.Sp2003;
					_DEVICE = Device.SmartPhone;
				}
			}
			else //WinNT
			{
				_CLR = Clr.Fx10; //assume 1.0
				if(strClrV.CompareTo("1.1.4322") >= 0) //1.1 or greater
					_CLR = Clr.Fx11;

				//TODO
				_OS = Os.WinNT;
				_DEVICE = Device.Desktop;
			}

			FindCryptoWoteInfo();
		}

		public override string ToString()
		{
			OperatingSystem os = System.Environment.OSVersion;
			Version v = System.Environment.Version;
			string wote = "dev: " + DEVICE.ToString() +
				"\r\nos: " + OS.ToString() +
				"\r\nclr: " + CLR.ToString() +
				"\r\n" +
				"\r\nplat: " + System.Environment.OSVersion.Platform.ToString() +
				"\r\nos: " + System.Environment.OSVersion.ToString() + 
				"\r\nver:" + System.Environment.Version.ToString();
			return wote;
		}

		public static void FindCryptoWoteInfo()
		{
			try	
			{
                OpenNETCF.Security.Cryptography.Internal.ProviderInfo[] pia = OpenNETCF.Security.Cryptography.Internal.Prov.EnumProviders();
				isCryptoApi = true;
                foreach (OpenNETCF.Security.Cryptography.Internal.ProviderInfo pi in pia)
				{
                    if (pi.name == OpenNETCF.Security.Cryptography.Internal.ProvName.MS_ENHANCED_PROV)
						isEnhanced = true;
                    if (pi.type == OpenNETCF.Security.Cryptography.Internal.ProvType.DSS_DH)
						isDsa = true;
				}
				//string provName = String.Empty;
				//if(isEnhanced == false)
				//	provName = OpenNETCF.Security.Cryptography.ProvName.MS_DEF_PROV; //dont default to enhanced anymore
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

		private static Clr _CLR;
		[CLSCompliant(false)]
		public static Clr CLR
		{
			get{return _CLR;}
		}
		private static Os _OS;
		[CLSCompliant(false)]
		public static Os OS
		{
			get{return _OS;}
		}
		private static Device _DEVICE;
		[CLSCompliant(false)]
		public static Device DEVICE
		{
			get{return _DEVICE;}
		}

		public enum Os
		{
			//NT
			WinNT,
			Win2000, //5.0.2195.0
			WinXP,
			MediaCenter10,
			TabletPc10, //5.1.2600.0
			Win2003,
			//CE
			WinCE,
			Ppc2000,
			Ppc2002, //3.0.11171
			Ppc2003, //4.20.1081 (phone edition?)
			//Sp2002, //wont run .NETcf
			Sp2003, //4.20.1088
			Ce30,
			Ce41, //4.10.908
			Ce42, //4.20
		}

		public enum Device
		{
			Desktop,
			MediaCenter,
			TabletPc,
			SmartPhone,
			PocketPc,
			//PocketPcPhoneEdition,
			Ce,
		}

		public enum Clr
		{
			Fx10, //1.0.3705 
			Fx11, //1.1.4322
			Cf10, //1.0.2268
			Cf11, //1.0.3111
			Cf12Recall, //1.0.3226
			Cf12Beta, //1.0.3227
			Cf12, //1.0.3316
			CfNewer,
		}

		//http://msdn.microsoft.com/mobility/?pull=/library/en-us/dnppcgen/html/mantsngbin.asp
		//BOOL SystemParametersInfo(UINT uiAction, UINT uiParam, out PVOID pvParam, UINT fWinIni);
		[DllImport("User32.dll")]
		private static extern bool SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, out IntPtr pvParam, UInt32 fWinIni);

		//The application uses the GetSystemMetrics Windows API, passing in SM_TABLETPC, 
		//to determine whether the application running on a Tablet PC. SM_TABLETPC is defined in WinUser.h.
		// The value of SM_TABLETPC is 86.
		//int retVal = GetSystemMetrics(86);
		//int GetSystemMetrics(int nIndex); 
		[DllImport("User32.dll")]
		private static extern int GetSystemMetrics(int nIndex);
	}
}
