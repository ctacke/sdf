using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;
using OpenNETCF.Runtime.InteropServices;

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
                characterCasing = value;
                if (StaticMethods.IsDesignMode(this))
                    return;

                if (this.Handle != IntPtr.Zero)
                {
                    switch(value)
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

        #region On Parent Changed
#if !SDF21
        //OnparentChanged is in TextBox2_2.1.cs for SDF 2.1.  SDF2.0 will still use this because of the conditional compile

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (characterCasing != CharacterCasing.Normal)
            {
                //force reset of window style
                CharacterCasing = characterCasing;
            }
        }
#endif
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
    }
}
