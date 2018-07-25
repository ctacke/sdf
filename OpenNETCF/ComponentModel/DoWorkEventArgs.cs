using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public class DoWorkEventArgs : System.ComponentModel.CancelEventArgs
    {
        private readonly object mArgument;
        private object mResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aArgument"></param>
        public DoWorkEventArgs(object aArgument)
        {
            mArgument = aArgument;
        }

        /// <summary>
        /// 
        /// </summary>
        public object Argument
        {
            get
            {
                return mArgument;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public object Result
        {
            get
            {
                return mResult;
            }
            set
            {
                mResult = value;
            }
        }
    }
}
