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
            {
                if (!HasDLLExtension(name))
                {
                    throw new ArgumentException("library must be a DLL.");
                }
            }
            else
            {
                name += ".dll";
            }

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
