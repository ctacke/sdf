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




using System.Xml;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Defines the contract that all configuration section handlers must implement in order to participate in the resolution of configuration settings.
	/// Reads key-value pair configuration information for a configuration section.
	/// </summary>
	public interface IConfigurationSectionHandler
	{
		/// <summary>
		/// Implemented by all configuration section handlers to parse the XML of the configuration section. The 
		/// returned object is added to the configuration collection and is accessed by 
		/// System.Configuration.ConfigurationSettings.GetConfig(System.String).
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="configContext">An System.Web.Configuration.HttpConfigurationContext when 
		/// System.Configuration.IConfigurationSectionHandler.Create(System.Object,System.Object,System.Xml.XmlNode) 
		/// is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is null.</param>
		/// <param name="section">The System.Xml.XmlNode that contains the configuration information from the 
		/// configuration file. Provides direct access to the XML contents of the configuration section.</param>
		/// <returns>A configuration object.</returns>
		object Create(object parent, object configContext, XmlNode section);
	}
}
