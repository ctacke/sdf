using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace OpenNETCF.Security.Cryptography.X509Certificates
{
    public class X509Certificate
    {
        internal IntPtr Handle { get; private set; }
        public string Name { get; private set; }

        internal X509Certificate(IntPtr pContext)
        {
            Handle = pContext;

            var name = new StringBuilder(512);

            NativeMethods.CertGetNameString(
                pContext,
                NativeMethods.CERT_NAME_FRIENDLY_DISPLAY_TYPE,
                0,
                IntPtr.Zero,
                name,
                name.Capacity
                );

            this.Name = name.ToString();
        }
    }
}
