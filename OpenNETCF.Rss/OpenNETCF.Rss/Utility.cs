

using System;
using System.Xml;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Implements utility class.
	/// </summary>
	public class Utility
	{
		public static bool ImplementsInterface(Type type, Type interfaceType)
		{
			Type[] typeArray = type.GetInterfaces();
			for (int num = 0; num < typeArray.Length; num++)
			{
				if (typeArray[num].IsAssignableFrom(interfaceType))
				{
					return true;
				}
			}
			return false;
		}

		public static string ReadAttribute(XmlReader reader, string name)
		{
			string result = reader.GetAttribute(name);

			return result;
		}

		public static string ReadElementString(XmlReader reader)
		{
			string result = reader.ReadString();

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.Skip();
				result += reader.ReadString();
			}

			return result;
		}
	}
}
