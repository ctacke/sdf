using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Drawing.Imaging
{
    public class ImageProperty
    {
        public ImageTag Id;
        public int Len;
        public TagType Type;
        protected object value;
        public void FromByteArray(byte[] data)
        {
            switch (Type)
            {
                case TagType.ASCII:
                    value = Encoding.ASCII.GetString(data, 0, Len);
                    break;
                case TagType.UNDEFINED:
                    value = data;
                    break;
                case TagType.BYTE:
                    value = data[0];
                    break;
                case TagType.LONG:
                    value = BitConverter.ToUInt32(data, 0);
                    break;
                case TagType.SHORT:
                    value = BitConverter.ToUInt16(data, 0);
                    break;
                case TagType.SLONG:
                    value = BitConverter.ToInt32(data, 0);
                    break;
                case TagType.RATIONAL:
                    value = new Rational(BitConverter.ToUInt32(data, 0), BitConverter.ToUInt32(data, 4));
                    break;
                case TagType.SRATIONAL:
                    value = new SRational(BitConverter.ToInt32(data, 0), BitConverter.ToUInt32(data, 4));
                    break;
            }
        }

        public byte[] ToByteArray()
        {
            byte[] ret = null;
            switch (Type)
            {
                case TagType.ASCII:
                    ret = Encoding.ASCII.GetBytes((string)value);
                    break;
                case TagType.UNDEFINED:
                    ret = (byte[])value;
                    break;
                case TagType.BYTE:
                    ret = new byte[] { (byte)value };
                    break;
                case TagType.LONG:
                    ret = BitConverter.GetBytes((uint)value);
                    break;
                case TagType.SHORT:
                    ret = BitConverter.GetBytes((short)value);
                    break;
                case TagType.SLONG:
                    ret = BitConverter.GetBytes((int)value);
                    break;
                case TagType.RATIONAL:
                    ret = new byte[8];
                    Buffer.BlockCopy(BitConverter.GetBytes(((Rational)value).Numerator), 0, ret, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(((Rational)value).Denominator), 0, ret, 4, 4);
                    break;
                case TagType.SRATIONAL:
                    ret = new byte[8];
                    Buffer.BlockCopy(BitConverter.GetBytes(((SRational)value).Numerator), 0, ret, 0, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(((SRational)value).Denominator), 0, ret, 4, 4);
                    break;
            }

            return ret;
        }

        public virtual object GetValue() { return value; }
    }
}
