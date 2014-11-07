using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RunWorkerCompletedEventArgs : System.EventArgs
    {
        // This class should inherit from AsyncCompletedEventArgs but I don't see the point in the CF's case
        private readonly object mResult;
        private readonly bool mCancelled;
        private readonly System.Exception mError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aResult"></param>
        /// <param name="aError"></param>
        /// <param name="aCancelled"></param>
        public RunWorkerCompletedEventArgs(object aResult, System.Exception aError, bool aCancelled)
        {
            mResult = aResult;
            mError = aError;
            mCancelled = aCancelled;
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
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancelled
        {
            get
            {
                return mCancelled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Exception Error
        {
            get
            {
                return mError;
            }
        }


        #region These are in the help but never seem to get used
        //		private object mUserState;
        //		public object UserState { 
        //			get{
        //				return mUserState;
        //			}
        //		}
        #endregion
    }
}
