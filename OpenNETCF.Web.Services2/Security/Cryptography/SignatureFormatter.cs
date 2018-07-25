using System;
using System.IO;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class SignatureFormatter
    {
        // Methods
        protected SignatureFormatter()
        {
        }
 

        public abstract byte[] Sign(Stream data);
        public abstract byte[] Sign(byte[] data);
        public abstract bool Verify(byte[] signature, Stream data);
        public abstract bool Verify(byte[] signature, byte[] data);

        // Properties
        public abstract string AlgorithmURI { get; }
    }

}
