
using System;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public interface IConfigurationSystem
	{
        /// <summary>
        /// Returns the config object for the specified key.  
        /// </summary>
        /// <param name="configKey">Section name of config object to retrieve. </param>
        /// <param name="context">Application provided context object that gets passed into the Create method of the IConfigurationSectionHandler</param>
        /// <returns></returns>
        object GetConfig(string configKey, object context);
        
        /// <summary>
		/// Initializes the configuration system. 
		/// </summary>
		void Init();
	}
}
