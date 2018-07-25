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
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	
	/// <summary>
	/// Identifies the feed item of a RSS feed.
	/// </summary>
	public class FeedItem
	{
		#region fields
		
		private string title;
		private string link;
		private string description;
		private string id;
		private string parentId;
		private DateTime pubDate;
		private string author;
		private int commentCount;
		private string category;
		private Enclosure enclosure;
		private Hashtable optionalElements; 
		private Feed parent;
		private bool dirtyFlag;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the FeedItem class.
		/// </summary>
		public FeedItem()
		{
			title = String.Empty;
			link = String.Empty;
			description = String.Empty;
			id = String.Empty;
			parentId = String.Empty;
			author = String.Empty;
			pubDate = DateTime.MaxValue;
			category = String.Empty;
			enclosure = null;
			optionalElements = null;
			dirtyFlag = false;
		} 

		#endregion

		/// <summary>
		/// Gets or sets feed item title.
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item link.
		/// </summary>
		public string Link
		{
			get
			{
				return link;
			}
			set
			{
				link = value;
			}
		}


		/// <summary>
		/// Gets or sets feed item Description.
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item Author.
		/// </summary>
		public string Author
		{
			get
			{
				return author;
			}
			set
			{
				author = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item Id.
		/// </summary>
		public string Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item ParentId.
		/// </summary>
		public string ParentId
		{
			get
			{
				return parentId;
			}
			set
			{
				parentId = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item PubDate.
		/// </summary>
		public DateTime PubDate
		{
			get
			{
				return pubDate;
			}
			set
			{
				pubDate = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item Category.
		/// </summary>
		public string Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item CommentCount.
		/// </summary>
		public int CommentCount
		{
			get
			{
				return commentCount;
			}
			set
			{
				commentCount = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item Enclosure.
		/// </summary>
		public Enclosure Enclosure
		{
			get
			{
				return enclosure;
			}
			set
			{
				enclosure = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item Enclosure.
		/// </summary>
		public Feed Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}

		/// <summary>
		/// Gets or sets internal flag to indictate new item.
		/// </summary>
		public bool DirtyFlag
		{
			get
			{
				return dirtyFlag;
			}
			set
			{
				dirtyFlag = value;
			}
		}

		/// <summary>
		/// Gets or sets feed item OptionalElements.
		/// </summary>
		public Hashtable OptionalElements
		{

			get
			{
				if (optionalElements == null)
				{
					this.optionalElements = new Hashtable();
				}
				return this.optionalElements;
			}
			set
			{
				this.optionalElements = value;
			}

		} 
	}
}
