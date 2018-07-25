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
using OpenNETCF.IO;

namespace StreamDriverSample
{
  public class FileDriver : StreamInterfaceDriver
  {
    public const int FIL_IOCTL_CREATE_FILE = 1;
    public const int FIL_IOCTL_GET_OPEN_FILE_NAME = 2;
    public const int FIL_IOCTL_GET_FILE_ATTRIBS = 3;
    public const int FIL_IOCTL_DO_NOTHING = 4;

    public FileDriver()
      : base("FIL1:")
    {
    }

    public new void Open(System.IO.FileAccess access, System.IO.FileShare share)
    {
      base.Open(access, share);
    }

    public new void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData, out int bytesReturned)
    {
      base.DeviceIoControl(controlCode, inData, outData, out bytesReturned);
    }

    public new void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData)
    {
      base.DeviceIoControl(controlCode, inData, outData);
    }

    public new void Close()
    {
      base.Close();
    }
  }
}
