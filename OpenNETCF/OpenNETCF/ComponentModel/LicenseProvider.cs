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
