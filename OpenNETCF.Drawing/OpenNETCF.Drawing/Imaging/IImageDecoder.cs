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

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace OpenNETCF.Drawing.Imaging
{
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
      Guid Guid
      );

    /// <summary>
    /// Set decoder parameters
    /// </summary>
    /// <param name="Guid"></param>
    /// <param name="Length"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    int SetDecoderParam(
      Guid Guid,
      uint Length,
      IntPtr Value
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
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
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
}
