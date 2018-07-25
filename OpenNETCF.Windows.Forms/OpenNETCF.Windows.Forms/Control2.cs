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
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#if !NDOC
using OpenNETCF.Diagnostics;
using OpenNETCF.Win32;
using Microsoft.WindowsCE.Forms;
#endif

// Intended as a base class for all custom OpenNETCF controls
// Peter Foot 2004/07/17

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Extends the standard <see cref="System.Windows.Forms.Control"/> class.
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Control"/>
	public abstract class Control2 : Control, IWin32Window
	{
        /// <summary>
        /// The child handle of the control
        /// </summary>
        protected IntPtr childHandle;

        private IntPtr defWindowProc;
        private WndProcDelegate windowProc;
		
		#region Constructor
		/// <summary>
		/// Creates a new <see cref="Control2"/> object.
		/// </summary>		
		protected Control2()
		{
		}
		#endregion

        #region InitCommonControlsEx
        /// <summary>
        /// Calls InitCommonControlsEx for the specified classes.
        /// </summary>
        /// <param name="controlClasses">Mask of control class groups.</param>
        /// <remarks>Not desktop compatible</remarks>
        protected static void InitCommonControls(int controlClasses)
        {
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                NativeMethods.INITCOMMONCONTROLSEX icc = new NativeMethods.INITCOMMONCONTROLSEX();
                icc.dwSize = 8;
                icc.dwICC = (NativeMethods.ICC)controlClasses;
                NativeMethods.InitCommonControlsEx(ref icc);
            }
        }
        #endregion

        #region Border Style

        //border style
		private BorderStyle border = BorderStyle.None;

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		/// <value>One of the <see cref="BorderStyle"/> values.
		/// Fixed3D is interpreted the same as FixedSingle.</value>
#if DESIGN
		[Category("Appearance"),
		Description("The style of the border."),
		DefaultValue(BorderStyle.None)]
#endif
		public BorderStyle BorderStyle
		{
			get
			{
				return border;
			}
			set
			{
				border = value;

                if (!StaticMethods.IsDesignMode(this))
                {
#if !NDOC
                    if (value == BorderStyle.None)
                    {
                        //remove WS_BORDER style
                        Win32Window.UpdateWindowStyle(Handle, (int)WS.BORDER, 0);
                    }
                    else
                    {
                        //set WS_Border style
                        Win32Window.UpdateWindowStyle(Handle, 0, (int)WS.BORDER);
                    }
#endif
                }
			}
		}
		#endregion

        #region Created
#if !DESIGN
        /// <summary>
		/// Gets a value indicating whether the control has been created.
		/// </summary>
		public bool Created
		{
			get
			{
				return (childHandle != IntPtr.Zero);
			}
		}
#endif
		#endregion

		#region Create Params
#if !DESIGN
		/// <summary>
		/// Gets the required creation parameters when the control handle is created.
		/// </summary>
		protected virtual CreateParams CreateParams 
		{
			get
			{
				//set defaults in createparams
				CreateParams cp = new CreateParams();
#if !NDOC
				cp.Parent = this.Handle;

				cp.ClassStyle = (int)(WS.VISIBLE | WS.TABSTOP | WS.CHILD);
				cp.Width = this.Width;
				cp.Height = this.Height;
				cp.X = 0;
				cp.Y = 0;
#endif
				return cp;
			}
		}
#endif
		#endregion

        #region Design Mode
        /// <summary>
        /// Gets a value indicating whether a control is being used on a design surface.
        /// </summary>
        protected internal bool DesignMode
        {
            get
            {
                if (this.Site != null && this.Site.DesignMode)
                    return true;

                return StaticMethods.IsDesignMode(this.Parent);
            }
        }
        #endregion

        #region Recreating Handle

        private bool recreating = false;
		/// <summary>
		/// Gets a value indicating whether the control is currently re-creating its handle.
		/// </summary>
		/// <remarks>true if the control is currently re-creating its handle; otherwise, false.</remarks>
		public bool RecreatingHandle
		{
			get
			{
				return recreating;
			}
		}
		#endregion

        #region Text
        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        public override string Text
        {
            get
            {
                if (this.childHandle != IntPtr.Zero)
                {
                    byte[] buffer = new byte[520];
                    int chars = NativeMethods.GetWindowText(this.childHandle, buffer, 260);
                    string text = System.Text.Encoding.Unicode.GetString(buffer, 0, chars * 2);
                    return text;
                }
                return base.Text;
            }
            set
            {
                if (this.childHandle != IntPtr.Zero)
                {
                    NativeMethods.SetWindowText(this.childHandle, value);
                }
                base.Text = value;
            }
        }
        #endregion


		#region Key Events
