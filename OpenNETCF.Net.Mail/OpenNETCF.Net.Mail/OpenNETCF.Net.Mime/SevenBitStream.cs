using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenNETCF.Net.Mail;

namespace OpenNETCF.Net.Mime
{
    internal class SevenBitStream : DelegatedStream
    {
        // Methods
        internal SevenBitStream(Stream stream)
            : base(stream)
        {
        }

        private void CheckBytes(byte[] buffer, int offset, int count)
        {
            for (int i = count; i < (offset + count); i++)
            {
                if (buffer[i] > 0x7f)
                {
                    throw new FormatException(SR.GetString("Mail7BitStreamInvalidCharacter"));
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((offset + count) > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            this.CheckBytes(buffer, offset, count);
            base.Write(buffer, offset, count);
        }
    }

 

}
