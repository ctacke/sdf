using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class SymmetricKeyExchangeFormatter : KeyExchangeFormatter
    {
        // Methods
        protected SymmetricKeyExchangeFormatter()
        {
            this._key = null;
        }


        protected SymmetricKeyExchangeFormatter(SymmetricAlgorithm key)
        {
            this._key = null;
            this.Key = key;
        }
 


        // Properties
        public virtual SymmetricAlgorithm Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        public abstract byte[] KeyBytes { get; set; }

        // Fields
        private SymmetricAlgorithm _key;
    }

}
