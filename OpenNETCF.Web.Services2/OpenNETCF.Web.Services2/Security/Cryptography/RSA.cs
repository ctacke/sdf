using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class RSA : AsymmetricKeyAlgorithm
    {
        // Methods
        protected RSA()
        {
        }

        protected RSA(System.Security.Cryptography.RSA key)
            : base(key)
        {
        }
 
        // Properties
        public override EncryptionFormatter EncryptionFormatter
        {
            get
            {
                AsymmetricEncryptionFormatter fmt = KeyAlgorithm.CreateEncryptionFormatter("http://www.w3.org/2001/04/xmlenc#rsa-1_5") as AsymmetricEncryptionFormatter;
                if (fmt == null)
                {
                    throw new System.Security.Cryptography.CryptographicException("Encryption Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#rsa-1_5");
                }
                fmt.SetKey(this.Key);
                return fmt;
            }
        }


        public override System.Security.Cryptography.AsymmetricAlgorithm Key
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


        public override SignatureFormatter SignatureFormatter
        {
            get
            {
                return new RSASHA1SignatureFormatter(this._key);
            }
        }
 
        // Fields
        private System.Security.Cryptography.RSA _key;
    }

}
