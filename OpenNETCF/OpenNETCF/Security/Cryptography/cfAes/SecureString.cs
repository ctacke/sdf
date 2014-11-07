using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security.Cryptography;

using OpenNETCF.Runtime.InteropServices;
using OpenNETCF.Security.Cryptography;

//namespace NCrypto.Security.Cryptography
namespace OpenNETCF.Security
{
	/// <summary>
	/// Summary description for SecureString.
	/// </summary>
	/// <remarks>
	/// SecureStrings are held in encrypted memory by the CLR (using DPAPI), 
	/// and are only unencrypted when they are accessed.  This limits the amount of 
	/// time that your string is in plaintext for an attacker to see.  
	/// Since SecureString uses DPAPI to help secure your data, it's not available 
	/// on Windows 98, ME, or Windows 2000 with anything less than service pack 3.
	/// </remarks>
    [ComVisible(false)]	
	public sealed class SecureString : IDisposable
	{
		#region Private Fields

		private byte[] m_buffer;
		private bool m_disposed;
		private int m_length;
		private bool m_readOnly;

		// For pinned data allocations
		private GCHandle m_bh;

		private const int MaxLength = 65536;
		private const int BlockSize = 16; //ProtectedMemory.ProtectedMemoryBlockSize;

		#endregion

		#region Constructors & Finalizer

