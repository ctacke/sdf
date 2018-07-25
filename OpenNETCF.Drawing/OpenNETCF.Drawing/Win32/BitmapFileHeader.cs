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

namespace OpenNETCF.Win32
{
	/// <summary>
	/// This structure contains information about the type, size, and layout of a file that containing a device-independent bitmap (DIB).
	/// </summary>
	/// <remarks>Wraps the native <b>BITMAPFILEHEADER</b> structure</remarks>
	public class BitmapFileHeader 
	{ 
		private byte[] data;

		public BitmapFileHeader()
		{
			data = new byte[14];
		}
		/// <summary>
		/// Specifies the file type. It must be BM.
		/// </summary>
		public short  Type
		{
			get { return BitConverter.ToInt16(data, 0); }
			set { BitConverter.GetBytes(value).CopyTo(data, 0); }
		}
		/// <summary>
		/// Specifies the size, in bytes, of the bitmap file.
		/// </summary>
		public int    Size
		{
			get { return BitConverter.ToInt32(data, 2); }
			set { BitConverter.GetBytes(value).CopyTo(data, 2); }
		}

		//public ushort  bfReserved1;
		//public ushort  bfReserved2; 
		/// <summary>
		/// Specifies the offset, in bytes, from the <b>BitmapFileHeader</b> structure to the bitmap bits.
		/// </summary>
		public int    OffBits
		{
			get { return BitConverter.ToInt32(data, 10); }
			set { BitConverter.GetBytes(value).CopyTo(data, 10); }
		}

        /// <summary>
        /// Internal data
        /// </summary>
		public byte[] Data { get { return data; } }
	}
}
