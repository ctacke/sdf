using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Win32
{
    internal class Core
    {
        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;

        internal const Int32 METHOD_BUFFERED = 0;
        internal const Int32 FILE_ANY_ACCESS = 0;
        internal const Int32 FILE_DEVICE_HAL = 0x00000101;
        internal static Int32 IOCTL_HAL_GET_DEVICEID = ((FILE_DEVICE_HAL) << 16) |
            ((FILE_ANY_ACCESS) << 14) | ((21) << 2) | (METHOD_BUFFERED);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool KernelIoControl(int dwIoControlCode, byte[] inBuf, int inBufSize, byte[] outBuf, int outBufSize, ref int bytesReturned);

        private static byte[] GetRawDeviceID()
        {
            // Initialize the output buffer to the size of a Win32 DEVICE_ID structure
            Int32 dwOutBytes = 0;
            //set an initial buffer size
            Int32 nBuffSize = 256;
            byte[] outbuff = new byte[nBuffSize];

            bool done = false;

            // Set DEVICEID.dwSize to size of buffer.  Some platforms look at
            // this field rather than the nOutBufSize param of KernelIoControl
            // when determining if the buffer is large enough.
            //
            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);

            // Loop until the device ID is retrieved or an error occurs
            while (!done)
            {
                if (KernelIoControl(IOCTL_HAL_GET_DEVICEID, null, 0, outbuff,
                        nBuffSize, ref dwOutBytes))
                {
                    done = true;
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    switch (error)
                    {
                        case ERROR_NOT_SUPPORTED:
                            throw new NotSupportedException("IOCTL_HAL_GET_DEVICEID is not supported on this device", new Exception("" + error));

                        case ERROR_INSUFFICIENT_BUFFER:
                            // The buffer wasn't big enough for the data.  The
                            // required size is in the first 4 bytes of the output
                            // buffer (DEVICE_ID.dwSize).
                            nBuffSize = BitConverter.ToInt32(outbuff, 0);
                            outbuff = new byte[nBuffSize];

                            // Set DEVICEID.dwSize to size of buffer.  Some
                            // platforms look at this field rather than the
                            // nOutBufSize param of KernelIoControl when
                            // determining if the buffer is large enough.
                            //
                            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
                            break;

                        default:
                            throw new Exception("Unexpected error: " + error);
                    }
                }
            }

            //return the raw buffer - a DEVICE_ID structure
            return outbuff;
        }

        /// <summary>
        /// Returns a Guid representing the unique idenitifier of the device.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <returns></returns>
        public static Guid GetDeviceGuid()
        {
            byte[] outbuff = GetRawDeviceID();

            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4); //	DEVICE_ID.dwPresetIDOffset
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8); // DEVICE_ID.dwPresetSize
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc); // DEVICE_ID.dwPlatformIDOffset
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10); // DEVICE_ID.dwPlatformIDBytes

            byte[] guidbytes = new byte[16];

            Buffer.BlockCopy(outbuff, dwPresetIDOffset + dwPresetIDSize - 10, guidbytes, 0, 10);
            Buffer.BlockCopy(outbuff, dwPlatformIDOffset + dwPlatformIDSize - 6, guidbytes, 10, 6);

            return new Guid(guidbytes);
        }

        /// <summary>
        /// Returns a string containing a unique identifier for the device.
        /// </summary>
        /// <returns>Devices unique ID.</returns>
        public static string GetDeviceID()
        {
            byte[] outbuff = GetRawDeviceID();

            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4); //	DEVICE_ID.dwPresetIDOffset
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8); // DEVICE_ID.dwPresetSize
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc); // DEVICE_ID.dwPlatformIDOffset
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10); // DEVICE_ID.dwPlatformIDBytes
            StringBuilder sb = new StringBuilder();

            for (int i = dwPresetIDOffset; i < dwPresetIDOffset + dwPresetIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            sb.Append("-");
            for (int i = dwPlatformIDOffset; i < dwPlatformIDOffset + dwPlatformIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            return sb.ToString();

        }
    }
}
