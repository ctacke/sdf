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
