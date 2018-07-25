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
using System.IO;
using System.Xml;
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Implements a parser for OPML files.
	/// </summary>
	public class OpmlParser
	{
		#region fields
		

		#endregion

		#region constructors
		
		public OpmlParser()
		{
		}

		#endregion

		#region public methods
		public Opml Process(string opmlPath)
		{
			FileStream fileStream = null;

			try
			{
				fileStream = new FileStream(opmlPath, FileMode.Open, FileAccess.Read);
			}
			catch (IOException ex)
			{
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
			}

			Opml opml = this.Process(fileStream);
			fileStream.Close();
			return opml;
		}

		public Opml Process(Stream stream)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(stream);

			return ProcessOpml(xmlDocument);
		} 
		#endregion


		#region helper methods
		private Opml ProcessOpml(XmlDocument document)
		{
			Opml opml = new Opml();

			XmlNode nodeOpml = document.ChildNodes[1];

			foreach (XmlNode node in nodeOpml.ChildNodes)
			{
				if (node.Name == "head")
				{
					ProcessHead(opml, node);
				}

				if (node.Name == "body")
				{
					ProcessOpmlElements(opml, node);
				}
			}

			return opml;
		}

		private void ProcessHead(Opml opml, XmlNode headNode)
		{
			foreach (XmlNode node in headNode.ChildNodes)
			{
				string text = node.LocalName;
				text = string.IsInterned(text);

				if (text == "title")
				{
					opml.Title = node.InnerText;
				}
				else if (text == "dateCreated")
				{
					opml.DateCreated = DateTimeExt.Parse(node.InnerText);
				}
				else if (text == "dateModified")
				{
					opml.DateModified = DateTimeExt.Parse(node.InnerText);
				}
				else if (text == "ownerEmail")
				{
					opml.OwnerEmail = node.InnerText;
				}
				else if (text == "ownerName")
				{
					opml.OwnerName = node.InnerText;
				}
			}
		}


		private void ProcessOpmlElements(Opml opml, XmlNode bodyNode)
		{
			ArrayList items = new ArrayList();

			foreach (XmlNode node in bodyNode.ChildNodes)
			{
				XmlElement element = node as XmlElement;

                // skip over non-elements (like comments)
                if (element == null) continue;

				if (element.LocalName == "outline")
				{
					OpmlItem item = new OpmlItem();

					if (element.HasAttribute("title"))
					{
						item.Title = element.Attributes["title"].Value;
					}

					if (element.HasAttribute("text"))
					{
						item.Title = element.Attributes["text"].Value;
					}

					if (element.HasAttribute("htmlUrl"))
					{
						item.HtmlUrl = element.Attributes["htmlUrl"].Value;
					}

					if (element.HasAttribute("xmlUrl"))
					{
						item.XmlUrl = element.Attributes["xmlUrl"].Value;
					}

					if (element.HasAttribute("Type"))
					{
						item.Type = element.Attributes["type"].Value;
					}
					items.Add(item);

					//if (element.HasChildNodes)
					//{
					//    ProcessOpmlElements(opml, element);
					//}
				}

			}

			opml.Items = (OpmlItem[])items.ToArray(typeof(OpmlItem));
		} 
		#endregion

	}
}
