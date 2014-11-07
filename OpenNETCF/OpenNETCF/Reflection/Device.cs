using System;

namespace OpenNETCF.Reflection
{
    /// <summary>
    /// A static (<b>Shared</b> in Visual Basic) class designed to help
    /// determine whether a Win32 library exists and if so, whether the 
    /// library exports a specific entry point. 
    /// </summary>
    /// <example>
    /// <code lang="VB">
    /// If Device.Win32Library("coredll").HasMethod("FindWindow") Then
    ///     NativeMethods.FindWindow("MyWindowName", String.Empty)
    /// End If
    /// </code>
    /// <code lang="C#">
    /// if (Device.Win32Library("coredll").HasMethod("FindWindow")
    /// {
    ///     NativeMethods.FindWindow("MyWindowName", String.Empty);
    /// }
    /// </code>
    /// </example>
    public static class Device
    {
        /// <summary>
        /// Creates an instance of the NativeLibrary class.
        /// </summary>
        /// <remarks>The returned instance will always be non-null. 
        /// To determine the existence of the library, check the 
        /// <see cref="NativeLibrary.Exists"/> property.</remarks>
        /// <param name="library">The name of the Win32 library. The 
        /// file extension is optional.</param>
        public static NativeLibrary Win32Library(string library)
        {
            if (String.IsNullOrEmpty(library))
                throw new ArgumentException("library must not be null or empty string.", library);

            if (NativeLibrary.NativeLibraryExists(library))
                return new NativeLibrary(library);
            else
                return new NativeLibrary();
        }
    }
}
