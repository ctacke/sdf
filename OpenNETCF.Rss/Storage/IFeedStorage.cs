
using System;
using System.Xml;
using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Summary description for IFeedStorage.
	/// </summary>
	public interface IFeedStorage
	{	
		
		/// <summary>
		/// Inits the storage provider.
		/// </summary>
		/// <param name="section">Represents a single node in the XML document</param>
		void Init(XmlNode section);

		/// <summary>
		///	Adds an element with the specified key and value into the storage.
		/// </summary>
		void Add (Feed feed);
		
		/// <summary>
		///	Removes all elements from the storage.
		/// </summary>
		void Flush ();
		
		/// <summary>
		///	Gets the element with the specified key.
		/// </summary>
		Feed GetFeed (string key);
		
		/// <summary>
		///	Removes the element with the specified key.
		/// </summary>
		void Remove	(string key);
		
		/// <summary>
		///	Updates the element with the specified key.
		/// </summary>
		int Update	(Feed feed);
		
		/// <summary>
		///	Gets the number of elements actually contained in the storage.
		/// </summary>
		int Size{ get; }
	}

}
