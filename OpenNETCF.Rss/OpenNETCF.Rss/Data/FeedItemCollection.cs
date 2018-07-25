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

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// Represents a collection of FeedItem objects 
	/// </summary>
	public class FeedItemCollection : CollectionBase
	{
		
		#region fields

		private Hashtable keyMaps;

		#endregion // fields

		#region constructors

		public FeedItemCollection()
		{
			keyMaps = new Hashtable();
		}

		#endregion // constructors


		#region public methods

		/// <summary>
		/// Adds the specified FeedItem to the collection.  
		/// </summary>
		/// <param name="value">The FeedItem to add to the control collection.</param>
		/// <returns>The index value of the added FeedItem.</returns>
		public int Add(FeedItem value)
		{
			int index = List.Add(value);
			keyMaps.Add(value.Id, index);	
			return index;
		}

		/// <summary>
		/// Inserts an existing FeedItem in the feed item collection at the specified location.  
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, FeedItem value)
		{
			List.Insert(index, value);
		}

		/// <summary>
		/// Removes the specified FeedItem from the collection.  
		/// </summary>
		/// <param name="value"></param>
		public void Remove(FeedItem value)
		{
			List.Remove(value);
			keyMaps.Remove(value.Id);
		}

		/// <summary>
		/// Determines whether the specified FeedItem is a member of the collection. 
		/// </summary>
		/// <param name="value">The FeedItem to locate in the collection.</param>
		/// <returns>true if the FeedItem is a member of the collection; otherwise, false. </returns>
		public bool Contains(FeedItem value)
		{
			return List.Contains(value);
		}

		/// <summary>
		/// Determines whether the specified FeedItem is a member of the collection. 
		/// </summary>
		/// <param name="key">The FeedItem unique id to locate in the collection.</param>
		/// <returns>true if the FeedItem is a member of the collection; otherwise, false.</returns>
		public bool Contains(string key)
		{
			return keyMaps.ContainsKey(key);
		}

		/// <summary>
		/// Gets or sets the item at the specified index within the collection.
		/// </summary>
		public FeedItem this[int index]
		{
			// Use base class to process actual collection operation
			get
			{
				FeedItem item = (FeedItem)List[index];
				return item;
			}
			set
			{
				List[index] = (FeedItem)value;
			}
		}
		
		/// <summary>
		/// Gets or sets the item by the specified unique id within the collection.
		/// </summary>
		public FeedItem this[string id]
		{
			// Use base class to process actual collection operation
			get
			{
				int index = (int)keyMaps[id];
				FeedItem item = (FeedItem)List[index];
				return item;
			}
			set
			{
				int index = (int)keyMaps[id];
				FeedItem item = (FeedItem)List[index];
				List[index] = (FeedItem)value;
			}
		}

		/// <summary>
		/// Retrieves the index of the specified FeedItem in the collection.
		/// </summary>
		/// <param name="value"> The FeedItem to locate in the collection.</param>
		/// <returns>A zero-based index value that represents the position of the specified FeedItem in the FeedItemCollection . </returns>
		public int IndexOf(FeedItem value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}

		/// <summary>
		/// Removes all elements from the FeedItemCollection.
		/// </summary>
		public new void Clear()
		{
			base.Clear();
			keyMaps.Clear();
		}

		#endregion // public methods


	}
}
