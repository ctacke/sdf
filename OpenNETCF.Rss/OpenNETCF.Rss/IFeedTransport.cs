
using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss
{
    /// <summary>
	/// Provides the base interface for objects that represent Feed transports. 
    /// </summary>
	public interface IFeedTransport
    {
        /// <summary>
		/// Gets the Feed input channel for the transport.
        /// </summary>
		/// <param name="address">The Url for the transport.</param>
		/// <returns>An IFeedInputChannel representing the Feed input channel for the transport. </returns>
		IFeedInputChannel GetInputChannel(Uri address);

        //IRssOutputChannel GetOutputChannel(Uri address);

    }
}
