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

namespace OpenNETCF.Drawing.Imaging
{
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
}
