using System;

using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenNETCF.Net
{
    internal class ClosableStream : DelegatedStream
    {
        // Fields
        private int closed;
        private EventHandler onClose;

        // Methods
        internal ClosableStream(Stream stream, EventHandler onClose)
            : base(stream)
        {
            this.onClose = onClose;
        }

        public override void Close()
        {
            if ((this.onClose != null))
            {
                this.onClose(this, new EventArgs());
            }
        }
    }

 

}
