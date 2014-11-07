
using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides a section handler definition for configuration sections read and handled by systems other than OpenNETCF.Configuration. 
	/// </summary>
	public class IgnoreSectionHandler: IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return null;
		}
	}
}
