using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// Specifies the OpenNETCF.ComponentModel.LicenseProvider to use with a class. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LicenseProviderAttribute : Attribute
    {
        #region fields

        private string licenseProviderName;
        private Type licenseProviderType;
        private LicenseResourceType resourceType;

        public static readonly LicenseProviderAttribute Default;

        #endregion

        #region constructors

        static LicenseProviderAttribute()
        {
            LicenseProviderAttribute.Default = new LicenseProviderAttribute();
        }

        public LicenseProviderAttribute() : this((string) null)
        {
        }

        public LicenseProviderAttribute(string typeName)
        {
            this.licenseProviderType = null;
            this.licenseProviderName = null;
            this.licenseProviderName = typeName;
            this.resourceType = LicenseResourceType.File;
        }

        public LicenseProviderAttribute(Type type)
        {
            this.licenseProviderType = null;
            this.licenseProviderName = null;
            this.licenseProviderType = type;
            this.resourceType = LicenseResourceType.File;
        }

        public LicenseProviderAttribute(Type type, LicenseResourceType resourceType)
        {
            this.licenseProviderType = null;
            this.licenseProviderName = null;
            this.licenseProviderType = type;
            this.resourceType = resourceType;
        }

        #endregion

        #region properties

        public Type LicenseProvider
        {
            get
            {
                if ((this.licenseProviderType == null) && (this.licenseProviderName != null))
                {
                    this.licenseProviderType = Type.GetType(this.licenseProviderName);
                }
                return this.licenseProviderType;
            }
        }

        public LicenseResourceType ResourceType
        {
            get
            {
                return this.resourceType;
            }
        }

        public object TypeId
        {
            get
            {
                string name = this.licenseProviderName;
                if ((name == null) && (this.licenseProviderType != null))
                {
                    name = this.licenseProviderType.FullName;
                }
                return (base.GetType().FullName + name);
            }
        }

        #endregion


    }
}
