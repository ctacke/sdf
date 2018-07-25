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
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsMobile
{
  internal sealed class RadioDevice
  {
    private const int DEV_NAME_PTR_OFFSET = 0;
    private const int DISP_NAME_PTR_OFFSET = 4;
    private const int STATE_OFFSET = 8;
    private const int DESIRED_OFFSET = 12;
    private const int TYPE_OFFSET = 16;
    private const int NEXT_OFFSET = 20;

    public RadioDevice(IntPtr pDeviceList, int offset)
    {
      Pointer = new IntPtr(pDeviceList.ToInt32() + offset);
    }

    public IntPtr Pointer { get; private set; }

    private int GetInt(int offset)
    {
      int[] data = new int[1];
      Marshal.Copy(new IntPtr(Pointer.ToInt32() + offset), data, 0, 1);
      return data[0];
    }

    public bool ActualState
    {
      get { return GetInt(STATE_OFFSET) != 0; }
    }

    public bool DesiredState
    {
      get { return GetInt(DESIRED_OFFSET) != 0; }
    }

    public RadioType RadioType
    {
      get { return (RadioType)GetInt(TYPE_OFFSET); }
    }

    public string DeviceName
    {
      get { return Marshal.PtrToStringUni(new IntPtr(GetInt(DEV_NAME_PTR_OFFSET)));  }
    }

    public string DisplayName
    {
      get { return Marshal.PtrToStringUni(new IntPtr(GetInt(DISP_NAME_PTR_OFFSET)));  }
    }

    public IntPtr Next
    {
      get { return new IntPtr(GetInt(NEXT_OFFSET)); }
    }

    [DllImport("coredll.dll", SetLastError = true)]
    private static extern int LocalFree(IntPtr hMem);
  }
}
