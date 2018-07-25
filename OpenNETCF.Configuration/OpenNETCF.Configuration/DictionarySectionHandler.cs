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
using System.Globalization;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Reads key-value pair configuration information for a configuration section.
	/// </summary>
	/// <example>
	/// <code>
	/// &lt;add key="name" value="text"> - sets key=text
	/// &lt;remove key="name"> - removes the definition of key
	/// &lt;clear/> - removes all definitions
	/// </code>
	/// </example>
	public class DictionarySectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// Make the name of the key attribute configurable by derived classes.
		/// </summary>
		protected virtual string KeyAttributeName
		{
			get { return "key"; }
		}

		/// <summary>
		/// Make the name of the value attribute configurable by derived classes.
		/// </summary>
		protected virtual string ValueAttributeName
		{
			get { return "value"; }
		}

		internal virtual bool ValueRequired
		{
			get { return false; }
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
			Hashtable result;

			// Create a shallow clone of the parent
			if (parent == null)
			{
                result = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				result = (Hashtable)((Hashtable)parent).Clone();
			}
			// Process XML
			HandlerBase.CheckForUnrecognizedAttributes(section);

			foreach(XmlNode child in section.ChildNodes)
			{
				// Skip whitespace and comments; throw exception if non-element
				if(HandlerBase.IsIgnorableAlsoCheckForNonElement(child))
				{
					continue;
				}

				// Handle <add>, <remove>, and <clear> tags
				if(child.Name == "add")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					string value = HandlerBase.RemoveAttribute(child, ValueAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					if(value == null)
					{
						value = string.Empty;
					}

					result[key] = value;
				}
				else if(child.Name == "remove")
				{
					string key = HandlerBase.RemoveRequiredAttribute(child, KeyAttributeName);
					HandlerBase.CheckForUnrecognizedAttributes(child);

					result.Remove(key);
				}
				else if(child.Name == "clear")
				{
					HandlerBase.CheckForUnrecognizedAttributes(child);
					result.Clear();
				}
				else
				{
					HandlerBase.ThrowUnrecognizedElement(child);
				}
			}
			return result;
		}
	}
}

