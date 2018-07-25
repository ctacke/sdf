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

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// DEVMODE struct used with ChangeDisplaySettingsEx
    /// </summary>
    internal class DEVMODE
    {
        public const short Size = 192;
        /*WCHAR dmDeviceName[CCHDEVICENAME]; 
        short dmSpecVersion; 
        short dmDriverVersion; 
        short dmSize; 
        short dmDriverExtra; 
        int dmFields; 
        short dmOrientation; 
        short dmPaperSize; 
        short dmPaperLength; 
        short dmPaperWidth; 
        short dmScale; 
        short dmCopies; 
        short dmDefaultSource; 
        short dmPrintQuality; 
        short dmColor; 
        short dmDuplex; 
        short dmYResolution; 
        short dmTTOption; 
        short dmCollate; 
        int dmFormName; 
        short dmLogPixels; 
        int dmBitsPerPel; 
        int dmPelsWidth; 
        int dmPelsHeight; 
        int dmDisplayFlags; 
        int dmDisplayFrequency;*/
        private byte[] mData;

        public DEVMODE()
        {
            mData = new byte[Size];
            BitConverter.GetBytes((short)Size).CopyTo(mData, 68);
        }

        public byte[] ToByteArray()
        {
            return mData;
        }

        public DM Fields
        {
            get
            {
                return (DM)BitConverter.ToInt32(mData, 72);
            }
            set
            {
                BitConverter.GetBytes((int)value).CopyTo(mData, 72);
            }
        }
        public int DisplayOrientation
        {
            get
            {
                return BitConverter.ToInt32(mData, 188);
            }
            set
            {
                BitConverter.GetBytes(value).CopyTo(mData, 188);
            }
        }
    }

    [Flags()]
    internal enum DM
    {
        ORIENTATION = 0x00000001,
        PAPERSIZE = 0x00000002,
        //PAPERLENGTH      = 0x00000004,
        //PAPERWIDTH       = 0x00000008,
        //SCALE            = 0x00000010,
        //COPIES           = 0x00000100,
        //DEFAULTSOURCE    = 0x00000200,
        PRINTQUALITY = 0x00000400,
        COLOR = 0x00000800,
        //DUPLEX           = 0x00001000,
        //YRESOLUTION      = 0x00002000,
        //TTOPTION         = 0x00004000,
        //COLLATE          = 0x00008000,
        //FORMNAME         = 0x00010000,
        //LOGPIXELS        = 0x00020000,
        BITSPERPEL = 0x00040000,
        PELSWIDTH = 0x00080000,
        PELSHEIGHT = 0x00100000,
        //DISPLAYFLAGS     = 0x00200000,
        //DISPLAYFREQUENCY = 0x00400000,
        DISPLAYORIENTATION = 0x00800000,
        DISPLAYQUERYORIENTATION = 0x01000000,
    }
}
