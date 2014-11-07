
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
