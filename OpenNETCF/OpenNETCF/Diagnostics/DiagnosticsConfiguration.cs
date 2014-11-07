using OpenNETCF.Configuration;
using System;
using System.Reflection;
using System.Collections;

namespace OpenNETCF.Diagnostics 
{
	internal enum InitState 
	{
		NotInitialized,
		Initializing,
		Initialized
	}

	internal class DiagnosticsConfiguration 
	{
		private static Hashtable configSection;
		private static InitState initState = InitState.NotInitialized;   
		private static string appBase = string.Empty;

		// Setting for TraceInternal.AutoFlush
		internal static bool AutoFlush 
		{
			get 
			{ 
				Initialize();
				if (configSection != null && configSection.ContainsKey("autoflush"))
					return (bool)configSection["autoflush"];
				else
					return false; // the default
			}
		}

		// Setting for TraceInternal.IndentSize
		internal static int IndentSize 
		{
			get 
			{ 
				Initialize();
				if (configSection != null && configSection.ContainsKey("indentsize"))
					return (int)configSection["indentsize"];
				else
					return 4; // the default
			}
		}

        // Setting for TraceInternal.Switches
        internal static IDictionary SwitchSettings
        {
            get
            {
                Initialize();
                if (configSection != null && configSection.ContainsKey("switches"))
                {
                    return (IDictionary)configSection["switches"];
                }

                return null;
            }
        }
        
		private static Hashtable GetConfigSection() 
		{
			Hashtable configTable = (Hashtable) ConfigurationSettings.GetConfig("opennetcf.diagnostics");
			return configTable;
		}

        internal static bool IsInitialized()
        {
            return (initState == InitState.Initialized);
        }

		internal static bool IsInitializing() 
		{
			return (initState == InitState.Initializing);
		}

		internal static bool CanInitialize() 
		{
			return (initState != InitState.Initializing);
		}
        
		internal static void Initialize() 
		{
			lock (typeof(DiagnosticsConfiguration)) 
			{
				if (initState != InitState.NotInitialized)
					return;

				// Prevent recursion
				initState = InitState.Initializing; 
				try 
				{
					configSection = GetConfigSection();
				}
				finally 
				{
					initState = InitState.Initialized;
				}
			}
		}

		internal static string AppBase
		{
			get { return appBase; }
			set { appBase = value; }
		}
	}
}


