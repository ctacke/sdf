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
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ToolHelp
{
    /// <summary>
    /// Wrapper around the ToolHelp ModuleEntry information
    /// </summary>
    /// <remarks>
    /// This class requires the toolhelp32.dll
    /// </remarks>
    public class ModuleEntry
    {
		private MODULEENTRY32 m_me;

		private ModuleEntry(MODULEENTRY32 me)
		{
			m_me = me;
		}

        /// <summary>
        /// The module name
        /// </summary>
        public string Name        
        {
            get { return m_me.szModule; }
        }

        /// <summary>
        /// The module path
        /// </summary>
        public string Path
        {
            get { return m_me.szExePath; }
        }

        /// <summary>
        /// The base address of the module in the context of the owning process. 
        /// </summary>
        [CLSCompliant(false)]
        public uint BaseAddress
        {
            get { return m_me.modBaseAddr; }
        }

        /// <summary>
        /// The size of the module, in bytes. 
        /// </summary>
        public int Size
        {
            get { return (int)m_me.modBaseSize; }
        }

        /// <summary>
        /// Retrieves an array of all running threads for a specified process
        /// </summary>
        /// <returns>Any array of ModuleEntry classes</returns>
        [CLSCompliant(false)]
        public static ModuleEntry[] GetModules(uint processID)
        {
            List<ModuleEntry> moduleList = new List<ModuleEntry>();

            IntPtr handle = ToolhelpAPI.CreateToolhelp32Snapshot(ToolhelpAPI.TH32CS_SNAPMODULE, processID);

            if ((int)handle > 0)
            {
                try
                {
                    MODULEENTRY32 meCurrent;
                    MODULEENTRY32 me32 = new MODULEENTRY32();

                    //Get byte array to pass to the API calls
                    byte[] meBytes = me32.ToByteArray();

                    //Get the first process
                    int retval = ToolhelpAPI.Module32First(handle, meBytes);

                    while (retval == 1)
                    {
                        //Convert bytes to the class
                        meCurrent = new MODULEENTRY32(meBytes);
                        //New instance
                        ModuleEntry module = new ModuleEntry(meCurrent);

                        moduleList.Add(module);

                        retval = ToolhelpAPI.Module32Next(handle, meBytes);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception: " + ex.Message);
                }

                //Close handle
                ToolhelpAPI.CloseToolhelp32Snapshot(handle); 

                return moduleList.ToArray();

            }
            else
            {
                throw new Exception("Unable to create snapshot");
            }
        }
    }
}
