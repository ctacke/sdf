using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Microsoft.WindowsCE.Forms;
using System.Reflection;
using OpenNETCF.Win32;
using System.Windows.Forms;


namespace OpenNETCF.Windows.Forms
{
    partial class TextBox2 : TextBox, IWin32Window
    {
        private IntPtr m_defWindowProc = IntPtr.Zero;
        private WndProcDelegate m_windowProc = null;

        /// <summary>
        /// Occurs when a WM_COPY message is sent to the textbox
        /// </summary>
        public event CancelEventHandler Copying;
        /// <summary>
        /// Occurs when a WM_CUT message is sent to the textbox
        /// </summary>
        public event CancelEventHandler Cutting;
        /// <summary>
        /// Occures when a WM_CLEAR message is sent to the textbox
        /// </summary>
        public event CancelEventHandler Clearing;
        /// <summary>
        /// Occurs when a WM_PASTE message is sent to the textbox
        /// </summary>
        public event CancelEventHandler Pasting;

#if SDF21
        //Added this define so we only have one OnParentChanged in the SDF 2.1
        /// <summary>
        /// Raises the System.Windows.Forms.Control.ParentChanged event.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (characterCasing != CharacterCasing.Normal)
            {
                //force reset of window style
                CharacterCasing = characterCasing;
            }

            //hook the wndproc
            if (!DesignMode)
            {
                if (this.Handle != IntPtr.Zero)
                {
                    this.m_defWindowProc = NativeMethods.GetWindowLong(this.Handle, (int)GWL.WNDPROC);
                    m_windowProc = new WndProcDelegate(Callback);
                    NativeMethods.SetWindowLong(this.Handle, (int)GWL.WNDPROC, m_windowProc);
                }
            }
        }
#endif
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

            return message.Result;
        }

        /// <summary>
        /// Invokes the default window procedure associated with this window.
        /// It is an error to call this method when the <see cref="Control.Handle"/> property is 0.  
        /// </summary>
        /// <param name="m">A <see cref="Message"/> that is associated with the current Windows message.</param>
        private void DefWndProc(ref Message m)
        {
            if (this.m_defWindowProc == IntPtr.Zero)
            {
                m.Result = NativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
            }
            else
            {
                m.Result = NativeMethods.CallWindowProc(this.m_defWindowProc, m.HWnd, (uint)m.Msg, m.WParam, m.LParam);
            }
        }

        /// <summary>
        /// The wndproc for the control.
        /// </summary>
        /// <param name="m">message sent by the system</param>
        protected virtual void WndProc(ref Message m)
        {
            CancelEventArgs args = new CancelEventArgs(false);

            switch ((WM)m.Msg)
            {
                case WM.COPY:
                    this.OnCopy(args);
                    break;
                case WM.CUT:
                    this.OnCut(args);
                    break;
                case WM.PASTE:
                    this.OnPaste(args);
                    break;
                case WM.CLEAR:
                    this.OnClear(args);
                    break;
            }

            //Call the default proc if it hasn't been cancled
            if (!args.Cancel)
                DefWndProc(ref m);
        }

        /// <summary>
        /// Raises the Copy event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCopy(CancelEventArgs e)
        {

            if (this.Copying != null)
                this.Copying(this, e);
        }

        /// <summary>
        /// Raises the Copy event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCut(CancelEventArgs e)
        {
            if (this.Cutting != null)
                this.Cutting(this, e);
        }

        /// <summary>
        /// Raises the Copy event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaste(CancelEventArgs e)
        {
            if (this.Pasting != null)
                this.Pasting(this, e);
        }

        /// <summary>
        /// Raises the Copy event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClear(CancelEventArgs e)
        {
            if (this.Clearing != null)
                this.Clearing(this, e);
        }

        protected bool DesignMode
        {
            get
            {
                return StaticMethods.IsDesignTime;
            }
        }

        /// <summary>
        /// Selects the text in the textBox control
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="textLength"></param>
        protected void SelectInternal(int start, int length, int textLength)
        {
            int num1;
            int num2;
            this.AdjustSelectionStartAndEnd(start, length, out num1, out num2, textLength);
            base.SelectionStart = num1;
            base.SelectionLength = num2;
        }

        /// <summary>
        /// Adjusts the selection
        /// </summary>
        /// <param name="selStart"></param>
        /// <param name="selLength"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="textLen"></param>
        protected void AdjustSelectionStartAndEnd(int selStart, int selLength, out int start, out int end, int textLen)
        {
            start = selStart;
            end = 0;
            if (start <= -1)
            {
                start = -1;
            }
            else
            {
                int num1;
                if (textLen >= 0)
                {
                    num1 = textLen;
                }
                else
                {
                    num1 = this.TextLength;
                }
                if (start > num1)
                {
                    start = num1;
                }
                try
                {
                    end = /*start +*/ selLength;
                }
                catch (OverflowException)
                {
                    end = (start > 0) ? 0x7fffffff : -2147483648;
                }
                if (end < 0)
                {
                    end = 0;
                }
                else if (end > num1)
                {
                    end = num1;
                }
            }
        }

        /// <summary>
        /// Clears the Undo buffer
        /// </summary>
        public void ClearUndo()
        {
            Win32Window.SendMessage(this.Handle, (int)EM.EMPTYUNDOBUFFER, 0, 0);
        }

        /// <summary>
        /// Gets the selection start and length
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        protected void GetSelectionStartAndLength(out int start, out int length)
        {
            start = this.SelectionStart;
            length = this.SelectionLength;
        }

        /// <summary>
        /// Sets the selected text while clearing the Undo if desired.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="clearUndo"></param>
        protected virtual void SetSelectedTextInternal(string text, bool clearUndo)
        {
            if (this.Handle != IntPtr.Zero)
            {
                if (text == null)
                {
                    text = "";
                }
                Win32Window.SendMessage(this.Handle, (int)EM.LIMITTEXT, 0, 0);
                if (clearUndo)
                {
                    Win32Window.SendMessage(this.Handle, (int)EM.REPLACESEL, 0, text);
                    Win32Window.SendMessage(this.Handle, (int)EM.SETMODIFY, 0, 0);
                    this.ClearUndo();
                }
                else
                {
                    Win32Window.SendMessage(this.Handle, (int)EM.REPLACESEL, -1, text);
                }
                Win32Window.SendMessage(this.Handle, (int)EM.LIMITTEXT, this.MaxLength, 0);
            }
        }

    }
}
