using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Drawing.Imaging
{
    [Flags]
    public enum ImageCodecFlags
    {
        Encoder = 1,
        Decoder = 2,
        SupportBitmap = 4,
        SupportVector = 8,
        SeekableEncode = 0x10,
        BlockingDecode = 0x20,
        BuiltIn = 0x10000,
        System = 0x20000,
        User = 0x40000
    }
}
