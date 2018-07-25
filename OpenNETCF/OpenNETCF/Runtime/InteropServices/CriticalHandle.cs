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

namespace OpenNETCF.Runtime.InteropServices
{
	/// <summary>
	/// Represents a wrapper class for handle resources.
	/// </summary>
	public abstract class CriticalHandle : IDisposable
	{
		private bool closed;
		internal bool invalid;

		/// <summary>
		/// Initializes a new instance of the CriticalHandle class with the specified invalid handle value.
		/// </summary>
		/// <param name="invalidHandleValue">The value of an invalid handle (usually 0 or -1).</param>
		protected CriticalHandle(IntPtr invalidHandleValue)
		{
			this.invalid = false;
			handle = invalidHandleValue;
			this.closed = false;
		}

		/// <summary>
		/// Specifies the handle to be wrapped.
		/// </summary>
		protected IntPtr handle;

		/// <summary>
		/// Gets a value indicating whether the handle is closed.
		/// </summary>
		/// <value>true if the handle is closed; otherwise, false.</value>
		public bool IsClosed
		{
			get
			{
				return closed;
			}
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the handle value is invalid.
		/// </summary>
		/// <value>true if the handle is valid; otherwise, false.</value>
		public abstract bool IsInvalid
		{
			get;
		}

		/// <summary>
		/// Marks the handle for releasing and freeing resources.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns></returns>
		protected abstract bool ReleaseHandle();

		/// <summary>
		/// Sets the handle to the specified pre-existing handle.
		/// </summary>
		/// <param name="handle"></param>
		protected void SetHandle(IntPtr handle)
		{
			this.handle = handle;
		}

		/// <summary>
		/// Marks a handle as invalid.
		/// </summary>
		public void SetHandleAsInvalid()
		{
			this.invalid = true;
			GC.SuppressFinalize(this);
		}

		#region IDisposable Members

		protected void Dispose(bool disposing)
		{
			if(!IsClosed && !IsInvalid)
			{
				ReleaseHandle();
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Marks the handle for releasing and freeing resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			
		}

		/// <summary>
		/// Frees all resources associated with the handle.
		/// </summary>
		~CriticalHandle()
		{
			Dispose(false);
		}
		#endregion
	}
}
