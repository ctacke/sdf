using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32.SafeHandles;

namespace OpenNETCF.Threading
{
    /// <summary>
    /// This class is used a wrapper for the handle owned by a <c>ThreadEx</c> class
    /// </summary>
    internal class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public delegate IntPtr CreateHandleDelegate();

        private CreateHandleDelegate m_createDelegate;

        /// <summary>
        /// Constructor for a SafeThreadHandle
        /// </summary>
        /// <param name="createHandleFunction"></param>
        public SafeThreadHandle(CreateHandleDelegate createHandleFunction)
            : base(true)
        {
            m_createDelegate = createHandleFunction;
        }

        /// <summary>
        /// Opens the instance of the SafeHandle by calling the <c>CreateHandleDelegate</c>
        /// </summary>
        public virtual void Open()
        {
            if (m_createDelegate != null)
            {
                handle = m_createDelegate();

                if (IsInvalid)
                {
                    throw new Exception("Created Handle is in list of Invalid Values");
                }
            }
            else
            {
                throw new Exception("No delegate avaialble to create SafeHandle");
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
    }
}
