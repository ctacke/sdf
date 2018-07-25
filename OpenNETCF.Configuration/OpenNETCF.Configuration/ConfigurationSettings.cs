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
using System.Threading;
using System.Globalization;

namespace OpenNETCF.Configuration
{
  /// <summary>
  /// Provides access to configuration settings in a specified configuration section. This class cannot be inherited.
  /// </summary>
  public static class ConfigurationSettings
  {
    // The Configuration System
    private static IConfigurationSystem m_configSystem = null;
    private static bool m_configurationInitialized = false;
    private static Exception initError = null;

    internal static bool SetConfigurationSystemInProgress
    {
      get { return ((m_configSystem != null) && (m_configurationInitialized == false)); }
    }

    /// <summary>
    /// Forces the settings provider to re-load the settings from the configuration file.
    /// </summary>
    public static void Reload()
    {
      m_configurationInitialized = false;
      m_configSystem = null;
    }

    /// <summary>
    /// Gets configuration settings in the configuration section.
    /// </summary>
    public static NameValueCollection AppSettings
    {
      get
      {
        ReadOnlyNameValueCollection appSettings = (ReadOnlyNameValueCollection)GetConfig("appSettings");

        if (appSettings == null)
        {
          appSettings = new ReadOnlyNameValueCollection(StringComparer.OrdinalIgnoreCase);

          appSettings.SetReadOnly();
        }

        return appSettings;
      }
    }

    /// <summary>
    /// Returns configuration settings for a user-defined configuration section.  
    /// </summary>
    /// <param name="sectionName">The configuration section to read.</param>
    /// <returns>The configuration settings for sectionName.</returns>
    public static object GetConfig(string sectionName)
    {
      return GetConfig(sectionName, null);
    }

    /// <summary>
    /// Returns configuration settings for a user-defined configuration section. 
    /// </summary>
    /// <param name="sectionName">The configuration section to read.</param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static object GetConfig(string sectionName, object context)
    {
      if (!m_configurationInitialized)
      {
        lock (typeof(ConfigurationSettings))
        {
          if (m_configSystem == null && !SetConfigurationSystemInProgress)
          {
            SetConfigurationSystem(new DefaultConfigurationSystem());
          }
        }
      }
      if (initError != null)
      {
        throw initError;
      }
      else
      {
        return m_configSystem.GetConfig(sectionName, context);
      }
    }

    internal static void SetConfigurationSystem(IConfigurationSystem ConfigSystem)
    {
      lock (typeof(ConfigurationSettings))
      {
        if (m_configSystem != null)
        {
          throw new InvalidOperationException("Config system already set");
        }

        try
        {
          m_configSystem = ConfigSystem;
          m_configSystem.Init();
        }
        catch (Exception e)
        {
          initError = e;
          throw;
        }

        m_configurationInitialized = true;
      }
    }
  }
}
