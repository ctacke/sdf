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

namespace OpenNETCF.Drawing.Imaging
{
    /// <summary>
    /// Describes Imaging codec
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageCodecInfo
    {
        #region unused
        // Methods
        //internal ImageCodecInfo() { }
        /*
        private static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
        {
            ImageCodecInfo[] infoArray1 = new ImageCodecInfo[numCodecs];
            for (int num1 = 0; num1 < numCodecs; num1++)
            {
                IntPtr ptr1 = (IntPtr)(((long)memoryStart) + (Marshal.SizeOf(typeof(ImageCodecInfoPrivate)) * num1));
                ImageCodecInfoPrivate private1 = new ImageCodecInfoPrivate();
                UnsafeNativeMethods.PtrToStructure(ptr1, private1);
                infoArray1[num1] = new ImageCodecInfo();
                infoArray1[num1].Clsid = private1.Clsid;
                infoArray1[num1].FormatID = private1.FormatID;
                infoArray1[num1].CodecName = Marshal.PtrToStringUni(private1.CodecName);
                infoArray1[num1].DllName = Marshal.PtrToStringUni(private1.DllName);
                infoArray1[num1].FormatDescription = Marshal.PtrToStringUni(private1.FormatDescription);
                infoArray1[num1].FilenameExtension = Marshal.PtrToStringUni(private1.FilenameExtension);
                infoArray1[num1].MimeType = Marshal.PtrToStringUni(private1.MimeType);
                infoArray1[num1].Flags = (ImageCodecFlags)private1.Flags;
                infoArray1[num1].Version = private1.Version;
                infoArray1[num1].SignaturePatterns = new byte[private1.SigCount][];
                infoArray1[num1].SignatureMasks = new byte[private1.SigCount][];
                for (int num2 = 0; num2 < private1.SigCount; num2++)
                {
                    infoArray1[num1].SignaturePatterns[num2] = new byte[private1.SigSize];
                    infoArray1[num1].SignatureMasks[num2] = new byte[private1.SigSize];
                    Marshal.Copy((IntPtr)(((long)private1.SigMask) + (num2 * private1.SigSize)), infoArray1[num1].SignatureMasks[num2], 0, private1.SigSize);
                    Marshal.Copy((IntPtr)(((long)private1.SigPattern) + (num2 * private1.SigSize)), infoArray1[num1].SignaturePatterns[num2], 0, private1.SigSize);
                }
            }
            return infoArray1;
        }
*/

        /*public static ImageCodecInfo[] GetImageDecoders()
        {
            ImageCodecInfo[] codecs;
            uint count;
            IntPtr decoders;

            ImagingFactory factory = new ImagingFactoryClass();
            int hresult = factory.GetInstalledDecoders(out count, out decoders);
            Marshal.ThrowExceptionForHR(hresult);
            if (count > 0)
            {
                codecs = new ImageCodecInfo[count];
                for (int iCodec = 0; iCodec < count; iCodec++)
                {
                    codecs[iCodec] = (ImageCodecInfo)Marshal.PtrToStructure((IntPtr)(decoders.ToInt32() + (76 * iCodec)), typeof(ImageCodecInfo));
                }
                Marshal.FreeCoTaskMem(decoders);
            }
            else
            {
                codecs = new ImageCodecInfo[0];
            }
            Marshal.ReleaseComObject(factory);
            return codecs;
            
            ImageCodecInfo[] infoArray1;
            int num1;
            int num2;
            
            int num3 = NativeMethods.Gdip.GdipGetImageDecodersSize(out num1, out num2);
            if (num3 != 0)
            {
                throw NativeMethods.Gdip.StatusException(num3);
            }
            IntPtr ptr1 = Marshal.AllocCoTaskMem(num2);
            try
            {
                num3 = NativeMethods.Gdip.GdipGetImageDecoders(num1, num2, ptr1);
                if (num3 != 0)
                {
                    throw new Exception(num3);
                }
                infoArray1 = ImageCodecInfo.ConvertFromMemory(ptr1, num1);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr1);
            }
            return infoArray1;
        }
        public static ImageCodecInfo[] GetImageEncoders()
        {
            ImageCodecInfo[] codecs;
            uint count;
            IntPtr encoders;

            ImagingFactory factory = new ImagingFactoryClass();
            int hresult = factory.GetInstalledEncoders(out count, out encoders);
            Marshal.ThrowExceptionForHR(hresult);
            if (count > 0)
            {
                codecs = new ImageCodecInfo[count];
                for (int iCodec = 0; iCodec < count; iCodec++)
                {
                    codecs[iCodec] = (ImageCodecInfo)Marshal.PtrToStructure((IntPtr)(encoders.ToInt32() + (76 * iCodec)), typeof(ImageCodecInfo));
                }
                Marshal.FreeCoTaskMem(encoders);
            }
            else
            {
                codecs = new ImageCodecInfo[0];
            }
            Marshal.ReleaseComObject(factory);
            return codecs;
        }*/
        
        #endregion

        // Fields
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] clsid;

