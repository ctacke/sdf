using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class EncryptionFormatter
    {
        // Methods
        protected EncryptionFormatter()
        {
        }
 

        public abstract byte[] Decrypt(byte[] data);
        public abstract byte[] Encrypt(byte[] data);

        // Properties
        public abstract string AlgorithmURI { get; }
        public virtual string Parameters
        {
            get
            {
                return null;
            }
            set
            {
                if ((value != null) && (value.Length != 0))
                {
                    throw new NotSupportedException("Explicit parameters are not supported");
                }
            }
        }
 

    }
}
