using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace OpenNETCF.Drawing.Imaging
{ 
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
