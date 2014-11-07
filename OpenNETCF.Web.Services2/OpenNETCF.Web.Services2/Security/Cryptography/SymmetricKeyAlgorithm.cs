using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class SymmetricKeyAlgorithm : KeyAlgorithm
    {
        // Methods
        protected SymmetricKeyAlgorithm()
        {
        }


        protected SymmetricKeyAlgorithm(SymmetricAlgorithm key)
        {
            this.Key = key;
        }


        protected SymmetricKeyAlgorithm(byte[] key)
        {
            this.KeyBytes = key;
        }
 

        public abstract void GenerateKey();

        // Properties
        public abstract SymmetricAlgorithm Key { get; set; }
        public abstract byte[] KeyBytes { get; set; }
        public abstract int KeySize { get; }
    }

}
