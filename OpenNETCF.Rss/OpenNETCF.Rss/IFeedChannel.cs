
using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss
{
    /// <summary>
	/// Defines the members for a feed channel. 
    /// </summary>
	public interface IFeedChannel
    {
        /// <summary>
		/// Closes the channel. 
        /// </summary>
		void Close();
    }
}
