using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace OpenNETCF.Drawing.Imaging
{
#if !NDOC
    /// <summary>
    /// This interface is used to create bitmaps and images and to manage image encoders and decoders.
    /// </summary>
    [ComImport, Guid("327ABDA8-072B-11D3-9D7B-0000F81EF32E")]
    [CLSCompliant(false)]
    public class ImagingFactoryClass: ImagingFactory
    {
        
        /// <summary>
        /// Create an image object from an input stream
        ///  stream doesn't have to seekable
        ///  caller should Release the stream if call is successful
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual extern int CreateImageFromStream(
        IStream stream,
        out IImage image
        );

        /// <summary>
        /// Create an image object from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual extern int CreateImageFromFile(
        string filename,
        out IImage image
        );

        /// <summary>
        /// Create an image object from a memory buffer
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <param name="disposalFlag"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public virtual extern int CreateImageFromBuffer(
        byte[] buf,
        uint size,
        BufferDisposalFlag disposalFlag,
        out IImage image
        );

        /// <summary>
        /// Create a new bitmap image object
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public virtual extern int CreateNewBitmap(
        uint width,
        uint height,
        PixelFormat pixelFormat,
        out IBitmapImage bitmap
        );

        /// <summary>
        /// Create a bitmap image from an IImage object
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="hints"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public virtual extern int CreateBitmapFromImage(
        IImage image,
        uint width,
        uint height,
        PixelFormat pixelFormat,
        InterpolationHint hints,
        out IBitmapImage bitmap
        );

        /// <summary>
        /// Create a new bitmap image object on user-supplied memory buffer
        /// </summary>
        /// <param name="bitmapData"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public virtual extern int CreateBitmapFromBuffer(
        ref BitmapDataInternal bitmapData,
        out IBitmapImage bitmap
        );

        // Create an image decoder object to process the given input stream

        public virtual extern int CreateImageDecoder(
        IStream stream,
        DecoderInitFlag flags,
        out IImageDecoder decoder
        );

        /// <summary>
        /// Create an image encoder object that can output data the
        /// specified image file format.
        /// </summary>
        /// <param name="clsid"></param>
        /// <param name="stream"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public virtual extern int CreateImageEncoderToStream(
        ref Guid clsid,
        IStream stream,
        out IImageEncoder encoder
        );

        /// <summary>
        /// Create an image encoder object that can output data the
        /// specified image file format.
        /// </summary>
        /// <param name="clsid"></param>
        /// <param name="filename"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public virtual extern int CreateImageEncoderToFile(
        ref Guid clsid,
        string filename,
        out IImageEncoder encoder
        );

        /// <summary>
        /// Get a list of all currently installed image decoders
        /// </summary>
        /// <param name="count"></param>
        /// <param name="decoders"></param>
        /// <returns></returns>
        public virtual extern int GetInstalledDecoders(
        out uint count,
        out IntPtr /*out ImageCodecInfo*/ decoders
        );

        /// <summary>
        /// Get a list of all currently installed image decoders
        /// </summary>
        /// <param name="count"></param>
        /// <param name="encoders"></param>
        /// <returns></returns>
        public virtual extern int GetInstalledEncoders(
        out uint count,
        //out IntPtr /*ImageCodecInfo*/ encoders
            //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
            //out ImageCodecInfo[] encoders
        out IntPtr /*out ImageCodecInfo[]*/ encoders
        );

        /// <summary>
        /// Install an image encoder / decoder
        ///  caller should do the regular COM component
        ///  installation before calling this method
        /// </summary>
        /// <param name="codecInfo"></param>
        /// <returns></returns>
        public virtual extern int InstallImageCodec(
        ref ImageCodecInfo codecInfo
        );

        /// <summary>
        /// Uninstall an image encoder / decoder
        /// </summary>
        /// <param name="codecName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public virtual extern int UninstallImageCodec(
        string codecName,
        uint flags
        );

    }

    /// <summary>
    /// This interface is used to create bitmaps and images and to manage image encoders and decoders.
    /// </summary>
    [Guid("327ABDA7-072B-11D3-9D7B-0000F81EF32E")]
    [ComImport, CoClass(typeof(ImagingFactoryClass))]
    [CLSCompliant(false)]
    public interface ImagingFactory : IImagingFactory
    {
    }
