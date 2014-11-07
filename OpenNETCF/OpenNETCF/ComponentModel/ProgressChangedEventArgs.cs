using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProgressChangedEventArgs : System.EventArgs
    {
        private readonly int mProgressPercent;
        private readonly object mUserState;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aProgressPercent"></param>
        /// <param name="aUserState"></param>
        public ProgressChangedEventArgs(int aProgressPercent, object aUserState)
        {
            mProgressPercent = aProgressPercent;
            mUserState = aUserState;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ProgressPercentage
        {
            get
            {
                return mProgressPercent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object UserState
        {
            get
            {
                return mUserState;
            }
        }
    }
}
