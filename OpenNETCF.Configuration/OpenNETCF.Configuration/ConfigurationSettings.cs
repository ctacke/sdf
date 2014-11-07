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
