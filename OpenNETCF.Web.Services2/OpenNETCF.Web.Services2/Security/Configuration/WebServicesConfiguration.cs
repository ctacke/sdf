using System;
using System.Collections.Specialized;
using System.Collections;
//using System.Security.Cryptography;
using OpenNETCF.Web.Services2.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Configuration
{
    public class WebServicesConfiguration
    {
        private static SecurityConfiguration _securityConfiguration;

        public static SecurityConfiguration SecurityConfiguration
        {
            get { return _securityConfiguration; }
            set { _securityConfiguration = value; }
        }

        static WebServicesConfiguration()
        {
            _securityConfiguration = new SecurityConfiguration();
        }

    }

    public class SecurityConfiguration
    {
        private IDictionary _keyAlgorithms;
        private IDictionary _encryptionFormatters;

        public IDictionary EncryptionFormatters
        {
            get { return _encryptionFormatters; }
        }
        private IDictionary _keyExchangeFormatters;

        public IDictionary KeyExchangeFormatters
        {
            get { return _keyExchangeFormatters; }
        }
        private IDictionary _signatureFormatters;

        public IDictionary SignatureFormatters
        {
            get { return _signatureFormatters; }
        }

        public IDictionary KeyAlgorithms
        {
            get { return _keyAlgorithms; }
        }

        public SecurityConfiguration()
        {
            _keyAlgorithms = InitializeKeyAlgorithms();
            _encryptionFormatters = InitializeEncryptionFormatters();
            _keyExchangeFormatters = InitializeKeyExchangeFormatters();
            _signatureFormatters = InitializeSignatureFormatters();
        }

        private IDictionary InitializeKeyAlgorithms()
        {
            HybridDictionary dict = new HybridDictionary();
            dict["AES128"] = typeof(AES128);
            //dict["AES192"] = typeof(AES192);
            //dict["AES256"] = typeof(AES256);
            dict["TripleDES"] = typeof(TripleDES);
            dict["RSA15"] = typeof(RSA15);
            //dict["RSAOAEP"] = typeof(RSAOAEP);
            return dict;
        }

        private IDictionary InitializeEncryptionFormatters()
        {
            HybridDictionary dict = new HybridDictionary();
            dict["http://www.w3.org/2001/04/xmlenc#aes128-cbc"] = typeof(AES128EncryptionFormatter);
            //dict["http://www.w3.org/2001/04/xmlenc#aes192-cbc"] = typeof(AES192EncryptionFormatter);
            //dict["http://www.w3.org/2001/04/xmlenc#aes256-cbc"] = typeof(AES256EncryptionFormatter);
            dict["http://www.w3.org/2001/04/xmlenc#tripledes-cbc"] = typeof(TripleDESEncryptionFormatter);
            dict["http://www.w3.org/2001/04/xmlenc#rsa-1_5"] = typeof(RSA15EncryptionFormatter);
            return dict;
        }

        private IDictionary InitializeKeyExchangeFormatters()
        {
            HybridDictionary dict = new HybridDictionary();
            dict["http://www.w3.org/2001/04/xmlenc#kw-aes128"] = typeof(AES128KeyExchangeFormatter);
            //dict["http://www.w3.org/2001/04/xmlenc#kw-aes192"] = typeof(AES192KeyExchangeFormatter);
            //dict["http://www.w3.org/2001/04/xmlenc#kw-aes256"] = typeof(AES256KeyExchangeFormatter);
            dict["http://www.w3.org/2001/04/xmlenc#kw-tripledes"] = typeof(TripleDESKeyExchangeFormatter);
            dict["http://www.w3.org/2001/04/xmlenc#rsa-1_5"] = typeof(RSA15KeyExchangeFormatter);
            //dict["http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p"] = typeof(RSAOAEPKeyExchangeFormatter);
            return dict;
        }

        private IDictionary InitializeSignatureFormatters()
        {
            HybridDictionary dict = new HybridDictionary();
            //dict["http://www.w3.org/2000/09/xmldsig#hmac-sha1"] = typeof(HMACSHA1SignatureFormatter);
            dict["http://www.w3.org/2000/09/xmldsig#rsa-sha1"] = typeof(RSASHA1SignatureFormatter);
            return dict;
        }
 

    }
}
