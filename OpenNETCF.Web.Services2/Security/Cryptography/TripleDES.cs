using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class TripleDES : SymmetricKeyAlgorithm
    {
        // Methods
        public TripleDES()
        {
        }

        public TripleDES(SymmetricAlgorithm key): base(key)
        {
        }

        public override void GenerateKey()
        {
            System.Security.Cryptography.TripleDES tdes = System.Security.Cryptography.TripleDES.Create();
            tdes.Padding = PaddingMode.None;
            tdes.KeySize = this.KeySize;
            tdes.GenerateKey();
            this._key = tdes;
        }


        // Properties
        public override EncryptionFormatter EncryptionFormatter
        {
            get
            {
                if (this._key == null)
                {
                    this.GenerateKey();
                }
                SymmetricEncryptionFormatter formatter = KeyAlgorithm.CreateEncryptionFormatter("http://www.w3.org/2001/04/xmlenc#tripledes-cbc") as SymmetricEncryptionFormatter;
                if (formatter == null)
                {
                    throw new CryptographicException("Encryption Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#tripledes-cbc");
                }
                formatter.Key = this._key;
                return formatter;
            }
        }

        public override SymmetricAlgorithm Key
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
                if (!(value is System.Security.Cryptography.TripleDES))
                {
                    throw new ArgumentException("Invalid Key Type");
                }
                if (value.KeySize != this.KeySize)
                {
                    throw new ArgumentException("Incorrect size for key material");
                }
                this._key = (System.Security.Cryptography.TripleDES)value;
                this._key.Padding = PaddingMode.None;
            }
        }

        public override byte[] KeyBytes
        {
            get
            {
                if (this._key == null)
                {
                    this.GenerateKey();
                }
                return (this._key.Key.Clone() as byte[]);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("KeyBytes");
                }
                if (value.Length != (this.KeySize >> 3))
                {
                    throw new ArgumentException("Incorrect size for key material");
                }
                if (System.Security.Cryptography.TripleDES.IsWeakKey(value))
                {
                    throw new ArgumentException("Value specified is a known weak TripleDES key");
                }
                if (this._key == null)
                {
                    this._key = System.Security.Cryptography.TripleDES.Create();
                    this._key.Padding = PaddingMode.None;
                    this._key.KeySize = this.KeySize;
                }
                this._key.Key = value.Clone() as byte[];
            }
        }

        public override KeyExchangeFormatter KeyExchangeFormatter
        {
            get
            {
                if (this._key == null)
                {
                    this.GenerateKey();
                }
                SymmetricKeyExchangeFormatter formatter = KeyAlgorithm.CreateKeyExchangeFormatter("http://www.w3.org/2001/04/xmlenc#kw-tripledes") as SymmetricKeyExchangeFormatter;
                if (formatter == null)
                {
                    throw new CryptographicException("KeyExchange Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#kw-tripledes");
                }
                formatter.Key = this._key;
                return formatter;
            }
        }

        public override int KeySize
        {
            get
            {
                return 0xc0;
            }
        }

        public override SignatureFormatter SignatureFormatter
        {
            get
            {
                if (this._key == null)
                {
                    this.GenerateKey();
                }
                KeyedHashSignatureFormatter formatter = KeyAlgorithm.CreateSignatureFormatter("http://www.w3.org/2000/09/xmldsig#hmac-sha1") as KeyedHashSignatureFormatter;
                if (formatter == null)
                {
                    throw new CryptographicException("Signature Formatter load failed for algorithm http://www.w3.org/2000/09/xmldsig#hmac-sha1");
                }
                formatter.Key = this._key.Key;
                return formatter;
            }
        }


        // Fields
        private System.Security.Cryptography.TripleDES _key;
    }
}
