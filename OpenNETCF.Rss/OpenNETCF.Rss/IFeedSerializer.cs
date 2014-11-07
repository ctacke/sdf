
using System;
using System.Collections;
using System.Text;
using System.IO;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Defines the members for a Feed serializer. 
	/// </summary>
	public interface IFeedSerializer
	{
		/// <summary>
		/// Deserializes a Feed from the specified stream. 
		/// </summary>
		/// <param name="stream">A System.IO.Stream that contains the Feed to deserialize.</param>
		/// <returns></returns>
		Feed Deserialize(Stream stream);

		/// <summary>
		/// Serializes the specified Feed into the specified stream. 
		/// </summary>
		/// <param name="feed">A Feed to serialize into the stream.</param>
		/// <param name="stream">A System.IO.Stream to serialize the Feed into.</param>
		void Serialize(Feed feed, Stream stream);
	}
}
