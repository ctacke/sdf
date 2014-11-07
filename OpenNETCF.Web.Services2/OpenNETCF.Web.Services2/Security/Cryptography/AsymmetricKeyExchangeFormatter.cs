using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AsymmetricKeyExchangeFormatter : KeyExchangeFormatter
    {
        // Methods
        protected AsymmetricKeyExchangeFormatter()
        {
        }

        protected AsymmetricKeyExchangeFormatter(AsymmetricAlgorithm key)
        {
            this.SetKey(key);
        }

        public abstract void SetKey(AsymmetricAlgorithm key);
    }
}
