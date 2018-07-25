using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mail
{
    public class AsyncCompletedEventArgs : EventArgs
    {
        // Fields
        private readonly bool cancelled;
        private readonly Exception error;
        private readonly object userState;

        public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
        {
            this.error = error;
            this.cancelled = cancelled;
            this.userState = userState;
        }

        protected void RaiseExceptionIfNecessary()
        {
            if (this.Cancelled)
            {
                throw new InvalidOperationException(Resources.Async_OperationCancelled);
            }
        }

        public bool Cancelled
        {
            get
            {
                return this.cancelled;
            }
        }

        public Exception Error
        {
            get
            {
                return this.error;
            }
        }

        public object UserState
        {
            get
            {
                return this.userState;
            }
        }
    }
}
