using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//Not supported on Windows Mobile

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Represents a common dialog box that allows the user to choose a folder.
    /// </summary>
    /// <remarks>Not supported on Windows Mobile, and possibly other platforms - throws a PlatformNotSupportedException if API is missing.</remarks>
    public sealed class FolderBrowserDialog : Component
    {
        private NativeMethods.BROWSEINFO info;
        private string folder = string.Empty;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderBrowserDialog"/> class.
        /// </summary>
        public FolderBrowserDialog()
        {
            info = new NativeMethods.BROWSEINFO();
            info.pszDisplayName = new string('\0', 260);
            //info.lpszTitle = string.Empty;
            NativeMethods.INITCOMMONCONTROLSEX icc = new NativeMethods.INITCOMMONCONTROLSEX();
            icc.dwSize = 8;
            icc.dwICC = NativeMethods.ICC.TREEVIEW_CLASSES | NativeMethods.ICC.TOOLTIP_CLASSES;
            NativeMethods.InitCommonControlsEx(ref icc);
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public void Reset()
        {
            info.hwndOwner = IntPtr.Zero;
            info.lpszTitle = null;
            info.pszDisplayName = new string('\0', 260);
            folder = string.Empty;
        }
        #endregion

        #region Show Dialog

        private bool RunDialog(IntPtr hwndOwner)
        {
            info.hwndOwner = hwndOwner;

            IntPtr pitemidlist;

            try
            {
                pitemidlist = NativeMethods.SHBrowseForFolder(ref info);
            }
            catch (MissingMethodException mme)
            {
                throw new PlatformNotSupportedException("Your platform doesn't support the SHBrowseForFolder API", mme);
            }

            if (pitemidlist == IntPtr.Zero)
            {
                return false;
            }

            //maxpath unicode chars
            byte[] buffer = new byte[520];
            bool success = NativeMethods.SHGetPathFromIDList(pitemidlist, buffer);
            //get string from buffer
            if (success)
            {
                folder = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);

                int nullindex = folder.IndexOf('\0');
                if (nullindex != -1)
                {
                    folder = folder.Substring(0, nullindex);
                }
            }

            //free native memory
            Marshal.FreeHGlobal(pitemidlist);

            return true;
        }

        /// <summary>
        /// Runs a common dialog box with a default owner.
        /// </summary>
        /// <returns>System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.</returns>
        public DialogResult ShowDialog()
        {
            return RunDialog(IntPtr.Zero) ? DialogResult.OK : DialogResult.Cancel;
        }
        /// <summary>
        /// Runs a common dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">Any object that implements <see cref="IWin32Window"/> that represents the top-level window that will own the modal dialog box.</param>
        /// <returns>System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise, System.Windows.Forms.DialogResult.Cancel.</returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            return RunDialog(owner.Handle) ? DialogResult.OK : DialogResult.Cancel;
        }
        #endregion


        #region Selected Path
        /// <summary>
        /// Gets the path selected by the user.
        /// </summary>
        public string SelectedPath
        {
            get
            {
                return folder;
            }
        }
        #endregion

        #region Description
        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public string Description
        {
            get
            {
                return info.lpszTitle;
            }
            set
            {
                info.lpszTitle = value;
            }
        }
        #endregion

    }
}
