using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class KeyExchangeFormatter
    {
        // Methods
        protected KeyExchangeFormatter()
        {
        }
 
        public abstract byte[] DecryptKey(byte[] cipherKey);
        public abstract byte[] EncryptKey(byte[] plainKey);

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
