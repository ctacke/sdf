using System;
using System.Collections.Generic;
using OpenNETCF.Web.Services2.Security.Configuration;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public abstract class KeyAlgorithm
    {
        // Methods
        protected KeyAlgorithm()
        {
        }


        public static KeyAlgorithm Create(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentNullException("name");
            }
            object typeOrName = WebServicesConfiguration.SecurityConfiguration.KeyAlgorithms[name];
            Type tAlg = null;
            if (typeOrName == null)
            {
                return null;
            }
            if (typeOrName is string)
            {
                tAlg = Type.GetType(typeOrName as string);
            }
            else if (typeOrName is Type)
            {
                tAlg = (Type)typeOrName;
            }
            if (tAlg == null)
            {
                return null;
            }
            return (Activator.CreateInstance(tAlg) as KeyAlgorithm);
        }


        public static object CreateEncryptionFormatter(string uri)
        {
            if ((uri == null) || (uri.Length == 0))
            {
                throw new ArgumentNullException("uri");
            }
            return KeyAlgorithm.CreateFormatter(WebServicesConfiguration.SecurityConfiguration.EncryptionFormatters[uri]);
        }


        private static object CreateFormatter(object typeOrName)
        {
            Type tAlg = null;
            if (typeOrName == null)
            {
                return null;
            }
            if (typeOrName is string)
            {
                tAlg = Type.GetType(typeOrName as string);
            }
            else if (typeOrName is Type)
            {
                tAlg = (Type)typeOrName;
            }
            if (tAlg == null)
            {
                return null;
            }
            return Activator.CreateInstance(tAlg);
        }


        public static object CreateKeyExchangeFormatter(string uri)
        {
            if ((uri == null) || (uri.Length == 0))
            {
                throw new ArgumentNullException("uri");
            }
            return KeyAlgorithm.CreateFormatter(WebServicesConfiguration.SecurityConfiguration.KeyExchangeFormatters[uri]);
        }


        public static object CreateSignatureFormatter(string uri)
        {
            if ((uri == null) || (uri.Length == 0))
            {
                throw new ArgumentNullException("uri");
            }
            return KeyAlgorithm.CreateFormatter(WebServicesConfiguration.SecurityConfiguration.SignatureFormatters[uri]);
        }
 


        // Properties
        public abstract EncryptionFormatter EncryptionFormatter { get; }
        public abstract KeyExchangeFormatter KeyExchangeFormatter { get; }
        public abstract SignatureFormatter SignatureFormatter { get; }
    }
}
