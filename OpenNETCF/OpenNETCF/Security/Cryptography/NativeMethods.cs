#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Runtime.InteropServices;
using System.Text;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
    internal static class NativeMethods
    {
        //BOOL CeGenRandom(DWORD dwLen, BYTE* pbBuffer);
        [DllImport("coredll.dll", EntryPoint = "CeGenRandom", SetLastError = true)]
        internal static extern bool CeGenRandom(int dwLen, byte[] pbBuffer);


        //BOOL CPAcquireContext(HCRYPTPROV* phProv, WCHAR* pszContainer, DWORD dwFlags, PVTableProvStruc pVTable);
        [DllImport("coredll.dll", EntryPoint = "CPAcquireContext", SetLastError = true)]
        internal static extern bool CPAcquireContext(out IntPtr phProv, StringBuilder pszContainer, uint dwFlags, byte[] pVTable);

        //1    0 0000ABCC CPAcquireContext 
        //BOOL WINAPI CryptAcquireContext(HCRYPTPROV* phProv, LPCTSTR pszContainer, LPCTSTR pszProvider, DWORD dwProvType, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptAcquireContext", SetLastError = true)]
        internal static extern bool CryptAcquireContext(out IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        //BOOL WINAPI CryptContextAddRef(HCRYPTPROV hProv, DWORD* pdwReserved, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptContextAddRef", SetLastError = true)]
        internal static extern bool CryptContextAddRef(IntPtr hProv, ref uint pdwReserved, uint dwFlags);

        //2    1 00004E78 CPCreateHash 
        //BOOL WINAPI CryptCreateHash(HCRYPTPROV hProv, ALG_ID Algid, HCRYPTKEY hKey, DWORD dwFlags, HCRYPTHASH* phHash);
        [DllImport("coredll.dll", EntryPoint = "CryptCreateHash", SetLastError = true)]
        internal static extern bool CryptCreateHash(IntPtr hProv, uint Algid, IntPtr hKey, uint dwFlags, out IntPtr phHash);

        //3    2 00004CC4 CPDecrypt 
        //BOOL CRYPTFUNC CryptDecrypt(HCRYPTKEY hKey, HCRYPTHASH hHash, BOOL Final, DWORD dwFlags, BYTE *pbData, DWORD *pdwDataLen);
        [DllImport("coredll.dll", EntryPoint = "CryptDecrypt", SetLastError = true)]
        internal static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

        //4    3 000066D0 CPDeriveKey 
        //BOOL CRYPTFUNC CryptDeriveKey(HCRYPTPROV hProv, ALG_ID Algid, HCRYPTHASH hBaseData, DWORD dwFlags, HCRYPTKEY *phKey);
        [DllImport("coredll.dll", EntryPoint = "CryptDeriveKey", SetLastError = true)]
        internal static extern bool CryptDeriveKey(IntPtr hProv, uint Algid, IntPtr hBaseData, uint dwFlags, out IntPtr phKey);

        //5    4 00005A90 CPDestroyHash 
        //BOOL CRYPTFUNC CryptDestroyHash(HCRYPTHASH hHash);
        [DllImport("coredll.dll", EntryPoint = "CryptDestroyHash", SetLastError = true)]
        internal static extern bool CryptDestroyHash(IntPtr hHash);

        //6    5 000076A8 CPDestroyKey 
        //BOOL CRYPTFUNC CryptDestroyKey(HCRYPTKEY hKey);
        [DllImport("coredll.dll", EntryPoint = "CryptDestroyKey", SetLastError = true)]
        internal static extern bool CryptDestroyKey(IntPtr hKey);

        //7    6 00005C00 CPDuplicateHash 
        //BOOL WINAPI CryptDuplicateHash(HCRYPTHASH hHash, DWORD* pdwReserved, DWORD dwFlags, HCRYPTHASH* phHash);
        [DllImport("coredll.dll", EntryPoint = "CryptDuplicateHash", SetLastError = true)]
        internal static extern bool CryptDuplicateHash(IntPtr hHash, ref uint pdwReserved, uint dwFlags, out IntPtr phHash);

        //8    7 000091C8 CPDuplicateKey 
        //BOOL WINAPI CryptDuplicateKey(HCRYPTKEY hKey, DWORD* pdwReserved, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport("coredll.dll", EntryPoint = "CryptDuplicateKey", SetLastError = true)]
        internal static extern bool CryptDuplicateKey(IntPtr hKey, ref uint pdwReserved, uint dwFlags, out IntPtr phKey);

        //9    8 00004838 CPEncrypt 
        //BOOL CRYPTFUNC CryptEncrypt(HCRYPTKEY hKey, HCRYPTHASH hHash, BOOL Final, DWORD dwFlags, BYTE *pbData, DWORD *pdwDataLen, DWORD dwBufLen);
        [DllImport("coredll.dll", EntryPoint = "CryptEncrypt", SetLastError = true)]
        internal static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen, uint dwBufLen);

        //BOOL WINAPI CryptEnumProviders(DWORD dwIndex, DWORD* pdwReserved, DWORD dwFlags, DWORD* pdwProvType, LPTSTR pszProvName, DWORD* pcbProvName);
        [DllImport("coredll.dll", EntryPoint = "CryptEnumProviders", SetLastError = true)]
        internal static extern bool CryptEnumProviders(uint dwIndex, ref uint pdwReserved, uint dwFlags, ref uint pdwProvType, StringBuilder pszProvName, ref uint pcbProvName);


        //BOOL WINAPI CryptEnumProviderTypes(DWORD dwIndex, DWORD* pdwReserved, DWORD dwFlags, DWORD* pdwProvType, LPTSTR pszTypeName, DWORD* pcbTypeName);
        [DllImport("coredll.dll", EntryPoint = "CryptEnumProviderTypes", SetLastError = true)]
        internal static extern bool CryptEnumProviderTypes(uint dwIndex, ref uint pdwReserved, uint dwFlags, ref uint pdwProvType, StringBuilder pszTypeName, ref uint pcbTypeName);


        //10    9 0000692C CPExportKey 
        //BOOL WINAPI CryptExportKey(HCRYPTKEY hKey, HCRYPTKEY hExpKey, DWORD dwBlobType, DWORD dwFlags, BYTE* pbData, DWORD* pdwDataLen);
        [DllImport("coredll.dll", EntryPoint = "CryptExportKey", SetLastError = true)]
        internal static extern bool CryptExportKey(IntPtr hKey, IntPtr hExpKey, uint dwBlobType, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

        //11    A 000062BC CPGenKey 
        //BOOL WINAPI CryptGenKey(HCRYPTPROV hProv, ALG_ID Algid, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport("coredll.dll", EntryPoint = "CryptGenKey", SetLastError = true)]
        internal static extern bool CryptGenKey(IntPtr hProv, uint Algid, uint dwFlags, out IntPtr phKey);

        //12    B 000092A8 CPGenRandom 
        //BOOL CRYPTFUNC CrptGenRandom(HCRYPTPROV hProv, DWORD dwLen, BYTE* pbBuffer);
        [DllImport("coredll.dll", EntryPoint = "CryptGenRandom", SetLastError = true)]
        internal static extern bool CryptGenRandom(IntPtr hProv, int dwLen, byte[] pbBuffer);


        //BOOL WINAPI CryptGetDefaultProvider(DWORD dwProvType, DWORD* pdwReserved, DWORD dwFlags, LPTSTR pszProvName, DWORD* pcbProvName);
        [DllImport("coredll.dll", EntryPoint = "CryptGetDefaultProvider", SetLastError = true)]
        internal static extern bool CryptGetDefaultProvider(uint dwProvType, ref uint pdwReserved, uint dwFlags, StringBuilder pszProvName, ref uint pcbProvName);


        //13    C 00008B90 CPGetHashParam 
        //BOOL WINAPI CryptGetHashParam(HCRYPTHASH hHash, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptGetHashParam", SetLastError = true)]
        internal static extern bool CryptGetHashParam(IntPtr hHash, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //14    D 00007C2C CPGetKeyParam 
        //BOOL CRYPTFUNC CryptGetKeyParam(HCRYPTKEY hKey, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptGetKeyParam", SetLastError = true)]
        internal static extern bool CryptGetKeyParam(IntPtr hKey, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //15    E 00008130 CPGetProvParam 
        //BOOL WINAPI CryptGetProvParam(HCRYPTPROV hProv, DWORD dwParam, BYTE* pbData, DWORD* pdwDataLen, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptGetProvParam", SetLastError = true)]
        internal static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);


        //16    F 000077D4 CPGetUserKey 
        //BOOL WINAPI CryptGetUserKey(HCRYPTPROV hProv, DWORD dwKeySpec, HCRYPTKEY* phUserKey);
        [DllImport("coredll.dll", EntryPoint = "CryptGetUserKey", SetLastError = true)]
        internal static extern bool CryptGetUserKey(IntPtr hProv, uint dwKeySpec, out IntPtr phUserKey);

        //17   10 000052DC CPHashData 
        //BOOL WINAPI CryptHashData(HCRYPTHASH hHash, BYTE* pbData, DWORD dwDataLen, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptHashData", SetLastError = true)]
        internal static extern bool CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, uint dwFlags);

        //18   11 0000577C CPHashSessionKey 
        //BOOL WINAPI CryptHashSessionKey(HCRYPTHASH hHash, HCRYPTKEY hKey, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptHashSessionKey", SetLastError = true)]
        internal static extern bool CryptHashSessionKey(IntPtr hHash, IntPtr hKey, uint dwFlags);

        //19   12 00006DDC CPImportKey 
        //BOOL WINAPI CryptImportKey(HCRYPTPROV hProv, BYTE* pbData, DWORD dwDataLenHCRYPTKEY hPubKey, DWORD dwFlags, HCRYPTKEY* phKey);
        [DllImport("coredll.dll", EntryPoint = "CryptImportKey", SetLastError = true)]
        internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbData, uint dwDataLen, IntPtr hPubKey, uint dwFlags, out IntPtr phKey);


        //BOOL WINAPI CryptProtectData(DATA_BLOB* pDataIn, LPCWSTR szDataDescr, DATA_BLOB* pOptionalEntropy, PVOID pvReserved, CRYPTPROTECT_PROMPTSTRUCT* pPromptStruct, DWORD dwFlags, DATA_BLOB* pDataOut);
        [DllImport("coredll.dll", EntryPoint = "CryptProtectData", SetLastError = true)]
        internal static extern bool CryptProtectData(ref CRYPTOAPI_BLOB pDataIn, StringBuilder szDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, ref CRYPTOAPI_BLOB pDataOut);


        //20   13 0000AC30 CPReleaseContext 
        //BOOL WINAPI CryptReleaseContext(HCRYPTPROV hProv, DWORD dwFlags); 
        [DllImport("coredll.dll", EntryPoint = "CryptReleaseContext", SetLastError = true)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);


        //21   14 000086E0 CPSetHashParam 
        //BOOL WINAPI CryptSetHashParam(HCRYPTHASH hHash, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptSetHashParam", SetLastError = true)]
        internal static extern bool CryptSetHashParam(IntPtr hHash, uint dwParam, byte[] pbData, uint dwFlags);


        //22   15 000078B0 CPSetKeyParam 
        //BOOL WINAPI CryptSetKeyParam(HCRYPTKEY hKey, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptSetKeyParam", SetLastError = true)]
        internal static extern bool CryptSetKeyParam(IntPtr hKey, uint dwParam, byte[] pbData, uint dwFlags);


        //BOOL CRYPTFUNC CryptSetProvider(LPCTSTR pszProvName, DWORD dwProvType);
        [DllImport("coredll.dll", EntryPoint = "CryptSetProvider", SetLastError = true)]
        internal static extern bool CryptSetProvider(string pszProvName, uint dwProvType);


        //BOOL WINAPI CryptSetProviderEx(LPCTSTR pszProvName, DWORD dwProvType, DWORD* pdwReserved, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptSetProviderEx", SetLastError = true)]
        internal static extern bool CryptSetProviderEx(string pszProvName, uint dwProvType, ref uint pdwReserved, uint dwFlags);


        //23   16 00008078 CPSetProvParam 
        //BOOL CRYPTFUNC CryptoSetProvParam(HCRYPTPROV hProv, DWORD dwParam, BYTE* pbData, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptSetProvParam", SetLastError = true)]
        internal static extern bool CryptSetProvParam(IntPtr hProv, uint dwParam, byte[] pbData, uint dwFlags);


        //24   17 00009304 CPSignHash 
        //BOOL WINAPI CryptSignHash(HCRYPTHASH hHash, DWORD dwKeySpec, LPCTSTR sDescription, DWORD dwFlags, BYTE* pbSignature, DWORD* pdwSigLen);
        [DllImport("coredll.dll", EntryPoint = "CryptSignHash", SetLastError = true)]
        internal static extern bool CryptSignHash(IntPtr hHash, uint dwKeySpec, string sDescription, uint dwFlags, byte[] pbSignature, ref uint pdwSigLen);


        //BOOL WINAPI CryptUnprotectData(DATA_BLOB* pDataIn, LPWSTR* ppszDataDescr, DATA_BLOB* pOptionalEntropy, PVOID pvReserved, CRYPTPROTECT_PROMPTSTRUCT* pPromptStruct, DWORD dwFlags, DATA_BLOB* pDataOut);
        [DllImport("coredll.dll", EntryPoint = "CryptUnprotectData", SetLastError = true)]
        internal static extern bool CryptUnprotectData(ref CRYPTOAPI_BLOB pDataIn, ref IntPtr ppszDataDescr, IntPtr pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, ref CRYPTOAPI_BLOB pDataOut);


        //25   18 000097B0 CPVerifySignature 
        //BOOL WINAPI CryptVerifySignature(HCRYPTHASH hHash, BYTE* pbSignature, DWORD dwSigLen, HCRYPTKEY hPubKey, LPCTSTR sDescription, DWORD dwFlags);
        [DllImport("coredll.dll", EntryPoint = "CryptVerifySignature", SetLastError = true)]
        internal static extern bool CryptVerifySignature(IntPtr hHash, byte[] pbSignature, uint dwSigLen, IntPtr hPubKey, string sDescription, uint dwFlags);

    }
}
