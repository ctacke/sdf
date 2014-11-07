using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace OpenNETCF.Drawing.Imaging
{
    [ComImport, Guid("327ABDAC-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IImageEncoder
    {
        // Initialize the image encoder object

        int InitEncoder(
        IStream stream
        );

        // Clean up the image encoder object

        int TerminateEncoder();

        // Get an IImageSink interface for encoding the next frame

        int GetEncodeSink(
        out IImageSink sink
        );

        // Set active frame dimension

        int SetFrameDimension(
        ref Guid dimensionID
        );

        int GetEncoderParameterListSize(
        out uint size
        );

        int GetEncoderParameterList(
        uint size,
        out IntPtr /*EncoderParameters*/ Params
        );

        int SetEncoderParameters(
        IntPtr /*ref EncoderParameters*/ Param
        );
    }
}
