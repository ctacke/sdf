using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class AES128 : AES
    {
        // Methods
        public AES128()
        {
        }


        public AES128(byte[] keyBytes)
            : base(keyBytes)
        {
        }
 

        public AES128(Rijndael key)
            : base(key)
        {
        }
 


        // Properties
        public override EncryptionFormatter EncryptionFormatter
        {
            get
            {
                if (this.Key == null)
                {
                    this.GenerateKey();
                }
                SymmetricEncryptionFormatter fmt = KeyAlgorithm.CreateEncryptionFormatter("http://www.w3.org/2001/04/xmlenc#aes128-cbc") as SymmetricEncryptionFormatter;
                if (fmt == null)
                {
                    throw new CryptographicException("Encryption Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#aes128-cbc");
                }
                fmt.Key = this.Key;
                return fmt;
            }
        }


        public override KeyExchangeFormatter KeyExchangeFormatter
        {
            get
            {
                if (this.Key == null)
                {
                    this.GenerateKey();
                }
                SymmetricKeyExchangeFormatter fmt = KeyAlgorithm.CreateKeyExchangeFormatter("http://www.w3.org/2001/04/xmlenc#kw-aes128") as SymmetricKeyExchangeFormatter;
                if (fmt == null)
                {
                    throw new CryptographicException("KeyExchange Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#kw-aes128");
                }
                fmt.Key = this.Key;
                return fmt;
            }
        }


        public override int KeySize
        {
            get
            {
                return 0x80;
            }
        }
    }

}
