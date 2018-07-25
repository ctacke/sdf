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

namespace OpenNETCF.Media.WaveAudio
{
    public class RiffStream: Stream
    {
        [CLSCompliant(false)]
        protected Stream _baseStream;
        [CLSCompliant(false)]
        protected FileAccess _accessMode;
        [CLSCompliant(false)]
        protected WaveFormat2 _fmt;
        MMChunk _mmc;
        internal DataChunk _ckData;
        internal FmtChunk _ckFmt;
        long _posData;

        internal RiffStream()
        {
        }

        public virtual WaveFormat2 Format
        {
            get { return _fmt; }
        }

        public static RiffStream OpenRead(Stream underlyingStream)
        {
            RiffStream stm = new RiffStream();
            stm._accessMode = FileAccess.Read;
            stm._baseStream = underlyingStream;
            stm.LoadHeader();
            if (stm.Format.FormatTag != FormatTag.PCM) // need conversion
            {
                stm.Detach();
                underlyingStream.Seek(0, SeekOrigin.Begin);
                stm = ACMStream.OpenRead(underlyingStream);
            }
            return stm;
        }

        public static RiffStream OpenWrite(Stream underlyingStream, WaveFormat2 fmt)
        {
            RiffStream stm = new RiffStream();
            stm._accessMode = FileAccess.Write;
            stm._baseStream = underlyingStream;
            stm.InitializeForWriting(underlyingStream, fmt, fmt);
            stm.WriteHeader();
            return stm;
        }

        protected virtual void WriteHeader()
        {
        }

        protected virtual void LoadHeader()
        {
            _mmc = MMChunk.FromStream(_baseStream);
            _ckFmt = _mmc.FindChunk(FourCC.Fmt) as FmtChunk;
            if (_ckFmt != null)
                _fmt = _ckFmt.WaveFormat;
            _ckData = _mmc.FindChunk(FourCC.Data) as DataChunk;
            if (_ckData != null)
                _posData = _ckData.DataStart;
            _baseStream.Seek(_posData, SeekOrigin.Begin);
        }

        protected virtual bool InitializeForWriting(Stream underlyingStream, WaveFormat2 wfIn, WaveFormat2 wfOut)
        {
            _baseStream.Position = 0;
            _mmc = new RiffChunk(underlyingStream);
            (_mmc as RiffChunk).BeginWrite();
            _ckFmt = new FmtChunk(underlyingStream, wfOut);
            _mmc.AppendChunk(_ckFmt);
            _ckFmt.BeginWrite();
            _ckFmt.Write(_ckFmt.WaveFormat.GetBytes());
            _fmt = wfOut;
            _ckData = new DataChunk(underlyingStream);
            _mmc.AppendChunk(_ckData);
            _ckData.BeginWrite();
            return true;
        }

        public override bool CanRead
        {
            get { return _accessMode == FileAccess.Read || _accessMode == FileAccess.ReadWrite; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return _accessMode == FileAccess.Write || _accessMode == FileAccess.ReadWrite; }
        }

        public override void Flush()
        {
            if (_accessMode == FileAccess.Write)
                UpdateHeader();
            _baseStream.Flush();
        }

        public override void Close()
        {
            if (_baseStream != null)
            {
                Flush();
                _baseStream.Close();
            }
        }

        public void Detach()
        {
            _baseStream = null;
        }

        public virtual void UpdateHeader()
        {
            _ckFmt.EndWrite();
            _ckData.UpdateSize();
            _ckData.EndWrite();
            (_mmc as RiffChunk).UpdateSize();
            (_mmc as RiffChunk).EndWrite();
        }

        public override long Length
        {
            get
            {
                if (_ckData == null)
                    throw new InvalidOperationException("Stream has not been opened");
                return _ckData.Size;
            }
        }

        public override long Position
        {
            get
            {
                if (_ckData == null)
                    throw new InvalidOperationException("Stream has not been opened");
                return _baseStream.Position - _ckData.DataStart;
            }
            set
            {
                Seek(value, SeekOrigin.Current);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_ckData == null || _accessMode != FileAccess.Read)
                throw new InvalidOperationException("Stream has not been opened");
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_ckData == null)
                throw new InvalidOperationException("Stream has not been opened");

            if (origin == SeekOrigin.Begin)
                return _baseStream.Seek(offset + _ckData.DataStart, origin);
            else
                return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            if (_ckData == null)
                throw new InvalidOperationException("Stream has not been opened");
            _baseStream.SetLength(value + _ckData.DataStart);
            _ckData.Size = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_ckData == null || _accessMode != FileAccess.Write)
                throw new InvalidOperationException("Stream has not been opened");
            _baseStream.Write(buffer, offset, count);
            if (Position > _ckData.Size)
                _ckData.Size = Position;
        }

        public virtual float PositionSeconds
        {
            set
            {
                if (_ckFmt == null)
                    throw new InvalidOperationException();
                Position = (long)(value * _fmt.AvgBytesPerSec);
            }
            get
            {
                if (_ckFmt == null)
                    throw new InvalidOperationException();
                return 1.0f * Position / _fmt.AvgBytesPerSec;
            }
        }

        public virtual float LengthInSeconds
        {
            set
            {
                if (_ckFmt == null)
                    throw new InvalidOperationException();
                SetLength((long)(value * _fmt.AvgBytesPerSec));
            }
            get
            {
                if (_ckFmt == null)
                    throw new InvalidOperationException();
                return 1.0f * Length / _fmt.AvgBytesPerSec;
            }
        }

        protected void SetFormat(WaveFormat2 fmt)
        {
            _fmt = fmt;
            if (_ckFmt == null)
            {
                _baseStream.Position = _ckData.Start + 12;
                _ckFmt = new FmtChunk(_baseStream, fmt);
            }
        }
    }

}
