using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Messaging
{
    internal sealed class MsgQueueOptions
    {
        public MsgQueueOptions(bool forReading, int maxMessageLength, int maxMessages)
        {
            if (maxMessageLength <= 0) throw new ArgumentException();
            if (maxMessages < 0) throw new ArgumentException();

            bReadAccess = forReading ? 1 : 0;
            dwMaxMessages = maxMessages;
            cbMaxMessage = maxMessageLength;
            dwSize = 20;
        }

        public MsgQueueOptions(bool forReading)
        {
            bReadAccess = forReading ? 1 : 0;
            dwSize = 20;
        }

        public Int32 dwSize;
        public Int32 dwFlags;
        public Int32 dwMaxMessages;
        public Int32 cbMaxMessage;
        public Int32 bReadAccess;
    }
}
