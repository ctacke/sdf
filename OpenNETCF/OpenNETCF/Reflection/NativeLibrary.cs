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

namespace OpenNETCF.Reflection
{
    public sealed class NativeLibrary
    {
        private readonly string libraryName;

        /// <summary>
        /// Indicates whether the library exists or not.
        /// </summary>
        public bool Exists
        {
            get
            {
                if(String.IsNullOrEmpty(libraryName))
                    return false;

                return NativeLibraryExists(libraryName);
            }
        }

        /// <summary>
        /// The name of the Win32 library.
        /// </summary>
        public string Name
        {
            get { return libraryName;  }
        }

        internal NativeLibrary()
        {
            libraryName = null;
        }

        internal NativeLibrary(string library)
        {
            libraryName = NormalizeLibraryName(library);
        }

        /// <summary>
        /// Indicates whether the Win32 library exports the specified entry point.
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public bool HasMethod(string methodName)
        {
            if (String.IsNullOrEmpty(methodName))
                return false;

            if(String.IsNullOrEmpty(libraryName))
                return false;

            return NativeEntryPointExists(libraryName, methodName);
        }

        private static string NormalizeLibraryName(string library)
        {
            string name = library.ToLower();

            if (name.LastIndexOf('.') == (name.Length - 4))
                if (!HasDLLExtension(name))
                    throw new ArgumentException("library must be a DLL.");
            else
                name += ".dll";

            if (name.IndexOf("\\") > 0)
                name = name.Substring(name.LastIndexOf("\\") + 1);

            return name;
        }   

        private static bool HasDLLExtension(string filename)
        {
            return (filename.ToLower().Substring(filename.Length - 3) == "dll");
        }

        internal static bool NativeLibraryExists(string libraryName)
        {
            string libName = NormalizeLibraryName(libraryName);

            IntPtr hLib = NativeMethods.LoadLibrary(libName);
            if (hLib != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(hLib);
                return true;
            }

            return false;
        }

        private static bool NativeEntryPointExists(string libraryName, string functionName)
        {
            IntPtr hLib = NativeMethods.LoadLibrary(libraryName);
            if (hLib != IntPtr.Zero)
            {
                try
                {
                    return (NativeMethods.GetProcAddress(hLib, functionName) != IntPtr.Zero);
                }
                finally
                {
                    NativeMethods.FreeLibrary(hLib);
                }
            }

            return false;
        }
    }
}
