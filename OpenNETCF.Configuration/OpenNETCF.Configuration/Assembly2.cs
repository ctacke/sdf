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
