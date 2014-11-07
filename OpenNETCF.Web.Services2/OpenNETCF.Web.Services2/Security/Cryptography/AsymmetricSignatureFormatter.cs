using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class AsymmetricSignatureFormatter : SignatureFormatter
    {
        // Methods
        protected AsymmetricSignatureFormatter()
        {
        }

        // Properties
        public abstract AsymmetricAlgorithm Key { get; set; }
    }
}
