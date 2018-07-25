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
		private static IConfigurationSystem configSystem = null;
		private static bool configurationInitialized = false;
		private static Exception initError = null;

		internal static bool SetConfigurationSystemInProgress
		{
			get { return ( (configSystem != null) && (configurationInitialized == false) ); }
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

        public static object GetConfig(string sectionName, object context)
		{
			if (!configurationInitialized)
			{
				lock(typeof(ConfigurationSettings))
				{
                    if (configSystem == null && !SetConfigurationSystemInProgress)
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
				return configSystem.GetConfig(sectionName, context);
			}
		}

		internal static void SetConfigurationSystem(IConfigurationSystem ConfigSystem)
		{
			lock(typeof(ConfigurationSettings))
			{
				if (configSystem != null)
				{
					throw new InvalidOperationException("Config system already set");
				}
			
				try
				{
					configSystem = ConfigSystem;
					configSystem.Init();
				}
				catch (Exception e)
				{
					initError = e;
					throw;
				}
				
				configurationInitialized = true;
			}
		}
	}
}
