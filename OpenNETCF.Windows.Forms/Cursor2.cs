using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Implements showing and hiding the cursor.  
    /// <example>
    /// using(new Cursor2())
    /// {
    ///     //Do your long procudure
    /// }//cursor will automatically be hidden
    /// </example>
    /// </summary>
    public class Cursor2 : IDisposable
    {
        /// <summary>
        /// Reference count to see if the cursor should be hidden or not
        /// </summary>
        private static int _refCount = 0;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public Cursor2()
        {
            //Show the cursor
            if (_refCount == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();
            }

            //Increment the refcount
            _refCount++;
        }

        /// <summary>
        /// Hides the cursor if there are no more references
        /// </summary>
        public void Dispose()
        {
            //Decrement the ref count
            _refCount--;

            //Make sure we are not less than 0
            if (_refCount < 0)
                _refCount = 0;

            //Hide the cursor if there are no more refcounts
            if (_refCount == 0)
            {
                Cursor.Current = Cursors.Default;
                Cursor.Hide();
            }
        }
    }
}
