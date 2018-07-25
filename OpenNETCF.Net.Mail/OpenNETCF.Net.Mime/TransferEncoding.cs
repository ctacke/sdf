using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.Mime
{
    /// <summary>
    /// Specifies the Content-Transfer-Encoding header information for an e-mail message attachment.
    /// </summary>
    public enum TransferEncoding
    {
        Base64 = 1,
        QuotedPrintable = 0,
        SevenBit = 2,
        Unknown = -1
    }
}
