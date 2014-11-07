using System;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public sealed class RSA15 : RSA
    {
        // Methods
        public RSA15()
        {
        }


        public RSA15(System.Security.Cryptography.RSA key)
            : base(key)
        {
        }


        // Properties
        public override KeyExchangeFormatter KeyExchangeFormatter
        {
            get
            {
                AsymmetricKeyExchangeFormatter fmt = KeyAlgorithm.CreateKeyExchangeFormatter("http://www.w3.org/2001/04/xmlenc#rsa-1_5") as AsymmetricKeyExchangeFormatter;
                if (fmt == null)
                {
                    throw new System.Security.Cryptography.CryptographicException("KeyExchange Formatter load failed for algorithm http://www.w3.org/2001/04/xmlenc#rsa-1_5");
                }
                fmt.SetKey(this.Key);
                return fmt;
            }
        }

    }

}
