using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AsymmetricKeyAlgorithm : KeyAlgorithm
    {
        // Methods
        protected AsymmetricKeyAlgorithm()
        {
            this._defaultSessionKeyAlgorithm = "AES128";
        }

        protected AsymmetricKeyAlgorithm(AsymmetricAlgorithm key)
        {
            this._defaultSessionKeyAlgorithm = "AES128";
            this.Key = key;
        }

        public virtual SymmetricKeyAlgorithm GenerateSessionKey()
        {
            SymmetricKeyAlgorithm alg = KeyAlgorithm.Create(this.DefaultSessionKeyAlgorithm) as SymmetricKeyAlgorithm;
            if (alg == null)
            {
                throw new ArgumentException("Failed to create Symmetric EncryptionFormatter named " + this.DefaultSessionKeyAlgorithm);
            }
            alg.GenerateKey();
            return alg;
        }

        public virtual SymmetricKeyAlgorithm GenerateSessionKey(byte[] keyBytes)
        {
            SymmetricKeyAlgorithm alg = KeyAlgorithm.Create(this.DefaultSessionKeyAlgorithm) as SymmetricKeyAlgorithm;
            if (alg == null)
            {
                throw new ArgumentException("Failed to create Symmetric EncryptionFormatter named " + this.DefaultSessionKeyAlgorithm);
            }
            alg.KeyBytes = keyBytes;
            return alg;
        }


        // Properties
        public string DefaultSessionKeyAlgorithm
        {
            get
            {
                return this._defaultSessionKeyAlgorithm;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    throw new ArgumentNullException("DefaultSessionKeyAlgorithm");
                }
                //if (!WebServicesConfiguration.SecurityConfiguration.KeyAlgorithms.Contains(value))
                //{
                //    throw new ArgumentException("Unsupported DefaultSessionKeyAlgorithm value " + value);
                //}
                this._defaultSessionKeyAlgorithm = value;
            }
        }

        public abstract AsymmetricAlgorithm Key { get; set; }

        // Fields
        private string _defaultSessionKeyAlgorithm;
    }

}
