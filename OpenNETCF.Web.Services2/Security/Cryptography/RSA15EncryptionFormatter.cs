using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class RSA15EncryptionFormatter : AsymmetricEncryptionFormatter
    {
        // Methods
        public RSA15EncryptionFormatter()
        {
        }


        public RSA15EncryptionFormatter(System.Security.Cryptography.RSA key)
        {
            this.SetKey(key);
        }


        public override byte[] Decrypt(byte[] cipherData)
        {
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Decrypt(cipherData, false);
            }
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Decrypt(cipherData, false);
            }
            return this._key.DecryptValue(cipherData);
        }


        public override byte[] Encrypt(byte[] plainData)
        {
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Encrypt(plainData, false);
            }
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Encrypt(plainData, false);
            }
            return this._key.EncryptValue(plainData);
        }


        public override void SetKey(System.Security.Cryptography.AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            if (!(key is System.Security.Cryptography.RSA))
            {
                throw new ArgumentException("key");
            }
            this._key = (System.Security.Cryptography.RSA)key;
        }
 


        // Properties
        public override string AlgorithmURI
        {
            get
            {
                return "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
            }
        }
 

        // Fields
        private System.Security.Cryptography.RSA _key;
    }

}
