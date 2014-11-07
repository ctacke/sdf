using LPCSTR = System.String;
using LPWSTR = System.String;
using DWORD = System.Int32;

using System;
using System.Runtime.InteropServices;
using System.Text;
using OpenNETCF.Security.Cryptography.Internal;
using System.Diagnostics;

namespace OpenNETCF.Security.Cryptography
{
    internal static class NativeMethods
    {
        private const string CRYPT32 = "crypt32.dll";
        private const string COREDLL = "coredll.dll";

        //BOOL CeGenRandom(DWORD dwLen, BYTE* pbBuffer);
        [DllImport(COREDLL, EntryPoint = "CeGenRandom", SetLastError = true)]
        internal static extern bool CeGenRandom(int dwLen, byte[] pbBuffer);


        //BOOL CPAcquireContext(HCRYPTPROV* phProv, WCHAR* pszContainer, DWORD dwFlags, PVTableProvStruc pVTable);
        [DllImport(COREDLL, EntryPoint = "CPAcquireContext", SetLastError = true)]
        internal static extern bool CPAcquireContext(out IntPtr phProv, StringBuilder pszContainer, uint dwFlags, byte[] pVTable);

        //1    0 0000ABCC CPAcquireContext 
        //BOOL WINAPI CryptAcquireContext(HCRYPTPROV* phProv, LPCTSTR pszContainer, LPCTSTR pszProvider, DWORD dwProvType, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptAcquireContext", SetLastError = true)]
        internal static extern bool CryptAcquireContext(out IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        //BOOL WINAPI CryptContextAddRef(HCRYPTPROV hProv, DWORD* pdwReserved, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptContextAddRef", SetLastError = true)]
        internal static extern bool CryptContextAddRef(IntPtr hProv, ref uint pdwReserved, uint dwFlags);

        //2    1 00004E78 CPCreateHash 
        //BOOL WINAPI CryptCreateHash(HCRYPTPROV hProv, ALG_ID Algid, HCRYPTKEY hKey, DWORD dwFlags, HCRYPTHASH* phHash);
        [DllImport(COREDLL, EntryPoint = "CryptCreateHash", SetLastError = true)]
        internal static extern bool CryptCreateHash(IntPtr hProv, uint Algid, IntPtr hKey, uint dwFlags, out IntPtr phHash);

        //3    2 00004CC4 CPDecrypt 
        //BOOL CRYPTFUNC CryptDecrypt(HCRYPTKEY hKey, HCRYPTHASH hHash, BOOL Final, DWORD dwFlags, BYTE *pbData, DWORD *pdwDataLen);
        [DllImport(COREDLL, EntryPoint = "CryptDecrypt", SetLastError = true)]
        internal static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

        //4    3 000066D0 CPDeriveKey 
        //BOOL CRYPTFUNC CryptDeriveKey(HCRYPTPROV hProv, ALG_ID Algid, HCRYPTHASH hBaseData, DWORD dwFlags, HCRYPTKEY *phKey);
        [DllImport(COREDLL, EntryPoint = "CryptDeriveKey", SetLastError = true)]
        internal static extern bool CryptDeriveKey(IntPtr hProv, uint Algid, IntPtr hBaseData, uint dwFlags, out IntPtr phKey);

        //5    4 00005A90 CPDestroyHash 
        //BOOL CRYPTFUNC CryptDestroyHash(HCRYPTHASH hHash);
        [DllImport(COREDLL, EntryPoint = "CryptDestroyHash", SetLastError = true)]
        internal static extern bool CryptDestroyHash(IntPtr hHash);

        //6    5 000076A8 CPDestroyKey 
        //BOOL CRYPTFUNC CryptDestroyKey(HCRYPTKEY hKey);
        [DllImport(COREDLL, EntryPoint = "CryptDestroyKey", SetLastError = true)]
        internal static extern bool CryptDestroyKey(IntPtr hKey);

        //7    6 00005C00 CPDuplicateHash 
        //BOOL WINAPI CryptDuplicateHash(HCRYPTHASH hHash, DWORD* pdwReserved, DWORD dwFlags, HCRYPTHASH* phHash);
        [DllImport(COREDLL, EntryPoint = "CryptDuplicateHash", SetLastError = true)]
        internal static extern bool CryptDuplicateHash(IntPtr hHash, ref uint pdwReserved, uint dwFlags, out IntPtr phHash);

        //8    7 000091C8 CPDuplicateKey 
        //BOOL WINAPI CryptDuplicateKey(HCRYPTKEY hKey, DWORD* pdwReserved, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport(COREDLL, EntryPoint = "CryptDuplicateKey", SetLastError = true)]
        internal static extern bool CryptDuplicateKey(IntPtr hKey, ref uint pdwReserved, uint dwFlags, out IntPtr phKey);

        //9    8 00004838 CPEncrypt 
        //BOOL CRYPTFUNC CryptEncrypt(HCRYPTKEY hKey, HCRYPTHASH hHash, BOOL Final, DWORD dwFlags, BYTE *pbData, DWORD *pdwDataLen, DWORD dwBufLen);
        [DllImport(COREDLL, EntryPoint = "CryptEncrypt", SetLastError = true)]
        internal static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen, uint dwBufLen);

