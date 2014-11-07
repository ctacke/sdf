using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct RSAPublicKeyBlob
    {
        internal byte bType;
        internal byte bVersion;
        internal short reserved;
        internal uint aiKeyAlg;
        internal uint magic;
        internal uint bitlen;
        internal uint pubexp;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct RSAPUBKEY
    {
        internal uint magic;
        internal uint bitlen;
        internal uint pubexp;
    }
 

}
