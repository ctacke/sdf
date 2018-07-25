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
