using LPCSTR = System.String;
using DWORD = System.Int32;

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using OpenNETCF.Security.Cryptography.X509Certificates;

namespace OpenNETCF.Security.Cryptography.Internal
{
    internal class PVKFile
    {
        public PVKEncryptionType EncryptionType { get; private set; }
        public int KeySpec { get; private set; }
        public byte[] SaltData { get; private set; }
        public byte[] KeyData { get; private set; }
        public X509Store Store { get; set; }

        public PVKFile(FILE_HDR hdr, byte[] data, X509Store store)
        {
            EncryptionType = hdr.dwEncryptType;
            KeySpec = hdr.dwKeySpec;
            Store = store;

            SaltData = new byte[hdr.cbEncryptData];
            var start = Marshal.SizeOf(hdr);
            Buffer.BlockCopy(data, start, SaltData, 0, hdr.cbEncryptData);

            KeyData = new byte[hdr.cbPvk];
            start += hdr.cbEncryptData;
            Buffer.BlockCopy(data, start, KeyData, 0, hdr.cbPvk);
        }

        public IntPtr Import(string password)
        {
            var containerName = "CERT";
            
            var context = Context.AcquireContext(containerName);

            try
            {
                var hHash = IntPtr.Zero;

                // get a symmetric key to decrypt the private key
                if (NativeMethods.CryptCreateHash(context, Const.CALG_SHA, IntPtr.Zero, 0, out hHash))
                {
                    try
                    {
                        if (SaltData.Length > 0)
                        {
                            if (!NativeMethods.CryptHashData(hHash, SaltData, SaltData.Length, 0))
                            {
                                throw new CryptographicException(Marshal.GetLastWin32Error());
                            }
                        }

                        var passwordBytes = Encoding.ASCII.GetBytes(password);
                        if (!NativeMethods.CryptHashData(hHash, passwordBytes, passwordBytes.Length, 0))
                        {
                            throw new CryptographicException(Marshal.GetLastWin32Error());
                        }
                        var decryptKey = IntPtr.Zero;

                        uint enc = EncryptionType == PVKEncryptionType.RC4Password ? Const.CALG_RC4 : Const.CALG_RC2;

                        if (!NativeMethods.CryptDeriveKey(context, enc, hHash, 0, out decryptKey))
                        {
                            throw new CryptographicException(Marshal.GetLastWin32Error());
                        }

                        try
                        {
                            // decrypt and import the private key
                            var importKey = IntPtr.Zero;
                            if (!NativeMethods.CryptImportKey(context, KeyData, (uint)KeyData.Length, decryptKey, Const.CRYPT_EXPORTABLE, out importKey))
                            {
                                throw new CryptographicException(Marshal.GetLastWin32Error());
                            }

                            // get the info length
                            int length = 0;
                            if (!NativeMethods.CryptExportPublicKeyInfo(context, KeySpec, Const.CRYPT_ASN_ENCODING, IntPtr.Zero, ref length))
                            {
                                throw new CryptographicException(Marshal.GetLastWin32Error());
                            }
                            var pInfo = Marshal.AllocHGlobal(length);

                            try
                            {
                                // get the public key info
                                if (!NativeMethods.CryptExportPublicKeyInfo(context, KeySpec, Const.CRYPT_ASN_ENCODING, pInfo, ref length))
                                {
                                    throw new CryptographicException(Marshal.GetLastWin32Error());
                                }

                                // find a certificate in the store that matches this private key
                                var pCert = NativeMethods.CertFindCertificateInStore(Store.Handle, Const.CRYPT_ASN_ENCODING, 0, Const.CERT_FIND_PUBLIC_KEY, pInfo, IntPtr.Zero);

                                if (pCert == IntPtr.Zero)
                                {
                                    throw new CryptographicException("No Certificate found in store for this Private Key");
                                }

                                var keyProvInfo = new CRYPT_KEY_PROV_INFO();

                                keyProvInfo.pwszContainerName = Context.KeyContainer;
                                keyProvInfo.pwszProvName = Context.ProviderName == null ? null : Const.MS_ENHANCED_PROV;
                                keyProvInfo.dwProvType = (int)Const.PROV_RSA_FULL;
                                keyProvInfo.dwFlags = 0;
                                keyProvInfo.cProvParam = 0;
                                keyProvInfo.rgProvParam = IntPtr.Zero;
                                keyProvInfo.dwKeySpec = KeySpec;


                                if (!NativeMethods.CertSetCertificateContextProperty(
                                        pCert, Const.CERT_KEY_PROV_INFO_PROP_ID,
                                        0, keyProvInfo))
                                {
                                    throw new CryptographicException(Marshal.GetLastWin32Error());
                                }

                                return pCert;

                            }
                            finally
                            {
                                Marshal.FreeHGlobal(pInfo);
                            }
                        }
                        finally
                        {
                            NativeMethods.CryptDestroyKey(decryptKey);
                        }
                    }
                    finally
                    {
                        NativeMethods.CryptDestroyHash(hHash);
                    }
                }
            }
            finally
            {
                Context.ReleaseContext(context);
            }

            return IntPtr.Zero;
        }

