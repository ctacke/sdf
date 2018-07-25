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

namespace OpenNETCF.WindowsCE
{
  /// <summary>
  /// Device family class to pass to the constructor of the DeviceStatusMonitor
  /// </summary>
  public enum DeviceClass
  {
    Unknown = -1,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the BLOCK_DRIVER_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute("{00000000-0000-0000-0000-000000000000}")]
    Any,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the BLOCK_DRIVER_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._BLOCK_DRIVER_GUID)]
    BlockDriver,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the STORE_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._STORE_MOUNT_GUID)]
    FileStore,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the FATFS_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._FATFS_MOUNT_GUID)]
    FATFileSystem,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the CDFS_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._CDFS_MOUNT_GUID)]
    CDFileSystem,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the UDFS_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._UDFS_MOUNT_GUID)]
    UDFileSystem,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the CDDA_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute(DeviceStatusMonitor._CDDA_MOUNT_GUID)]
    CDDAFileSystem,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the DEVCLASS_STREAM_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute("{f8a6ba98-087a-43ac-a9d8-b7f13c5bae31}")]
    StreamDevice,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the DEVCLASS_KEYBOARD_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute("{CBE6DDF2-F5D4-4e16-9F61-4CCC0B6695F3}")]
    Keyboard,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the PMCLASS_GENERIC_DEVICE class identifier
    /// </summary>
    [DeviceClassAtrribute("{A32942B7-920C-486b-B0E6-92A702A99B35}")]
    PMGeneric,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the PMCLASS_NDIS_MINIPORT class identifier
    /// </summary>
    [DeviceClassAtrribute("{98C5250D-C29A-4985-AE5F-AFE5367E5006}")]
    PMMiniport,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the PMCLASS_BLOCK_DEVICE class identifier
    /// </summary>
    [DeviceClassAtrribute("{8DD679CE-8AB4-43c8-A14A-EA4963FAA715}")]
    PMBlock,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the PMCLASS_DISPLAY class identifier
    /// </summary>
    [DeviceClassAtrribute("{EB91C7C9-8BF6-4a2d-9AB8-69724EED97D1}")]
    PMDisplay,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the FSD_MOUNT_GUID class identifier
    /// </summary>
    [DeviceClassAtrribute("{8C77EDE8-47B9-45ae-8BC9-86E7B8D00EDD}")]
    FileSystem,
    /// <summary>
    /// Initializes a DeviceStatusMonitor class with the DMCLASS_PROTECTEDBUSNAMESPACE class identifier
    /// </summary>
    [DeviceClassAtrribute("{6F40791D-300E-44E4-BC38-E0E63CA8375C}")]
    ProtectedBus,
    /// <summary>
    /// A PCMCIA Card Services device
    /// </summary>
    [DeviceClassAtrribute("{6BEAB08A-8914-42fd-B33F-61968B9AAB32}")]
    PCMCIACard,
    /// <summary>
    /// A Serial Port device
    /// </summary>
    [DeviceClassAtrribute("{CC5195AC-BA49-48a0-BE17-DF6D1B0173DD}")]
    Serial,
  }
}
