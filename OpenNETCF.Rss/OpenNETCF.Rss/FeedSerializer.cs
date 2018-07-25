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
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Implements feed serializer class
	/// </summary>
	public class FeedSerializer : IFeedSerializer
	{
		#region fields

		private FeedParser parser;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the FeedSerializer.
		/// </summary>
		public FeedSerializer()
		{
			parser = new FeedParser();
		} 

		#endregion

		#region IFeedSerializer Members

		/// <summary>
		/// Creates a Feed object from an XML stream.
		/// </summary>
		/// <param name="stream">An XML stream.</param>
		/// <returns>A Feed object.</returns>
		public Feed Deserialize(Stream stream)
		{
			//RssFeed feed = serializer.Deserialize(stream) as RssFeed;
			Feed feed = null;
			if (stream != null)
			{
				feed = parser.Process(stream);
			}
			return feed;
		}
		
		/// <summary>
		/// Serializes the Feed object into XML stream.
		/// </summary>
		/// <param name="feed">The Feed object.</param>
		/// <param name="stream">The Stream object.</param>
		public void Serialize(Feed feed, Stream stream)
		{
			XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
			this.Write(feed, writer);
			writer.Close();
		}

		private void Write(Feed feed, XmlWriter writer)
		{
				writer.WriteStartDocument(); 

				//<rss version="2.0">
				writer.WriteStartElement("rss"); 
				writer.WriteAttributeString("version", "2.0"); 

				//<channel>
				writer.WriteStartElement("channel"); 

				//<title />
				writer.WriteElementString("title", feed.Title); 

				//<link /> 
				writer.WriteElementString("link", feed.Link); 

				//<description /> 
				writer.WriteElementString("description", feed.Description); 

				//<rssbandit:maxItemAge />
				//writer.WriteElementString("maxItemAge", "http://www.25hoursaday.com/2003/RSSBandit/feeds/", this.maxItemAge.ToString()); 

				//other stuff
//				foreach(XmlNode node in feed.OptionalElements.Values)
//				{
//					writer.WriteRaw(node.OuterXml); 	  
//				}

				//<item />
				foreach(FeedItem item in feed.Items)
				{													
					this.WriteItem(item, writer, true); 					
				}
					
				writer.WriteEndElement();			
						
				
				writer.WriteEndElement();
				

				writer.WriteEndDocument(); 
			

		}


		private void WriteItem(FeedItem feedItem, XmlWriter writer, bool useGMTDate)
		{
		
			//<item>
			writer.WriteStartElement("item"); 

			// <title />
			if((feedItem.Title != null) && (feedItem.Title.Length != 0))
			{ 
				writer.WriteElementString("title", feedItem.Title); 				
			}

			// <link /> 
			if((feedItem.Link != null) && (feedItem.Link.Length != 0))
			{ 
				writer.WriteElementString("link", feedItem.Link); 
			}

			// <pubDate /> 			we write it with InvariantInfo to get them stored in a non-localized format
			if(useGMTDate)
			{
				writer.WriteElementString("pubDate", feedItem.PubDate.ToString("r", System.Globalization.DateTimeFormatInfo.InvariantInfo	)); 				
			}
			else
			{
				writer.WriteElementString("pubDate", feedItem.PubDate.ToLocalTime().ToString("F", System.Globalization.DateTimeFormatInfo.InvariantInfo)); 	
			}

			// <category />
			if((feedItem.Category != null) && (feedItem.Category.Length != 0))
			{ 
				writer.WriteElementString("category", feedItem.Category); 	
			}
				
			//<guid>
			if((feedItem.Id != null) && (feedItem.Id.Length != 0) && (feedItem.Id.Equals(feedItem.Link) == false))
			{ 
				writer.WriteStartElement("guid"); 				
				writer.WriteAttributeString("isPermaLink", "false");
				writer.WriteString(feedItem.Id); 
				writer.WriteEndElement();
			}

			//<dc:creator>
			if((feedItem.Author != null) && (feedItem.Author.Length != 0))
			{ 
				writer.WriteElementString("creator", "http://purl.org/dc/elements/1.1/", feedItem.Author);  	
			}

			//<annotate:reference>
			if((feedItem.ParentId != null) && (feedItem.ParentId.Length != 0))
			{ 
				writer.WriteStartElement("annotate", "reference", "http://purl.org/rss/1.0/modules/annotate/");  				
				writer.WriteAttributeString("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", feedItem.ParentId); 
				writer.WriteEndElement();
			}

				/* if(this.ContentType != ContentType.Xhtml){ */ 
				writer.WriteStartElement("description"); 
				writer.WriteCData(feedItem.Description);
				writer.WriteEndElement(); 
				/* }else // if(this.contentType == ContentType.Xhtml)  { 
					writer.WriteStartElement("xhtml", "body",  "http://www.w3.org/1999/xhtml");
					writer.WriteRaw(this.Content); 
					writer.WriteEndElement();
				} */ 

			

			//<wfw:comment />
//			if((this.commentUrl != null) && (this.commentUrl.Length != 0))
//			{ 
//
//				if(this.commentStyle == SupportedCommentStyle.CommentAPI)
//				{
//					writer.WriteStartElement("wfw", "comment", RssHelper.NsCommentAPI); 
//					writer.WriteString(this.commentUrl); 
//					writer.WriteEndElement();
//				}
//			}
//
//			//<wfw:commentRss />
//			if((this.commentRssUrl != null) && (this.commentRssUrl.Length != 0))
//			{ 				
//				writer.WriteStartElement("wfw", "commentRss", RssHelper.NsCommentAPI); 
//				writer.WriteString(this.commentRssUrl); 
//				writer.WriteEndElement();				
//			}

			//dirtyFlag - internal flag to indicate new item
			writer.WriteElementString("dirtyFlag", feedItem.DirtyFlag.ToString()); 	

			//<slash:comments>
			if(feedItem.CommentCount != 0)
			{
			
				writer.WriteStartElement("slash", "comments", "http://purl.org/rss/1.0/modules/slash/"); 
				writer.WriteString(feedItem.CommentCount.ToString()); 
				writer.WriteEndElement(); 
			}


//			if(format == NewsItemSerializationFormat.NewsPaper)
//			{
//
//		
//				writer.WriteStartElement("fd", "state", "http://www.bradsoft.com/feeddemon/xmlns/1.0/"); 
//				writer.WriteStartAttribute("read", this.BeenRead ? "1" : "0"); 
//				writer.WriteStartAttribute("flagged", FlagStatus == Flagged.None ? "0" : "1"); 
//				writer.WriteEndElement(); 
//
//			} 
//			else 
//			{ 
//				//<rssbandit:flag-status />
//				if(FlagStatus != Flagged.None)
//				{
//					writer.WriteElementString("flag-status", "http://www.25hoursaday.com/2003/RSSBandit/feeds/", FlagStatus.ToString()); 
//				}
//			}


			/* everything else */ 
			foreach(XmlNode xn in feedItem.OptionalElements.Values)
			{
			
				writer.WriteRaw(xn.OuterXml); 
			}

			//end </item> 
			writer.WriteEndElement(); 
		}

		#endregion
}
}
