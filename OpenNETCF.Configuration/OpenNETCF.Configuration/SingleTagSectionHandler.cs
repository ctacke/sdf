
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
