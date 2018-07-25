using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AES : SymmetricKeyAlgorithm
    {
        // Methods
        protected AES()
        {
        }


        protected AES(Rijndael key)
            : base(key)
        {
        }


        protected AES(byte[] keyBytes)
            : base(keyBytes)
        {
        }


        public override void GenerateKey()
        {
            Rijndael rijndael = Rijndael.Create();
            rijndael.KeySize = this.KeySize;
            rijndael.Padding = PaddingMode.None;
            rijndael.GenerateKey();
            this._key = rijndael;
        }
 


        // Properties
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
                if (!(value is Rijndael))
                {
                    throw new ArgumentException("Invalid Key Type");
                }
                if (value.KeySize != this.KeySize)
                {
                    throw new ArgumentException("Incorrect size for key material");
                }
                this._key = (Rijndael)value;
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
                    throw new ArgumentException("Incorrect size for key material, expected " + (this.KeySize >> 3) + " bytes");
                }
                if (this._key == null)
                {
                    this._key = Rijndael.Create();
                    this._key.KeySize = this.KeySize;
                    this._key.Padding = PaddingMode.None;
                }
                this._key.Key = value.Clone() as byte[];
            }
        }


        public override SignatureFormatter SignatureFormatter
        {
            get
            {
                if (this.Key == null)
                {
                    this.GenerateKey();
                }
                KeyedHashSignatureFormatter fmt = KeyAlgorithm.CreateSignatureFormatter("http://www.w3.org/2000/09/xmldsig#hmac-sha1") as KeyedHashSignatureFormatter;
                if (fmt == null)
                {
                    throw new CryptographicException("Signature Formatter load failed for algorithm http://www.w3.org/2000/09/xmldsig#hmac-sha1");
                }
                fmt.Key = this._key.Key;
                return fmt;
            }
        }
 


        // Fields
        private Rijndael _key;
    }

}
