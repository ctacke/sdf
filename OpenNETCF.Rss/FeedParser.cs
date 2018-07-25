
using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Net;
using System.Text;

using OpenNETCF.Rss.Utils;
using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Implements feed parser.
	/// </summary>
	public class FeedParser
	{
		#region fields

		private static object[] nameRefs;
		
		#endregion

		/// <summary>
		/// XmlDocument object where nodes placed in NewsItem.OptionalElements come from.
		/// </summary>
		private static XmlDocument optionalElementsDoc = new XmlDocument();

		static FeedParser()
		{
			//nameTable = PrepareNameTable();	
		}

		/// <summary>
		/// Initializes a new instance of the FeedParser.
		/// </summary>
		public FeedParser()
		{
			//nameTable = PrepareNameTable();	
		}

		public Feed Process(Stream feedStream)
		{
			XmlTextReader reader = new XmlTextReader(feedStream, PrepareNameTable());
			reader.WhitespaceHandling = WhitespaceHandling.Significant;
			//XmlValidatingReader vr = new XmlValidatingReader(r);
			//vr.ValidationType = ValidationType.None;
			//vr.XmlResolver = new ProxyXmlUrlResolver(NameTableIndexes.GlobalProxy); 
			return ProcessFeed(reader, false);
		}


		private Feed ProcessFeed(XmlReader feedReader, bool cachedStream)
		{
			ArrayList items = new ArrayList();
			Hashtable optionalElements = new Hashtable();
			string feedLink = String.Empty;
			string feedDescription = String.Empty;
			string feedTitle = String.Empty;
			string maxItemAge = String.Empty;
			SyndicationFormat feedFormat = SyndicationFormat.Unknown;
			DateTime defaultItemDate = DateTime.MinValue;
			DateTime channelBuildDate = DateTime.Now.ToUniversalTime();
			Feed feed = null;

			string rssNamespaceUri = String.Empty;


			//setup the NameTable used by the XmlReader
			//object[] nameRefs = FillNameTable(feedReader.NameTable);

			feedReader.MoveToContent();
			object localname = feedReader.LocalName;

			try
			{

				if ((localname == nameRefs[(int)NameTableIndexes.rdf]) &&
					feedReader.NamespaceURI.Equals("http://www.w3.org/1999/02/22-rdf-syntax-ns#"))
				{ //RSS 0.9 or 1.0

					feedReader.Read(); //go to end of 'RDF' start tag
					feedReader.MoveToContent(); //go to the next element  

					feedFormat = SyndicationFormat.Rdf;

					localname = feedReader.LocalName;
					//figure out if 0.9 or 1.0 by peeking at namespace URI of <channel> element    
					if (localname == nameRefs[(int)NameTableIndexes.channel])
					{

						rssNamespaceUri = feedReader.NamespaceURI;
					}
					else
					{ //no channel, just assume RSS 1.0 
						rssNamespaceUri = "http://purl.org/rss/1.0/";
					}

				}
				else if (localname == nameRefs[(int)NameTableIndexes.rss])
				{ //RSS 0.91 or 2.0 

					feedFormat = SyndicationFormat.Rss;
					rssNamespaceUri = feedReader.NamespaceURI;

					do
					{
						feedReader.Read(); //go to end of 'rss' start tag
						feedReader.MoveToContent(); //go to the next element		
						localname = feedReader.LocalName;
					} while (localname != nameRefs[(int)NameTableIndexes.channel]);

				}
				else if (feedReader.NamespaceURI.Equals("http://purl.org/atom/ns#")
			  && (localname == nameRefs[(int)NameTableIndexes.feed]))
				{ //Atom 0.3

					rssNamespaceUri = feedReader.NamespaceURI;

					if (feedReader.MoveToAttribute("version") && feedReader.Value.Equals("0.3"))
					{
						feedFormat = SyndicationFormat.Atom;
						feedReader.MoveToElement(); //move back to 'feed' start element						
					}
					else
					{
						throw new ApplicationException("ExceptionUnsupportedAtomVersion");
					}

				}
				else
				{
					throw new ApplicationException("ExceptionUnknownXmlDialect");
				}

				feed = new Feed();
				ProcessFeedElements(feed, feedReader, rssNamespaceUri, feedFormat, optionalElements, items, defaultItemDate);

			}
			finally
			{
				feedReader.Close();
			}

			feedReader = null;
			nameRefs = null;
//			if (feed != null)
//			{
//				feed.Items = (FeedItem[])items.ToArray(typeof(FeedItem));
//			}

			return feed;
		}

		private static FeedItem MakeFeedItem(XmlReader reader, DateTime defaultItemDate)
		{
			string description = null;
			string id = null;
			string parentId = null;
			string link = null;
			string title = null;
			string subject = null;
			string author = null;
			Enclosure enclosure = null;
			bool dirtyFlag = false;
			//int commentCount = NewsItem.NoComments;
			int commentCount = 0;
			DateTime date = defaultItemDate;
			DateTime now = date;
			Hashtable optionalElements = new Hashtable();
			//Flagged flagged = Flagged.None;
			ArrayList subjects = new ArrayList();
			
			string itemNamespaceUri = reader.NamespaceURI; //the namespace URI of the RSS item


			bool nodeRead = false; //indicates whether the last node was read using XmlDocument.ReadNode()	

			while ((nodeRead || reader.Read()) && reader.NodeType != XmlNodeType.EndElement)
			{

				nodeRead = false;
				object localname = reader.LocalName;
				object namespaceuri = reader.NamespaceURI;

				if (reader.NodeType != XmlNodeType.Element) { continue; }

				/* string nodeNamespaceUri = reader.NamespaceURI;
				if (StringHelper.EmptyOrNull(nodeNamespaceUri))
					nodeNamespaceUri = itemNamespaceUri;	*/
				// if in node has no namespace, assume in RSS namespace

				// save some string comparisons
				bool nodeNamespaceUriEqual2Item = reader.NamespaceURI.Equals(itemNamespaceUri);
				bool nodeNamespaceUriEqual2DC = (namespaceuri == nameRefs[(int)NameTableIndexes.ns_dc]);


				if ((description == null) || (localname == nameRefs[(int)NameTableIndexes.body]) || (localname == nameRefs[(int)NameTableIndexes.encoded]))
				{ //prefer to replace rss:description/dc:description with content:encoded

					if ((namespaceuri == nameRefs[(int)NameTableIndexes.ns_xhtml])
						&& (localname == nameRefs[(int)NameTableIndexes.body]))
					{
						if (!reader.IsEmptyElement)
						{
							XmlElement elem = (XmlElement)optionalElementsDoc.ReadNode(reader);
							nodeRead = true;
							description = elem.InnerXml;
							elem = null;
						}
						continue;
					}
					else if ((namespaceuri == nameRefs[(int)NameTableIndexes.ns_content])
						&& (localname == nameRefs[(int)NameTableIndexes.encoded]))
					{

						if (!reader.IsEmptyElement)
						{
							description = ReadElementString(reader);
						}
						continue;
					}
					else if ((nodeNamespaceUriEqual2Item || nodeNamespaceUriEqual2DC)
					   && (localname == nameRefs[(int)NameTableIndexes.description]))
					{
						if (!reader.IsEmptyElement)
						{
							description = ReadElementString(reader);
						}
						continue;
					}

				}

				if (localname == nameRefs[(int)NameTableIndexes.enclosure])
				{
					enclosure = new Enclosure();
					enclosure.Url = FeedParser.ReadAttribute(reader, "url");
					enclosure.Type = FeedParser.ReadAttribute(reader, "type");
					enclosure.Length = Convert.ToInt32(FeedParser.ReadAttribute(reader, "length"));

				}

				if (link != null && link.Trim().Length == 0)
					link = null;	// reset on empty elements

				if ((link == null) || (localname == nameRefs[(int)NameTableIndexes.guid]))
				{ //favor rss:guid over rss:link

					if (nodeNamespaceUriEqual2Item
						&& (localname == nameRefs[(int)NameTableIndexes.guid]))
					{

						if ((reader["isPermaLink"] == null) ||
							(StringHelper.AreEqualCaseInsensitive(reader["isPermaLink"], "true")))
						{
							if (!reader.IsEmptyElement)
							{
								link = ReadElementString(reader);
							}
						}
						else if (StringHelper.AreEqualCaseInsensitive(reader["isPermaLink"], "false"))
						{
							if (!reader.IsEmptyElement)
							{
								id = ReadElementString(reader);
							}
						}

						continue;
					}
					else if (nodeNamespaceUriEqual2Item
						&& (localname == nameRefs[(int)NameTableIndexes.link]))
					{
						if (!reader.IsEmptyElement)
						{
							link = ReadElementString(reader);
						}
						continue;
					}

				}

				if (title == null)
				{

					if (nodeNamespaceUriEqual2Item
						&& (localname == nameRefs[(int)NameTableIndexes.title]))
					{
						if (!reader.IsEmptyElement)
						{
							title = ReadElementString(reader);
						}
						continue;
					}

				}

				if (localname == nameRefs[(int)NameTableIndexes.dirtyFlag])
				{
					if (!reader.IsEmptyElement)
					{
						dirtyFlag = Boolean.Parse(ReadElementString(reader));
					}
					continue;
				}

				if ((author == null) || (localname == nameRefs[(int)NameTableIndexes.creator]))
				{ //prefer dc:creator to <author>

					if (nodeNamespaceUriEqual2DC &&
						(localname == nameRefs[(int)NameTableIndexes.creator] ||
						 localname == nameRefs[(int)NameTableIndexes.author]))
					{
						if (!reader.IsEmptyElement)
						{
							author = ReadElementString(reader);
						}
						continue;
					}
					else if (nodeNamespaceUriEqual2Item && (localname == nameRefs[(int)NameTableIndexes.author]))
					{
						if (!reader.IsEmptyElement)
						{
							author = ReadElementString(reader);
						}
						continue;
					}

				}

				if ((parentId == null) && (localname == nameRefs[(int)NameTableIndexes.reference]))
				{

					if (namespaceuri == nameRefs[(int)NameTableIndexes.ns_annotate])
					{
						parentId = reader.GetAttribute("resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
					}
					continue;
				}

				if (nodeNamespaceUriEqual2DC && (localname == nameRefs[(int)NameTableIndexes.subject]))
				{
					if (!reader.IsEmptyElement)
					{
						subjects.Add(ReadElementString(reader));
					}
					continue;
				}
				else if (nodeNamespaceUriEqual2Item && (localname == nameRefs[(int)NameTableIndexes.category]))
				{
					if (!reader.IsEmptyElement)
					{
						subjects.Add(ReadElementString(reader));
					}
					continue;
				}


				if (commentCount == 0)
				{
					if ((localname == nameRefs[(int)NameTableIndexes.comments])
						&& (namespaceuri == nameRefs[(int)NameTableIndexes.ns_slash]))
					{
						try
						{
							if (!reader.IsEmptyElement)
							{
								commentCount = Int32.Parse(ReadElementString(reader));
							}
						}
						catch (Exception) { /* DO NOTHING */}
						continue;
					}
				}

				if (date == now)
				{

					try
					{
						if (nodeNamespaceUriEqual2Item
							&& (localname == nameRefs[(int)NameTableIndexes.pubdate]))
						{
							if (!reader.IsEmptyElement)
							{
								date = DateTimeExt.Parse(ReadElementString(reader));
							}
							continue;
						}
						else if (nodeNamespaceUriEqual2DC && (localname == nameRefs[(int)NameTableIndexes.date]))
						{
							if (!reader.IsEmptyElement)
							{
								date = DateTimeExt.ToDateTime(ReadElementString(reader));
							}
							continue;
						}


					}
					catch (FormatException fe)
					{
                        System.Diagnostics.Debug.WriteLine(fe.ToString());
                        /* date was improperly formated*/
						//_log.Warn("Error parsing date from item {" + subject +
						//	"} from feed {" + link + "}: " + fe.Message);
						continue;
					}

				}

				XmlQualifiedName qname = new XmlQualifiedName(reader.LocalName, reader.NamespaceURI);
				XmlNode optionalNode = optionalElementsDoc.ReadNode(reader);
				nodeRead = true;

				/* some elements occur multiple times in feeds, only the 1st is picked */
				if (!optionalElements.Contains(qname))
				{
					optionalElements.Add(qname, optionalNode);
				}

			}//while				

			//HACK: Sometimes we get garbled items due to network issues, this ensures we don't send them to the UI
			if (link == null && id == null && title == null && date == now)
			{
				return null;
			}

			/* create Subject if any */
			for (int i = 0; i < subjects.Count; i++)
			{
				subject += (i > 0 ? " | " + subjects[i] : subjects[i]);
			}

			/* set value of id to link if no guid in XML stream */
			id = (id == null ? link : id);

			FeedItem newsItem = new FeedItem(); //f, title, link, description, date, subject, ctype, optionalElements, id, parentId);
			newsItem.Title = title;
			newsItem.Id = id;
			newsItem.Link = link;
			newsItem.Description = description;
			newsItem.PubDate = date;
			newsItem.Enclosure = enclosure;
			newsItem.OptionalElements = optionalElements;
			newsItem.CommentCount = commentCount;
			newsItem.Author = author;
			newsItem.DirtyFlag = dirtyFlag;

			return newsItem; 				
		}

		private static void ProcessFeedElements(Feed feed, XmlReader reader, string rssNamespaceUri, SyndicationFormat format, Hashtable optionalElements, ArrayList items, DateTime defaultItemDate)
		{

			bool matched = false; //indicates whether this is a known element
			bool nodeRead = false; //indicates whether the last node was read using XmlDocument.ReadNode()

			//string feedTitle = "";
			//string feedDescription = "";
			//string feedLink = "";

			DateTime channelBuildDate = DateTime.MinValue;

			if ((format == SyndicationFormat.Rdf) || (format == SyndicationFormat.Rss))
			{

				while ((nodeRead || reader.Read()) && reader.NodeType != XmlNodeType.EndElement)
				{

					object localname = reader.LocalName;
					object namespaceuri = reader.NamespaceURI;
					matched = false;
					nodeRead = false;

					if (reader.NodeType != XmlNodeType.Element) { continue; }

					if (reader.NamespaceURI.Equals(rssNamespaceUri) || reader.NamespaceURI.Equals(String.Empty))
					{

						if (localname == nameRefs[(int)NameTableIndexes.title])
						{
							if (!reader.IsEmptyElement)
							{
								feed.Title = ReadElementString(reader);
							}
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.description])
						{
							if (!reader.IsEmptyElement)
							{
								feed.Description = ReadElementString(reader);
							}
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.link])
						{
							if (!reader.IsEmptyElement)
							{
								feed.Link = ReadElementString(reader);
							}
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.lastbuilddate])
						{
							try
							{
								if (!reader.IsEmptyElement)
								{
									feed.LastBuildDate = DateTimeExt.Parse(ReadElementString(reader));
								}
							}
							catch (FormatException fex)
							{
                                System.Diagnostics.Debug.WriteLine(fex.ToString());
                                //_log.Warn("Error parsing date from channel {" + feedTitle +
								//    "} from feed {" + (feedLink == null ? f.title : feedLink) + "}: ", fex);
							}
							finally
							{
								matched = true;
							}
						}
						else if (localname == nameRefs[(int)NameTableIndexes.items])
						{
							reader.Skip();
							matched = true;
						}
						else if ((localname == nameRefs[(int)NameTableIndexes.image]) && format == SyndicationFormat.Rdf)
						{
							reader.Skip();
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.item])
						{
							if (!reader.IsEmptyElement)
							{
								FeedItem rssItem = MakeFeedItem(reader, defaultItemDate);
								if (rssItem != null)
								{
								   rssItem.Parent = feed;
									//items.Add(rssItem);
								   feed.Items.Add(rssItem);
								}
							}
							matched = true;
						}

					}
					else if (namespaceuri == nameRefs[(int)NameTableIndexes.ns_bandit_2003])
					{
						if (localname == nameRefs[(int)NameTableIndexes.maxitemage])
						{
							if (!reader.IsEmptyElement)
							{
								// get the old v1.2 value from cached feed
								// We used the TimeSpan.Parse() / maxItemAge.ToString() there, so we cannot simply take over the string.
								// Instead we convert to TimeSpan, then convert to valid xs:duration datatype to proceed correctly
								//f.maxitemage = XmlConvert.ToString(TimeSpan.Parse(ReadElementString(reader)));
								//f.maxitemage = ReadElementString(reader); 
							}
							matched = true;
						}
					}

					if (!matched)
					{

						XmlQualifiedName qname = new XmlQualifiedName(reader.LocalName, reader.NamespaceURI);
						XmlNode optionalNode = optionalElementsDoc.ReadNode(reader);

						if (!optionalElements.Contains(qname))
						{
							optionalElements.Add(qname, optionalNode);
						}

						nodeRead = true;
					}//if(!matched)

				}//while


				if (format == SyndicationFormat.Rdf)
				{

					reader.ReadEndElement(); //move to <image> or first <item>. 					

					do
					{
						object localname = reader.LocalName;
						nodeRead = false;

						if ((localname == nameRefs[(int)NameTableIndexes.image]) &&
							reader.NamespaceURI.Equals(rssNamespaceUri))
						{ //RSS 1.0 can have <image> outside <channel>
							XmlNode optionalNode = optionalElementsDoc.ReadNode(reader);
							((XmlElement)optionalNode).SetAttribute("xmlns", String.Empty); //change namespace decl to no namespace

							XmlQualifiedName qname = new XmlQualifiedName(optionalNode.LocalName, optionalNode.NamespaceURI);

							if (!optionalElements.Contains(qname))
							{
								optionalElements.Add(qname, optionalNode);
							}

							nodeRead = true;
						}

						if ((localname == nameRefs[(int)NameTableIndexes.item]) &&
							reader.NamespaceURI.Equals(rssNamespaceUri))
						{
							if (!reader.IsEmptyElement)
							{
								FeedItem rssItem = MakeFeedItem(reader, defaultItemDate);
								if (rssItem != null)
								{
									items.Add(rssItem);
								}
							}
						}

					} while (nodeRead || reader.Read());

				}// if(format == SyndicationFormat.Rdf)

			}
			else if (format == SyndicationFormat.Atom)
			{

				while ((nodeRead || reader.Read()) && reader.NodeType != XmlNodeType.EndElement)
				{

					object localname = reader.LocalName;
					object namespaceuri = reader.NamespaceURI;
					matched = false;
					nodeRead = false;

					if (reader.NodeType != XmlNodeType.Element) { continue; }


					if (reader.NamespaceURI.Equals(rssNamespaceUri) || reader.NamespaceURI.Equals(String.Empty))
					{

						if (localname == nameRefs[(int)NameTableIndexes.title])
						{
							if (!reader.IsEmptyElement)
							{
								feed.Title = ReadElementString(reader);
							}
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.tagline])
						{
							if (!reader.IsEmptyElement)
							{
								feed.Description = ReadElementString(reader);
							}
							matched = true;
						}
						else if (localname == nameRefs[(int)NameTableIndexes.link])
						{

							string rel = reader.GetAttribute("rel");
							string href = reader.GetAttribute("href");

							if (feed.Link == String.Empty)
							{
								if ((rel != null) && (href != null) &&
									rel.Equals("alternate"))
								{
									feed.Link = href;
									matched = true;
								}
							}

						}
						else if (localname == nameRefs[(int)NameTableIndexes.modified])
						{
							try
							{
								if (!reader.IsEmptyElement)
								{
									feed.LastBuildDate = DateTimeExt.Parse(ReadElementString(reader));
								}
							}
							catch (FormatException fex)
							{
                                System.Diagnostics.Debug.WriteLine(fex.ToString());
                                //_log.Warn("Error parsing date from channel {" + feedTitle +
								//	"} from feed {" + (feedLink == null ? f.title : feedLink) + "}: ", fex);
							}
							finally
							{
								matched = true;
							}
						}
						else if (localname == nameRefs[(int)NameTableIndexes.entry])
						{
							if (!reader.IsEmptyElement)
							{
								FeedItem rssItem = MakeFeedItem(reader, defaultItemDate);
								if (rssItem != null)
								{
									items.Add(rssItem);
								}
							}
							matched = true;
						}

					}
					else if (namespaceuri == nameRefs[(int)NameTableIndexes.ns_bandit_2003])
					{
						if (localname == nameRefs[(int)NameTableIndexes.maxitemage])
						{
							if (!reader.IsEmptyElement)
							{
								// get the old v1.2 value from cached feed
								// We used the TimeSpan.Parse() / maxItemAge.ToString() there, so we cannot simply take over the string.
								// Instead we convert to TimeSpan, then convert to valid xs:duration datatype to proceed correctly
								TimeSpan maxItemAgeTS = TimeSpan.Parse(ReadElementString(reader));

								if (maxItemAgeTS != TimeSpan.MaxValue)
								{
									//f.maxitemage = XmlConvert.ToString(maxItemAgeTS);
								}
							}
							matched = true;
						}
					}

					if (!matched)
					{

						XmlQualifiedName qname = new XmlQualifiedName(reader.LocalName, reader.NamespaceURI);
						XmlNode optionalNode = optionalElementsDoc.ReadNode(reader);

						if (!optionalElements.Contains(qname))
						{
							optionalElements.Add(qname, optionalNode);
						}

						nodeRead = true;

					}//if(!matched)

				}//while

			}

		}

		private static string ReadAttribute(XmlReader reader, string name)
		{
			string result = reader.GetAttribute(name);

			return result;
		}

		private static string ReadElementString(XmlReader reader)
		{
			string result = reader.ReadString();

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.Skip();
				result += reader.ReadString();
			}

			return result;
		}

		private static NameTable PrepareNameTable()
		{
			/* For examples of the perf improvements from using name tables see 
			 * http://blogs.msdn.com/mfussell/archive/2004/04/28/121854.aspx
			 * http://www.tkachenko.com/blog/archives/000181.html 
			 */
			NameTable nt = new NameTable();

			nameRefs = new object[(int)NameTableIndexes.Size];

			nameRefs[(int)NameTableIndexes.author] = nt.Add("author");
			nameRefs[(int)NameTableIndexes.body] = nt.Add("body");
			nameRefs[(int)NameTableIndexes.category] = nt.Add("category");
			nameRefs[(int)NameTableIndexes.channel] = nt.Add("channel");
			nameRefs[(int)NameTableIndexes.comments] = nt.Add("comments");
			nameRefs[(int)NameTableIndexes.content] = nt.Add("content");
			nameRefs[(int)NameTableIndexes.creator] = nt.Add("creator");
			nameRefs[(int)NameTableIndexes.date] = nt.Add("date");
			nameRefs[(int)NameTableIndexes.description] = nt.Add("description");
			nameRefs[(int)NameTableIndexes.encoded] = nt.Add("encoded");
			nameRefs[(int)NameTableIndexes.entry] = nt.Add("entry");
			nameRefs[(int)NameTableIndexes.flagstatus] = nt.Add("flag-status");
			nameRefs[(int)NameTableIndexes.feed] = nt.Add("feed");
			nameRefs[(int)NameTableIndexes.guid] = nt.Add("guid");
			nameRefs[(int)NameTableIndexes.href] = nt.Add("href");
			nameRefs[(int)NameTableIndexes.id] = nt.Add("id");
			nameRefs[(int)NameTableIndexes.image] = nt.Add("image");
			nameRefs[(int)NameTableIndexes.ispermalink] = nt.Add("isPermaLink");
			nameRefs[(int)NameTableIndexes.issued] = nt.Add("issued");
			nameRefs[(int)NameTableIndexes.item] = nt.Add("item");
			nameRefs[(int)NameTableIndexes.items] = nt.Add("items");
			nameRefs[(int)NameTableIndexes.lastbuilddate] = nt.Add("lastBuildDate");
			nameRefs[(int)NameTableIndexes.link] = nt.Add("link");
			nameRefs[(int)NameTableIndexes.maxitemage] = nt.Add("maxItemAge");
			nameRefs[(int)NameTableIndexes.modified] = nt.Add("modified");
			nameRefs[(int)NameTableIndexes.name] = nt.Add("name");
			nameRefs[(int)NameTableIndexes.pubdate] = nt.Add("pubDate");
			nameRefs[(int)NameTableIndexes.rdf] = nt.Add("RDF");
			nameRefs[(int)NameTableIndexes.reference] = nt.Add("reference");
			nameRefs[(int)NameTableIndexes.rel] = nt.Add("rel");
			nameRefs[(int)NameTableIndexes.rss] = nt.Add("rss");
			nameRefs[(int)NameTableIndexes.subject] = nt.Add("subject");
			nameRefs[(int)NameTableIndexes.summary] = nt.Add("summary");
			nameRefs[(int)NameTableIndexes.tagline] = nt.Add("tagline");
			nameRefs[(int)NameTableIndexes.title] = nt.Add("title");
			nameRefs[(int)NameTableIndexes.type] = nt.Add("type");
			nameRefs[(int)NameTableIndexes.ns_dc] = nt.Add("http://purl.org/dc/elements/1.1/");
			nameRefs[(int)NameTableIndexes.ns_xhtml] = nt.Add("http://www.w3.org/1999/xhtml");
			nameRefs[(int)NameTableIndexes.ns_content] = nt.Add("http://purl.org/rss/1.0/modules/content/");
			nameRefs[(int)NameTableIndexes.ns_annotate] = nt.Add("http://purl.org/rss/1.0/modules/annotate/");
			nameRefs[(int)NameTableIndexes.ns_bandit_2003] = nt.Add("http://www.25hoursaday.com/2003/RSSBandit/feeds/");
			nameRefs[(int)NameTableIndexes.ns_slash] = nt.Add("http://purl.org/rss/1.0/modules/slash/");
			nameRefs[(int)NameTableIndexes.enclosure] = nt.Add("enclosure");
			nameRefs[(int)NameTableIndexes.dirtyFlag] = nt.Add("dirtyFlag");

			return nt;

		}

		public enum SyndicationFormat
		{
			/// <summary>
			/// Dave Winer's Family of specs including RSS 0.91 &amp; RSS 2.0
			/// </summary>
			Rss,
			/// <summary>
			/// The RDF based syndication formats such as RSS 0.9 and RSS 1.0 
			/// </summary>
			Rdf,
			/// <summary>
			/// The Atom syndication format
			/// </summary>
			Atom,
			/// <summary>
			/// An unknown and hence unsupported feed format
			/// </summary>
			Unknown
		}

		private enum NameTableIndexes
		{
			ispermalink = 0,
			description = 1,		
			body = 2,
			encoded = 3,
			guid = 4,
			link = 5,
			title = 6,
			pubdate = 7,
			date = 8,
			category = 9,
			subject = 10,
			comments = 11,
			flagstatus = 12,
			content = 13,
			summary = 14,
			rel = 15,
			href = 16,
			modified = 17,
			issued = 18,
			type = 19,
			rss = 20,
			rdf = 21,
			feed = 22,
			channel = 23,
			lastbuilddate = 24,
			image = 25,
			item = 26,
			items = 27,
			maxitemage = 28,
			tagline = 29,
			entry = 30,
			id = 31,
			author = 32,
			creator = 33,
			name = 34,
			reference = 35,
			ns_dc = 36,
			ns_xhtml = 37,
			ns_content = 38,
			ns_annotate = 39,
			ns_bandit_2003 = 40,
			ns_slash = 41,
			enclosure = 42,
			dirtyFlag = 43,
			Size = 44
		}
	}
}
