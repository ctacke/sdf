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
using System.IO;
using System.Reflection;
using System.Threading;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Summary description for DefaultConfigurationSystem.
	/// </summary>
	class DefaultConfigurationSystem : IConfigurationSystem 
	{
		private const string ConfigExtension = ".config";
        private const string UnitTestExtension = ".unittest";
        private const string MachineConfigFilename = "machine.config";
		private const string MachineConfigSubdirectory = "config";
		private ConfigurationRecord _application;
		
		internal DefaultConfigurationSystem()
		{
		}

        [Obsolete("There is no need to call this constructor anymore.", false)]
		public DefaultConfigurationSystem(string CodeBase)
		{
		}

        object IConfigurationSystem.GetConfig(string configKey, object context)
        {
            if (_application != null)
            {
                return _application.GetConfig(configKey, context);
            }
            else
            {
                throw new InvalidOperationException("Client config init error");
            }
        }
        
        void IConfigurationSystem.Init() 
		{
			lock(this) 
			{
				if(_application == null) 
				{
					ConfigurationRecord machineConfig = null;
					string machineConfigFilename = MachineConfigurationFilePath;
					_application = machineConfig = new ConfigurationRecord();
					bool machineConfigExists = machineConfig.Load(machineConfigFilename);

                    if (!machineConfigExists)
                    {
                        // Load machine.config from embedded resource
                        System.Resources.ResourceManager resMgr = new System.Resources.ResourceManager("OpenNETCF.Configuration.machineconfig", System.Reflection.Assembly.GetExecutingAssembly());
                        string machineConfigXml = resMgr.GetString("machine.config");
                        bool machineConfigLoaded = machineConfig.LoadXml(machineConfigXml);
                    }

					Uri appConfigFilename = AppConfigPath;

                    // test to see if a unit test config exists first
                    string unitTestConfig = appConfigFilename.LocalPath + UnitTestExtension;

                    if(File.Exists(unitTestConfig))
                    {
                        appConfigFilename = new Uri("file://" + unitTestConfig);
                    }

					// Only load the app.config is machine.config exists
					if(appConfigFilename != null) 
					{
						_application = new ConfigurationRecord(machineConfig);
						//jsm - Bug 80 - need to throw exception if app.config file doesn't exist
						if (!_application.Load(appConfigFilename.ToString()))
						{
							throw new ConfigurationException("Unable to load application configuration file");
						}
					}
				}
			}            
		}

		internal static string MsCorLibDirectory 
		{
			get 
			{ 
				string corCodeBase = typeof(object).Assembly.GetName().CodeBase;
				int separatorIndex = corCodeBase.IndexOf("\\", 2) + 1;
				
				string filename = corCodeBase.Substring(0, separatorIndex);//.Replace('/','\\');
				filename = filename.Replace("/","\\");
				return Path.GetDirectoryName(filename);
			}
		}

		internal static string MachineConfigurationFilePath 
		{
			get 
			{
				return Path.Combine(Path.Combine(MsCorLibDirectory, MachineConfigSubdirectory), MachineConfigFilename);
			}
		}

		internal static Uri AppConfigPath 
		{
			get 
			{                
				try 
				{
					string appBase = ApplicationBase();
                
					// we need to ensure AppBase ends in an '/'.                                  
					if (appBase.Length > 0) 
					{
						char lastChar = appBase[appBase.Length - 1];
						if (lastChar != '/' && lastChar != '\\') 
						{
							appBase += '\\';
						}
					}
					Uri uri = new Uri(appBase);
					string config = ConfigurationFile();
					if (config != null && config.Length > 0) 
					{
						uri = new Uri(uri, config);
						return uri;
					}
				}
				finally 
				{
                    
				}
				return null;
			}
		}

		private static string GetCodeBase() 
		{
			return OpenNETCF.Reflection.Assembly2.GetEntryAssembly().GetName().CodeBase; 
		}

		private static string ApplicationBase() 
		{
			string codeBase = GetCodeBase();
            // neilco: Bug #330: UriFormatException when reading app.config from root directory
			return codeBase.Substring(0,codeBase.LastIndexOf("\\") + 1);
		}

		private static string ConfigurationFile() 
		{
			return GetCodeBase() + ConfigExtension;
		}
    }
}
