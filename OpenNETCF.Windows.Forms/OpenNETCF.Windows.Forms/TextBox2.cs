using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;
using OpenNETCF.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.WindowsCE.Forms;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Enhances the <see cref="TextBox"/> control.
  /// </summary>
  public partial class TextBox2 : TextBox, IWin32Window
  {
    #region Append Text
    /// <summary>
    /// Appends text to the current text of a text box.
    /// </summary>
    /// <param name="text">The text to append to the current contents of the text box.</param>
    public void AppendText(string text)
    {
      IntPtr pText = Marshal2.StringToHGlobalUni(text);
      this.SelectionStart = this.TextLength;
      Win32Window.SendMessage(this.Handle, (int)EM.REPLACESEL, -1, pText);
      Marshal.FreeHGlobal(pText);
    }
    #endregion

    #region Character Casing
    private CharacterCasing characterCasing = CharacterCasing.Normal;

    /// <summary>
    /// Gets or sets whether the <see cref="TextBox2"/> control modifies the case of characters as they are typed.
    /// </summary>
    /// <value>One of the <see cref="CharacterCasing"/> enumeration values that specifies whether the <see cref="TextBox2"/> control modifies the case of characters.
    /// The default is <see cref="CharacterCasing">CharacterCasing.Normal</see>.</value>
    /// <remarks>You can use the <see cref="CharacterCasing"/> property to change the case of characters as required by your application.
    /// For example, you could change the case of all characters entered in a <see cref="TextBox2"/> control used for password entry to uppercase or lowercase to enforce a policy for passwords.</remarks>
    public CharacterCasing CharacterCasing
    {
        get
        {
            return characterCasing;
        }
        set
        {
            if (!DesignMode)
            {
                characterCasing = value;

                if (this.DesignMode) return;

                if (this.Handle != IntPtr.Zero)
                {
                    switch (value)
                    {
                        case CharacterCasing.Normal:
                            Win32Window.UpdateWindowStyle(this.Handle, (int)(ES.UPPERCASE | ES.LOWERCASE), 0);
                            break;
                        case CharacterCasing.Upper:
                            Win32Window.UpdateWindowStyle(this.Handle, (int)ES.LOWERCASE, (int)ES.UPPERCASE);
                            break;
                        case CharacterCasing.Lower:
                            Win32Window.UpdateWindowStyle(this.Handle, (int)ES.UPPERCASE, (int)ES.LOWERCASE);
                            break;
                    }
                }
            }
        }
    }
    #endregion

    #region Lines
    /// <summary>
    /// Gets or sets the lines of text in a text box control.
    /// </summary>
    public string[] Lines
    {
      get
      {
        //workaround for design mode
        if (StaticMethods.IsDesignMode(this))
        {
          return System.Text.RegularExpressions.Regex.Split(this.Text, "\r\n");
        }
        else
        {

          if (this.Handle != IntPtr.Zero)
          {
            byte[] buffer = new byte[512];
            int linecount = (int)Win32Window.SendMessage(this.Handle, (int)EM.GETLINECOUNT, 0, 0);
            string[] lines = new string[linecount];
            for (int iline = 0; iline < linecount; iline++)
            {
              BitConverter.GetBytes((short)512).CopyTo(buffer, 0);
              int chars = (int)Win32Window.SendMessage(this.Handle, (int)EM.GETLINE, iline, buffer);
              lines[iline] = Encoding.Unicode.GetString(buffer, 0, chars * 2);
            }

            return lines;
          }
        }

        return new string[0];
      }
    }
    #endregion


    #region Clipboard Support
    /// <summary>
    /// Moves the current selection in the <see cref="TextBox2"/> to the Clipboard.
    /// </summary>
    public void Cut()
    {
      //send Cut message
      Win32Window.SendMessage(this.Handle, (int)WM.CUT, 0, 0);
    }

    /// <summary>
    /// Copies the current selection in the <see cref="TextBox2"/> to the Clipboard.
    /// </summary>
    public void Copy()
    {
      //send Copy message
      Win32Window.SendMessage(this.Handle, (int)WM.COPY, 0, 0);
    }

    /// <summary>
    /// Replaces the current selection in the <see cref="TextBox2"/> with the contents of the Clipboard.
    /// </summary>
    public void Paste()
    {
      //send Paste message
      Win32Window.SendMessage(this.Handle, (int)WM.PASTE, 0, 0);
    }

    /// <summary>
    /// Clears all text from the <see cref="TextBox2"/> control.
    /// </summary>
    public void Clear()
    {
      //send Clear message
      Win32Window.SendMessage(this.Handle, (int)WM.CLEAR, 0, 0);
    }
    #endregion

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
      if (!this.DesignMode)
      {
        if (this.Handle != IntPtr.Zero)
        {
          this.m_defWindowProc = NativeMethods.GetWindowLong(this.Handle, (int)GWL.WNDPROC);
          m_windowProc = new WndProcDelegate(Callback);
          NativeMethods.SetWindowLong(this.Handle, (int)GWL.WNDPROC, m_windowProc);
        }
      }
    }

    private IntPtr Callback(IntPtr hWnd, WM msg, IntPtr wparam, IntPtr lparam)
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
        return !(this.Site == null) && this.Site.DesignMode;
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
