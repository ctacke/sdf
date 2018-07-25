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
using System.Reflection;
using System.Collections;
using OpenNETCF.Configuration;

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
			return (initState == InitState.Initializing || ConfigurationSettings.SetConfigurationSystemInProgress);
		}

		internal static bool CanInitialize() 
		{
			return (initState != InitState.Initializing) && !(ConfigurationSettings.SetConfigurationSystemInProgress);
		}
        
		internal static void Initialize() 
		{
			lock (typeof(DiagnosticsConfiguration)) 
			{
				if (initState != InitState.NotInitialized || ConfigurationSettings.SetConfigurationSystemInProgress)
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


