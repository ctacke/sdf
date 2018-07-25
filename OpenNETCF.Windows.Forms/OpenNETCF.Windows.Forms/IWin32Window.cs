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
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Provides an interface to expose Win32 HWND handles.
	/// </summary>
	/// <remarks>This interface is implemented on objects that expose Win32 HWND handles.
	/// The resultant handle can be used with Win32 API calls.
	/// <para>Unlike the desktop .NET Framework this interface is not implemented in the base <see cref="Control"/> class.
	/// However you can implement it on any class which derives from <see cref="Control"/> or <see cref="Form"/>.
	/// The <see cref="Handle"/> property can then be passed to native API functions such as those contained in the <see cref="OpenNETCF.Win32.Win32Window"/> class.</para></remarks>
	/// <example>
	/// <code>
	/// [VB]
	/// Imports OpenNETCF.Windows.Forms
	/// Imports OpenNETCF.Win32
	/// 
	/// Public Class MyControl
	///		Inherits System.Windows.Forms.Control
	///		Implements IWin32Window
	///		
	///		Overridable ReadOnly Property Handle() As System.IntPtr
	///			Get
	///				Me.Capture = True
	///				Dim thishandle As IntPtr
	///				thishandle = Win32Window.GetCapture()
	///				Me.Capture = False
	///
	///				Handle = thishandle
	///			End Get
	///		End Property
	///		
	/// End Class
	/// </code>
	/// <code>
	/// [C#]
	/// using OpenNETCF.Windows.Forms;
	/// using OpenNETCF.Win32;
	/// 
	/// public class MyControl : Control, IWin32Window
	/// {
	///		
	///		public IntPtr Handle
	///		{
	///			get
	///			{
	///				this.Capture = true;
	///				IntPtr thishandle = Win32Window.GetCapture();
	///				this.Capture = false;
	///				
	///				return thishandle;
	///			}
	///		}
	/// }</code>
	/// </example>
	public interface IWin32Window
	{
		/// <summary>
		/// Gets the handle to the window represented by the implementer.
		/// </summary>
		IntPtr Handle
		{
			get;
		}
	}
}
