using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenNETCF.Drawing.Imaging
{
    /// <summary>
    /// Image property ID tags (PROPID's from the EXIF tags)
    /// </summary>
    public enum ImageTag
    {
        /// <summary>
        /// 
        /// </summary>
        TAG_EXIF_IFD = 0x8769,
        /// <summary>
        /// 
        /// </summary>
        TAG_GPS_IFD = 0x8825,
        /// <summary>
        /// 
        /// </summary>
        TAG_NEW_SUBFILE_TYPE = 0x00FE,
        /// <summary>
        /// 
        /// </summary>
        TAG_SUBFILE_TYPE = 0x00FF,
        /// <summary>
        /// 
        /// </summary>
        TAG_IMAGE_WIDTH = 0x0100,
        /// <summary>
        /// 
        /// </summary>
        TAG_IMAGE_HEIGHT = 0x0101,
        /// <summary>
        /// 
        /// </summary>
        TAG_BITS_PER_SAMPLE = 0x0102,
        /// <summary>
        /// 
        /// </summary>
        TAG_COMPRESSION = 0x0103,
        /// <summary>
        /// 
        /// </summary>
        TAG_PHOTOMETRIC_INTERP = 0x0106,
        /// <summary>
        /// 
        /// </summary>
        TAG_THRESH_HOLDING = 0x0107,
        /// <summary>
        /// 
        /// </summary>
        TAG_CELL_WIDTH = 0x0108,
        /// <summary>
        /// 
        /// </summary>
        TAG_CELL_HEIGHT = 0x0109,
        /// <summary>
        /// 
        /// </summary>
        TAG_FILL_ORDER = 0x010A,
        /// <summary>
        /// 
        /// </summary>
        TAG_DOCUMENT_NAME = 0x010D,
        /// <summary>
        /// 
        /// </summary>
        TAG_IMAGE_DESCRIPTION = 0x010E,
        /// <summary>
        /// 
        /// </summary>
        TAG_EQUIP_MAKE = 0x010F,
        /// <summary>
        /// 
        /// </summary>
        TAG_EQUIP_MODEL = 0x0110,
        /// <summary>
        /// 
        /// </summary>
        TAG_STRIP_OFFSETS = 0x0111,
        /// <summary>
        /// 
        /// </summary>
        TAG_ORIENTATION = 0x0112,
        /// <summary>
        /// 
        /// </summary>
        TAG_SAMPLES_PER_PIXEL = 0x0115,
        /// <summary>
        /// 
        /// </summary>
        TAG_ROWS_PER_STRIP = 0x0116,
        /// <summary>
        /// 
        /// </summary>
        TAG_STRIP_BYTES_COUNT = 0x0117,
        /// <summary>
        /// 
        /// </summary>
        TAG_MIN_SAMPLE_VALUE = 0x0118,
        /// <summary>
        /// 
        /// </summary>
        TAG_MAX_SAMPLE_VALUE = 0x0119,
        /// <summary>
        /// 
        /// </summary>
        TAG_X_RESOLUTION = 0x011A,  // Image resolution width direction
        /// <summary>
        /// 
        /// </summary>
        TAG_Y_RESOLUTION = 0x011B,  // Image resolution height direction
        /// <summary>
        /// 
        /// </summary>
        TAG_PLANAR_CONFIG = 0x011C,  // Image data arrangement
        /// <summary>
        /// 
        /// </summary>
        TAG_PAGE_NAME = 0x011D,
        /// <summary>
        /// 
        /// </summary>
        TAG_X_POSITION = 0x011E,
        /// <summary>
        /// 
        /// </summary>
        TAG_Y_POSITION = 0x011F,
        /// <summary>
        /// 
        /// </summary>
        TAG_FREE_OFFSET = 0x0120,
        /// <summary>
        /// 
        /// </summary>
        TAG_FREE_BYTE_COUNTS = 0x0121,
        /// <summary>
        /// 
        /// </summary>
        TAG_GRAY_RESPONSE_UNIT = 0x0122,
        /// <summary>
        /// 
        /// </summary>
        TAG_GRAY_RESPONSE_CURVE = 0x0123,
        /// <summary>
        /// 
        /// </summary>
        TAG_T4_OPTION = 0x0124,
        /// <summary>
        /// 
        /// </summary>
        TAG_T6_OPTION = 0x0125,
        /// <summary>
        /// 
        /// </summary>
        TAG_RESOLUTION_UNIT = 0x0128,  // Unit of X and Y resolution
        /// <summary>
        /// 
        /// </summary>
        TAG_PAGE_NUMBER = 0x0129,
        /// <summary>
        /// 
        /// </summary>
        TAG_TRANSFER_FUNCTION = 0x012D,
        /// <summary>
        /// 
        /// </summary>
        TAG_SOFTWARE_USED = 0x0131,
        /// <summary>
        /// 
        /// </summary>
        TAG_DATE_TIME = 0x0132,
        /// <summary>
        /// 
        /// </summary>
        TAG_ARTIST = 0x013B,
        /// <summary>
        /// 
        /// </summary>
        TAG_HOST_COMPUTER = 0x013C,
        /// <summary>
        /// 
        /// </summary>
        TAG_PREDICTOR = 0x013D,
        /// <summary>
        /// 
        /// </summary>
        TAG_WHITE_POINT = 0x013E,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRIMAY_CHROMATICS = 0x013F,
        /// <summary>
        /// 
        /// </summary>
        TAG_COLOR_MAP = 0x0140,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_HINTS = 0x0141,
        /// <summary>
        /// 
        /// </summary>
        TAG_TILE_WIDTH = 0x0142,
        /// <summary>
        /// 
        /// </summary>
        TAG_TILE_LENGTH = 0x0143,
        /// <summary>
        /// 
        /// </summary>
        TAG_TILE_OFFSET = 0x0144,
        /// <summary>
        /// 
        /// </summary>
        TAG_TILE_BYTE_COUNTS = 0x0145,
        /// <summary>
        /// 
        /// </summary>
        TAG_INK_SET = 0x014C,
        /// <summary>
        /// 
        /// </summary>
        TAG_INK_NAMES = 0x014D,
        /// <summary>
        /// 
        /// </summary>
        TAG_NUMBER_OF_INKS = 0x014E,
        /// <summary>
        /// 
        /// </summary>
        TAG_DOT_RANGE = 0x0150,
        /// <summary>
        /// 
        /// </summary>
        TAG_TARGET_PRINTER = 0x0151,
        /// <summary>
        /// 
        /// </summary>
        TAG_EXTRA_SAMPLES = 0x0152,
        /// <summary>
        /// 
        /// </summary>
        TAG_SAMPLE_FORMAT = 0x0153,
        /// <summary>
        /// 
        /// </summary>
        TAG_SMIN_SAMPLE_VALUE = 0x0154,
        /// <summary>
        /// 
        /// </summary>
        TAG_SMAX_SAMPLE_VALUE = 0x0155,
        /// <summary>
        /// 
        /// </summary>
        TAG_TRANSFER_RANGE = 0x0156,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_PROC = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_INTER_FORMAT = 0x0201,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_INTER_LENGTH = 0x0202,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_RESTART_INTERVAL = 0x0203,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_LOSSLESS_PREDICTORS = 0x0205,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_POINT_TRANSFORMS = 0x0206,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_Q_TABLES = 0x0207,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_DC_TABLES = 0x0208,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_AC_TABLES = 0x0209,
        /// <summary>
        /// 
        /// </summary>
        TAG_YCbCr_COEFFICIENTS = 0x0211,
        /// <summary>
        /// 
        /// </summary>
        TAG_YCbCr_SUBSAMPLING = 0x0212,
        /// <summary>
        /// 
        /// </summary>
        TAG_YCbCr_POSITIONING = 0x0213,
        /// <summary>
        /// 
        /// </summary>
        TAG_REF_BLACK_WHITE = 0x0214,
        /// <summary>
        /// 
        /// </summary>
        TAG_ICC_PROFILE = 0x8773,      // This TAG is defined by ICC
        /// <summary>
        /// 
        /// </summary>
        TAG_GAMMA = 0x0301,
        /// <summary>
        /// 
        /// </summary>
        TAG_ICC_PROFILE_DESCRIPTOR = 0x0302,
        /// <summary>
        /// 
        /// </summary>
        TAG_SRGB_RENDERING_INTENT = 0x0303,
        /// <summary>
        /// 
        /// </summary>
        TAG_IMAGE_TITLE = 0x0320,
        /// <summary>
        /// 
        /// </summary>
        TAG_COPYRIGHT = 0x8298,

        // Extra TAGs (Like Adobe Image Information tags etc.)

        /// <summary>
        /// 
        /// </summary>
        TAG_RESOLUTION_X_UNIT = 0x5001,
        /// <summary>
        /// 
        /// </summary>
        TAG_RESOLUTION_Y_UNIT = 0x5002,
        /// <summary>
        /// 
        /// </summary>
        TAG_RESOLUTION_X_LENGTH_UNIT = 0x5003,
        /// <summary>
        /// 
        /// </summary>
        TAG_RESOLUTION_Y_LENGTH_UNIT = 0x5004,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRINT_FLAGS = 0x5005,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRINT_FLAGS_VERSION = 0x5006,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRINT_FLAGS_CROP = 0x5007,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRINT_FLAGS_BLEEDWIDTH = 0x5008,
        /// <summary>
        /// 
        /// </summary>
        TAG_PRINT_FLAGS_BLEEDWIDTHSCALE = 0x5009,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_LPI = 0x500A,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_LPI_UNIT = 0x500B,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_DEGREE = 0x500C,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_SHAPE = 0x500D,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_MISC = 0x500E,
        /// <summary>
        /// 
        /// </summary>
        TAG_HALFTONE_SCREEN = 0x500F,
        /// <summary>
        /// 
        /// </summary>
        TAG_JPEG_QUALITY = 0x5010,
        /// <summary>
        /// 
        /// </summary>
        TAG_GRID_SIZE = 0x5011,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_FORMAT = 0x5012,  // 1 = JPEG, 0 = RAW RGB
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_WIDTH = 0x5013,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_HEIGHT = 0x5014,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_COLORDEPTH = 0x5015,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_PLANES = 0x5016,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_RAWBYTES = 0x5017,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_SIZE = 0x5018,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_COMPRESSED_SIZE = 0x5019,
        /// <summary>
        /// 
        /// </summary>
        TAG_COLORTRANSFER_FUNCTION = 0x501A,
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_DATA = 0x501B,  // RAW thumbnail bits JPEG
        // format or RGB format depends
        // on TAG_THUMBNAIL_FORMAT

        // Thumbnail related TAGs

        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_IMAGE_WIDTH = 0x5020,  // Thumbnail width
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_IMAGE_HEIGHT = 0x5021,  // Thumbnail height
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_BITS_PER_SAMPLE = 0x5022,  // Number of bits per component
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_COMPRESSION = 0x5023,  // Compression Scheme
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_PHOTOMETRIC_INTERP = 0x5024, // Pixel composition
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_IMAGE_DESCRIPTION = 0x5025,  // Image Tile
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_EQUIP_MAKE = 0x5026,  // Manufacturer of Image Input
        // equipment
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_EQUIP_MODEL = 0x5027,  // Model of Image input
        // equipment
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_STRIP_OFFSETS = 0x5028,  // Image data location
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_ORIENTATION = 0x5029,  // Orientation of image
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_SAMPLES_PER_PIXEL = 0x502A,  // Number of components
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_ROWS_PER_STRIP = 0x502B,  // Number of rows per strip
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_STRIP_BYTES_COUNT = 0x502C,  // Bytes per compressed strip
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_RESOLUTION_X = 0x502D,  // Resolution width direction
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_RESOLUTION_Y = 0x502E,  // Resolution height direc
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_PLANAR_CONFIG = 0x502F,  // Image data arrangement
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_RESOLUTION_UNIT = 0x5030,  // Unit of X and Y Resolution
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_TRANSFER_FUNCTION = 0x5031,  // Transfer function
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_SOFTWARE_USED = 0x5032,  // Software used
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_DATE_TIME = 0x5033,  // File change date and time
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_ARTIST = 0x5034,  // Person who created the image
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_WHITE_POINT = 0x5035,  // White point chromaticity
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_PRIMAY_CHROMATICS = 0x5036,  // Chromaticities of primaries
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_YCbCr_COEFFICIENTS = 0x5037, // Color space transformation
        // coefficients
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_YCbCr_SUBSAMPLING = 0x5038,  // Subsampling ratio of Y to C
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_YCbCr_POSITIONING = 0x5039,  // Y and C position
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_REF_BLACK_WHITE = 0x503A,  // Pair of black and white
        // reference values
        /// <summary>
        /// 
        /// </summary>
        TAG_THUMBNAIL_COPYRIGHT = 0x503B,  // CopyRight holder

        // Special JPEG internal values

        /// <summary>
        /// 
        /// </summary>
        TAG_LUMINANCE_TABLE = 0x5090,
        /// <summary>
        /// 
        /// </summary>
        TAG_CHROMINANCE_TABLE = 0x5091,

        // GIF image

        /// <summary>
        /// 
        /// </summary>
        TAG_FRAMEDELAY = 0x5100,
        /// <summary>
        /// 
        /// </summary>
        TAG_LOOPCOUNT = 0x5101,

        // PNG Image

        /// <summary>
        /// 
        /// </summary>
        TAG_PIXEL_UNIT = 0x5110,  // Unit specifier for pixel/unit
        /// <summary>
        /// 
        /// </summary>
        TAG_PIXEL_PER_UNIT_X = 0x5111,  // Pixels per unit X
        /// <summary>
        /// 
        /// </summary>
        TAG_PIXEL_PER_UNIT_Y = 0x5112,  // Pixels per unit Y
        /// <summary>
        /// 
        /// </summary>
        TAG_PALETTE_HISTOGRAM = 0x5113,  // Palette histogram

        // EXIF specific tag

        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_EXPOSURE_TIME = 0x829A,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_F_NUMBER = 0x829D,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_EXPOSURE_PROG = 0x8822,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SPECTRAL_SENSE = 0x8824,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_ISO_SPEED = 0x8827,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_OECF = 0x8828,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_VER = 0x9000,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_D_T_ORIG = 0x9003, // Date & time of original
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_D_T_DIGITIZED = 0x9004, // Date & time of digital data generation
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_COMP_CONFIG = 0x9101,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_COMP_BPP = 0x9102,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SHUTTER_SPEED = 0x9201,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_APERATURE = 0x9202,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_BRIGHTNESS = 0x9203,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_EXPOSURE_BIAS = 0x9204,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_MAX_APERATURE = 0x9205,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SUBJECT_DIST = 0x9206,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_METERING_MODE = 0x9207,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_LIGHT_SOURCE = 0x9208,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FLASH = 0x9209,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FOCAL_LENGTH = 0x920A,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_MAKER_NOTE = 0x927C,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_USER_COMMENT = 0x9286,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_D_T_SUBSEC = 0x9290,  // Date & Time subseconds
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_D_T_ORIG_SS = 0x9291,  // Date & Time original subseconds
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_D_T_DIG_SS = 0x9292,  // Date & TIme digitized subseconds
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FPX_VER = 0xA000,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_COLOR_SPACE = 0xA001,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_PIX_X_DIM = 0xA002,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_PIX_Y_DIM = 0xA003,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_RELATED_WAV = 0xA004,  // related sound file
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_INTEROP = 0xA005,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FLASH_ENERGY = 0xA20B,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SPATIAL_FR = 0xA20C,  // Spatial Frequency Response
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FOCAL_X_RES = 0xA20E,  // Focal Plane X Resolution
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FOCAL_Y_RES = 0xA20F,  // Focal Plane Y Resolution
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FOCAL_RES_UNIT = 0xA210,  // Focal Plane Resolution Unit
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SUBJECT_LOC = 0xA214,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_EXPOSURE_INDEX = 0xA215,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SENSING_METHOD = 0xA217,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_FILE_SOURCE = 0xA300,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_SCENE_TYPE = 0xA301,
        /// <summary>
        /// 
        /// </summary>
        EXIF_TAG_CFA_PATTERN = 0xA302,
    }

    /// <summary>
    /// Image tyge type
    /// </summary>
    public enum TagType: short
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
        /// the second LONG expresses the denomintor. 
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
        /// Two SLONGs. First is numerator, second is denominator
        /// </summary>
        SRATIONAL = 10,  
    }

}