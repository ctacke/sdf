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
using System.Drawing;

namespace OpenNETCF.Drawing.Imaging
{
  public class ColorPalette
  {
    // Methods
    internal ColorPalette()
      : this(0)
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
}
