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
using System.Runtime.InteropServices;

namespace OpenNETCF.Reflection
{
	/// <summary>
	/// Contains helper functions for the <see cref="System.Reflection.Assembly"/> class.
	/// </summary>
	/// <seealso cref="System.Reflection.Assembly"/>
	internal static class Assembly2
	{
		/// <summary>
		/// Gets the process executable.
		/// </summary>
		/// <returns>The <see cref="Assembly"/> that is the process executable.</returns>
		public static Assembly GetEntryAssembly()
		{
			byte[] buffer = new byte[256 * Marshal.SystemDefaultCharSize];
			int chars = GetModuleFileName(IntPtr.Zero, buffer, 255);

			if(chars > 0)
			{
				if(chars > 255)
				{
					throw new System.IO.PathTooLongException("Assembly name is longer than MAX_PATH characters.");
				}

				string assemblyPath = System.Text.Encoding.Unicode.GetString(buffer, 0, chars * Marshal.SystemDefaultCharSize);

				return Assembly.LoadFrom(assemblyPath);
			}
			else
			{
				return null;
			}

		}

		[DllImport("coredll.dll", SetLastError=true)]
		private static extern int GetModuleFileName(IntPtr hModule, byte[] lpFilename, int nSize);

	}
}
