

using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// Identifies the OPML item.
	/// </summary>
	public class OpmlItem
	{
		#region fields
		
		private string title;
		private string htmlUrl;
		private string xmlUrl;
		private string type;
		private OpmlItem[] items; 

		#endregion

		#region construtor
		/// <summary>
		/// Initializes a new instance of the OpmlItem class.
		/// </summary>
		public OpmlItem()
		{
			title = String.Empty;
			htmlUrl = String.Empty;
			xmlUrl = String.Empty;
			type = String.Empty;
		}
		
		#endregion

		#region properties
		/// <summary>
		/// Gets or sets OPML item's sub items
		/// </summary>
		public OpmlItem[] Items
		{
			get
			{
				return items;
			}
			set
			{
				items = value;
			}
		}

		/// <summary>
		/// Gets or sets OPML item's Title
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
		/// Gets or sets OPML item's HtmlUrl
		/// </summary>
		public string HtmlUrl
		{
			get
			{
				return htmlUrl;
			}
			set
			{
				htmlUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets OPML item's XmlUrl
		/// </summary>
		public string XmlUrl
		{
			get
			{
				return xmlUrl;
			}
			set
			{
				xmlUrl = value;
			}
		}

		/// <summary>
		/// Gets or sets OPML item's Type
		/// </summary>
		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		} 
		#endregion

	
	}
}
