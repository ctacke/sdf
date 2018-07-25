#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
