using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// Provides the abstract base class for implementing a license provider.
    /// </summary>
    public abstract class LicenseProvider
    {
        /// <summary>
        /// Initializes a new instance of the OpenNETCF.ComponentModel.LicenseProvider class. 
        /// </summary>
        protected LicenseProvider()
        {

        }

        /// <summary>
        /// When overridden in a derived class, gets a license for an instance or type of component, when given a context and whether the denial of a license throws an exception.
        /// </summary>
        /// <param name="type">A System.Type that represents the component requesting the license.</param>
        /// <param name="typeAssembly"></param>
        /// <param name="resourceType">LicenseResourceType that represents the way the client license is supplied.</param>
        /// <param name="allowExceptions">true if a System.ComponentModel.LicenseException should be thrown when the component cannot be granted a license; otherwise, false.</param>
        /// <returns>A valid OpenNETCF.ComponentModel.License.</returns>
        public abstract License GetLicense(Type type, Assembly typeAssembly, LicenseResourceType resourceType, bool allowExceptions);
    }

}
