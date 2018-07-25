using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    public class Utils
    {

        static public X509Certificate LoadCertificate(Stream stm)
        {
            byte[] rawKey = new byte[(int)stm.Length];
            stm.Read(rawKey, 0, rawKey.Length);
            return new X509Certificate(rawKey);
        }

        static public X509Certificate LoadCertificate(string path)
        {
            return LoadCertificate(new FileStream(path, System.IO.FileMode.Open, FileAccess.Read));
        }

        static public byte[] LoadBlob(Stream stm)
        {
            byte[] rawKey = new byte[(int)stm.Length];
            stm.Read(rawKey, 0, rawKey.Length);
            return rawKey;
        }

        static public byte[] LoadBlob(string path)
        {
            return LoadBlob(new FileStream(path, System.IO.FileMode.Open, FileAccess.Read));
        }

        static public RSAParameters CertToRsaParams(X509Certificate cert)
        {
            byte[] exponent;
            byte[] modulus;
            OpenNETCF.Web.Services2.DecodeCertKey.GetPublicRsaParams(cert, out exponent, out modulus);
            RSAParameters rsaParam = new RSAParameters();
            rsaParam.Exponent = exponent;
            rsaParam.Modulus = modulus;
            return rsaParam;
        }
    }
}
