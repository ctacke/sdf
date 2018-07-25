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



using System.Drawing;
using System;

namespace OpenNETCF.Drawing
{
  public enum RotationAngle
  {
    Zero = 0,
    Clockwise90 = 90,
    Clockwise180 = 180,
    Clockwise270 = 270,
    CounterClockwise90 = Clockwise270
  }

  [Flags]
  public enum FlipAxis
  {
    None = 0,
    X = 1,
    Y = 2
  }

  /// <summary>
  /// Specifies the direction of an image's rotation and the axis used to flip the image.
  /// </summary>
  [Obsolete("Deprecated.  Consider using the FlipAxis and RotationAngle flags.", false)]
  // Deprecated in v 2.3 with no error
  public enum RotateFlipType
  {
    /// <summary>
    /// Specifies a 180-degree rotation followed by a horizontal and vertical flip.
    /// </summary>
    Rotate180FlipXY = 0,
    /// <summary>
    /// Specifies no rotation and no flipping.
    /// </summary>
    RotateNoneFlipNone = 0,
    /// <summary>
    /// Specifies a 270-degree rotation followed by a horizontal and vertical flip.
    /// </summary>
    Rotate270FlipXY = 1,
    /// <summary>
    /// Specifies a 90-degree rotation without flipping.
    /// </summary>
    Rotate90FlipNone = 1,
    /// <summary>
    /// Specifies a 180-degree rotation without flipping.
    /// </summary>
    Rotate180FlipNone = 2,
    /// <summary>
    /// Specifies no rotation followed by a horizontal and vertical flip.
    /// </summary>
    RotateNoneFlipXY = 2,
    /// <summary>
    /// Specifies a 270-degree rotation without flipping.
    /// </summary>
    Rotate270FlipNone = 3,
    /// <summary>
    /// Specifies a 90-degree rotation followed by a horizontal and vertical flip.
    /// </summary>
    Rotate90FlipXY = 3,
    /// <summary>
    /// Specifies a 180-degree rotation followed by a vertical flip.
    /// </summary>
    Rotate180FlipY = 4,
    /// <summary>
    /// Specifies no rotation followed by a horizontal flip.
    /// </summary>
    RotateNoneFlipX = 4,
    /// <summary>
    /// Specifies a 90-degree rotation followed by a horizontal flip.
    /// </summary>
    Rotate90FlipX = 5,
    /// <summary>
    /// Specifies a 270-degree rotation followed by a vertical flip.
    /// </summary>
    Rotate270FlipY = 5,
    /// <summary>
    /// Specifies no rotation followed by a vertical flip.
    /// </summary>
    RotateNoneFlipY = 6,
    /// <summary>
    /// Specifies a 180-degree rotation followed by a horizontal flip.
    /// </summary>
    Rotate180FlipX = 6,
    /// <summary>
    /// Specifies a 90-degree rotation followed by a vertical flip.
    /// </summary>
    Rotate90FlipY = 7,
    /// <summary>
    /// Specifies a 270-degree rotation followed by a horizontal flip.
    /// </summary>
    Rotate270FlipX = 7,
  }
}