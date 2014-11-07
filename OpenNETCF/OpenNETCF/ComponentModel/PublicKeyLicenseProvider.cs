using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;



namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// Provides an implementation of a OpenNETCF.ComponentModel.LicenseProvider. 
    /// </summary>
    public class PublicKeyLicenseProvider : LicenseProvider
    {
        #region fields

        private bool allowExceptions = false;
        private LicenseResourceType resourceType;
        private Assembly typeAssembly;

        #endregion

        #region overrides

        /// <summary>
        /// Returns a license for the instance of the component, if one is available.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="allowExceptions"></param>
        /// <param name="resourceType"></param>
        /// <param name="typeAssembly"></param>
        /// <returns></returns>
        public override License GetLicense(Type type, Assembly typeAssembly, LicenseResourceType resourceType, bool allowExceptions)
        {
            this.typeAssembly = typeAssembly;
            this.allowExceptions = allowExceptions;
            this.resourceType = resourceType;
            PublicKeyLicense license = new PublicKeyLicense(this, type);
            return license;
        }

        protected virtual bool IsKeyValid(Type type)
        {
            PublicKeyLicense license = new PublicKeyLicense(this, type);
            return license.IsValid;
        }

        #endregion

        #region properties

        public LicenseResourceType ResourceType
        {
            set
            {
                resourceType = value;
            }
            get
            {
                return resourceType;
            }
        }

        #endregion

        #region PrivateKeyLicense class

        private class PublicKeyLicense : License
        {
            #region fields

            private string key;
            private string productName;
            private string productVersion;
            private string customerName;
            private string customerEmail;
            private string licenseVersion;
            private string publicKeyResourceName;
            private Type type;
            private byte[] publicKey;
            private PublicKeyLicenseProvider owner;

            private XmlDocument xmlLicenseKey;
            private SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider();

            #endregion

            #region constructor

            public PublicKeyLicense(PublicKeyLicenseProvider owner, Type type)
            {
                  this.type = type;
                  this.owner = owner;
                  this.xmlLicenseKey = new XmlDocument();
                  string fileName = type.FullName + ".lic";
                  BinaryReader licReader = null;

                  if (owner.ResourceType == LicenseResourceType.File)
                  {
                      this.LoadLicenseKey(fileName);
                  }
                  else
                  {
                      try
                      {
                          licReader = new BinaryReader(
                              Assembly.GetCallingAssembly().GetManifestResourceStream(fileName));
                          this.LoadLicenseKey(licReader.BaseStream);
                      }
                      catch (Exception ex)
                      {
                          if (owner.allowExceptions)
                          {
                              throw ex;
                          }
                          else
                          {
                              return;
                          }
                      }
                  }
                  this.LoadPublicKey(type.Namespace + ".Pub.key");
            }

            #endregion

            #region helper methods

            private void LoadPublicKey(string publicKeyResourceName)
            {
                BinaryReader keyReader = null;

                if (publicKeyResourceName != null)
                {
                    try
                    {
                        
                        keyReader = new BinaryReader(
                            owner.typeAssembly.GetManifestResourceStream(publicKeyResourceName));
                        publicKey = keyReader.ReadBytes((int)keyReader.BaseStream.Length);
                    }
                    catch (Exception ex)
                    {
                        if (owner.allowExceptions)
                        {
                            throw ex;
                        }
                        else
                        {
                            return;
                        }

                    }
                }
            }

            private void LoadLicenseKey(Stream resourceStream)
            {
                try
                {
                    // load license key.
                    xmlLicenseKey.Load(resourceStream);
                }
                catch
                {
                    if (owner.allowExceptions)
                    {
                        throw new LicenseException();
                    }
                    else
                    {
                        return;
                    }
                }

                ProcessXml(xmlLicenseKey);


            }

            private void LoadLicenseKey(string fileName)
		    {
                   fileName = Path.GetDirectoryName(owner.typeAssembly.GetName().CodeBase) + "\\" + fileName; 
                  
                   if (fileName == null)
                    {
                        if (owner.allowExceptions)
                        {
                            throw new ArgumentNullException("fileName");
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (fileName.Length == 0)
                    {
                        if (owner.allowExceptions)
                        {
                            throw new ArgumentOutOfRangeException("fileName");
                        }
                        else
                        {
                            return;
                        }
                    }


                    if (!File.Exists(fileName))
                    {
                        if (owner.allowExceptions)
                        {
                            throw new FileNotFoundException("License key file does not exist.");
                        }
                        else
                        {
                            return;
                        }
                    }

                    try
                    {
                        // load license key.
                        xmlLicenseKey.Load(fileName);
                    }
                    catch
                    {
                         if (owner.allowExceptions)
                        {
                            throw new LicenseException();
                        }
                        else
                        {
                            return;
                        }
                    }

                    ProcessXml(xmlLicenseKey);

		    }

            private void ProcessXml(XmlDocument xmlLicenseKey)
            {
                try
                {

                    // find license key node.
                    XmlNode licenseKey = xmlLicenseKey.SelectSingleNode("/licenseKey");

                    // find product name node.
                    XmlNode productNode = licenseKey.SelectSingleNode("product");

                    // find customer name node.
                    customerName = licenseKey.SelectSingleNode("customerInfo/Name").InnerText;

                    // find customer email node.
                    customerEmail = licenseKey.SelectSingleNode("customerInfo/Email").InnerText;

                    // find key node.
                    key = licenseKey.SelectSingleNode("key").InnerText;

                    // find license version attribute.
                    licenseVersion = licenseKey.Attributes["version"].Value;

                    // find product version attribute.
                    productVersion = productNode.Attributes["version"].Value;
                    productName = productNode.InnerText;
                }
                catch
                {
                    if (owner.allowExceptions)
                    {
                        throw new LicenseException();
                    }
                    else
                    {
                        return;
                    }
                }


            }


            private void LoadLicenseKey(Stream fileStream, string publicKeyResourceName)
            {
                this.publicKeyResourceName = publicKeyResourceName;

                try
                {
                    // load license key.
                    xmlLicenseKey.Load(fileStream);

                    // find license key node.
                    XmlNode licenseKey = xmlLicenseKey.SelectSingleNode("/licenseKey");

                    // find product name node.
                    XmlNode productNode = licenseKey.SelectSingleNode("product");

                    // find customer name node.
                    customerName = licenseKey.SelectSingleNode("customerInfo/Name").InnerText;

                    // find customer email node.
                    customerEmail = licenseKey.SelectSingleNode("customerInfo/Email").InnerText;

                    // find key node.
                    key = licenseKey.SelectSingleNode("key").InnerText;

                    // find license version attribute.
                    licenseVersion = licenseKey.Attributes["version"].Value;

                    // find product version attribute.
                    productVersion = productNode.Attributes["version"].Value;
                    productName = productNode.InnerText;
                }
                catch
                {
                    throw new LicenseException();
                }

            }

            #endregion

            #region properties

            public override string LicenseKey
            {
                get { return key; }
            }

            private byte[] SignatureData
            {
                get
                {
                    StringBuilder dataBuilder = new StringBuilder(100);

                    dataBuilder.Append(productName);
                    dataBuilder.Append(productVersion);
                    dataBuilder.Append(customerName);
                    dataBuilder.Append(customerEmail);

                    return Encoding.UTF8.GetBytes(dataBuilder.ToString());
                }
            }

            public override bool IsValid
            {
                get 
                { 
                    RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                    if (publicKey != null)
                    {
                        try
                        {
                            rsaProvider.ImportCspBlob(publicKey);
                        }
                        catch (Exception ex)
                        {
                            if (owner.allowExceptions)
                            {
                                throw ex;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }
                    else
                    {
                        if (owner.allowExceptions)
                        {
                            throw new LicenseException("Public key is missing.");
                        }
                        else
                        {
                            return false;
                        }

                    }

                    
                    bool result = false;
                    if (key != null)
                    {
                        try
                        {
                            result = rsaProvider.VerifyData(SignatureData, sha1Provider, Convert.FromBase64String(key));
                        }
                        catch (Exception ex)
                        {
                            if (owner.allowExceptions)
                            {
                                throw ex;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                    return result;
                }
            }

            #endregion

            public override void Dispose()
            {
                sha1Provider.Clear();
                sha1Provider = null;
            }
        }

        #endregion

    }
}