#endif
    [Guid("327ABDA7-072B-11D3-9D7B-0000F81EF32E")]
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IImagingFactory
    {
        /// <summary>
        /// Create an image object from an input stream
        ///  stream doesn't have to seekable
        ///  caller should Release the stream if call is successful
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="image"></param>
        /// <returns></returns>
    	int CreateImageFromStream(
        IStream stream,
        out IImage image
        );

        /// <summary>
        /// Create an image object from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <returns></returns>
    	int CreateImageFromFile(
        string filename,
        out IImage image
        );
    
        /// <summary>
        /// Create an image object from a memory buffer
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <param name="disposalFlag"></param>
        /// <param name="image"></param>
        /// <returns></returns>
    	int CreateImageFromBuffer(
        byte[] buf,
        uint size,
        BufferDisposalFlag disposalFlag,
        out IImage image
        );

        /// <summary>
        /// Create a new bitmap image object
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
    	int CreateNewBitmap(
        uint width,
        uint height,
        PixelFormat pixelFormat,
        out IBitmapImage bitmap
        );

        /// <summary>
        /// Create a bitmap image from an IImage object
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="hints"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
    	int CreateBitmapFromImage(
        IImage image,
        uint width,
        uint height,
        PixelFormat pixelFormat,
        InterpolationHint hints,
        out IBitmapImage bitmap
        );

        /// <summary>
        /// Create a new bitmap image object on user-supplied memory buffer
        /// </summary>
        /// <param name="bitmapData"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
    	int CreateBitmapFromBuffer(
        ref BitmapDataInternal bitmapData,
        out IBitmapImage bitmap
        );

        /// <summary>
        /// Create an image decoder object to process the given input stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="flags"></param>
        /// <param name="decoder"></param>
        /// <returns></returns>
    	int CreateImageDecoder(
        IStream stream,
        DecoderInitFlag flags,
        out IImageDecoder decoder
        );

        /// <summary>
        /// Create an image encoder object that can output data the
        /// specified image file format.
        /// </summary>
        /// <param name="clsid"></param>
        /// <param name="stream"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
    	int CreateImageEncoderToStream(
        ref Guid clsid,
        IStream stream,
        out IImageEncoder encoder
        );

    	int CreateImageEncoderToFile(
        ref Guid clsid,
        string filename,
        out IImageEncoder encoder
        );

        /// <summary>
        /// Get a list of all currently installed image decoders
        /// </summary>
        /// <param name="count"></param>
        /// <param name="decoders"></param>
        /// <returns></returns>
    	int GetInstalledDecoders(
        out uint count,
        out IntPtr /*out ImageCodecInfo*/ decoders
        );

        /// <summary>
        /// Get a list of all currently installed image decoders
        /// </summary>
        /// <param name="count"></param>
        /// <param name="encoders"></param>
        /// <returns></returns>
    	int GetInstalledEncoders(
        out uint count,
            //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
            //out ImageCodecInfo[] encoders
        out IntPtr /*out ImageCodecInfo[]*/ encoders
        );

        /// <summary>
        /// Install an image encoder / decoder
        ///  caller should do the regular COM component
        ///  installation before calling this method
        /// </summary>
        /// <param name="codecInfo"></param>
        /// <returns></returns>
    	int InstallImageCodec(
        ref ImageCodecInfo codecInfo
        );

        /// <summary>
        /// Uninstall an image encoder / decoder
        /// </summary>
        /// <param name="codecName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
    	int UninstallImageCodec(
        string codecName,
        uint flags
        );

    }

    /// <summary>
    /// This is the basic interface to an image object. It allows applications to do the following: 
    /// Display the image onto a destination graphics context  
    /// Push image data into an image sink 
    /// Access image properties and metadata Decoded image objects and in-memory bitmap image objects support the IImage interface
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("327ABDA9-072B-11D3-9D7B-0000F81EF32E")]
    [CLSCompliant(false)]
    public interface IImage
    {
        /// <summary>
        /// Get the device-independent physical dimension of the image
        ///  unit of 0.01mm
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
    	int GetPhysicalDimension(
        out Size size
        );

        /// <summary>
        /// Get basic image info
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
    	int GetImageInfo(
        out ImageInfo imageInfo
        );

        /// <summary>
        /// Set image flags
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
    	int SetImageFlags(
        ImageFlags flags
        );

        /// <summary>
        /// Display the image a GDI device context
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="dstRect"></param>
        /// <param name="srcRect"></param>
        /// <returns></returns>
    	int Draw(
        IntPtr hdc,
        RECT dstRect,
        RECT srcRect
        );

        /// <summary>
        /// Push image data into an IImageSink
        /// </summary>
        /// <param name="sink"></param>
        /// <returns></returns>
    	int PushIntoSink(
        IImageSink sink
        );

        /// <summary>
        /// Get a thumbnail representation for the image object
        /// </summary>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <param name="thumbImage"></param>
        /// <returns></returns>
    	int GetThumbnail(
        uint thumbWidth,
        uint thumbHeight,
        out IImage thumbImage
        );
    }

    /// <summary>
    /// This is a low-level interface to an image decoder object. 
    /// Many simple applications do not need to work with decoder objects directly and can work with 
    /// higher level decoded image objects instead. 
    /// More sophisticated applications can use the IImageDecoder interface to have finer control over the interaction with decoder objects.
    /// The IImageDecoder interface can support images with multiple frames. Multiframe images are accessed via multidimensional indices. 
    /// This interface can support an arbitrary number of dimensions in nonrectangular arrangements
    /// </summary>
    [ComImport, Guid("327ABDAB-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IImageDecoder
    {
        /// <summary>
        /// Initialize the image decoder object
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
    	int InitDecoder(
        IStream stream,
        DecoderInitFlag flags
        );

        /// <summary>
        /// Clean up the image decoder object
        /// </summary>
        /// <returns></returns>
    	int TerminateDecoder();

        /// <summary>
        /// Start decoding the current frame
        /// </summary>
        /// <param name="sink"></param>
        /// <param name="newPropSet"></param>
        /// <returns></returns>
    	int BeginDecode(
        IImageSink sink,
        IntPtr /*IPropertySetStorage*/ newPropSet
        );

        /// <summary>
        /// Continue decoding
        /// </summary>
        /// <returns></returns>
    	int Decode();

        /// <summary>
        /// Stop decoding the current frame
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
    	int EndDecode(
        int statusCode
        );

        /// <summary>
        /// Query multi-frame dimensions
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
    	int GetFrameDimensionsCount(
        out uint count
        );
    
        /// <summary>
        /// Query multi-frame dimensions
        /// </summary>
        /// <param name="dimensionIDs"></param>
        /// <param name="count"></param>
        /// <returns></returns>
    	int GetFrameDimensionsList(
        out Guid dimensionIDs,
        out uint count
        );

        /// <summary>
        /// Get number of frames for the specified dimension
        /// </summary>
        /// <param name="dimensionID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
    	int GetFrameCount(
        ref Guid dimensionID,
        out uint count
        );

        /// <summary>
        /// Select currently active frame
        /// </summary>
        /// <param name="dimensionID"></param>
        /// <param name="frameIndex"></param>
        /// <returns></returns>
    	int SelectActiveFrame(
        ref Guid dimensionID,
        uint frameIndex
        );

        /// <summary>
        /// Get basic information about the image
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
    	int GetImageInfo(
        out ImageInfo imageInfo
        );

        /// <summary>
        /// Get image thumbnail
        /// </summary>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <param name="thumbImage"></param>
        /// <returns></returns>
    	int GetThumbnail(
        uint thumbWidth,
        uint thumbHeight,
        out IImage thumbImage
        );

        /// <summary>
        /// Query decoder parameters
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
    	int QueryDecoderParam(
        Guid     Guid
        );

        /// <summary>
        /// Set decoder parameters
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="Length"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
    	int SetDecoderParam(
        Guid     Guid,
        uint     Length,
        IntPtr    Value
        );
    
        /// <summary>
        /// Get image property count
        /// </summary>
        /// <param name="numOfProperty"></param>
        /// <returns></returns>
    	int GetPropertyCount(
        out uint numOfProperty
        );

        /// <summary>
        /// Get selected image properties
        /// </summary>
        /// <param name="numOfProperty"></param>
        /// <param name="list"></param>
        /// <returns></returns>
    	int GetPropertyIdList(
        uint numOfProperty,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
        ImageTag[] list
        );

        /// <summary>
        /// Get the size of property data
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="size"></param>
        /// <returns></returns>
    	int GetPropertyItemSize(
        ImageTag propId, 
        out uint size
        );
    
        /// <summary>
        /// Get the property data
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="propSize"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
    	int GetPropertyItem(
        ImageTag propId,
        uint propSize,
        IntPtr buffer
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalBufferSize"></param>
        /// <param name="numProperties"></param>
        /// <returns></returns>
    	int GetPropertySize(
        out uint totalBufferSize,
		out uint numProperties
        );

    	int GetAllPropertyItems(
        uint totalBufferSize,
        uint numProperties,
        IntPtr pItems
        );

    	int RemovePropertyItem(
        ImageTag propId
        );

        /// <summary>
        /// Sets property on the image
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
    	int SetPropertyItem(
        PropertyItem item
        );

    }

    /// <summary>
    /// This interface allows an image source, such as an image decoder, and an image sink to exchange data. 
    /// The most important interaction between the source and the sink happens when they negotiate data transfer 
    /// parameters during the call to the IImageSink::BeginSink method
    /// </summary>
    [ComImport, Guid("327ABDAE-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IImageSink
    {
        /// <summary>
        /// Begin the sink process
        /// </summary>
        /// <param name="imageInfo"></param>
        /// <param name="subarea"></param>
        /// <returns></returns>
    	int BeginSink(
        out ImageInfo imageInfo,
        out RECT subarea
        );

        /// <summary>
        /// End the sink process
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
    	int EndSink(
        int  statusCode
        );

        /// <summary>
        /// Pass the color palette to the image sink
        /// </summary>
        /// <param name="palette"></param>
        /// <returns></returns>
    	int SetPalette(
        ref ColorPalette palette
        );

        /// <summary>
        /// Ask the sink to allocate pixel data buffer
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="lastPass"></param>
        /// <param name="bitmapData"></param>
        /// <returns></returns>
    	int GetPixelDataBuffer(
        RECT rect,
        PixelFormat pixelFormat,
        bool lastPass,
        out BitmapDataInternal bitmapData
        );

        /// <summary>
        /// Give the sink pixel data and release data buffer
        /// </summary>
        /// <param name="bitmapData"></param>
        /// <returns></returns>
    	int ReleasePixelDataBuffer(
        ref BitmapDataInternal bitmapData
        );

        /// <summary>
        /// Push pixel data
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="bitmapData"></param>
        /// <param name="lastPass"></param>
        /// <returns></returns>
    	int PushPixelData(
        RECT rect,
        ref BitmapDataInternal bitmapData,
        bool lastPass
        );

        /// <summary>
        /// Push raw image data
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufsize"></param>
        /// <returns></returns>
    	int PushRawData(
        IntPtr buffer,
        uint bufsize
        );

        /// <summary>
        /// This method is used by an image source to determine whether an image 
        /// sink should rotate an image and by how much it should rotate the image
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
    	int NeedTransform(
        out uint rotation
        );

        /// <summary>
        /// This method is used by an image source to determine whether an image sink can accept raw properties
        /// </summary>
        /// <returns></returns>
    	int NeedRawProperty(
        );
    
        /// <summary>
        /// This method is used by an image source to send raw information about an image to an image sink
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
    	int PushRawInfo(
        byte[] info
        );

        /// <summary>
        /// Retrieves a property as a blob
        /// </summary>
        /// <param name="uiTotalBufferSize"></param>
        /// <param name="ppBuffer"></param>
        /// <returns></returns>
    	int GetPropertyBuffer(
            int uiTotalBufferSize,
            IntPtr  ppBuffer
        );
    
    	int PushPropertyItems(
        uint              numOfItems,
        uint            uiTotalBufferSize,
        PropertyItem[]    item
        );
    }

    [ComImport, Guid("327ABDAA-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IBitmapImage
    {
        /// <summary>
        /// Get bitmap dimensions pixels
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
    	int GetSize(
        out Size size
        );

        /// <summary>
        /// Get bitmap pixel format
        /// </summary>
        /// <param name="pixelFormat"></param>
        /// <returns></returns>
    	int GetPixelFormatID(
        out PixelFormat pixelFormat
        );

        /// <summary>
        /// Access bitmap data the specified pixel format
        ///  must support at least PIXFMT_DONTCARE and
        ///  the canonical formats.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="flags"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="lockedBitmapData"></param>
        /// <returns></returns>
    	int LockBits(
        RECT rect,
        uint flags,
        PixelFormat pixelFormat,
        ref BitmapDataInternal lockedBitmapData
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockedBitmapData"></param>
        /// <returns></returns>
    	int UnlockBits(
        ref BitmapDataInternal lockedBitmapData
        ) ;

        /// <summary>
        /// Set/get palette associated with the bitmap image
        /// Do not use directly. Use <c>ImageUtils.GetBitmapPalette</c> instead
        /// </summary>
        /// <param name="palette"></param>
        /// <returns></returns>
    	int GetPalette(
        /*out ColorPalette palette*/
            out IntPtr pPalette
        );

        /// <summary>
        /// Do not use directly. Use <c>ImageUtils.SetBitmapPalette</c> instead
        /// </summary>
        /// <param name="palette"></param>
        /// <returns></returns>
    	int SetPalette(
        /*ref ColorPalette palette*/
            IntPtr pPalette
        );

    }

    /// <summary>
    /// 
    /// </summary>
    [ComImport, Guid("327ABDAF-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CLSCompliant(false)]
    public interface IBasicBitmapOps
    {
        /// <summary>
        /// Clone an area of the bitmap image
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outbmp"></param>
        /// <param name="bNeedCloneProperty"></param>
        /// <returns></returns>
        int Clone(
            RECT rect,
            out IBitmapImage outbmp,
            bool bNeedCloneProperty
            );

        /// <summary>
        /// Flip the bitmap image in x- and/or y-direction
        /// </summary>
        /// <param name="flipX"></param>
        /// <param name="flipY"></param>
        /// <param name="outbmp"></param>
        /// <returns></returns>
        int Flip(
            bool flipX,
            bool flipY,
            out IBitmapImage outbmp
            );


        /// <summary>
        /// Resize the bitmap image
        /// </summary>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="hints"></param>
        /// <param name="outbmp"></param>
        /// <returns></returns>
        int Resize(
            uint newWidth,
            uint newHeight,
            PixelFormat pixelFormat,
            InterpolationHint hints,
            out IBitmapImage outbmp
            );

        /// <summary>
        /// Rotate the bitmap image by the specified angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="hints"></param>
        /// <param name="outbmp"></param>
        /// <returns></returns>
        int Rotate(
            float angle,
            InterpolationHint hints,
            out IBitmapImage outbmp
            );

        /// <summary>
        /// Adjust the brightness of the bitmap image
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        int AdjustBrightness(
            float percent
            );


        /// <summary>
        /// Adjust the contrast of the bitmap image
        /// </summary>
        /// <param name="shadow"></param>
        /// <param name="highlight"></param>
        /// <returns></returns>
        int AdjustContrast(
            float shadow,
            float highlight
            );


        /// <summary>
        /// Adjust the gamma of the bitmap image
        /// </summary>
        /// <param name="gamma"></param>
        /// <returns></returns>
        int AdjustGamma(
            float gamma
            );
    }

    public class ColorPalette
    {
        // Methods
        internal ColorPalette():this(0)
        {
        }
        internal ColorPalette(int count)
        {
            this.entries = new Color[count];
            flags = 0;
        }
        internal void ConvertFromMemory(IntPtr memory)
        {
            flags = Marshal.ReadInt32(memory);
            int num1 = Marshal.ReadInt32((IntPtr)(((long)memory) + 4));
            entries = new Color[num1];
            for (int num2 = 0; num2 < num1; num2++)
            {
                int num3 = Marshal.ReadInt32((IntPtr)((((long)memory) + 8) + (num2 * 4)));
                this.entries[num2] = Color.FromArgb(num3);
            }
        }
        internal IntPtr ConvertToMemory()
        {
            IntPtr ptr1 = Marshal.AllocCoTaskMem((int)(4 * (2 + this.entries.Length)));
            Marshal.WriteInt32(ptr1, 0, this.flags);
            Marshal.WriteInt32((IntPtr)(((long)ptr1) + 4), 0, this.entries.Length);
            for (int num1 = 0; num1 < this.entries.Length; num1++)
            {
                Marshal.WriteInt32((IntPtr)(((long)ptr1) + (4 * (num1 + 2))), 0, this.entries[num1].ToArgb());
            }
            return ptr1;
        }

        // Properties
        public Color[] Entries
        {
            get
            {
                return this.entries;
            }
        }


        public int Flags
        {
            get
            {
                return this.flags;
            }
        }
 


        // Fields
        private Color[] entries;
        private int flags;
    }
 

    /// <summary>
    /// Property tag types
    /// </summary>
    public enum TAG_TYPE
    {
        /// <summary>
        /// 8-bit unsigned int
        /// </summary>
        BYTE = 1,           
        /// <summary>
        /// 8-bit byte containing one 7-bit ASCII code.
        /// NULL terminated.
        /// </summary>
        ASCII = 2,          
        /// <summary>
        /// 16-bit unsigned int
        /// </summary>
        SHORT = 3,         
        /// <summary>
        /// 32-bit unsigned int
        /// </summary>
        LONG = 4,           
        /// <summary>
        /// Two LONGs.  The first LONG is the numerator,
        /// the second LONG expresses the denominator.
        /// </summary>
        RATIONAL = 5,       
        /// <summary>
        /// 8-bit byte that can take any value depending
        /// on field definition
        /// </summary>
        UNDEFINED = 7,      
        /// <summary>
        /// 32-bit singed integer (2's complement notation)
        /// </summary>
        SLONG = 9,          
        /// <summary>
        /// Two SLONGs. First is numerator, second is the denominator
        /// </summary>
        SRATIONAL = 10,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapDataInternal
    {
        private int width;
        private int height;
        private int stride;
        private int pixelFormat;
        private IntPtr scan0;
        private int reserved;
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }
        public int Stride
        {
            get
            {
                return this.stride;
            }
            set
            {
                this.stride = value;
            }
        }
        public PixelFormat PixelFormat
        {
            get
            {
                return (PixelFormat)this.pixelFormat;
            }
            set
            {
                this.pixelFormat = (int)value;
            }
        }
        public IntPtr Scan0
        {
            get
            {
                return this.scan0;
            }
            set
            {
                this.scan0 = value;
            }
        }
        public int Reserved
        {
            get
            {
                return this.reserved;
            }
            set
            {
                this.reserved = value;
            }
        }
        //public BitmapDataInternal() { }
        public BitmapDataInternal(System.Drawing.Imaging.BitmapData bd) 
        {
            this.width = bd.Width;
            this.height = bd.Height;
            this.stride = bd.Stride;
            this.scan0 = bd.Scan0;
            this.pixelFormat = (int)bd.PixelFormat;
            reserved = 0;
        }
/*
        public static implicit operator BitmapData(BitmapDataInternal bdi)
        {
            Type t = typeof(BitmapData);
            BitmapData bd = new BitmapData();
            bd.Height = bdi.height;
            bd.Width = bdi.width;
            bd.Scan0 = bdi.scan0;
            bd.Stride = bdi.stride;
            bd.PixelFormat = bdi.PixelFormat;
            return bd;
        }
 */
    }
/*
    public enum PixelFormat
    {
        // Fields
        Alpha = 0x40000,
        Canonical = 0x200000,
        DontCare = 0,
        Extended = 0x100000,
        Format16bppArgb1555 = 0x61007,
        Format16bppGrayScale = 0x101004,
        Format16bppRgb555 = 0x21005,
        Format16bppRgb565 = 0x21006,
        Format1bppIndexed = 0x30101,
        Format24bppRgb = 0x21808,
        Format32bppArgb = 2498570,
        Format32bppPArgb = 0xe200b,
        Format32bppRgb = 0x22009,
        Format48bppRgb = 0x10300c,
        Format4bppIndexed = 0x30402,
        Format64bppArgb = 0x34400d,
        Format64bppPArgb = 0x1c400e,
        Format8bppIndexed = 0x30803,
        Gdi = 0x20000,
        Indexed = 0x10000,
        Max = 15,
        PAlpha = 0x80000,
        Undefined = 0
    }
*/

    [StructLayout(LayoutKind.Sequential)]
    public class RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
        public RECT(Rectangle r)
        {
            this.left = r.Left;
            this.top = r.Top;
            this.right = r.Right;
            this.bottom = r.Bottom;
        }
        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }
        public Size Size
        {
            get
            {
                return new Size(this.right - this.left, this.bottom - this.top);
            }
        }
        public Rectangle ToRectangle()
        {
            return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
        }
    }

    public enum BufferDisposalFlag
    {
        BufferDisposalFlagNone,
        BufferDisposalFlagGlobalFree,
        BufferDisposalFlagCoTaskMemFree,
        BufferDisposalFlagUnmapView
    };

    //public enum PixelFormatID
    //{
    //    PixelFormatIndexed = 0x00010000, // Indexes into a palette
    //    PixelFormatGDI = 0x00020000, // Is a GDI-supported format
    //    PixelFormatAlpha = 0x00040000, // Has an alpha component
    //    PixelFormatPAlpha = 0x00080000, // Pre-multiplied alpha
    //    PixelFormatExtended = 0x00100000, // Extended color 16 bits/channel
    //    PixelFormatCanonical = 0x00200000,

    //    PixelFormatUndefined = 0,
    //    PixelFormatDontCare = 0,

    //    PixelFormat1bppIndexed = (1 | (1 << 8) | PixelFormatIndexed | PixelFormatGDI),
    //    PixelFormat4bppIndexed = (2 | (4 << 8) | PixelFormatIndexed | PixelFormatGDI),
    //    PixelFormat8bppIndexed = (3 | (8 << 8) | PixelFormatIndexed | PixelFormatGDI),
    //    PixelFormat16bppGrayScale = (4 | (16 << 8) | PixelFormatExtended),
    //    PixelFormat16bppRGB555 = (5 | (16 << 8) | PixelFormatGDI),
    //    PixelFormat16bppRGB565 = (6 | (16 << 8) | PixelFormatGDI),
    //    PixelFormat16bppARGB1555 = (7 | (16 << 8) | PixelFormatAlpha | PixelFormatGDI),
    //    PixelFormat24bppRGB = (8 | (24 << 8) | PixelFormatGDI),
    //    PixelFormat32bppRGB = (9 | (32 << 8) | PixelFormatGDI),
    //    PixelFormat32bppARGB = (10 | (32 << 8) | PixelFormatAlpha | PixelFormatGDI | PixelFormatCanonical),
    //    PixelFormat32bppPARGB = (11 | (32 << 8) | PixelFormatAlpha | PixelFormatPAlpha | PixelFormatGDI),
    //    PixelFormat48bppRGB = (12 | (48 << 8) | PixelFormatExtended),
    //    PixelFormat64bppARGB = (13 | (64 << 8) | PixelFormatAlpha | PixelFormatCanonical | PixelFormatExtended),
    //    PixelFormat64bppPARGB = (14 | (64 << 8) | PixelFormatAlpha | PixelFormatPAlpha | PixelFormatExtended),
    //    PixelFormatMax = 15,
    //}

    public enum InterpolationHint
    {
        InterpolationHintDefault,
        InterpolationHintNearestNeighbor,
        InterpolationHintBilinear,
        InterpolationHintAveraging,
        InterpolationHintBicubic
    };

    public enum DecoderInitFlag
    {
        DecoderInitFlagNone = 0,

        // NOBLOCK indicates that the caller requires non-blocking
        // behavior.  This will be honored only by non-blocking decoders, that
        // is, decoders that don't have the IMGCODEC_BLOCKING_DECODE flag.

        DecoderInitFlagNoBlock = 0x0001,

        // Choose built-decoders first before looking at any
        // installed plugdecoders.

        DecoderInitFlagBuiltIn1st = 0x0002
    };

    /// <summary>
    /// Internal class
    /// </summary>
    public struct PropertyItem
    {
        // Methods
        //internal PropertyItem() { }

        // Properties
        public ImageTag Id{get{return this.id;}set{this.id = value;}}

        public int Len{ get { return this.len; } set { this.len = value; } }

        public TagType Type { get { return this.type; } set { this.type = value; } }

        public IntPtr Value { get { return this.value; } set { this.value = value; } }

        // Fields
        private ImageTag id;
        private int len;
        private TagType type;
        private IntPtr value;
    }


    [StructLayout(LayoutKind.Sequential)]
    public sealed class EncoderParameter : IDisposable
    {
        [MarshalAs(UnmanagedType.Struct)]
        private Guid parameterGuid;
        private int numberOfValues;
        private int parameterValueType;
        private IntPtr parameterValue;
        public void Dispose() { }
        //public IEncoder Encoder { get; set; }
        /*
        public EncoderParameterValueType Type { get; }
        public EncoderParameterValueType ValueType { get; }
        public int NumberOfValues { get; }
        private void Dispose(bool disposing);
        */
        /*
        public EncoderParameter(Encoder encoder, byte value);
        public EncoderParameter(Encoder encoder, byte value, bool undefined);
        public EncoderParameter(Encoder encoder, short value);
        public EncoderParameter(Encoder encoder, long value);
        public EncoderParameter(Encoder encoder, int numerator, int demoninator);
        public EncoderParameter(Encoder encoder, long rangebegin, long rangeend);
        public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2);
        public EncoderParameter(Encoder encoder, string value);
        public EncoderParameter(Encoder encoder, byte[] value);
        public EncoderParameter(Encoder encoder, byte[] value, bool undefined);
        public EncoderParameter(Encoder encoder, short[] value);
        public EncoderParameter(Encoder encoder, long[] value);
        public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator);
        public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend);
        public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2);
        public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value);
        private static IntPtr Add(IntPtr a, int b);
        private static IntPtr Add(int a, IntPtr b);
         * */
    }

    [CLSCompliant(false)]
    public struct ImageInfo
    {
        [MarshalAs(UnmanagedType.Struct)]
        public Guid RawDataFormat;
        public PixelFormat PixelFormat;
        public uint Width;
        public uint Height;
        public uint TileWidth;
        public uint TileHeight;
        public double Xdpi;
        public double Ydpi;
        public uint Flags;
    };

    public enum EncoderParameterValueType
    {
        // Fields
        ValueTypeAscii = 2,
        ValueTypeByte = 1,
        ValueTypeLong = 4,
        ValueTypeLongRange = 6,
        ValueTypeRational = 5,
        ValueTypeRationalRange = 8,
        ValueTypeShort = 3,
        ValueTypeUndefined = 7
    }

    public enum ImageFlags
    {
        ImageFlagsNone = 0,
        ImageFlagsScalable = 0x0001,
        ImageFlagsHasAlpha = 0x0002,
        ImageFlagsHasTranslucent = 0x0004,
        ImageFlagsPartiallyScalable = 0x0008,
        ImageFlagsColorSpaceRGB = 0x0010,
        ImageFlagsColorSpaceCMYK = 0x0020,
        ImageFlagsColorSpaceGRAY = 0x0040,
        ImageFlagsColorSpaceYCBCR = 0x0080,
        ImageFlagsColorSpaceYCCK = 0x0100,
        ImageFlagsHasRealDPI = 0x1000,
        ImageFlagsHasRealPixelSize = 0x2000,
        ImageFlagsReadOnly = 0x00010000,
        ImageFlagsCaching = 0x00020000,
        ImageFlagsValid = 0x00030000
    };

    public class InvalidEnumArgumentException : Exception
    {
        public InvalidEnumArgumentException(string argumentName, int invalidValue, Type enumClass)
            : base(String.Format("Invalid Enum Argument: Name: {0}, Value: {1}, Enum type:{2}", new object[3] { argumentName, invalidValue, enumClass.Name }))
        {
        }
 

    }

    /// <summary>
    /// Standard image format Guids
    /// </summary>
    public class ImageFormatGuid
    {
        static public readonly Guid IMGFMT_BMP = new Guid("b96b3cab-0728-11d3-9d7b-0000f81ef32e");
        static public readonly Guid IMGFMT_JPEG = new Guid("b96b3cae-0728-11d3-9d7b-0000f81ef32e");
        static public readonly Guid IMGFMT_PNG = new Guid("b96b3caf-0728-11d3-9d7b-0000f81ef32e");
        static public readonly Guid IMGFMT_GIF = new Guid("b96b3cb0-0728-11d3-9d7b-0000f81ef32e");
    }

}
