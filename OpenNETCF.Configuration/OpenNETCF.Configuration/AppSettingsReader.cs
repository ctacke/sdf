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
using System.Collections.Specialized;
using System.Globalization;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides a method for reading values of a particular type from the .config file.
	/// </summary>
	public class AppSettingsReader
	{
		private NameValueCollection map;
		private static Type stringType = typeof(string);
		private static Type[] paramsArray = new Type[]{stringType};
		private static string NullString = "None";

		/// <summary>
		/// 
		/// </summary>
		public AppSettingsReader()
		{
          map = ConfigurationSettings.AppSettings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public object GetValue(string key, Type type)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string keyVal = map[key];
			if (keyVal == null)
			{
				throw new InvalidOperationException("No Key: " + key);
			}
			if (type == stringType)
			{
				int i = GetNoneNesting(keyVal);
				if (i == 0)
				{
					return keyVal;
				}
				if (i == 1)
				{
					return null;
				}
				else
				{
					return keyVal.Substring(1, keyVal.Length - 2);
				}
			}
			try
			{
				return Convert.ChangeType(keyVal, type, null);
			}
			catch (Exception)
			{
				string exceptionVal = (keyVal.Length != 0) ? keyVal : "AppSettingsReaderEmptyString";
				throw new InvalidOperationException("Can't Parse " +  exceptionVal + " for key " + key + " of type " + type.ToString());
			}
		}

		private int GetNoneNesting(string val)
		{
			int i = 0;
			int j = val.Length;
			char[] chars = val.ToCharArray();
			if (j > 1)
			{
				for (i++; chars[i] == '(' && chars[j - i - 1] == ')'; i++)
				{
				}
				if (i > 0 && String.Compare(NullString, 0, val, i, j - 2 * i, false, CultureInfo.InvariantCulture) != 0)
				{
					i = 0;
				}
			}
			return i;
		}
	}
}
