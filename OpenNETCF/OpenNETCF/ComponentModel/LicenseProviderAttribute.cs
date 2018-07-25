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
