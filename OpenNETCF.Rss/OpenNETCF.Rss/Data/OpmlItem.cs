

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
		/// <summary>
		/// Initializes a new instance of the OpmlItem class.
		/// </summary>
		public OpmlItem()
		{
			Title = String.Empty;
			HtmlUrl = String.Empty;
			XmlUrl = String.Empty;
			Type = String.Empty;
		}
		
		#region properties
		/// <summary>
		/// Gets or sets OPML item's sub items
		/// </summary>
		public OpmlItem[] Items { get; set; }

		/// <summary>
		/// Gets or sets OPML item's Title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets OPML item's HtmlUrl
		/// </summary>
		public string HtmlUrl { get; set; }

		/// <summary>
		/// Gets or sets OPML item's XmlUrl
		/// </summary>
		public string XmlUrl { get; set; }

		/// <summary>
		/// Gets or sets OPML item's Type
		/// </summary>
		public string Type { get; set; }

        #endregion

	
	}
}
