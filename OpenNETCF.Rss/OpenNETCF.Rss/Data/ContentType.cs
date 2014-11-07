
using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// NewsItem description content types.
	/// </summary>
	public enum ContentType
	{
		/// <summary>No content available</summary>
		None,
		/// <summary>Unknown or not supported</summary>
		Unknown,
		/// <summary>Simple text</summary>
		Text,
		/// <summary>HTML formated text</summary>
		Html,
		/// <summary>XHTML formated</summary>
		Xhtml
	}
}
