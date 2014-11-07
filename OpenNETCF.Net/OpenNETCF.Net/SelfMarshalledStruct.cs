using System;
using System.Text;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Summary description for SelfMarshalledStruct.
	/// </summary>
	internal class SelfMarshalledStruct
	{
		public SelfMarshalledStruct(int size)
		{
			data = new byte[size];
		}

		public Char GetChar( int offset )
		{
			return BitConverter.ToChar(data, offset);
		}

		public void SetChar( int offset, Char val )
		{
			Buffer.BlockCopy(BitConverter.GetBytes( val ), 0, data, offset, 2 );
		}

		public Int32 GetInt32( int offset )
		{
			return BitConverter.ToInt32(data, offset);
		}
		
		public void SetInt32( int offset, Int32 val )
		{
			Buffer.BlockCopy(BitConverter.GetBytes( val ), 0, data, offset, 4 );
		}

		public UInt32 GetUInt32( int offset )
		{
			return BitConverter.ToUInt32(data, offset);
		}
		
		public void SetUInt32( int offset, UInt32 val )
		{
			Buffer.BlockCopy(BitConverter.GetBytes( val ), 0, data, offset, 4 );
		}

		public Int16 GetInt16( int offset )
		{
			return BitConverter.ToInt16(data, offset);
		}
		
		public void SetInt16( int offset, Int16 val )
		{
			Buffer.BlockCopy(BitConverter.GetBytes( val ), 0, data, offset, 2 );
		}

		public UInt16 GetUInt16( int offset )
		{
			return BitConverter.ToUInt16(data, offset);
		}
		
		public void SetUInt16( int offset, UInt16 val )
		{
			Buffer.BlockCopy(BitConverter.GetBytes( val ), 0, data, offset, 2 );
		}

		public string GetStringUni(int offset, int len)
		{
			return Encoding.Unicode.GetString(data, offset, len).TrimEnd('\0');
		}

		public void SetStringUni(string str, int offset, int len)
		{
			Encoding.Unicode.GetBytes(str, 0, Math.Min(str.Length, len), data, offset);
		}

		public byte this[int offset]
		{
			get { return data[offset]; }
			set { data[offset] = value; }
		}

		public object Get(Type t, int offset)
		{
			if ( t.IsPrimitive )
			{
				if ( t.BaseType == typeof(Int32) || t == typeof(Int32))
					return GetInt32(offset);
				else if ( t.BaseType == typeof(Int16) || t == typeof(Int16) )
					return GetInt16(offset);
				else if ( t.BaseType == typeof(UInt32) || t == typeof(UInt32) )
					return GetInt32(offset);
				else if ( t.BaseType == typeof(UInt16) || t == typeof(UInt16) )
					return GetUInt16(offset);
				else if ( t.BaseType == typeof(byte)|| t == typeof(byte) )
					return this[offset];
			}
			return null;
		}

		public void Set(Type t, int offset, object Val)
		{
			if ( t.IsPrimitive )
			{
				if ( t.BaseType == typeof(Int32) || t == typeof(Int32))
					SetInt32(offset, (int)Val);
				else if ( t.BaseType == typeof(Int16) || t == typeof(Int16) )
					SetInt16(offset, (short)Val);
				else if ( t.BaseType == typeof(UInt32) || t == typeof(UInt32) )
					SetUInt32(offset, (UInt32)Val);
				else if ( t.BaseType == typeof(UInt16) || t == typeof(UInt16) )
					SetUInt16(offset, (ushort)Val);
				else if ( t.BaseType == typeof(byte)|| t == typeof(byte) )
					this[offset] = (byte)Val;
			}
			else if ( t == typeof(string) )
			{
				string s = (string)Val + '\0';
				SetStringUni(s, offset, s.Length);
			}
		}

		protected byte[] data;
		public byte[] Data { get { return data; } }
	}
}