        public static PVKFile Create(string filename, X509Store targetStore)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            byte[] buffer;

            using (var reader = File.OpenRead(filename))
            {
                buffer = new byte[reader.Length];
                reader.Read(buffer, 0, buffer.Length);
            }

            return Create(buffer, targetStore);
        }

        public static PVKFile Create(byte[] buffer, X509Store targetStore)
        {
            if (buffer == null) throw new ArgumentException();

            var hdr = BufferToStruct<FILE_HDR>(buffer, 0);

            if (hdr.dwMagic != PVK_MAGIC)
            {
                throw new Exception("Bad PVK File");
            }
            if (hdr.dwVersion != PVK_FILE_VERSION_0)
            {
                throw new Exception("Bad PVK File: Bad version");
            }
            if (hdr.cbEncryptData > MAX_PVK_FILE_LEN)
            {
                throw new Exception("Bad PVK File: Bad data length");
            }
            var expectedLength = Marshal.SizeOf(typeof(FILE_HDR)) + hdr.cbEncryptData;
            if (buffer.Length < expectedLength)
            {
                throw new Exception("Bad PVK File: Not enough data");
            }

            switch (hdr.dwEncryptType)
            {
                case PVKEncryptionType.None:
                case PVKEncryptionType.RC4Password:
                case PVKEncryptionType.RC2CBCPassword:
                    break;
                default:
                    throw new Exception("Bad PVK File: Unsupported encryption type: " + hdr.dwEncryptType.ToString());
            }

            return new PVKFile(hdr, buffer, targetStore);
        }

        private static T BufferToStruct<T>(byte[] buffer, int start)
            where T : struct
        {
            var type = typeof(T);
            var minLength = Marshal.SizeOf(type);
            if ((buffer.Length - start) < minLength)
            {
                throw new ArgumentException("buffer is too small");
            }

            // pin the buffer
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
            }
            finally
            {
                // release the pin
                handle.Free();
            }
        }

        internal struct FILE_HDR
        {
            public uint dwMagic;
            public DWORD dwVersion;
            public DWORD dwKeySpec;
            public PVKEncryptionType dwEncryptType;
            public DWORD cbEncryptData;
            public DWORD cbPvk;
        }

        private const uint PVK_MAGIC = 0xb0b5f11e;
        private const DWORD PVK_FILE_VERSION_0 = 0;
        private const DWORD MAX_PVK_FILE_LEN = 4096;

        internal enum PVKEncryptionType : int
        {
            None = 0,
            RC4Password = 1,
            RC2CBCPassword = 2
        }

        //#define MAX_PASSWORD_LEN 16
    }
}
