using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenNETCF.Media.WaveAudio
{
    public class ACMStream : RiffStream
    {
        const int DefaultBufferSizeInSeconds = 5;

        protected IntPtr hStreamComp;
        protected IntPtr hStreamConv;
        protected ACMSTREAMHEADER hdr1, hdr2;
        protected GCHandle hinBuffer1, houtBuffer1;
        protected GCHandle hinBuffer2, houtBuffer2;
        protected byte[] inBuffer1, outBuffer1;
        protected byte[] inBuffer2, outBuffer2;
        protected int ptrBuffer1, ptrBuffer2;
        protected SlidingBuffer bufferRead;
        private bool firstBlock;
        [CLSCompliant(false)]
        protected WaveFormat2 _fmt2;

        public override WaveFormat2 Format
        {
            get { return DestinationFormat; }
        }

        public WaveFormat2 SourceFormat
        {
            get
            {
                if (_accessMode == FileAccess.Read)
                    return _fmt;
                else
                    return _fmt2;
            }
        }

        public WaveFormat2 DestinationFormat
        {
            get
            {
                if (_accessMode == FileAccess.Read)
                    return _fmt2;
                else
                    return _fmt;
            }
        }


        protected override void LoadHeader()
        {
            base.LoadHeader();

            byte[] fmtIn = _fmt.GetBytes();
            if (_fmt2 == null)
                _fmt2 = WaveFormat2.GetPCMWaveFormat(_fmt.SamplesPerSec, _fmt.Channels, _fmt.BitsPerSample != 0 ? _fmt.BitsPerSample : (short)16);
            byte[] fmtOut = _fmt2.GetBytes();
            MMResult mmr;
            _fmt2 = new WaveFormat2(fmtOut);
            mmr = AcmNativeMethods.acmStreamOpen(out hStreamComp, IntPtr.Zero, fmtIn, fmtOut, IntPtr.Zero, 0, 0, AcmStreamOpenFlags.CALLBACK_NULL);
            hdr1 = new ACMSTREAMHEADER();
            hdr1.cbStruct = Marshal.SizeOf(hdr1);
            hdr2 = new ACMSTREAMHEADER();
            hdr2.cbStruct = Marshal.SizeOf(hdr2);

            int cbIn = (_fmt.AvgBytesPerSec * DefaultBufferSizeInSeconds / _fmt.BlockAlign + 1) * _fmt.BlockAlign;
            inBuffer1 = new byte[cbIn];
            inBuffer2 = new byte[cbIn];
            int cbOut = _fmt2.BlockAlign * _fmt2.AvgBytesPerSec * DefaultBufferSizeInSeconds;
            mmr = AcmNativeMethods.acmStreamSize(hStreamComp, cbIn, out cbOut, AcmStreamSizeFlags.SOURCE);
            outBuffer1 = new byte[cbOut];
            outBuffer2 = new byte[cbOut];
            hinBuffer1 = GCHandle.Alloc(inBuffer1, GCHandleType.Pinned);
            houtBuffer1 = GCHandle.Alloc(outBuffer1, GCHandleType.Pinned);
            hinBuffer2 = GCHandle.Alloc(inBuffer2, GCHandleType.Pinned);
            houtBuffer2 = GCHandle.Alloc(outBuffer2, GCHandleType.Pinned);

            hdr1.pbSrc = hinBuffer1.AddrOfPinnedObject();
            hdr1.pbDst = houtBuffer1.AddrOfPinnedObject();
            hdr1.cbSrcLength = cbIn;
            hdr1.cbDstLength = cbOut;

            hdr2.pbSrc = hinBuffer2.AddrOfPinnedObject();
            hdr2.pbDst = houtBuffer2.AddrOfPinnedObject();
            hdr2.cbSrcLength = cbIn;
            hdr2.cbDstLength = cbOut;

            ptrBuffer1 = ptrBuffer1 = 0;

            bufferRead = new SlidingBuffer(cbOut);
            firstBlock = true;

            bufferRead.BufferStarving = (BufferStarvingHandler)delegate(SlidingBuffer obj, int spaceAvailable)
            {
                if (hdr1.cbDstLengthUsed == ptrBuffer1)
                {
                    int cb = ReadInternal(inBuffer1, 0, inBuffer1.Length);
                    if (cb == 0)
                        return 0;
                    ptrBuffer1 = 0;
                    AcmStreamConvertFlags flags;
                    if (firstBlock)
                    {
                        flags = AcmStreamConvertFlags.START;
                        firstBlock = false;
                    }
                    else if (cb == inBuffer1.Length)
                        flags = AcmStreamConvertFlags.BLOCKALIGN;
                    else
                        flags = AcmStreamConvertFlags.END;

                    hdr1.cbSrcLength = cb;
                    MMResult res = AcmNativeMethods.acmStreamConvert(hStreamComp, ref hdr1, flags);
                    return cb;
                }

                int written = obj.Append(outBuffer1, ptrBuffer1, hdr1.cbDstLengthUsed - ptrBuffer1);
                ptrBuffer1 += written;
                return written;
            };

            mmr = AcmNativeMethods.acmStreamPrepareHeader(hStreamComp, ref hdr1, 0);
            mmr = AcmNativeMethods.acmStreamPrepareHeader(hStreamComp, ref hdr2, 0);


        }

        private int ReadInternal(byte[] buffer, int start, int cb)
        {
            return base.Read(buffer, start, cb);
        }

        protected override void WriteHeader()
        {
            base.WriteHeader();
        }

        public static new ACMStream OpenRead(Stream underlyingStream)
        {
            ACMStream stm = new ACMStream();
            stm._accessMode = FileAccess.Read;
            stm._baseStream = underlyingStream;
            stm.LoadHeader();
            return stm;
        }

        private static new ACMStream OpenWrite(Stream underlyingStream, WaveFormat2 fmt)
        {
            return null;
        }

        public static ACMStream Append(Stream underlyingStream, WaveFormat2 fmt)
        {
            ACMStream stm = new ACMStream();
            stm._accessMode = FileAccess.Write;
            stm._baseStream = underlyingStream;
            stm._baseStream.Position = 0;
            WaveFormat2 fmtOut = WaveFormat2.FromStream(underlyingStream);
            stm._baseStream.Position = stm._baseStream.Length;
            stm.InitializeForWriting(underlyingStream, fmt ?? fmtOut, fmtOut, true);
            stm.WriteHeader();
            return stm;
        }

        public static ACMStream OpenWrite(Stream underlyingStream, WaveFormat2 fmtIn, WaveFormat2 fmtOut)
        {
            ACMStream stm = new ACMStream();
            stm._accessMode = FileAccess.Write;
            stm._baseStream = underlyingStream;
            stm.InitializeForWriting(underlyingStream, fmtIn, fmtOut, false);
            stm.WriteHeader();
            return stm;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            //return base.Read(buffer, offset, count);
            return bufferRead.Read(buffer, offset, count);
        }

        protected override bool InitializeForWriting(Stream underlyingStream, WaveFormat2 wfIn, WaveFormat2 wfOut, bool fAppend)
        {
            base.InitializeForWriting(underlyingStream, wfIn, wfOut, fAppend);
            MMResult mmr;

            int driverId = 0;
            foreach (AcmDriverInfo drvInfo in ACMSupport.SupportedDrivers)
            {
                foreach (AcmFormatInfo fmtInfo in drvInfo.Formats)
                {
                    if (fmtInfo.Format == wfOut)
                    {
                        driverId = fmtInfo.DriverId;
                        break;
                    }
                }
            }

            foreach (AcmDriverInfo drvInfo in ACMSupport.SupportedDrivers)
            {
                foreach (AcmFormatInfo fmtInfo in drvInfo.Formats)
                {
                    if (fmtInfo.DriverId == driverId && fmtInfo.Format.FormatTag == wfIn.FormatTag)
                    {
                        wfIn.CopyFrom(fmtInfo.Format);
                        break;
                    }
                }
            }

            byte[] fmtIn = wfIn.GetBytes();
            byte[] fmtOut = wfOut.GetBytes();

            IntPtr hDrv = IntPtr.Zero;
            if (driverId != 0)
                AcmNativeMethods.acmDriverOpen(out hDrv, driverId, 0);
            mmr = AcmNativeMethods.acmStreamOpen(out hStreamComp, hDrv, fmtIn, fmtOut, IntPtr.Zero, 0, 0, AcmStreamOpenFlags.CALLBACK_NULL | AcmStreamOpenFlags.NONREALTIME);
            hdr1 = new ACMSTREAMHEADER();
            hdr1.cbStruct = Marshal.SizeOf(hdr1);
            hdr2 = new ACMSTREAMHEADER();
            hdr2.cbStruct = Marshal.SizeOf(hdr2);

            int cbIn = (wfIn.AvgBytesPerSec * DefaultBufferSizeInSeconds / wfIn.BlockAlign + 1) * wfIn.BlockAlign;
            inBuffer1 = new byte[cbIn];
            inBuffer2 = new byte[cbIn];
            int cbOut = wfOut.BlockAlign * wfOut.AvgBytesPerSec * DefaultBufferSizeInSeconds;
            mmr = AcmNativeMethods.acmStreamSize(hStreamComp, cbIn, out cbOut, AcmStreamSizeFlags.DESTINATION);
            outBuffer1 = new byte[cbOut];
            outBuffer2 = new byte[cbOut];
            hinBuffer1 = GCHandle.Alloc(inBuffer1, GCHandleType.Pinned);
            houtBuffer1 = GCHandle.Alloc(outBuffer1, GCHandleType.Pinned);
            hinBuffer2 = GCHandle.Alloc(inBuffer2, GCHandleType.Pinned);
            houtBuffer2 = GCHandle.Alloc(outBuffer2, GCHandleType.Pinned);

            hdr1.pbSrc = hinBuffer1.AddrOfPinnedObject();
            hdr1.pbDst = houtBuffer1.AddrOfPinnedObject();
            hdr1.cbSrcLength = cbIn;
            hdr1.cbDstLength = cbOut;

            hdr2.pbSrc = hinBuffer2.AddrOfPinnedObject();
            hdr2.pbDst = houtBuffer2.AddrOfPinnedObject();
            hdr2.cbSrcLength = cbIn;
            hdr2.cbDstLength = cbOut;

            ptrBuffer1 = ptrBuffer1 = 0;

            firstBlock = true;

            mmr = AcmNativeMethods.acmStreamPrepareHeader(hStreamComp, ref hdr1, 0);
            mmr = AcmNativeMethods.acmStreamPrepareHeader(hStreamComp, ref hdr2, 0);

            return true;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            do
            {
                int cb = 0;
                int bufferSize = hdr1.cbSrcLength;
                if (ptrBuffer1 == hdr1.cbSrcLength || count == 0)
                {
                    if (count == 0)
                        hdr1.cbSrcLength = ptrBuffer1;

                    AcmStreamConvertFlags flags;
                    flags = AcmStreamConvertFlags.BLOCKALIGN;

                    if (firstBlock)
                    {
                        flags = AcmStreamConvertFlags.START;
                        firstBlock = false;
                    }
                    else if (count != 0 || ptrBuffer1 > 0)
                        flags = AcmStreamConvertFlags.BLOCKALIGN;
                    else
                    {
                        flags = AcmStreamConvertFlags.END;
                    }

                    MMResult mmr = AcmNativeMethods.acmStreamConvert(hStreamComp, ref hdr1, flags);

                    ptrBuffer1 = 0;
                    hdr1.cbSrcLength = bufferSize;
                    base.Write(outBuffer1, 0, hdr1.cbDstLengthUsed);

                }

                cb = Math.Min(hdr1.cbSrcLength - ptrBuffer1, count);
                Array.Copy(buffer, offset, inBuffer1, ptrBuffer1, cb);
                count -= cb;
                ptrBuffer1 += cb;
                offset += cb;
            } while (count > 0);
        }

        public override void Flush()
        {
            if (_accessMode == FileAccess.Write)
            {
                Write(new byte[] { }, 0, 0);
            }
            base.Flush();
        }

        public override void Close()
        {
            Flush();

            if ((hdr1.fdwStatus & AcmStreamHeaderStatus.PREPARED) != 0)
                AcmNativeMethods.acmStreamUnprepareHeader(hStreamComp, ref hdr1, 0);
            if ((hdr2.fdwStatus & AcmStreamHeaderStatus.PREPARED) != 0)
                AcmNativeMethods.acmStreamUnprepareHeader(hStreamComp, ref hdr2, 0);

            if (hStreamComp != IntPtr.Zero)
                AcmNativeMethods.acmStreamClose(hStreamComp, 0);
            hStreamComp = IntPtr.Zero;

            if (hinBuffer1.IsAllocated)
                hinBuffer1.Free();
            if (hinBuffer2.IsAllocated)
                hinBuffer2.Free();
            if (houtBuffer1.IsAllocated)
                houtBuffer1.Free();
            if (houtBuffer2.IsAllocated)
                houtBuffer2.Free();

            if (_baseStream != null)
                _baseStream.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Close();
            }
        }
    }
}
