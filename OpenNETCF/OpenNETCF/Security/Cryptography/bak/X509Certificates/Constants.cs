using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace OpenNETCF.Security.Cryptography.X509Certificates
{
    public enum StoreLocation
    {
        CurrentUser = 1,
        /// <summary>
        /// Unsupported on Windows CE
        /// </summary>
        LocalMachine = 2,
    }

    public enum StoreName
    {
        CertificateAuthority = 3,
        My = 5,
        Root = 6,
    }
}
