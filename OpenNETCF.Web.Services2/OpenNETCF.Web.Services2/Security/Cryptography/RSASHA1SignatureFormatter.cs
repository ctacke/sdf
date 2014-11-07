using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class RSASHA1SignatureFormatter : AsymmetricSignatureFormatter
    {
        // Methods
        public RSASHA1SignatureFormatter()
        {
        }

        public RSASHA1SignatureFormatter(System.Security.Cryptography.RSA key)
        {
            this.Key = key;
        }

        public override byte[] Sign(byte[] data)
        {
            SHA1 sha1 = SHA1.Create();
            return this.SignHash(sha1.ComputeHash(data));
        }

        public override byte[] Sign(Stream data)
        {
            SHA1 sha1 = SHA1.Create();
            return this.SignHash(sha1.ComputeHash(data));
        }

        private byte[] SignHash(byte[] rgbHash)
        {
            string oid = CryptoConfig.MapNameToOID("SHA1");
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).SignHash(rgbHash, oid);
            }
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).SignHash(rgbHash, oid);
            }
            int keySizeBytes = this._key.KeySize / 8;
            byte[] keyBuffer = new byte[keySizeBytes];
            byte[] oidBuffer = CryptoConfig.EncodeOID(oid);
            int oidLength = oidBuffer.Length;
            byte[] encBuffer = new byte[(oidLength + 8) + rgbHash.Length];
            encBuffer[0] = 0x30;
            int num5 = encBuffer.Length - 2;
            encBuffer[1] = (byte)num5;
            encBuffer[2] = 0x30;
            num5 = oidBuffer.Length + 2;
            encBuffer[3] = (byte)num5;
            Buffer.BlockCopy(oidBuffer, 0, encBuffer, 4, oidLength);
            encBuffer[4 + oidLength] = 5;
            encBuffer[(4 + oidLength) + 1] = 0;
            encBuffer[(4 + oidLength) + 2] = 4;
            encBuffer[(4 + oidLength) + 3] = (byte)rgbHash.Length;
            Buffer.BlockCopy(rgbHash, 0, encBuffer, oidLength + 8, rgbHash.Length);
            int cboid = (keySizeBytes - rgbHash.Length) - encBuffer.Length;
            if (cboid <= 2)
            {
                throw new CryptographicUnexpectedOperationException("Cryptography_InvalidOID");
            }
            keyBuffer[0] = 0;
            keyBuffer[1] = 1;
            for (int num3 = 2; num3 < (cboid - 1); num3++)
            {
                keyBuffer[num3] = 0xff;
            }
            keyBuffer[cboid - 1] = 0;
            Buffer.BlockCopy(encBuffer, 0, keyBuffer, cboid, encBuffer.Length);
            Buffer.BlockCopy(rgbHash, 0, keyBuffer, cboid + encBuffer.Length, rgbHash.Length);
            return this._key.DecryptValue(keyBuffer);
        }

        public override bool Verify(byte[] signature, byte[] data)
        {
            SHA1 sha1 = SHA1.Create();
            return this.VerifyHash(signature, sha1.ComputeHash(data));
        }


        public override bool Verify(byte[] signature, Stream data)
        {
            SHA1 sha1 = SHA1.Create();
            return this.VerifyHash(signature, sha1.ComputeHash(data));
        }


        private bool VerifyHash(byte[] signature, byte[] rgbHash)
        {
            string oid = CryptoConfig.MapNameToOID("SHA1");
            if (this._key is RSACryptoServiceProvider)
            {
                return ((RSACryptoServiceProvider)this._key).VerifyHash(rgbHash, oid, signature);
            }
            if (this._key is RSACryptoServiceProvider)
            {
                return ((RSACryptoServiceProvider)this._key).VerifyHash(rgbHash, oid, signature);
            }
            int keySizeBytes = this._key.KeySize / 8;
            byte[] keyBuffer = new byte[keySizeBytes];
            byte[] oidBuffer = CryptoConfig.EncodeOID(oid);
            int oidLength = oidBuffer.Length;
            byte[] encBuffer = new byte[(oidLength + 8) + rgbHash.Length];
            encBuffer[0] = 0x30;
            int totalSize = encBuffer.Length - 2;
            encBuffer[1] = (byte)totalSize;
            encBuffer[2] = 0x30;
            totalSize = oidBuffer.Length + 2;
            encBuffer[3] = (byte)totalSize;
            Buffer.BlockCopy(oidBuffer, 0, encBuffer, 4, oidLength);
            encBuffer[4 + oidLength] = 5;
            encBuffer[(4 + oidLength) + 1] = 0;
            encBuffer[(4 + oidLength) + 2] = 4;
            encBuffer[(4 + oidLength) + 3] = (byte)rgbHash.Length;
            Buffer.BlockCopy(rgbHash, 0, encBuffer, oidLength + 8, rgbHash.Length);
            int cboid = (keySizeBytes - rgbHash.Length) - encBuffer.Length;
            if (cboid <= 2)
            {
                throw new CryptographicUnexpectedOperationException("Cryptography_InvalidOID");
            }
            keyBuffer[0] = 0;
            keyBuffer[1] = 1;
            for (int i = 2; i < (cboid - 1); i++)
            {
                keyBuffer[i] = 0xff;
            }
            keyBuffer[cboid - 1] = 0;
            Buffer.BlockCopy(encBuffer, 0, keyBuffer, cboid, encBuffer.Length);
            Buffer.BlockCopy(rgbHash, 0, keyBuffer, cboid + encBuffer.Length, rgbHash.Length);
            byte[] encryptedValue = this._key.EncryptValue(signature);
            return encryptedValue.Equals(keyBuffer);
        }


        // Properties
        public override string AlgorithmURI
        {
            get
            {
                return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            }
        }

        public override AsymmetricAlgorithm Key
        {
            get
            {
                return this._key;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Key");
                }
                if (!(value is System.Security.Cryptography.RSA))
                {
                    throw new ArgumentException("Invalid Key Type");
                }
                this._key = (System.Security.Cryptography.RSA)value;
            }
        }


        // Fields
        private System.Security.Cryptography.RSA _key;
    }
}
