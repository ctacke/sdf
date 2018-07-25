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
using System.IO;
using System.Runtime.InteropServices;

namespace StreamDriverSample
{
  class Program
  {
    static void Main(string[] args)
    {
      Register();

      Load();

      try
      {
        FileDriver driver = new FileDriver();

        // open the driver
        driver.Open(FileAccess.ReadWrite, FileShare.ReadWrite);

        try
        {
          // create a test file
          string fileName = @"\TestFile.txt";
          byte[] nameBytes = Encoding.Unicode.GetBytes(fileName + "\0");
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_CREATE_FILE, nameBytes, null);

          // verify its creation
          if (!File.Exists(fileName))
          {
          }

          // get the filename back
          byte[] outNameBytes = new byte[260];
          int returned = 0;
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_OPEN_FILE_NAME, null, outNameBytes, out returned);

          //        Assert.AreEqual<int>(nameBytes.Length, returned);
          //        Assert.AreEqual<string>(fileName, Encoding.Unicode.GetString(outNameBytes, 0, returned).TrimEnd('\0'));

          byte[] attribBytes = new byte[4];
          driver.DeviceIoControl(FileDriver.FIL_IOCTL_GET_FILE_ATTRIBS, nameBytes, attribBytes, out returned);
          int returnedAttribs = BitConverter.ToInt32(attribBytes, 0);
          int expectedAttribs = GetFileAttributes(fileName);

          driver.DeviceIoControl(FileDriver.FIL_IOCTL_DO_NOTHING, null, null);
        }
        finally
        {
          driver.Close();
          driver.Dispose();
        }
      }
      finally
      {
        Unload();
      }
    }
    [DllImport("FileDriver.dll")]
    private static extern void Register();
    [DllImport("FileDriver.dll")]
    private static extern void Load();
    [DllImport("FileDriver.dll")]
    private static extern void Unload();
    [DllImport("coredll.dll")]
    private static extern int GetFileAttributes(string fileName);
  }
}
