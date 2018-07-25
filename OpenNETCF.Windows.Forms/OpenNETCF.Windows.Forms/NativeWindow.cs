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
using Microsoft.WindowsCE.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Provides a low-level encapsulation of a window handle and a window procedure. 
	/// </summary>
	public class NativeWindow
	{
		#region fields

		private IntPtr handle;
		private IntPtr defWindowProc;
		private IntPtr windowProcPtr = IntPtr.Zero;
		private WndProcDelegate windowProc;
		private bool ownHandle;

		#endregion // fields

		#region contructors

		public NativeWindow()
		{
			handle = IntPtr.Zero;
			ownHandle = true;
		}

		#endregion // contructors

		#region properties

		public IntPtr Handle 
		{
			get 
			{
				return handle;
			}
		}

		#endregion // properties

		#region methods

		/// <summary>
		/// Assigns a handle to this window.   
		/// </summary>
		/// <param name="handle">The handle to assign to this window.</param>
		public void AssignHandle(IntPtr handle) 
		{
			if (this.handle != IntPtr.Zero) 
			{
				ReleaseHandle();
			}
			this.handle = handle;
			this.ownHandle = false;
			Subclass();
			OnHandleChange();
		}

		/// <summary>
		///  Creates a window and its handle with the specified creation parameters.   
		/// </summary>
		/// <param name="cp">CreateParams that specifies the creation parameters for this window.</param>
		public virtual void CreateHandle(CreateParams cp) 
		{
			IntPtr ptr = IntPtr.Zero;

			if (cp != null) 
			{
				IntPtr hInstance = NativeMethods.GetModuleHandle(null);

				ptr = NativeMethods.CreateWindowEx((uint)cp.ExStyle, cp.ClassName, cp.Caption, (uint)cp.Style, cp.X, cp.Y, cp.Width, cp.Height, cp.Parent, IntPtr.Zero, hInstance, cp.Param);

				if (ptr == IntPtr.Zero)
				{
					int err = Marshal.GetLastWin32Error();
					throw new Win32Exception(err, "Error Creating Handle");
				}

				handle = ptr;
				ownHandle = true;
				// Subclass window
				this.Subclass();
			}
		}

		/// <summary>
		/// Releases the handle associated with this window. 
		/// </summary>
		public void ReleaseHandle()
		{
			
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			
			this.UnSubclass(false);

			this.handle = IntPtr.Zero;
		
			this.defWindowProc = IntPtr.Zero;
			this.windowProc = null;
			if (ownHandle)
			{
				NativeMethods.DestroyWindow(this.handle);
			}

			this.handle = IntPtr.Zero;
		
		}

		/// <summary>
		/// Invokes the default window procedure associated with this window.   
		/// </summary>
		/// <param name="m">A System.Windows.Forms.Message that is associated with the current Windows message.</param>
		protected virtual void WndProc(ref Message m)
		{
			this.DefWndProc(ref m);
		}
		
		/// <summary>
		/// Invokes the default window procedure associated with this window. It is an error to call this method when the System.Windows.Forms.NativeWindow.Handle property is 0.  
		/// </summary>
		/// <param name="m">A System.Windows.Forms.Message that is associated with the current Windows message.</param>
		public void DefWndProc(ref Message m)
		{
			if (this.defWindowProc == IntPtr.Zero)
			{
				m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
			}
			else
			{
                if (m.Result == IntPtr.Zero)
                {
                    m.Result = NativeMethods.CallWindowProc(this.defWindowProc, m.HWnd, (uint)m.Msg, m.WParam, m.LParam);
                }               
			}
		}

		/// <summary>
		/// Destroys the window and its handle.  
		/// </summary>
		public virtual void DestroyHandle() 
		{
			ReleaseHandle();
		}

		protected virtual void OnHandleChange()
		{
			
		}

		#endregion // methods

		#region static methods

		/// <summary>
		/// Retrieves the window associated with the specified handle.  
		/// </summary>
		/// <param name="handle">A handle to a window.</param>
		/// <returns>The System.Windows.Forms.NativeWindow associated with the specified handle. This method returns null when the handle does not have an associated window.</returns>
		public static NativeWindow FromHandle(IntPtr handle) 
		{
			NativeWindow window = new NativeWindow();

			window.AssignHandle(handle);

			return window;
		}

		#endregion // static methods
		
		#region helper

		private IntPtr Callback(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam)
		{
			Message message = Message.Create(hWnd, (int)msg, wparam, lparam);
			try
			{
				this.WndProc(ref message);
			}
			catch /*(Exception exception)*/
			{
				throw;
			}
			if (msg == 130)
			{
				this.ReleaseHandle();
			}
			return message.Result;
		}


		private void Subclass()
		{
			if (this.handle != IntPtr.Zero)
			{
				this.defWindowProc = NativeMethods.GetWindowLong(this.handle, -4);
				windowProc = new WndProcDelegate(Callback);
				NativeMethods.SetWindowLong(handle, (-4), windowProc);
			}

		}

		private void UnSubclass(bool finalizing)
		{
			
			if (this.windowProcPtr != NativeMethods.GetWindowLong(this.handle, -4))
			{
				return;
			}

			NativeMethods.SetWindowLong(handle, -4, (int)this.windowProcPtr);
		}

		#endregion // helper


		~NativeWindow() 
		{
			if (handle != IntPtr.Zero) 
			{
				ReleaseHandle();
			}
		}




	}
}