		/// <summary>
		/// Default initializer
		/// </summary>
		public SecureString()
		{
			AllocateBuffer( BlockSize );
			m_length = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Pointer to a Unicode characters array.</param>
		/// <param name="length">The number of Unicode characters of the array.</param>
		/*
		[CLSCompliant(false)]
		unsafe public SecureString(char* value, int length)
		{
			IntPtr p = new IntPtr( value );

			// Validate parameters
			if( p == IntPtr.Zero )
			{
				throw new ArgumentNullException("value"); 
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
 			}
			if (length > MaxLength)
			{
				throw new ArgumentOutOfRangeException("length"); 
			}

			AllocateBuffer( length * 2 );

			try
			{
				Marshal.Copy( p, m_buffer, 0, length * 2 );
			}
			catch(NullReferenceException)
			{
				throw new ArgumentOutOfRangeException("pointer"); //, Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
 			}

			m_length = length;
			ProtectMemory(); 
		}
		*/

		internal SecureString(SecureString str)
		{
			AllocateBuffer( str.m_buffer.Length );
			Array.Copy( str.m_buffer, 0, m_buffer, 0, str.m_buffer.Length );
			m_length = str.m_length; 
		}

		/// <summary>
		/// Finalizer
		/// </summary>
		~SecureString()
		{
			Dispose(false);
			return;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void AppendChar(char c)
		{
			EnsureNotDisposed();
			EnsureNotReadOnly();
			EnsureCapacity( m_length + 1 );
			
			try
			{
				UnProtectMemory();
				m_buffer[ m_length * 2 ] = Convert.ToByte( c );
				m_buffer[ m_length * 2 + 1 ] = 0;
				m_length++;
				return; 
			}
			finally
			{
				ProtectMemory(); 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Clear()
		{
			EnsureNotDisposed();
			EnsureNotReadOnly();
			ClearBuffer(); 
			m_length = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public SecureString Copy()
		{
			EnsureNotDisposed();
			return new SecureString(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="c"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void InsertAt(int index, char c)
		{
			EnsureNotDisposed();
			EnsureNotReadOnly();
			
			if( index < 0 || index > m_length )
			{
				throw new ArgumentOutOfRangeException("index"); //, Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			EnsureCapacity( this.m_length + 1 );
			
			try
			{
				UnProtectMemory();
				if( index < m_length )
				{
					Array.Copy( m_buffer, index * 2, m_buffer, index * 2 + 2, (m_length - index)*2 );
 				}
				m_buffer[ index * 2 ] = Convert.ToByte( c );
				m_buffer[ index * 2 + 1 ] = Byte.MinValue;
				m_length++;				
				return;
			}
			finally
			{
				ProtectMemory();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public bool IsReadOnly()
		{
			EnsureNotDisposed();
			return m_readOnly;
		}

		/// <summary>
		/// 
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void MakeReadOnly()
		{
			EnsureNotDisposed();
			m_readOnly = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void RemoveAt(int index)
		{			
			EnsureNotDisposed();
			EnsureNotReadOnly();

			if( index < 0 || index >= m_length )
			{
				throw new ArgumentOutOfRangeException("index"); //, Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}

			try
			{
				UnProtectMemory();
				m_length--;
				if( index < m_length )
				{
					Array.Copy( m_buffer, index * 2 + 2, m_buffer, index*2, (m_length - index)*2 );
 				}
				m_buffer[ m_length * 2 ] = Byte.MinValue;
				return;
			}
			finally
			{
				this.ProtectMemory();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="c"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SetAt(int index, char c)
		{
			EnsureNotDisposed();
			EnsureNotReadOnly();
			
			if( index < 0 || index >= m_length )
			{
				throw new ArgumentOutOfRangeException("index"); //, Environment.GetResourceString("ArgumentOutOfRange_Index"));
 			}
			
			try
			{
				UnProtectMemory();
				m_buffer[ index*2 ] = Convert.ToByte( c );
				return;
			}
			finally
			{
				ProtectMemory();
			}
		} 

		/// <summary>
		/// 
		/// </summary>
		public int Length
		{
			get
			{
				EnsureNotDisposed();
				return m_length;
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Dispose(bool disposing)
		{
			if( !m_disposed )
			{
				ClearBuffer();
				m_disposed = true; 
			} 
		}

		#endregion

		#region Marshal SecureString extensions

		/// <summary>
		/// Getting data out of the SecureString.
		/// </summary>
		/// <remarks>
		/// This method is located in the <see cref="Marshal"/> class in .NET Framework v2.0
		/// </remarks>
		public static IntPtr SecureStringToGlobalAllocUni(SecureString s)
		{
			if( s == null )
			{
				throw new ArgumentNullException("s");
			}
			return s.ToUniStr(true);
		}

		/// <summary>
		/// This will the allocated memory from method <see cref="SecureStringToGlobalAllocUni"/>.
		/// </summary>
		/// <remarks>
		/// This method is located in the <see cref="Marshal"/> class in .NET Framework v2.0
		/// </remarks>
		public static void ZeroFreeGlobalAllocUni(IntPtr s)
		{
			// Demand Permission
			//new CryptographicPermission( CryptographicPermissionFlags.Decrypt ).Demand();
			//Win32Native.ZeroMemory( s, (uint)Win32Native.lstrlenW(s) * 2 );
			//int strLen = Win32Native.lstrlen(s);
			//Win32Native.ZeroMemory( s, (uint)Win32Native.lstrlen(s) * 2 );
			int strLen = Marshal.SizeOf(s);
			Win32Native.memset(s, 0, (uint) strLen * 2);
			MarshalEx.FreeHGlobal(s); 
		}

		#endregion

		#region Internal Methods

		[MethodImpl(MethodImplOptions.Synchronized)]
		internal IntPtr ToUniStr(bool allocateFromHeap)
		{
			IntPtr ptr;
			EnsureNotDisposed();

			// Demand Permission
			//new CryptographicPermission( CryptographicPermissionFlags.Decrypt ).Demand();

			if( allocateFromHeap )
			{
				ptr = MarshalEx.AllocHGlobal( (m_length+1) * 2 );
			}
			else
			{
				ptr = MarshalEx.AllocHLocal( (m_length+1) * 2 );
				//ptr = MarshalEx.AllocCoTaskMem( (m_length+1) * 2 );
			}

			try
			{
				this.UnProtectMemory();
				Win32Native.memset(ptr, 0, (uint)((m_length+1) * 2) );
				//Win32Native.ZeroMemory( ptr, (uint)((m_length+1) * 2) );
				Marshal.Copy( m_buffer, 0, ptr, m_length * 2 );
				return ptr;
			}
			finally
			{
				// Assert Permission
				//new CryptographicPermission( CryptographicPermissionFlags.Encrypt ).Assert();
				ProtectMemory();
			}
		}

		#endregion

		#region Private Methods

		private void AllocateBuffer(int size)
		{
			m_buffer = new byte[ GetAlignedSize( size ) ]; 
			m_bh = GCHandle.Alloc( m_buffer, GCHandleType.Pinned );
		}

		private void ClearBuffer()
		{
			// Clear the byte array
		 	Array.Clear( m_buffer, 0, m_buffer.Length );
			
			// Zero out the memory buffer
			Win32Native.memset(m_bh.AddrOfPinnedObject(), 0, (uint)m_buffer.Length);
			//Win32Native.ZeroMemory( m_bh.AddrOfPinnedObject(), (uint)m_buffer.Length );

			// Release the allocated memory
			if( m_bh.IsAllocated )
			{
				m_bh.Free();
			}
		}

		private void EnsureCapacity(int capacity)
		{
			capacity *= 2; //(unicode size);

			if( capacity <= m_buffer.Length )
			{
				return;
 			}
			if (capacity > MaxLength)
			{
				throw new ArgumentOutOfRangeException("capacity"); //, Environment.GetResourceString("ArgumentOutOfRange_Capacity"));
 			}

			// Set new capacity
			byte[] newBuffer = new byte[ GetAlignedSize(capacity) ];

			Array.Copy( m_buffer, 0, newBuffer, 0, m_buffer.Length );
			ClearBuffer();
			AllocateBuffer( capacity ); 
			Array.Copy( newBuffer, 0, m_buffer, 0, m_buffer.Length );
		}

		private void EnsureNotDisposed()
		{
			if( m_disposed )
			{
				throw new ObjectDisposedException(null);
			}
		}

		private void EnsureNotReadOnly()
		{
			if( m_readOnly )
			{
				throw new InvalidOperationException(); //Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		private int GetAlignedSize(int size)
		{
			int blkSize = (size / BlockSize) * BlockSize;
			
			if( (size % BlockSize) == 0 && size != 0 )
			{
				return blkSize;
			}
			return ( blkSize + BlockSize );
		}

		// This method will demand CryptographicPermissionFlags.Encrypt = true
		private void ProtectMemory()
		{
			if( m_length == 0 )
			{
				return;
			}
			ProtectedMemory.Protect( m_buffer, MemoryProtectionScope.SameProcess );
		}

		private void UnProtectMemory()
		{
			if( m_length == 0 )
			{
				return;
			}

			// This is an internal operation so stop the Decrypt Demand
			//new CryptographicPermission( CryptographicPermissionFlags.Decrypt ).Assert();

			ProtectedMemory.Unprotect( m_buffer, MemoryProtectionScope.SameProcess );
		}

		#endregion
	}

	//[SuppressUnmanagedCodeSecurity]
	internal sealed class Win32Native
	{
		//[DllImport("coredll.dll", CharSet=CharSet.Unicode)]
		//internal static extern void ZeroMemory(IntPtr handle, uint lenght);

		[DllImport("coredll.dll", CharSet=CharSet.Unicode)]
		internal static extern void memset(IntPtr handle, uint start, uint length);

		[DllImport("coredll.dll", CharSet=CharSet.Unicode)]
		public static extern int lstrlen(IntPtr ptr);
		//public static extern int lstrlenW(IntPtr ptr);
	}
}
