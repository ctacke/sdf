using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Security.Cryptography.Internal
{
    
	[StructLayout(LayoutKind.Sequential)]
	internal struct DHPUBKEY 
	{
		public uint magic;  
		public uint bitlen;
	}

	[StructLayout(LayoutKind.Sequential)]
    internal struct DSSSEED 
	{
		//A DWORD containing the counter value. If the counter value is 0xFFFFFFFF, the seed and counter values are not available. 
		public uint counter; //DWORD  
		//A BYTE string containing the seed value. 
		public byte [] seed; //BYTE [20]
	}

	[StructLayout(LayoutKind.Sequential)]
    internal struct DSSPUBKEY
	{
		//This must always be set to 0x31535344, the ASCII encoding of DSS1. 
		public uint magic; //DWORD
		//Number of bits in the DSS key BLOB's prime, P. 
		public uint bitlen; //DWORD
	} 
    /*
	[StructLayout(LayoutKind.Sequential)]
    internal struct RSAPUBKEY 
	{
		public uint magic; //DWORD
		public uint bitlen; //DWORD
		public uint pubexp; //DWORD
	}*/

	[StructLayout(LayoutKind.Sequential)]
	internal struct PUBLICKEYSTRUC 
	{
		public byte bType; //BYTE
		public byte bVersion; //BYTE
		public ushort reserved; //WORD
		public uint aiKeyAlg; //ALG_ID
	} 
    /*
	[StructLayout(LayoutKind.Sequential)]
    internal struct HMAC_Info 
	{
		public uint HashAlgid; //ALG_ID
		public IntPtr pbInnerString; //BYTE*
		public uint cbInnerString; //DWORD
		public IntPtr pbOuterString; //BYTE*
		public uint cbOuterString; //DWORD
	}

	[StructLayout(LayoutKind.Sequential)]
    internal struct VTableProvStruc 
	{
		public uint Version; //DWORD 
		public IntPtr FuncVerifyImage; //FARPROC 
		public IntPtr FuncReturnhWnd; //FARPROC
		public uint dwProvType; //DWORD
		public byte[] pbContextInfo; //BYTE*
		public uint cbContextInfo; //DWORD
		public string pszProvName; //LPWSTR
	} */

	[StructLayout(LayoutKind.Sequential)]
    internal struct PROV_ENUMALGS_EX 
	{
		public uint aiAlgid; //ALG_ID
		public uint dwDefaultLen; //DWORD
		public uint dwMinLen; //DWORD
		public uint dwMaxLen; //DWORD
		public uint dwProtocols; //DWORD
		public uint dwNameLen; //DWORD
		public string szName; //WCHAR[20]
		public uint dwLongNameLen; //DWORD
		public string szLongName; //WCHAR[40]
	} //4 + 4 + 4 + 4 + 4 + 4 + 40 + 4 + 80 = 148

	[StructLayout(LayoutKind.Sequential)]
    internal struct PROV_ENUMALGS 
	{
		public uint aiAlgid; //ALG_ID
		public uint dwBitLen; //DWORD
		public uint dwNameLen; //DWORD
		public string szName; //WCHAR[20]
	} //4 + 4 + 4 + 40 = 52

	[StructLayout(LayoutKind.Sequential)]
    internal struct CRYPTOAPI_BLOB 
	{
		public int cbData; //DWORD
		public IntPtr pbData; //BYTE*
	}
/*
	[StructLayout(LayoutKind.Sequential)]
    internal struct CRYPTPROTECT_PROMPTSTRUCT
	{
		public uint cbSize; //DWORD
		public uint dwPromptFlags; //DWORD
		public IntPtr  hwndApp; //HWND
		public string szPrompt; //LPCWSTR
	} */
    
	[StructLayout(LayoutKind.Sequential)]
    internal class ProviderInfo
	{
		public string name;
		public ProvType type;
	}
}
