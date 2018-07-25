using System;
using System.Globalization;
using System.Collections;
using System.Reflection;

namespace OpenNETCF
{
	/// <summary>
	/// Provides helper functions for Enumerations.
	/// </summary>
	/// <remarks>Extends the <see cref="System.Enum">System.Enum Class</see>.</remarks>
	/// <seealso cref="System.Enum">System.Enum Class</seealso>
	public static class Enum2
	{

		#region Get Name
		/// <summary>
		/// Retrieves the name of the constant in the specified enumeration that has the specified value.
		/// </summary>
		/// <param name="enumType">An enumeration type.</param>
		/// <param name="value">The value of a particular enumerated constant in terms of its underlying type.</param>
		/// <returns> A string containing the name of the enumerated constant in enumType whose value is value, or null if no such constant is found.</returns>
		/// <exception cref="System.ArgumentException"> enumType is not an System.Enum.  -or-  value is neither of type enumType nor does it have the same underlying type as enumType.</exception>
		/// <example>The following code sample illustrates the use of GetName (Based on the example provided with desktop .NET Framework):
		/// <code>[Visual Basic] 
		/// Imports System
		/// 
		///		Public Class GetNameTest
		/// 
		/// 		Enum Colors
		/// 			Red
		/// 			Green
		/// 			Blue
		/// 			Yellow
		/// 		End Enum 'Colors
		/// 
		///			Enum Styles
		/// 			Plaid
		/// 			Striped
		/// 			Tartan
		/// 			Corduroy
		/// 		End Enum 'Styles
		/// 
		///		Public Shared Sub Main() 
		/// 		MessageBox.Show("The 4th value of the Colors Enum is " + [OpenNETCF.Enum].GetName(GetType(Colors), 3))
		///			MessageBox.Show("The 4th value of the Styles Enum is " + [OpenNETCF.Enum].GetName(GetType(Styles), 3))
		///		End Sub 'Main
		///		
		/// End Class 'GetNameTest</code>
		/// <code>[C#] 
		/// using System;
		/// 
		/// public class GetNameTest 
		/// {
		/// 	enum Colors { Red, Green, Blue, Yellow };
		/// 	enum Styles { Plaid, Striped, Tartan, Corduroy };
		/// 
		/// 	public static void Main() 
		/// 	{
		/// 		MessageBox.Show("The 4th value of the Colors Enum is " + OpenNETCF.Enum.GetName(typeof(Colors), 3));
		/// 		MessageBox.Show("The 4th value of the Styles Enum is " + OpenNETCF.Enum.GetName(typeof(Styles), 3));
		/// 	}
		/// }</code>
		/// </example>
		/// <seealso cref="M:System.Enum.GetName(System.Type,System.Object)">System.Enum.GetName Method</seealso>
		public static string GetName(Type enumType, object value)
		{
			//check that the type supplied inherits from System.Enum
			if(enumType.BaseType==Type.GetType("System.Enum"))
			{
				//get details of all the public static fields (enum items)
				FieldInfo[] fi = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
				
				//cycle through the enum values
				foreach(FieldInfo thisField in fi)
				{
					object numericValue = 0;

					try
					{
						//convert the enum value to the numeric type supplied
						numericValue = Convert.ChangeType(thisField.GetValue(null), value.GetType(), null);
					}
					catch
					{
						throw new ArgumentException();
					}

					//if value matches return the name
					if(numericValue.Equals(value))
					{
						return thisField.Name;
					}
				}
				//if there is no match return null
				return null;
			}
			else
			{
				//the type supplied does not derive from enum
				throw new ArgumentException("enumType parameter is not an System.Enum");
			}
		}
		#endregion

		#region Get Names
		/// <summary>
		/// Retrieves an array of the names of the constants in a specified enumeration.
		/// </summary>
		/// <param name="enumType">An enumeration type.</param>
		/// <returns>A string array of the names of the constants in enumType. The elements of the array are sorted by the values of the enumerated constants.</returns>
		/// <exception cref="System.ArgumentException">enumType parameter is not an System.Enum</exception>
		/// <example>The follow example shows how to enumerate the members of the System.DayOfWeek enumeration by adding them to a ComboBox:-
		/// <code>[Visual Basic]
		/// Dim thisDOW As New DayOfWeek
		/// For Each thisDOW In OpenNETCF.Enum.GetValues(Type.GetType("System.DayOfWeek"))
		///		ComboBox1.Items.Add(thisDOW)
		/// Next</code>
		/// <code>[C#]
		/// foreach(DayOfWeek thisdow in OpenNETCF.Enum.GetValues(typeof(DayOfWeek)))
		/// {
		///		comboBox1.Items.Add(thisdow);
		/// }</code></example>
		/// <seealso cref="M:System.Enum.GetNames(System.Type)">System.Enum.GetNames Method</seealso>
		public static string[] GetNames(Type enumType)
		{
			if(enumType.BaseType==Type.GetType("System.Enum"))
			{
				//get the public static fields (members of the enum)
				System.Reflection.FieldInfo[] fi = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
			
				//create a new enum array
				string[] names = new string[fi.Length];

				//populate with the values
				for(int iEnum = 0; iEnum < fi.Length; iEnum++)
				{
					names[iEnum] = fi[iEnum].Name;
				}

				//return the array
				return names;
			}
			else
			{
				//the type supplied does not derive from enum
				throw new ArgumentException("enumType parameter is not an System.Enum");
			}
		}
		#endregion

