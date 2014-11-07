using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography.X509Certificates
{
    public class X509CertificateCollection : IEnumerable<X509Certificate>
    {
        private List<X509Certificate> m_certs = new List<X509Certificate>();
        private X509Store m_store;

        internal X509CertificateCollection(X509Store store)
        {
            m_store = store;
        }

        public IEnumerator<X509Certificate> GetEnumerator()
        {
            return m_certs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return m_certs.Count; }
        }

        internal void Add(X509Certificate certificate)
        {
            m_certs.Add(certificate);
        }

        public void Import(string fileName)
        {
            Import(fileName, null);
        }

        public void Import(string fileName, string password)
        {
            // get the extension of the file - we support CER and PVK only
            var extension = Path.GetExtension(fileName).ToLower();

            switch (extension)
            {
                case ".cer":
                    LoadCert(fileName);
                    break;
                case ".pvk":
                    var certContext = LoadPvk(fileName, password, m_store);
                    if (certContext != IntPtr.Zero)
                    {
                        this.Add(new X509Certificate(certContext));
                        NativeMethods.CertFreeCertificateContext(certContext);
                    }
                    break;
                default:
                    throw new NotSupportedException("Provided Certificate File is not supported.");
            }
        }

        public void Import(byte[] rawData)
        {
            Import(rawData, null);
        }

        public void Import(byte[] rawData, string password)
        {
            // we assume that non-passwrd data is a cert and passwrd data is a PVK
            if (password == null)
            {
                LoadCert(rawData);
            }
            else
            {
                var certContext = LoadPvk(rawData, password, m_store);
                if (certContext != IntPtr.Zero)
                {
                    this.Add(new X509Certificate(certContext));
                    NativeMethods.CertFreeCertificateContext(certContext);
                }
            }
        }

        private IntPtr LoadPvk(byte[] rawData, string password, X509Store targetStore)
        {
            var pvk = PVKFile.Create(rawData, targetStore);

            return pvk.Import(password);
        }

        private IntPtr LoadPvk(string fileName, string password, X509Store targetStore)
        {
            var pvk = PVKFile.Create(fileName, targetStore);

            return pvk.Import(password);
        }

        private void LoadCert(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            byte[] fileData;

            using (var stream = File.OpenRead(fileName))
            {
                fileData = new byte[stream.Length];

                stream.Read(fileData, 0, (int)stream.Length);
            }

            LoadCert(fileData);
        }

        private void LoadCert(byte[] fileData)
        {
            if (fileData == null)
            {
                throw new ArgumentException();
            }

            int i = 0;

            // is it unicode base64? (every other byte will be 0x00)
            for (i = 0; i < fileData.Length && (fileData[i + 1] == 0); i += 2) ;

            if (i == fileData.Length)
            {
                var encodedString = Encoding.Unicode.GetString(fileData, 0, fileData.Length);
                fileData = Convert.FromBase64String(encodedString);
            }
            else
            {
                var token = "-----BEGIN CERTIFICATE-----";
                // see if it's ascii base64
                var checkToken = Encoding.ASCII.GetString(fileData, 0, token.Length);

                if (token == checkToken)
                {
                    // it looks like it is ASCII base 64
                    var encodedString = Encoding.ASCII.GetString(fileData, 0, fileData.Length);
                    fileData = Convert.FromBase64String(encodedString);
                }

            }

            var pContext = IntPtr.Zero;

            // at this point we have the cert data (converted from base 64 if required)
            var result = NativeMethods.CertAddEncodedCertificateToStore(
                m_store.Handle,
                NativeMethods.X509_ASN_ENCODING,
                fileData,
                fileData.Length,
                NativeMethods.CERT_STORE_ADD_NEWER_INHERIT_PROPERTIES,
                ref pContext);

            if (result)
            {
                this.Add(new X509Certificate(pContext));
                NativeMethods.CertFreeCertificateContext(pContext);
            }

        }
    }
}
