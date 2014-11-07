using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    /// <summary>
    /// Provides AES128 encryption/decryption services
    /// </summary>
    public class AES128EncryptionFormatter: SymmetricEncryptionFormatter
    {
        public AES128EncryptionFormatter()
        {
            Rijndael rijndael = Rijndael.Create();
            rijndael.KeySize = 0x80;
            Key = rijndael;
        }

        public AES128EncryptionFormatter(Rijndael key)
            : base(key)
        {
        }

        public override SymmetricAlgorithm Key
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Key");
                }
                if (!(value is Rijndael))
                {
                    throw new ArgumentNullException("Key");
                }
                if (value.KeySize != 0x80)
                {
                    throw new CryptographicException("Invalid Key Size");
                }
                base.Key = value;
            }
        }

        public override string AlgorithmURI
        {
            get
            {
                return "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
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
                if (value.Length != 0x10)
                {
                    throw new CryptographicException("Invalid KeyBytes Size");
                }
                Rijndael rijndael = Rijndael.Create();
                rijndael.KeySize = 0x80;
                rijndael.Key = value;
                rijndael.Padding = PaddingMode.None;
                base.Key = rijndael;
            }
        }

    }
}