		#region Get Underlying Type
		/// <summary>
		/// Returns the underlying type of the specified enumeration.>
		/// </summary>
		/// <param name="enumType">An enumeration type.</param>
		/// <returns>The underlying <see cref="System.Type"/> of <paramref>enumType</paramref>.</returns>
		/// <seealso cref="M:System.Enum.GetUnderlyingType(System.Type)">System.Enum.GetUnderlyingType Method</seealso>
		public static Type GetUnderlyingType(Type enumType)
		{
			return System.Enum.GetUnderlyingType(enumType);
		}
		#endregion

		#region Get Values
		/// <summary>
		/// Retrieves an array of the values of the constants in a specified enumeration.
		/// </summary>
		/// <param name="enumType">An enumeration type.</param>
		/// <returns>An System.Array of the values of the constants in enumType. The elements of the array are sorted by the values of the enumeration constants.</returns>
		/// <exception cref="System.ArgumentException">enumType parameter is not an System.Enum</exception>
		/// <seealso cref="M:System.Enum.GetValues(System.Type)">System.Enum.GetValues Method</seealso>
		public static System.Enum[] GetValues(Type enumType)
		{
			if(enumType.BaseType==Type.GetType("System.Enum"))
			{
				//get the public static fields (members of the enum)
				System.Reflection.FieldInfo[] fi = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
			
				//create a new enum array
				System.Enum[] values = new System.Enum[fi.Length];

				//populate with the values
				for(int iEnum = 0; iEnum < fi.Length; iEnum++)
				{
					values[iEnum] = (System.Enum)fi[iEnum].GetValue(null);
				}

				//return the array
				return values;
			}
			else
			{
				//the type supplied does not derive from enum
				throw new ArgumentException("enumType parameter is not an System.Enum");
			}
		}
		#endregion

		#region Is Defined
		/// <summary>
		/// Returns an indication whether a constant with a specified value exists in a specified enumeration.
		/// </summary>
		/// <param name="enumType">An enumeration type.</param>
		/// <param name="value">The value or name of a constant in enumType.</param>
		/// <returns><b>true</b> if a constant in <paramref>enumType</paramref> has a value equal to value; otherwise, <b>false</b>.</returns>
		/// <seealso cref="M:System.Enum.IsDefined(System.Type,System.Object)">System.Enum.IsDefined Method</seealso>
		public static bool IsDefined(Type enumType, object value)
		{
			return System.Enum.IsDefined(enumType, value);
		}
		#endregion

		#region Parse
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
		/// </summary>
		/// <param name="enumType">The <see cref="T:System.Type"/> of the enumeration.</param>
		/// <param name="value">A string containing the name or value to convert.</param>
		/// <returns>An object of type enumType whose value is represented by value.</returns>
		public static object Parse(System.Type enumType, string value)
		{
			//do case sensitive parse
			return System.Enum.Parse(enumType, value, false);
		}
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
		/// A parameter specifies whether the operation is case-sensitive.
		/// </summary>
		/// <param name="enumType">The <see cref="T:System.Type"/> of the enumeration.</param>
		/// <param name="value">A string containing the name or value to convert.</param>
		/// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
		/// <returns>An object of type enumType whose value is represented by value.</returns>
		/// <exception cref="System.ArgumentException">enumType is not an <see cref="T:System.Enum"/>.
		///  -or-  value is either an empty string ("") or only contains white space.
		///  -or-  value is a name, but not one of the named constants defined for the enumeration.</exception>
		///  <seealso cref="M:System.Enum.Parse(System.Type,System.String,System.Boolean)">System.Enum.Parse Method</seealso>
		public static object Parse(System.Type enumType, string value, bool ignoreCase)
		{
            return System.Enum.Parse(enumType, value, ignoreCase);
		}
		#endregion

