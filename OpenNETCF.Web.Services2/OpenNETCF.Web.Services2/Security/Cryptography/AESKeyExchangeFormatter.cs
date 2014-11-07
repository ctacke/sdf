using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AESKeyExchangeFormatter : SymmetricKeyExchangeFormatter
    {
        // Methods
        static AESKeyExchangeFormatter()
        {
            AESKeyExchangeFormatter.AES_KW_IV = new byte[] { 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6, 0xa6 };
        }


        protected AESKeyExchangeFormatter()
        {
        }


        protected AESKeyExchangeFormatter(Rijndael key)
            : base(key)
        {
        }


        public override byte[] DecryptKey(byte[] cipherText)
        {
            if (cipherText == null)
            {
                throw new ArgumentNullException("cipherText");
            }
            int cbCypherBytes = (cipherText.Length >> 3) - 1;
            if (((cipherText.Length % 8) != 0) || (cbCypherBytes <= 0))
            {
                throw new CryptographicException("BadSizeForKeyToBeWrapped");
            }
            byte[] bufCypher = new byte[cbCypherBytes << 3];
            Rijndael rijndael = (Rijndael)this.Key;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.None;
            ICryptoTransform transform = rijndael.CreateDecryptor();
            if (cbCypherBytes == 1)
            {
                byte[] finalBlock = transform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                for (int num2 = 0; num2 < 8; num2++)
                {
                    if (finalBlock[num2] != AESKeyExchangeFormatter.AES_KW_IV[num2])
                    {
                        throw new CryptographicException("BadSizeForKeyToBeUnwrapped");
                    }
                }
                Buffer.BlockCopy(finalBlock, 8, bufCypher, 0, 8);
                return bufCypher;
            }
            long cb = 0;
            Buffer.BlockCopy(cipherText, 8, bufCypher, 0, bufCypher.Length);
            byte[] keyBuffer = new byte[8];
            byte[] transformBuffer = new byte[0x10];
            Buffer.BlockCopy(cipherText, 0, keyBuffer, 0, 8);
            for (int i = 5; i >= 0; i--)
            {
                for (int j = cbCypherBytes; j >= 1; j--)
                {
                    cb = j + (i * cbCypherBytes);
                    for (int k = 0; k < 8; k++)
                    {
                        byte[] buffer6;
                        int p;
                        byte exp = (byte)((cb >> ((8 * (7 - k)) & 0x3f)) & 0xff);
                        (buffer6 = keyBuffer)[(p = k)] = (byte)(buffer6[p] ^ exp);
                    }
                    Buffer.BlockCopy(keyBuffer, 0, transformBuffer, 0, 8);
                    Buffer.BlockCopy(bufCypher, 8 * (j - 1), transformBuffer, 8, 8);
                    byte[] finalBlock = transform.TransformFinalBlock(transformBuffer, 0, 0x10);
                    Buffer.BlockCopy(finalBlock, 8, bufCypher, 8 * (j - 1), 8);
                    Buffer.BlockCopy(finalBlock, 0, keyBuffer, 0, 8);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                if (keyBuffer[i] != AESKeyExchangeFormatter.AES_KW_IV[i])
                {
                    throw new CryptographicException("BadSizeForKeyToBeUnwrapped");
                }
            }
            return bufCypher;
        }

        public override byte[] EncryptKey(byte[] plainText)
        {
            if (plainText == null)
            {
                throw new ArgumentNullException("plainText");
            }
            int textLengthBytes = plainText.Length >> 3;
            if (((plainText.Length % 8) != 0) || (textLengthBytes <= 0))
            {
                throw new CryptographicException("BadSizeForKeyToBeWrapped");
            }
            Rijndael rijndael = (Rijndael)this.Key;
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.None;
            ICryptoTransform transform = rijndael.CreateEncryptor();
            if (textLengthBytes == 1)
            {
                byte[] buffer1 = new byte[AESKeyExchangeFormatter.AES_KW_IV.Length + plainText.Length];
                Buffer.BlockCopy(AESKeyExchangeFormatter.AES_KW_IV, 0, buffer1, 0, AESKeyExchangeFormatter.AES_KW_IV.Length);
                Buffer.BlockCopy(plainText, 0, buffer1, AESKeyExchangeFormatter.AES_KW_IV.Length, plainText.Length);
                return transform.TransformFinalBlock(buffer1, 0, buffer1.Length);
            }
            long cb = 0;
            byte[] encryptedKey = new byte[(textLengthBytes + 1) << 3];
            Buffer.BlockCopy(plainText, 0, encryptedKey, 8, plainText.Length);
            byte[] key = new byte[8];
            byte[] encryptionBuffer = new byte[0x10];
            Buffer.BlockCopy(AESKeyExchangeFormatter.AES_KW_IV, 0, key, 0, 8);
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 1; j <= textLengthBytes; j++)
                {
                    cb = j + (i * textLengthBytes);
                    Buffer.BlockCopy(key, 0, encryptionBuffer, 0, 8);
                    Buffer.BlockCopy(encryptedKey, 8 * j, encryptionBuffer, 8, 8);
                    byte[] buffer5 = transform.TransformFinalBlock(encryptionBuffer, 0, 0x10);
                    for (int k = 0; k < 8; k++)
                    {
                        byte exp = (byte)((cb >> ((8 * (7 - k)) & 0x3f)) & 0xff);
                        key[k] = (byte)(exp ^ buffer5[k]);
                    }
                    Buffer.BlockCopy(buffer5, 8, encryptedKey, 8 * j, 8);
                }
            }
            Buffer.BlockCopy(key, 0, encryptedKey, 0, 8);
            return encryptedKey;
        }


        // Fields
        private static readonly byte[] AES_KW_IV;
    }

}
