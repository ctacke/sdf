using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace OpenNETCF.Security.Cryptography.X509Certificates
{
    public sealed class X509Store
    {
        public X509CertificateCollection Certificates { get; private set; }
        public StoreName Name { get; private set; }
        public StoreLocation Location { get; set; }
        internal IntPtr Handle { get; private set; }

        public X509Store(StoreName name, StoreLocation location)
        {
            string nameString;
            switch (name)
            {
                case StoreName.My:
                    nameString = "My";
                    break;
                case StoreName.CertificateAuthority:
                    nameString = "CertificateAuthority";
                    break;
                case StoreName.Root:
                    nameString = "Root";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("name");
            }

            Name = name;

            switch (location)
            {
                case StoreLocation.CurrentUser:
                    // this is the only supported one in CE
                    break;
                default:
                    throw new ArgumentOutOfRangeException("location");
            }

            Location = location;

            Handle = NativeMethods.CertOpenStore(
                NativeMethods.CERT_STORE_PROV_SYSTEM,
                0,
                IntPtr.Zero,
                NativeMethods.CERT_SYSTEM_STORE_CURRENT_USER | NativeMethods.CERT_STORE_MAXIMUM_ALLOWED_FLAG,
                nameString
                );

            Certificates = new X509CertificateCollection(this);

            var pContext = NativeMethods.CertEnumCertificatesInStore(Handle, IntPtr.Zero);

            while (pContext != IntPtr.Zero)
            {

                var cert = new X509Certificate(pContext);
                Certificates.Add(cert);

                pContext = NativeMethods.CertEnumCertificatesInStore(Handle, pContext);
            }
        }

        ~X509Store()
        {
            if (Handle != IntPtr.Zero)
            {
                NativeMethods.CertCloseStore(Handle, 0);
                Handle = IntPtr.Zero;
            }
        }
    }
}
