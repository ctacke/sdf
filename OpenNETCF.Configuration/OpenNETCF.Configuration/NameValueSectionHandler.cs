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
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides name-value pair configuration information from a configuration section.
	/// </summary>
	public class NameValueSectionHandler: IConfigurationSectionHandler
	{
		private const string defaultKeyAttribute = "key";
		private const string defaultValueAttribute = "value";

		/// <summary>
		/// 
		/// </summary>
		protected virtual string KeyAttributeName
		{
			get { return "key"; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual string ValueAttributeName
		{
			get { return "value"; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="context"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object context, XmlNode section)
		{
			return CreateStatic(parent, section, KeyAttributeName, ValueAttributeName);
		}

		internal static object CreateStatic(object parent, XmlNode section)
		{
			return CreateStatic(parent, section, "key", "value");
		}

		internal static object CreateStatic(object parent, XmlNode section, string keyAttriuteName, string valueAttributeName)
		{
			ReadOnlyNameValueCollection result;

			if (parent == null)
			{
                result = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);

			}
			else
			{
				result = new ReadOnlyNameValueCollection((ReadOnlyNameValueCollection)parent);
			}
			HandlerBase.CheckForUnrecognizedAttributes(section);

			foreach(XmlNode child in section.ChildNodes)
			{
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
					continue;

				if (child.Name == "add")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, keyAttriuteName);
					string val = HandlerBase.RemoveRequiredAttribute(child, valueAttributeName, true);
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result[key] = val;
				}
				else if (child.Name == "remove")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, keyAttriuteName);
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Remove(key);
				}
				else if (child.Name.Equals("clear"))
				{
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Clear();
				}
				else
				{
					HandlerBase.ThrowUnrecognizedElement(child);
				}
			}

			result.SetReadOnly();
			return result;
		}
	}
}
