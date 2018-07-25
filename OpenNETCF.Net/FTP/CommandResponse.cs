using System;

namespace OpenNETCF.Net.Ftp
{
    /// <summary>
    /// Summary description for CommandResponse.
    /// </summary>
    public class CommandResponse
    {
        internal int status;
        internal string message;

        internal CommandResponse()
        {
        }

        public bool PositiveCompletion
        {
            get
            {
                return Status / 100 == 2;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
        }

        public int Status
        {
            get
            {
                return status;
            }
        }
    }
}