#if DESIGN

		[Browsable(true),
		Category("Key"),
		Description("Occurs when a key is first pressed.")]
		public new event KeyEventHandler KeyDown;

		[Browsable(true),
		Category("Key"),
		Description("Occurs after a user is finished pressing a key.")]
		public new event KeyPressEventHandler KeyPress;

		[Browsable(true),
		Category("Key"),
		Description("Occurs when a key is released.")]
		public new event KeyEventHandler KeyUp;
#endif	
		#endregion

		#region Create Control
#if !DESIGN
		/// <summary>
		/// Forces the creation of the control, including the creation of the handle and any child controls.
		/// </summary>
		public void CreateControl()
		{
#if !NDOC
			//if running on device
            if (!StaticMethods.IsDesignMode(this))
            {
                //check if a control is already created
                if (childHandle != IntPtr.Zero)
                {
                    recreating = true;

                    //destroy the existing control
                    Win32Window.DestroyWindow(childHandle);

                    childHandle = IntPtr.Zero;
                }

                CreateParams cp = this.CreateParams;

                //dont create null window
                if (cp.ClassName != null)
                {
                    //create the control
                    childHandle = Win32Window.CreateWindowEx(cp.ExStyle, cp.ClassName, cp.Caption, cp.ClassStyle, cp.X, cp.Y, cp.Width, cp.Height, cp.Parent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                }
            }

			recreating = false;
#endif
		}
#endif
		#endregion

		#region Designer OnPaint Support
        /// <summary>
        /// Raises the System.Windows.Forms.Control.Paint event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
            if (StaticMethods.IsDesignMode(this))
            {
                //fill background
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);

                if (this.border != BorderStyle.None)
                {
                    //draw a border
                    e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                }

                e.Graphics.DrawString("Place holder for " + this.Name,this.Font,new SolidBrush(Color.Black),2,2);
            }
			base.OnPaint (e);
		}
		#endregion

		#region On Parent Changed
		/// <summary>
		/// Occurs when the control is associated with a new Parent.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged(EventArgs e)
		{
           // Added for desing-time support
           if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
           {
                if (this.Handle != IntPtr.Zero)
                {
                    this.defWindowProc = NativeMethods.GetWindowLong(this.Handle, -4);
                    windowProc = new WndProcDelegate(Callback);
                    NativeMethods.SetWindowLong(this.Handle, (-4), windowProc);
                }
            }
            
			//(re)create the native control once this control is placed on a form.
			CreateControl();

			base.OnParentChanged (e);
		}
		#endregion

		#region Set Bounds
		/// <summary>
		/// Sets the bounds of the control to the specified location and size.
		/// </summary>
		/// <param name="x">The new <see cref="Control.Left"/> property value of the control.</param>
		/// <param name="y">The new <see cref="Control.Top"/> property value of the control.</param>
		/// <param name="width">The new <see cref="Control.Width"/> property value of the control.</param>
		/// <param name="height"> The new <see cref="Control.Height"/> property value of the control.</param>
		public void SetBounds(int x, int y, int width, int height)
		{
            if(!StaticMethods.IsDesignMode(this))
			this.Bounds = new Rectangle(x, y, width, height);
		}
		#endregion		

        #region WndProc
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnNotifyMessage(ref Message m)
        {
        }

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
            /*if (msg == 130)
            {
                this.ReleaseHandle();
            }*/
            return message.Result;
        }

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// It is an error to call this method when the <see cref="Control.Handle"/> property is 0.  
        /// </summary>
        /// <param name="m">A <see cref="Message"/> that is associated with the current Windows message.</param>
        public void DefWndProc(ref Message m)
        {
            if (this.defWindowProc == IntPtr.Zero)
            {
                m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
            }
            else
            {
                m.Result = NativeMethods.CallWindowProc(this.defWindowProc, m.HWnd, (uint)m.Msg, m.WParam, m.LParam);
            }
        }

        protected virtual void WndProc(ref Message m)
        {
            switch ((WM)m.Msg)
            {
                case WM.NOTIFY:
                    OnNotifyMessage(ref m);
                    break;
                case WM.ERASEBKGND:
                    OnEraseBkgndMessage(ref m);
                    break;
            }

            DefWndProc(ref m);
        }
        #endregion

        #region Message Processing Members
