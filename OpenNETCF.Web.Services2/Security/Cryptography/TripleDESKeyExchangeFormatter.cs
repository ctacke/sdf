using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class TripleDESKeyExchangeFormatter : SymmetricKeyExchangeFormatter
    {
        // Methods
        static TripleDESKeyExchangeFormatter()
        {
            TripleDESKeyExchangeFormatter.s_rgbTripleDES_KW_IV = new byte[] { 0x4a, 0xdd, 0xa2, 0x2c, 0x79, 0xe8, 0x21, 5 };
        }

        public TripleDESKeyExchangeFormatter()
        {
        }

        public TripleDESKeyExchangeFormatter(System.Security.Cryptography.TripleDES key)
            : base(key)
        {
        }

        public override byte[] DecryptKey(byte[] rgbEncryptedWrappedKeyData)
        {
            System.Security.Cryptography.TripleDES tdes = (System.Security.Cryptography.TripleDES)this.Key;
            if (((rgbEncryptedWrappedKeyData.Length != 0x20) && (rgbEncryptedWrappedKeyData.Length != 40)) && (rgbEncryptedWrappedKeyData.Length != 0x30))
            {
                throw new CryptographicException("BadSizeForKeyToBeWrapped");
            }
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.None;
            ICryptoTransform transform = tdes.CreateDecryptor(tdes.Key, TripleDESKeyExchangeFormatter.s_rgbTripleDES_KW_IV);
            byte[] buffer1 = transform.TransformFinalBlock(rgbEncryptedWrappedKeyData, 0, rgbEncryptedWrappedKeyData.Length);
            Array.Reverse(buffer1);
            byte[] buffer2 = new byte[8];
            Buffer.BlockCopy(buffer1, 0, buffer2, 0, 8);
            byte[] buffer3 = new byte[buffer1.Length - buffer2.Length];
            Buffer.BlockCopy(buffer1, 8, buffer3, 0, buffer3.Length);
            ICryptoTransform transform2 = tdes.CreateDecryptor(tdes.Key, buffer2);
            byte[] buffer4 = transform2.TransformFinalBlock(buffer3, 0, buffer3.Length);
            byte[] buffer5 = new byte[buffer4.Length - 8];
            Buffer.BlockCopy(buffer4, 0, buffer5, 0, buffer5.Length);
            SHA1 sha1 = SHA1.Create();
            byte[] buffer6 = sha1.ComputeHash(buffer5);
            int bufLen = buffer5.Length;
            for (int keyBufPos = 0; bufLen < buffer4.Length; keyBufPos++)
            {
                if (buffer4[bufLen] != buffer6[keyBufPos])
                {
                    throw new CryptographicException("BadSizeForKeyToBeUnwrapped");
                }
                bufLen++;
            }
            return buffer5;
        }


        public override byte[] EncryptKey(byte[] rgbWrappedKeyData)
        {
            byte[] buffer1 = this.Key.Key;
            SHA1 sha1 = SHA1.Create();
            byte[] buffer2 = sha1.ComputeHash(rgbWrappedKeyData);
            RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
            byte[] buffer3 = new byte[8];
            rngProvider.GetBytes(buffer3);
            byte[] buffer4 = new byte[rgbWrappedKeyData.Length + 8];
            System.Security.Cryptography.TripleDES tdes = System.Security.Cryptography.TripleDES.Create();
            tdes.Padding = PaddingMode.None;
            ICryptoTransform transform = tdes.CreateEncryptor(buffer1, buffer3);
            Buffer.BlockCopy(rgbWrappedKeyData, 0, buffer4, 0, rgbWrappedKeyData.Length);
            Buffer.BlockCopy(buffer2, 0, buffer4, rgbWrappedKeyData.Length, 8);
            byte[] buffer5 = transform.TransformFinalBlock(buffer4, 0, buffer4.Length);
            byte[] buffer6 = new byte[buffer3.Length + buffer5.Length];
            Buffer.BlockCopy(buffer3, 0, buffer6, 0, buffer3.Length);
            Buffer.BlockCopy(buffer5, 0, buffer6, buffer3.Length, buffer5.Length);
            Array.Reverse(buffer6);
            transform = tdes.CreateEncryptor(buffer1, TripleDESKeyExchangeFormatter.s_rgbTripleDES_KW_IV);
            return transform.TransformFinalBlock(buffer6, 0, buffer6.Length);
        }
 


        // Properties
        public override string AlgorithmURI
        {
            get
            {
                return "http://www.w3.org/2001/04/xmlenc#kw-tripledes";
            }
        }

        public override SymmetricAlgorithm Key
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Key");
                }
                if (!(value is System.Security.Cryptography.TripleDES))
                {
                    throw new ArgumentNullException("Key");
                }
                if (value.KeySize != 0xc0)
                {
                    throw new CryptographicException("Invalid Key Size");
                }
                base.Key = value;
            }
        }

        public override byte[] KeyBytes
        {
            get
            {
                if (this.Key != null)
                {
                    return this.Key.Key;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("KeyBytes");
                }
                if (value.Length != 0x18)
                {
                    throw new CryptographicException("Invalid KeyBytes Size");
                }
                System.Security.Cryptography.TripleDES edes1 = System.Security.Cryptography.TripleDES.Create();
                edes1.KeySize = 0xc0;
                edes1.Key = value;
                edes1.Padding = PaddingMode.None;
                base.Key = edes1;
            }
        }


        // Fields
        private static readonly byte[] s_rgbTripleDES_KW_IV;
    }
}
