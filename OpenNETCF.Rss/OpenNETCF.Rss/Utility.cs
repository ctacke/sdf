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