        //BOOL WINAPI CryptEnumProviders(DWORD dwIndex, DWORD* pdwReserved, DWORD dwFlags, DWORD* pdwProvType, LPTSTR pszProvName, DWORD* pcbProvName);
        [DllImport(COREDLL, EntryPoint = "CryptEnumProviders", SetLastError = true)]
        internal static extern bool CryptEnumProviders(uint dwIndex, ref uint pdwReserved, uint dwFlags, ref uint pdwProvType, StringBuilder pszProvName, ref uint pcbProvName);


        //BOOL WINAPI CryptEnumProviderTypes(DWORD dwIndex, DWORD* pdwReserved, DWORD dwFlags, DWORD* pdwProvType, LPTSTR pszTypeName, DWORD* pcbTypeName);
        [DllImport(COREDLL, EntryPoint = "CryptEnumProviderTypes", SetLastError = true)]
        internal static extern bool CryptEnumProviderTypes(uint dwIndex, ref uint pdwReserved, uint dwFlags, ref uint pdwProvType, StringBuilder pszTypeName, ref uint pcbTypeName);


        //10    9 0000692C CPExportKey 
        //BOOL WINAPI CryptExportKey(HCRYPTKEY hKey, HCRYPTKEY hExpKey, DWORD dwBlobType, DWORD dwFlags, BYTE* pbData, DWORD* pdwDataLen);
        [DllImport(COREDLL, EntryPoint = "CryptExportKey", SetLastError = true)]
        internal static extern bool CryptExportKey(IntPtr hKey, IntPtr hExpKey, uint dwBlobType, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

        //11    A 000062BC CPGenKey 
        //BOOL WINAPI CryptGenKey(HCRYPTPROV hProv, ALG_ID Algid, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport(COREDLL, EntryPoint = "CryptGenKey", SetLastError = true)]
        internal static extern bool CryptGenKey(IntPtr hProv, uint Algid, uint dwFlags, out IntPtr phKey);

        //12    B 000092A8 CPGenRandom 
        //BOOL CRYPTFUNC CrptGenRandom(HCRYPTPROV hProv, DWORD dwLen, BYTE* pbBuffer);
        [DllImport(COREDLL, EntryPoint = "CryptGenRandom", SetLastError = true)]
        internal static extern bool CryptGenRandom(IntPtr hProv, int dwLen, byte[] pbBuffer);


        //BOOL WINAPI CryptGetDefaultProvider(DWORD dwProvType, DWORD* pdwReserved, DWORD dwFlags, LPTSTR pszProvName, DWORD* pcbProvName);
        [DllImport(COREDLL, EntryPoint = "CryptGetDefaultProvider", SetLastError = true)]
        internal static extern bool CryptGetDefaultProvider(uint dwProvType, ref uint pdwReserved, uint dwFlags, StringBuilder pszProvName, ref uint pcbProvName);


        //13    C 00008B90 CPGetHashParam 
        //BOOL WINAPI CryptGetHashParam(HCRYPTHASH hHash, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptGetHashParam", SetLastError = true)]
        internal static extern bool CryptGetHashParam(IntPtr hHash, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //14    D 00007C2C CPGetKeyParam 
        //BOOL CRYPTFUNC CryptGetKeyParam(HCRYPTKEY hKey, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptGetKeyParam", SetLastError = true)]
        internal static extern bool CryptGetKeyParam(IntPtr hKey, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //15    E 00008130 CPGetProvParam 
        //BOOL WINAPI CryptGetProvParam(HCRYPTPROV hProv, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptGetProvParam", SetLastError = true)]
        internal static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //16    F 000077D4 CPGetUserKey 
        //BOOL WINAPI CryptGetUserKey(HCRYPTPROV hProv, DWORD dwKeySpec, HCRYPTKEY* phUserKey);
        [DllImport(COREDLL, EntryPoint = "CryptGetUserKey", SetLastError = true)]
        internal static extern bool CryptGetUserKey(IntPtr hProv, uint dwKeySpec, out IntPtr phUserKey);

        //17   10 000052DC CPHashData 
        //BOOL WINAPI CryptHashData(HCRYPTHASH hHash, BYTE* pbData, DWORD dwDataLen, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptHashData", SetLastError = true)]
        internal static extern bool CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, uint dwFlags);

        //18   11 0000577C CPHashSessionKey 
        //BOOL WINAPI CryptHashSessionKey(HCRYPTHASH hHash, HCRYPTKEY hKey, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptHashSessionKey", SetLastError = true)]
        internal static extern bool CryptHashSessionKey(IntPtr hHash, IntPtr hKey, uint dwFlags);

        //19   12 00006DDC CPImportKey 
        //BOOL WINAPI CryptImportKey(HCRYPTPROV hProv, BYTE* pbData, DWORD dwDataLenHCRYPTKEY hPubKey, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport(COREDLL, EntryPoint = "CryptImportKey", SetLastError = true)]
        internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbData, uint dwDataLen, IntPtr hPubKey, uint dwFlags, out IntPtr phKey);


        //BOOL WINAPI CryptProtectData(DATA_BLOB* pDataIn, LPCWSTR szDataDescr, DATA_BLOB* pOptionalEntropy, PVOID pvReserved, CRYPTPROTECT_PROMPTSTRUCT* pPromptStruct, DWORD dwFlags, DATA_BLOB* pDataOut);
        [DllImport(COREDLL, EntryPoint = "CryptProtectData", SetLastError = true)]
        internal static extern bool CryptProtectData(ref CRYPTOAPI_BLOB pDataIn, StringBuilder szDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, ref CRYPTOAPI_BLOB pDataOut);


        //20   13 0000AC30 CPReleaseContext 
        //BOOL WINAPI CryptReleaseContext(HCRYPTPROV hProv, DWORD dwFlags); 
        [DllImport(COREDLL, EntryPoint = "CryptReleaseContext", SetLastError = true)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);


        //21   14 000086E0 CPSetHashParam 
        //BOOL WINAPI CryptSetHashParam(HCRYPTHASH hHash, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptSetHashParam", SetLastError = true)]
        internal static extern bool CryptSetHashParam(IntPtr hHash, uint dwParam, byte[] pbData, uint dwFlags);


        //22   15 000078B0 CPSetKeyParam 
        //BOOL WINAPI CryptSetKeyParam(HCRYPTKEY hKey, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptSetKeyParam", SetLastError = true)]
        internal static extern bool CryptSetKeyParam(IntPtr hKey, uint dwParam, byte[] pbData, uint dwFlags);


        //BOOL CRYPTFUNC CryptSetProvider(LPCTSTR pszProvName, DWORD dwProvType);
        [DllImport(COREDLL, EntryPoint = "CryptSetProvider", SetLastError = true)]
        internal static extern bool CryptSetProvider(string pszProvName, uint dwProvType);


        //BOOL WINAPI CryptSetProviderEx(LPCTSTR pszProvName, DWORD dwProvType, DWORD* pdwReserved, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptSetProviderEx", SetLastError = true)]
        internal static extern bool CryptSetProviderEx(string pszProvName, uint dwProvType, ref uint pdwReserved, uint dwFlags);


        //23   16 00008078 CPSetProvParam 
        //BOOL CRYPTFUNC CryptoSetProvParam(HCRYPTPROV hProv, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptSetProvParam", SetLastError = true)]
        internal static extern bool CryptSetProvParam(IntPtr hProv, uint dwParam, byte[] pbData, uint dwFlags);


        //24   17 00009304 CPSignHash 
        //BOOL WINAPI CryptSignHash(HCRYPTHASH hHash, DWORD dwKeySpec, LPCTSTR sDescription, DWORD dwFlags, BYTE* pbSignature, DWORD* pdwSigLen);
        [DllImport(COREDLL, EntryPoint = "CryptSignHash", SetLastError = true)]
        internal static extern bool CryptSignHash(IntPtr hHash, uint dwKeySpec, string sDescription, uint dwFlags, byte[] pbSignature, ref uint pdwSigLen);


        //BOOL WINAPI CryptUnprotectData(DATA_BLOB* pDataIn, LPWSTR* ppszDataDescr, DATA_BLOB* pOptionalEntropy, PVOID pvReserved, CRYPTPROTECT_PROMPTSTRUCT* pPromptStruct, DWORD dwFlags, DATA_BLOB* pDataOut);
        [DllImport(COREDLL, EntryPoint = "CryptUnprotectData", SetLastError = true)]
        internal static extern bool CryptUnprotectData(ref CRYPTOAPI_BLOB pDataIn, ref IntPtr ppszDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, ref CRYPTOAPI_BLOB pDataOut);


        //25   18 000097B0 CPVerifySignature 
        //BOOL WINAPI CryptVerifySignature(HCRYPTHASH hHash, BYTE* pbSignature, DWORD dwSigLen, HCRYPTKEY hPubKey, LPCTSTR sDescription, DWORD dwFlags);
        [DllImport(COREDLL, EntryPoint = "CryptVerifySignature", SetLastError = true)]
        internal static extern bool CryptVerifySignature(IntPtr hHash, byte[] pbSignature, uint dwSigLen, IntPtr hPubKey, string sDescription, uint dwFlags);


        public const int CERT_STORE_PROV_SYSTEM = 10;
        public const int CERT_SYSTEM_STORE_CURRENT_USER_ID = 1;
        public const int CERT_SYSTEM_STORE_LOCATION_SHIFT = 16;
        public const int CERT_SYSTEM_STORE_CURRENT_USER =
            CERT_SYSTEM_STORE_CURRENT_USER_ID << CERT_SYSTEM_STORE_LOCATION_SHIFT;
        public const int CERT_STORE_MAXIMUM_ALLOWED_FLAG = 0x00001000;
        public const int CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5;
        public const int X509_ASN_ENCODING = 1;
        public const int CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES = 7;

        [DllImport(CRYPT32, SetLastError = true)]
        public static extern IntPtr CertOpenStore(
            int lpszStoreProvider,
            DWORD dwMsgAndCertEncodingType,
            IntPtr hCryptProv,
            DWORD dwFlags,
            string pvPara
            );

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CertCloseStore(
            IntPtr hCertStore,
            int dwFlags
        );

        [DllImport(CRYPT32, SetLastError = true)]
        public static extern IntPtr CertEnumCertificatesInStore(
            IntPtr hCertStore,
            IntPtr pPrevCertContext
        );

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CertGetNameString(
            IntPtr hCertContext,
            DWORD certType,
            DWORD dwFlags,
            IntPtr pvTypePara,
            StringBuilder name,
            DWORD nameLength
        );

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CertFreeCertificateContext(
            IntPtr pCertContext
        );

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CertAddEncodedCertificateToStore(
          IntPtr hCertStore,
          DWORD dwCertEncodingType,
          byte[] pbCertEncoded,
          DWORD cbCertEncoded,
          DWORD dwAddDisposition,
          ref IntPtr ppCertContext
        );



        [DllImport(COREDLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(
          out IntPtr phProv,
          string pszContainer,
          string pszProvider,
          DWORD dwProvType,
          DWORD dwFlags
        );

        [DllImport(COREDLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptCreateHash(
          IntPtr hProv,
          DWORD Algid,
          DWORD hKey,
          DWORD dwFlags,
          byte[] phHash
        );

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptExportPublicKeyInfo(
            IntPtr hCryptProvOrNCryptKey,
            DWORD dwKeySpec,
            DWORD dwCertEncodingType,
            IntPtr pInfo,
            ref DWORD pcbInfo
        );

        [DllImport(CRYPT32, SetLastError = true)]
        internal static extern IntPtr CertFindCertificateInStore(
            IntPtr hCertStore,
            DWORD dwCertEncodingType,
            DWORD dwFindFlags,
            DWORD dwFindType,
            IntPtr pvFindPara,
            IntPtr pPrevCertContext);

        [DllImport(CRYPT32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CertSetCertificateContextProperty(
            IntPtr pCertContext,
            DWORD dwPropId,
            DWORD dwFlags,
            CRYPT_KEY_PROV_INFO pvData
        );
    }

    internal class CertContext
    {
        internal CertContext(IntPtr pContext)
        {
            var context = (CERT_CONTEXT)Marshal.PtrToStructure(pContext, typeof(CERT_CONTEXT));
            var info = (CERT_INFO)Marshal.PtrToStructure(context.pCertInfo, typeof(CERT_INFO));
        }

        public DWORD dwCertEncodingType { get; private set; }

        public string Issuer { get; private set; }
    }

    internal class CERT_PUBLIC_KEY_INFO
    {
        public CRYPT_ALGORITHM_IDENTIFIER Algorithm;
        public DWORD KeyLength;
        public IntPtr pKeyData;
    }

    internal struct CERT_CONTEXT
    {
        public DWORD dwCertEncodingType;
        public IntPtr pbCertEncoded;
        public DWORD cbCertEncoded;
        public IntPtr pCertInfo;
        public IntPtr hCertStore;

    }

    internal struct CERT_INFO
    {
        public DWORD dwVersion;
        public CRYPT_INTEGER_BLOB SerialNumber;
        public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
        public CERT_NAME_BLOB Issuer;
        public long NotBefore;
        public long NotAfter;
        public CERT_NAME_BLOB Subject;

        // note, this is not complete - there are more fields after here
    }

    internal struct CRYPT_INTEGER_BLOB
    {
        public DWORD Length;
        public IntPtr pData;
    }

    internal unsafe struct CRYPT_ALGORITHM_IDENTIFIER
    {
        public IntPtr pszObjId;
        public DWORD ParamLength;
        public IntPtr pParamData;

        public string Identifier
        {
            get
            {
                var sb = new StringBuilder();

                byte* c = (byte*)pszObjId;

                while (*c != 0x00)
                {
                    sb.Append((char)*c++);
                }

                return sb.ToString();
            }
        }
    }

    internal unsafe struct CERT_NAME_BLOB
    {
        public DWORD Length;
        public IntPtr pData;

        public string Name
        {
            get
            {
                var sb = new StringBuilder();

                byte* c = (byte*)pData;
                var i = 0;

                while (i++ < (Length - 1))
                {
                    Debug.WriteLine(i.ToString());
                    sb.Append((char)*c++);
                }

                return sb.ToString(); ;
            }
        }
    }

    internal class CRYPT_KEY_PROV_INFO
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public LPWSTR pwszContainerName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public LPWSTR pwszProvName;
        public DWORD dwProvType;
        public DWORD dwFlags;
        public DWORD cProvParam;
        public IntPtr rgProvParam;
        public DWORD dwKeySpec;
    }
}