		#region To Object
		/// <summary>
		/// Returns an instance of the specified enumeration set to the specified value.
		/// </summary>
		/// <param name="enumType">An enumeration.</param>
		/// <param name="value">The value.</param>
		/// <returns>An enumeration object whose value is <paramref>value</paramref>.</returns>
		/// <seealso cref="System.Enum.ToObject(System.Type, System.Object)">System.Enum.ToObject Method</seealso>
		public static object ToObject(System.Type enumType, object value)
		{
			return System.Enum.ToObject(enumType, value);
		}
		#endregion

		#region Format
		private static string InternalFormat(Type enumType, object value)
		{
			if (enumType.IsDefined(typeof(FlagsAttribute), false))
				return InternalFlagsFormat(enumType, value);

			string t = GetName(enumType, value);
			if (t == null) 
				return value.ToString();
			
			return t;
		}

		private static string InternalFlagsFormat(Type enumType, object value)
		{
			string t = GetName(enumType, value);
			if (t == null) 
				return value.ToString();

			return t;
		}

		private static string InternalValuesFormat(Type enumType, object value, bool showValues)
		{
			string [] names = null;
			if (!showValues) names = GetNames(enumType);
			ulong v = Convert.ToUInt64(value); 
			Enum [] e = GetValues(enumType);
			ArrayList al = new ArrayList();
			for(int i = 0; i < e.Length; i++)
			{
				ulong ev = (ulong)Convert.ChangeType(e[i], typeof(ulong), null);
				if (i == 0 && ev == 0) continue;
				if ((v & ev) == ev)
				{
					v -= ev;
					if (showValues)
						al.Add(ev.ToString());
					else
						al.Add(names[i]);
				}
			}

			if (v != 0)
				return value.ToString();


			string [] t = (string [])al.ToArray(typeof(string)); 
			return string.Join(", ", t);
		}

		/// <summary>
		/// Converts the specified value of a specified enumerated type to its equivalent string representation according to the specified format. 
		/// </summary>
		/// <remarks>
		/// The valid format values are: 
		/// "G" or "g" - If value is equal to a named enumerated constant, the name of that constant is returned; otherwise, the decimal equivalent of value is returned.
		/// For example, suppose the only enumerated constant is named, Red, and its value is 1. If value is specified as 1, then this format returns "Red". However, if value is specified as 2, this format returns "2".
		/// "X" or "x" - Represents value in hexadecimal without a leading "0x". 
		/// "D" or "d" - Represents value in decimal form.
		/// "F" or "f" - Behaves identically to "G" or "g", except the FlagsAttribute is not required to be present on the Enum declaration. 
		/// "V" or "v" - If value is equal to a named enumerated constant, the value of that constant is returned; otherwise, the decimal equivalent of value is returned.
		/// </remarks>
		/// <param name="enumType">The enumeration type of the value to convert.</param>
		/// <param name="value">The value to convert.</param>
		/// <param name="format">The output format to use.</param>
		/// <returns>A string representation of value.</returns>
		public static string Format(Type enumType, object value, string format)
		{
			if (enumType == null) throw new ArgumentNullException("enumType");
			if (value == null) throw new ArgumentNullException("value");
			if (format == null) throw new ArgumentNullException("format");
			if (!enumType.IsEnum) throw new ArgumentException("The argument enumType must be an System.Enum.");

			if (string.Compare(format, "G", true, CultureInfo.InvariantCulture) == 0)
				return InternalFormat(enumType, value);
			if (string.Compare(format, "F", true, CultureInfo.InvariantCulture) == 0)
				return InternalValuesFormat(enumType, value, false);
			if (string.Compare(format, "V", true, CultureInfo.InvariantCulture) == 0)
				return InternalValuesFormat(enumType, value, true);
			if (string.Compare(format, "X", true, CultureInfo.InvariantCulture) == 0)
				return InternalFormattedHexString(value);
			if (string.Compare(format, "D", true, CultureInfo.InvariantCulture) == 0)
				return Convert.ToUInt64(value).ToString(); 

			throw new FormatException("Invalid format.");
		}

		private static string InternalFormattedHexString(object value)
		{
			switch (Convert.GetTypeCode(value))
			{
				case TypeCode.SByte:
				{
					sbyte n = (sbyte) value;
					return n.ToString("X2", null);
				}
				case TypeCode.Byte:
				{
					byte n = (byte) value;
					return n.ToString("X2", null);
				}
				case TypeCode.Int16:
				{
					short n = (short) value;
					return n.ToString("X4", null);
				}
				case TypeCode.UInt16:
				{
					ushort n = (ushort) value;
					return n.ToString("X4", null);
				}
				case TypeCode.Int32:
				{
					int n = (int) value;
					return n.ToString("X8", null);
				}
				case TypeCode.UInt32:
				{
					uint n = (uint) value;
					return n.ToString("X8", null);
				}

			}

			throw new InvalidOperationException("Unknown enum type.");
		}
		#endregion
	}
}
