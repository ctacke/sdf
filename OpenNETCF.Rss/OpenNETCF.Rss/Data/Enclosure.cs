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
