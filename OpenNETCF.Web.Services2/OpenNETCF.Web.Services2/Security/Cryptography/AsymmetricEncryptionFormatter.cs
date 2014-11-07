using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AsymmetricEncryptionFormatter : EncryptionFormatter
    {
        // Methods
        protected AsymmetricEncryptionFormatter()
        {
        }

        public abstract void SetKey(AsymmetricAlgorithm key);
    }

}
