using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class RSA15KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
    {
        // Methods
        public RSA15KeyExchangeFormatter()
        {
            this._parameters = string.Empty;
        }


        public RSA15KeyExchangeFormatter(System.Security.Cryptography.AsymmetricAlgorithm key)
            : base(key)
        {
            this._parameters = string.Empty;
        }


        public override byte[] DecryptKey(byte[] cipherKey)
        {
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Decrypt(cipherKey, false);
            }
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Decrypt(cipherKey, false);
            }
            //TODO
            OpenNETCF.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter deformatter1 = new OpenNETCF.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter();
            deformatter1.SetKey(this._key);
            deformatter1.Parameters = _parameters;
            return deformatter1.DecryptKeyExchange(cipherKey);
        }


        public override byte[] EncryptKey(byte[] plainKey)
        {
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Encrypt(plainKey, false);
            }
            if (this._key is System.Security.Cryptography.RSACryptoServiceProvider)
            {
                return ((System.Security.Cryptography.RSACryptoServiceProvider)this._key).Encrypt(plainKey, false);
            }
            OpenNETCF.Security.Cryptography.RSAPKCS1KeyExchangeFormatter formatter1 = new OpenNETCF.Security.Cryptography.RSAPKCS1KeyExchangeFormatter();
            formatter1.SetKey(this._key);
            this._parameters = formatter1.Parameters;
            return formatter1.CreateKeyExchange(plainKey);
        }


        public override void SetKey(System.Security.Cryptography.AsymmetricAlgorithm key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!(key is System.Security.Cryptography.RSA))
            {
                throw new ArgumentException("Incorrect Key Type");
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
        private string _parameters;
    }

}
