#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion




using System;

namespace OpenNETCF.Win32
{
  /// <summary>
  /// Wrapper for WIN32 <b>DIBSECTION</b> structure
  /// </summary>
  public class DibSection
  {
    /// <summary>
    /// Specifies the bitmap type; set to zero.
    /// </summary>
    public int bmType;
    /// <summary>
    /// Specifies the width, in pixels, of the bitmap.
    /// The width must be greater than zero.
    /// </summary>
    public int bmWidth;
    /// <summary>
    /// Specifies the height, in pixels, of the bitmap.
    /// The height must be greater than zero.
    /// </summary>
    public int bmHeight;
    /// <summary>
    /// Specifies the number of bytes in each scan line.
    /// This value must be divisible by 2, because the system assumes that the bit values of a bitmap form an array that is word aligned.
    /// </summary>
    public int bmWidthBytes;
    /// <summary>
    /// Specifies the count of color planes.
    /// </summary>
    public short bmPlanes;
    /// <summary>
    /// Specifies the number of bits required to indicate the color of a pixel.
    /// </summary>
    public short bmBitsPixel;
    /// <summary>
    /// Pointer to the location of the bit values for the bitmap.
    /// The bmBits member must be a long pointer to an array of character (1-byte) values.
    /// </summary>
    public IntPtr bmBits;
    /// <summary>
    /// Specifies the number of bytes required by the structure.
    /// </summary>
    public int biSize;
    /// <summary>
    /// Specifies the width of the bitmap, in pixels.
    /// </summary>
    public int biWidth;
    /// <summary>
    /// Specifies the height of the bitmap, in pixels. If biHeight is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner. If biHeight is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
    /// If biHeight is negative, indicating a top-down DIB, biCompression must be either BI_RGB or BI_BITFIELDS. Top-down DIBs cannot be compressed. 
    /// </summary>
    public int biHeight;
    /// <summary>
    /// Specifies the number of planes for the target device. This value must be set to 1.
    /// </summary>
    public short biPlanes; 
    /// <summary>
    /// Specifies the number of bits-per-pixel. The biBitCount member of the BITMAPINFOHEADER structure determines the number of bits that define each pixel and the maximum number of colors in the bitmap.
    /// </summary>
    public short biBitCount;
    /// <summary>
    /// Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).
    /// </summary>
    public int biCompression;
    /// <summary>
    /// Specifies the size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps.
    /// </summary>
    public int biSizeImage;
    /// <summary>
    /// Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap. An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.
    /// </summary>
    public int biXPelsPerMeter;
    /// <summary>
    /// Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.
    /// </summary>
    public int biYPelsPerMeter;
    /// <summary>
    /// Specifies the number of color indexes in the color table that are actually used by the bitmap. If this value is zero, the bitmap uses the maximum number of colors corresponding to the value of the biBitCount member for the compression mode specified by biCompression.
    /// If biClrUsed is nonzero and the biBitCount member is less than 16, the biClrUsed member specifies the actual number of colors the graphics engine or device driver accesses. If biBitCount is 16 or greater, the biClrUsed member specifies the size of the color table used to optimize performance of the system color palettes. If biBitCount equals 16 or 32, the optimal color palette starts immediately following the three DWORD masks. 
    /// </summary>
    public int biClrUsed;
    /// <summary>
    /// Specifies the number of color indexes that are required for displaying the bitmap. If this value is zero, all colors are required.
    /// </summary>
    public int biClrImportant;
    /// <summary>
    /// Specifies three color masks for the DIB. This field is only valid when the BitCount member of the BITMAPINFOHEADER structure has a value greater than 8. Each color mask indicates the bits that are used to encode one of the three color channels (red, green, and blue).
    /// </summary>
    public int dsBitfields0;
    /// <summary>
    /// 
    /// </summary>
    public int dsBitfields1;
    /// <summary>
    /// 
    /// </summary>
    public int dsBitfields2;
    /// <summary>
    /// Contains a handle to the file mapping object that the CreateDIBSection function used to create the DIB. If CreateDIBSection was called with a NULL value for its hSection parameter, causing the system to allocate memory for the bitmap, the dshSection member will be NULL.
    /// </summary>
    public IntPtr dshSection;
    /// <summary>
    /// Specifies the offset to the bitmap's bit values within the file mapping object referenced by dshSection. If dshSection is NULL, the dsOffset value has no meaning.
    /// </summary>
    public int dsOffset;
  }
}
