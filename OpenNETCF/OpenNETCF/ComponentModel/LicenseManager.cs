using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
//using System.ComponentModel.Design;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// Provides properties and methods to add a license to a component and to manage a LicenseProvider. This class cannot be inherited.
    /// </summary>
    public class LicenseManager
    {
        #region fields

        private static Hashtable assemblies;

        #endregion

        private static void CacheAssembly(Type type)
        {
            if (LicenseManager.assemblies == null)
            {
                LicenseManager.assemblies = new Hashtable();
            }
            Assembly typeAssembly = Assembly.GetCallingAssembly();
            LicenseManager.assemblies[type] = typeAssembly;
        }

        /// <summary>
        /// Determines whether a valid license can be granted for the specified instance of the type. This method creates a valid License.
        /// </summary>
        /// <param name="type">A System.Type that represents the type of object that requests the License.</param>
        /// <param name="license">A License that is a valid license, or null if a valid license cannot be granted.</param>
        /// <returns>true if a valid license can be granted; otherwise, false.</returns>
        public static bool IsValid(Type type, out License license)
        {
            Assembly typeAssembly = Assembly.GetCallingAssembly();
            LicenseManager.ValidateInternal(type, typeAssembly, true, out license);
            if (license != null)
            {
                return license.IsValid;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a valid license can be granted for the specified type.
        /// </summary>
        /// <param name="type">A System.Type that represents the type of object that requests the License.</param>
        /// <returns>A valid OpenNETCF.ComponentModel.License.</returns>
        public static License Validate(Type type)
        {
            License license;
            Assembly typeAssembly = Assembly.GetCallingAssembly();
            LicenseManager.ValidateInternal(type, typeAssembly, true, out license);
            return license;
        }


        /// <summary>
        /// Determines whether a valid license can be granted for the specified type.
        /// </summary>
        /// <param name="type">A System.Type that represents the type of object that requests the License.</param>
        /// <returns>true if a valid license can be granted; otherwise, false.</returns>
        public static bool IsValid(Type type)
        {
            License license;

            Assembly typeAssembly = Assembly.GetCallingAssembly();
            bool flag = LicenseManager.ValidateInternal(type, typeAssembly, false, out license);
            if (license != null)
            {
                license.Dispose();
                license = null;
            }
            return flag;
        }

        private static bool ValidateInternal(Type type, Assembly typeAssembly, bool allowExceptions, out License license)
        {
            LicenseProvider provider = null;
            LicenseProviderAttribute providerAttrib = (LicenseProviderAttribute)Attribute.GetCustomAttribute(type, typeof(LicenseProviderAttribute), false);
            
            if (providerAttrib != null)
            {
                Type providerType = providerAttrib.LicenseProvider;
                if (providerType != null)
                {
                    provider = (LicenseProvider)Activator.CreateInstance(providerType);
                }
            }

            license = null;

            if (provider != null)
            {

                license = provider.GetLicense(type, typeAssembly, providerAttrib.ResourceType, allowExceptions);
            }

            return license.IsValid;
        }


    }
}
