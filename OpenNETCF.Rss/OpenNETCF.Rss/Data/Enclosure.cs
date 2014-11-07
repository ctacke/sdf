
using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// Identifies the enclosure of a RSS feed item.
	/// </summary>
	public class Enclosure
	{

		#region fields
		//<enclosure url="http://www.scripting.com/mp3s/weatherReportSuite.mp3" length="12216320" type="audio/mpeg" />
		private string url;
		private int length;
		private string type; 
		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the Enclosure class.
		/// </summary>
		public Enclosure()
		{
			this.url = String.Empty;
			this.type = String.Empty;
		} 
		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the url.
		/// </summary>
		public string Url
		{
			get
			{
				return url;
			}
			set
			{
				url = value;
			}
		}

		/// <summary>
		/// Gets or sets the length value.
		/// </summary>
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				length = value;
			}
		}

		/// <summary>
		/// Gets or sets the type value.
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
