

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
