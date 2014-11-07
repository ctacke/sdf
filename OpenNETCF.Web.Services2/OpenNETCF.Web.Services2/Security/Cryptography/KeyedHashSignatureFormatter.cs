using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class KeyedHashSignatureFormatter : SignatureFormatter
    {
        // Methods
        protected KeyedHashSignatureFormatter()
        {
        }
 
        // Properties
        public abstract byte[] Key { get; set; }
    }

}
