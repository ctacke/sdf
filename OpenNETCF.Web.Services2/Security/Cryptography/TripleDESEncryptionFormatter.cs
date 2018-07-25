using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class TripleDESEncryptionFormatter : SymmetricEncryptionFormatter
    {
        // Methods
        public TripleDESEncryptionFormatter()
        {
        }

        public TripleDESEncryptionFormatter(System.Security.Cryptography.TripleDES key)
            : base(key)
        {
        }
 


        // Properties
        public override string AlgorithmURI
        {
            get
            {
                return "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
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
                    throw new ArgumentNullException("key");
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
                System.Security.Cryptography.TripleDES tdes = System.Security.Cryptography.TripleDES.Create();
                tdes.KeySize = 0xc0;
                tdes.Key = value;
                tdes.Padding = PaddingMode.None;
                base.Key = tdes;
            }
        }
    }
}
