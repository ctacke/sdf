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
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace OpenNETCF.Windows.Forms
{
  internal class StaticMethods
  {
    /// <summary>
    /// Determins whether the control is in design mode or not
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    public static bool IsDesignMode(Control control)
    {
      return IsDesignTime;
    }

    public static bool IsDesignMode(System.ComponentModel.Component component)
    {
      return IsDesignTime;

    }

    /// <summary>
    /// Determine if this instance is running against .NET Framework
    /// </summary>
    private static bool IsDesignTime
    {
      get
      {
        // Determine if this instance is running against .NET Framework by using the MSCoreLib PublicKeyToken
        System.Reflection.Assembly mscorlibAssembly = typeof(int).Assembly;
        if ((mscorlibAssembly != null))
        {
          if (mscorlibAssembly.FullName.ToUpper().EndsWith("B77A5C561934E089"))
          {
            return true;
          }
        }
        return false;
      }
    }
    /// <summary>
    /// Determine if this instance is running against .NET Compact Framework
    /// </summary>
    private static bool IsRunTime
    {
      get
      {
        // Determine if this instance is running against .NET Compact Framework by using the MSCoreLib PublicKeyToken
        System.Reflection.Assembly mscorlibAssembly = typeof(int).Assembly;
        if ((mscorlibAssembly != null))
        {
          if (mscorlibAssembly.FullName.ToUpper().EndsWith("969DB8053D3322AC"))
          {
            return true;
          }
        }
        return false;
      }
    }
    /// <summary>
    /// Produces a number to use for scaling graphics
    /// </summary>
    /// <param name="g">Graphics object to use</param>
    /// <returns></returns>
    public static float Scale(Graphics g)
    {
      return g.DpiX / 96;
    }

  }
}