#if !DESIGN && !NDOC
        

		//used internally by the ControlMessageWindow to pass through messages
		protected virtual void OnEraseBkgndMessage(ref Microsoft.WindowsCE.Forms.Message m)
		{
		}

		
#endif
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="Control2"/> and its child controls and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
            if (StaticMethods.IsDesignMode(this))
                return;

#if !NDOC
            //kill native control if present
			if(childHandle != IntPtr.Zero)
			{
				//destroy native control
				Win32Window.DestroyWindow(childHandle);
				childHandle = IntPtr.Zero;
			}

            //unsubclass
            if (this.defWindowProc != NativeMethods.GetWindowLong(this.Handle, -4))
            {
                NativeMethods.SetWindowLong(this.Handle, -4, (int)this.defWindowProc);
            }

#endif

			base.Dispose (disposing);
		}
		#endregion


        //static

        #region InvokeRequired
#if !DESIGN

        /// <summary>
		/// Gets a value indicating whether the caller must call an invoke method when making method calls to the control because the caller is on a different thread than the one the control was created on. 
		/// </summary>
		/// <param name="c">Control which must be checked that it's created on a different thread than the calling thread.</param>
		/// <returns>true if the control was created on a different thread than the calling thread (indicating that you must make calls to the control through an invoke method); otherwise, false.</returns>
		public static bool InvokeRequiredForControl(Control c)
		{
#if !NDOC
			object t = typeof(Control).GetField("m_thread", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c);
			if (t == null) return false;
			int controlThreadId = ((IntPtr)typeof(Thread).GetField("m_ThreadId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(t)).ToInt32();
			int curThreadId = ProcessHelper.GetCurrentThreadID();
			return (controlThreadId != curThreadId);			

#else
			return false;
#endif
		}
#endif
        
		#endregion

        #region Modifier Keys
        /// <summary>
        /// Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in a pressed state.
        /// </summary>
        /// <value>A bitwise combination of the <see cref="Keys"/> values.
        /// The default is <see cref="System.Windows.Forms.Keys.None"/>.</value>
        /// <example>The following code example hides a button when the CTRL key is pressed while the button is clicked. This example requires that you have a Button named button1 on a Form.
        /// <code>[VB]
        /// Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        ///    ' If the CTRL key is pressed when the 
        ///    ' control is clicked, hide the control. 
        ///    If Control.ModifierKeys = Keys.Control Then
        ///       CType(sender, Control).Hide()
        ///    End If
        /// End Sub
        /// </code>
        /// <code>[C#]
        /// private void button1_Click(object sender, System.EventArgs e)
        /// {
        ///   /* If the CTRL key is pressed when the 
        ///      * control is clicked, hide the control. */
        ///   if(Control.ModifierKeys == Keys.Control)
        ///   {
        ///      ((Control)sender).Hide();
        ///   }
        /// }
        /// </code></example>
        public static Keys ModifierKeys
        {
            get
            {
                Keys keys = Keys.None;
                if (NativeMethods.GetKeyState(0x10) < 0)
                {
                    keys |= Keys.Shift;
                }
                if (NativeMethods.GetKeyState(0x11) < 0)
                {
                    keys |= Keys.Control;
                }
                if (NativeMethods.GetKeyState(0x12) < 0)
                {
                    keys |= Keys.Alt;
                }
                return keys;
            }
        }
        #endregion

	}
}
