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

namespace OpenNETCF.Media.WaveAudio
{
    public delegate int BufferStarvingHandler(SlidingBuffer buffer, int spaceAvailable);
    public class SlidingBuffer
    {
        private int _size;
        private byte[] _buffer;
        private int _readPtr, _writePtr;

        public BufferStarvingHandler BufferStarving;

        public SlidingBuffer(int size)
        {
            Initialize(size);
        }

        public SlidingBuffer()
        {
            Initialize(1024);
        }

        private void Initialize(int size)
        {
            _size = size;
            _buffer = new byte[_size];
            _readPtr = _writePtr = 0;
        }

        public int Append(byte[] data)
        {
            return Append(data, 0, data.Length);
        }

        public int Append(byte[] data, int count)
        {
            return Append(data, 0, count);
        }

        public int Append(byte[] data, int offset, int count)
        {
            int cb = Math.Min(_buffer.Length - _writePtr, count);
            Buffer.BlockCopy(data, offset, _buffer, _writePtr, cb);
            _writePtr += cb;
            return cb;
        }

        private void Trim()
        {
            Array.Copy(_buffer, _readPtr, _buffer, 0, _writePtr - _readPtr);
            _writePtr -= _readPtr;
            _readPtr = 0;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int cb = count;
            int totalRead = 0;
            while (count > 0 && cb > 0)
            {
                cb = Math.Min(count, _writePtr - _readPtr);
                Buffer.BlockCopy(_buffer, _readPtr, buffer, offset, cb);
                count -= cb;
                totalRead += cb;
                _readPtr += cb;
                offset += cb;
                if (_readPtr == _writePtr)
                {
                    Trim();
                    cb = BufferStarving(this, _buffer.Length - _writePtr);
                }
            }
            return totalRead;
        }

        public int Read(byte[] buffer)
        {
            return Read(buffer, 0, buffer.Length);
        }
    }
}
