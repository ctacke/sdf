using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace OpenNETCF.IO
{
    /// <summary>
    /// Use this abstract base class to create wrappers around Stream Interface
    /// Drivers that are not supported by the CF
    /// </summary>
    public abstract partial class StreamInterfaceDriver : IDisposable
    {
        /// <summary>
        /// Internal native handle to the open port
        /// </summary>
        protected IntPtr m_hPort = IntPtr.Zero;
        string m_portName = null;

        /// <summary>
        /// Create an instance of the StreamInterfaceDriver class
        /// </summary>
        /// <param name="portName">Name of port (prefix and index) to open</param>
        protected StreamInterfaceDriver(string portName)
        {
            if (portName == null) throw new ArgumentNullException();
            if (portName == string.Empty) throw new ArgumentException();

            m_portName = portName;
        }
        private static object m_syncRoot = new object();

        /// <summary>
        /// Create an instance of the StreamInterfaceDriver class
        /// </summary>
        protected StreamInterfaceDriver()
        {
        }

        /// <summary>
        /// Gets or Sets the Port Name
        /// </summary>
        protected string PortName
        {
            set { m_portName = value; }
            get { return m_portName; }
        }

        /// <summary>
        /// Gets the underlying native port Handle
        /// </summary>
        protected IntPtr Handle
        {
            get { return m_hPort; }
        }

        /// <summary>
        /// Returns true if the port is open, otherwise returns false
        /// </summary>
        protected bool IsOpen
        {
            get { return (m_hPort != IntPtr.Zero) && (m_hPort.ToInt32() != OpenNETCF.NativeMethods.INVALID_HANDLE_VALUE); }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~StreamInterfaceDriver()
        {
            this.Dispose();
        }

        /// <summary>
        /// Open the driver
        /// </summary>
        /// <param name="access">File Access (Read, Write or Both)</param>
        /// <param name="share">Share Mode (Read, Write or both)</param>
        [CLSCompliant(false)]
        protected void Open(System.IO.FileAccess access, System.IO.FileShare share)
        {
            m_hPort = FileHelper.CreateFile(m_portName, access, share, IO.FileCreateDisposition.OpenExisting, 0);

            if ((int)m_hPort == -1)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot open driver");
            }
        }

        private FileAccess2 ConvertAccess(System.IO.FileAccess access)
        {
            switch (access)
            {
                case System.IO.FileAccess.Read:
                    return FileAccess2.Read;
                case System.IO.FileAccess.Write:
                    return FileAccess2.Write;
                case System.IO.FileAccess.ReadWrite:
                    return FileAccess2.Read | FileAccess2.Write;
            }

            return FileAccess2.All;
        }

        /// <summary>
        /// Read data from the driver
        /// </summary>
        /// <param name="bytesToRead">The number of bytes requested.</param>
        /// <returns>A byte array returned by the driver</returns>
        protected byte[] Read(int bytesToRead)
        {
            byte[] buffer = new byte[bytesToRead];
            int read = 0;

            FileHelper.ReadFile(m_hPort, buffer, bytesToRead, ref read);

            return buffer;
        }

        /// <summary>
        /// Write data to the driver
        /// </summary>
        /// <param name="data">Data to write</param>
        /// <returns>Number of bytes actually written</returns>
        protected int Write(byte[] data)
        {
            return Write(data, data.Length);
        }

        /// <summary>
        /// Write data to the driver
        /// </summary>
        /// <param name="data">Data to write</param>
        /// <param name="bytesToWrite">Number of bytes from passed in buffer to write</param>
        /// <returns>Number of bytes actually written</returns>
        protected int Write(byte[] data, int bytesToWrite)
        {
            int written = 0;

            FileHelper.WriteFile(m_hPort, data, bytesToWrite, ref written);

            return written;
        }

        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData)
        {
            int xfer = 0;
            DeviceIoControl(controlCode, inData, outData, out xfer);
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        /// <param name="controlCode">The IOCTL code</param>
        /// <param name="inData">Data to pass into the IOCTL</param>
        /// <param name="outData">Data returned by the IOCTL</param>
        /// <param name="bytesReturned">Data returned by the call</param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData, out int bytesReturned)
        {
            if (outData == null)
            {
                if (inData == null)
                {
                    if (NativeMethods.DeviceIoControl(m_hPort, controlCode, IntPtr.Zero, 0, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero) == 0)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                    }
                }
                else
                {
                    if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inData, inData.Length, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero) == 0)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                    }
                }
            }
            else if (inData == null)
            {
                if (NativeMethods.DeviceIoControl(m_hPort, controlCode, IntPtr.Zero, 0, outData, outData.Length, out bytesReturned, IntPtr.Zero) == 0)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                }
            }
            else if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inData, inData.Length, outData, outData.Length, out bytesReturned, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
            }
        }

        /// <summary>
        /// This function moves the file pointer of an open file
        /// <seealso cref="System.IO.File.Move"/>
        /// </summary>
        /// <param name="distance">Bytes to move - a positive number moves forward, a negative moves backward</param>
        /// <param name="seekFrom">Starting position from where distance is measured</param>
        /// <returns>New file position</returns>
        /// <remarks>The current file position can be queried using seekFrom(0, MoveMethod.CurrentPosition)</remarks>
        protected int Seek(int distance, System.IO.SeekOrigin seekFrom)
        {
            int dist = NativeMethods.SetFilePointer(m_hPort, distance, 0, seekFrom);

            if (dist == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Seek Failed");
            }

            return dist;
        }

        /// <summary>
        /// Close the driver
        /// </summary>
        protected void Close()
        {
            if (m_hPort != IntPtr.Zero)
            {
                NativeMethods.CloseHandle(m_hPort);
                m_hPort = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        public static void ActivateDevice(string registryPath)
        {
            lock (m_syncRoot)
            {
                IntPtr ptr = NativeMethods.ActivateDevice(registryPath, 0);

                if (ptr == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
            }
        }

        public static void DeactivateDevice(string registryPath)
        {
            // since we use a "feature" of the OS that CE 6.0 blocks, make sure we validate the OS version
            if (Environment.OSVersion.Version.Major > 5)
            {
                throw new Exception("This function is viable only for CE 5.0 and earlier");
            }

            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath);
            if (key == null)
            {
                throw new ArgumentException("No driver information found at provided path");
            }

            // get the prefix
            string prefix = key.GetValue("Prefix") as string;
            key.Close();
            if (prefix == null)
            {
                throw new ArgumentException("No driver information found at provided path");
            }

            key = Registry.LocalMachine.OpenSubKey("Drivers\\Active");
            string[] activeNames = key.GetSubKeyNames();
            key.Close();

            foreach (string subkey in activeNames)
            {
                key = Registry.LocalMachine.OpenSubKey(string.Format("Drivers\\Active\\{0}", subkey));
                string driverName = key.GetValue("Name") as string;
                if ((driverName != null) && (driverName.StartsWith(prefix)))
                {
                    int driverHandle = Convert.ToInt32(key.GetValue("Hnd", 0));
                    NativeMethods.DeactivateDevice(new IntPtr(driverHandle));
                    key.Close();
                    return;
                }
                key.Close();
            }
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        /// <param name="controlCode">The IOCTL code</param>
        /// <param name="inData">Data to pass into the IOCTL</param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode, byte[] inData)
        {
            DeviceIoControl(controlCode, inData, null);
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        /// <param name="controlCode">The IOCTL code</param>
        /// <param name="inData">Data to pass into the IOCTL</param>
        /// <param name="inSize"></param>
        /// <param name="bytesReturned"></param>
        /// <param name="outData"></param>
        /// <param name="outSize"></param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode, IntPtr inData, int inSize, IntPtr outData, int outSize, out int bytesReturned)
        {
            if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inData, inSize, outData, outSize, out bytesReturned, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
            }
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        [CLSCompliant(false)]
        protected unsafe void DeviceIoControl(uint controlCode, void* inData, int inSize, void *outData, int outSize, out int bytesReturned)
        {
            if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inData, inSize, outData, outSize, out bytesReturned, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
            }
        }
        
        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        [CLSCompliant(false)]
        protected unsafe void DeviceIoControl<TInput, TOutput>(uint controlCode, TInput inData, ref TOutput outData, out int bytesReturned)
            where TInput : struct
            where TOutput : struct
        {
            bytesReturned = 0;
            GCHandle inPtr = GCHandle.Alloc(inData, GCHandleType.Pinned);
            try
            {
                GCHandle outPtr = GCHandle.Alloc(outData, GCHandleType.Pinned);
                try
                {
                    if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inPtr.AddrOfPinnedObject(), Marshal.SizeOf(inData), outPtr.AddrOfPinnedObject(), Marshal.SizeOf(outData), IntPtr.Zero, IntPtr.Zero) == 0)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), string.Format("IOControl call failed (0x{4:x}).\r\ninData address: 0x{0:8x}\r\ninData size: {1}\r\noutData address: 0x{2:8x}\r\noutData size: {3}",
                            inPtr.AddrOfPinnedObject(), Marshal.SizeOf(inData), outPtr.AddrOfPinnedObject(), Marshal.SizeOf(outData), Marshal.GetLastWin32Error()));
                    }
                    outData = (TOutput)outPtr.Target;
                }
                finally { outPtr.Free(); }
            }
            finally { inPtr.Free(); }
        }

        //[CLSCompliant(false)]
        //protected unsafe void DeviceIoControl<TInput, TOutput>(uint controlCode, TInput inData, ref TOutput outData, out int bytesReturned)
        //    where TInput : struct
        //    where TOutput : struct
        //{
        //    IntPtr pIn = Marshal.AllocHGlobal(Marshal.SizeOf(inData));
        //    try
        //    {
        //        Marshal.StructureToPtr(inData, pIn, true);

        //        IntPtr pOut = Marshal.AllocHGlobal(Marshal.SizeOf(outData));
        //        try
        //        {
        //            Marshal.StructureToPtr(outData, pOut, true);

        //            bytesReturned = 0;

        //            if (NativeMethods.DeviceIoControl(m_hPort, controlCode, pIn, Marshal.SizeOf(inData), pOut, Marshal.SizeOf(outData), IntPtr.Zero, IntPtr.Zero) == 0)
        //            {
        //                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), string.Format("IOControl call failed (0x{4:x}).\r\ninData address: 0x{0:8x}\r\ninData size: {1}\r\noutData address: 0x{2:8x}\r\noutData size: {3}",
        //                    pIn, Marshal.SizeOf(inData), pOut, Marshal.SizeOf(outData), Marshal.GetLastWin32Error()));
        //            }
        //        }
        //        finally { Marshal.FreeHGlobal(pOut); }
        //    }
        //    finally { Marshal.FreeHGlobal(pIn); }
        //}

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        [CLSCompliant(false)]
        protected void DeviceIoControl<TInput>(uint controlCode, TInput inData)
            where TInput : struct
        {
            int bytesReturned = 0;
            GCHandle inPtr = GCHandle.Alloc(inData, GCHandleType.Pinned);
            try
            {
                if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inPtr.AddrOfPinnedObject(), Marshal.SizeOf(inData), IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero) == 0)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                }
            }
            finally { inPtr.Free(); }
        }

        [CLSCompliant(false)]
        protected void DeviceIoControl<TInput>(uint controlCode, TInput inData, byte[] outputData)
            where TInput : struct
        {
            int bytesReturned = 0;
            GCHandle inPtr = GCHandle.Alloc(inData, GCHandleType.Pinned);
            try
            {
                if (NativeMethods.DeviceIoControl(m_hPort, controlCode, inPtr.AddrOfPinnedObject(), Marshal.SizeOf(inData), outputData, outputData.Length, out bytesReturned, IntPtr.Zero) == 0)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                }
            }
            finally { inPtr.Free(); }
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        [CLSCompliant(false)]
        protected void DeviceIoControl<TOutput>(uint controlCode, ref TOutput outData, out int bytesReturned)
            where TOutput : struct
        {
            GCHandle outPtr = GCHandle.Alloc(outData, GCHandleType.Pinned);
            try
            {
                if (NativeMethods.DeviceIoControl(m_hPort, controlCode, IntPtr.Zero, 0, outPtr.AddrOfPinnedObject(), Marshal.SizeOf(outData), out bytesReturned, IntPtr.Zero) == 0)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "IOControl call failed");
                }
                outData = (TOutput)outPtr.Target;
            }
            finally { outPtr.Free(); }
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        /// <param name="controlCode">The IOCTL code</param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode)
        {
            DeviceIoControl(controlCode, null, null);
        }
    }
}
