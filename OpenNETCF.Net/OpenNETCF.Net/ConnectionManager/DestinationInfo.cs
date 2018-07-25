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
using OpenNETCF.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Contains information about a specific network.
	/// </summary>
	public class DestinationInfo
	{
		private object syncRoot = new object();

		/// <summary>
		/// Size of the DestinationInfo structure in unmanaged memory.
		/// </summary>
		internal static int NativeSize = 272;

		/// <summary>
		/// The destination's GUID identifier.
		/// </summary>
		public Guid Guid;

		/// <summary>
		/// The destination's description.
		/// </summary>
		public string Description = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DestinationInfo()
		{
		}

		/// <summary>
		/// Creates a new instance of DestinationInfo at the specific memory address.
		/// </summary>
		/// <param name="baseAddr">Memory address where the DestinationInfo object should be created.</param>
		public DestinationInfo(IntPtr baseAddr)
		{
			lock(syncRoot)
			{
				Guid = new Guid(Marshal2.ReadByteArray(baseAddr, 0, 16));	

				//Bug 144 - Turns out that calling PtrToStringUni in quick succession was causing a coredll.dll exception
				//			Now using PtrToStringAuto and not searching for null char	
				if (Marshal2.IsSafeToRead(new IntPtr(baseAddr.ToInt32() + 16), 256))
				{
					Description = Marshal2.PtrToStringAuto(new IntPtr(baseAddr.ToInt32() + 16));
				}
				
			}
		}
	}
}
