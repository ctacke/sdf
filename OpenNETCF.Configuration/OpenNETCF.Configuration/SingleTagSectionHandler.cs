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
using System.Collections;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class SingleTagSectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// Returns a collection of configuration section values.
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="context">This parameter is reserved and is null.</param>
		/// <param name="section">An <see cref="System.Xml.XmlNode"/> that contains configuration information from the configuration file.
		/// Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A <see cref="Hashtable"/> containing configuration section directives.</returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			Hashtable result;

			// start result off as a shallow clone of the parent
			if (parent == null)
			{
				result = new Hashtable();
			}
			else
			{
				result = new Hashtable((Hashtable)parent);
			}

			// Check for child nodes
			HandlerBase.CheckForChildNodes(section);
			
			foreach(XmlNode attribute in section.Attributes)
			{
				result[attribute.Name] = attribute.Value;
			}

			return result;
		}
	}
}
