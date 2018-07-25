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
