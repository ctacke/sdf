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

namespace OpenNETCF.Win32.SafeHandles
{
	/// <summary>
	/// Provides common functionality that supports safe Win32 handle types.
	/// </summary>
	public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeHandleMinusOneIsInvalid"/> class.
		/// </summary>
		/// <param name="ownsHandle">true to reliably release the handle during the finalization phase; otherwise, false (not recommended).</param>
		protected SafeHandleMinusOneIsInvalid(bool ownsHandle) : base(new IntPtr(-1), ownsHandle){}

		/// <summary>
		/// Gets a value indicating whether a handle is invalid.
		/// </summary>
		public override bool IsInvalid
		{
			get
			{
				return(handle.ToInt32() == -1);
			}
		}
	}
}
