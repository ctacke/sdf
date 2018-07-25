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