        //public Guid Clsid
        //{
        //    get { return new Guid(clsid); }
        //    set { clsid = value; }
        //}

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] formatID;

        //public Guid FormatID
        //{
        //   get { return new Guid(formatID); }
        //    set { formatID = value; }
        //}


        //[MarshalAs(UnmanagedType.LPWStr)]
        private IntPtr codecName;

        /// <summary>
        /// Codec name
        /// </summary>
        public string CodecName
        {
            get { return Marshal.PtrToStringUni(codecName); }
            //set { codecName = value; }
        }

        //[MarshalAs(UnmanagedType.LPWStr)]
        private IntPtr dllName;

        /// <summary>
        /// Codec Dll name
        /// </summary>
        public string DllName
        {
            get { return Marshal.PtrToStringUni(dllName); }
            //set { dllName = value; }
        }

        //[MarshalAs(UnmanagedType.LPWStr)]
        private IntPtr formatDescription;

        /// <summary>
        /// Codec format description
        /// </summary>
        public string FormatDescription
        {
            get { return Marshal.PtrToStringUni(formatDescription); }
            //set { formatDescription = value; }
        }

        //[MarshalAs(UnmanagedType.LPWStr)]
        private IntPtr filenameExtension;

        /// <summary>
        /// Codec's file's extension (e.g. BMP)
        /// </summary>
        public string FilenameExtension
        {
            get { return Marshal.PtrToStringUni(filenameExtension); }
            //set { filenameExtension = value; }
        }

        //[MarshalAs(UnmanagedType.LPWStr)]
        private IntPtr mimeType;

        /// <summary>
        /// Codec's image MIME type
        /// </summary>
        public string MimeType
        {
            get { return Marshal.PtrToStringUni(mimeType); }
            //set { mimeType = value; }
        }

        private ImageCodecFlags flags;

        /// <summary>
        /// Codec flags
        /// </summary>
        public ImageCodecFlags Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        private int version;

        /// <summary>
        /// Codec version
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }
        private int sigCount;
        private int sigSize;

        //[MarshalAs(UnmanagedType.LPArray)]
        private IntPtr signatureMasks;

        /*
                public byte[] SignatureMasks
                {
                    get 
                    { 
                        byte[] val = new byte[sigSize];
                        Marshal.Copy(signatureMasks, val, 0, sigSize);
                        return val; 
                    }
                    //set { signatureMasks = value; }
                }
        */
        //[MarshalAs(UnmanagedType.LPArray)]
        private IntPtr signaturePatterns;
        /*
                public byte[] SignaturePatterns
                {
                    get 
                    { 
                        byte[] val = new byte[sigSize];
                        Marshal.Copy(signaturePatterns, val, 0, sigSize);
                        return val; 
                    }
                    //set { signaturePatterns = value; }
                }
        */
    }
}
