using System;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.IO
{
	/// <summary>
	/// This class is used to access memory mapped addresses
	/// You can cause serious problems using this class without knowing what you're doing!
	/// We reiterate the statement in our license that OpenNETCF provides absolutely no warranty on this code and you use it at your own risk
	/// </summary>
    [CLSCompliant(false)]
	public class PhysicalAddressPointer : IDisposable
	{
		// use 4k pages
		private const uint PAGE_SIZE  = 0x1000; 

		private IntPtr m_virtualAddress = IntPtr.Zero;
		private IntPtr m_addressPointer = IntPtr.Zero;

		/// <summary>
		/// An accessor class to a physical memory address.
		/// </summary>
		/// <param name="physicalAddress">Physical Address to map</param>
		/// <param name="size">Minimum size of the desired allocation</param>
		/// <remarks>The physical address does not need to be aligned as the PhysicalAddressPointer will handle alignment
		/// The size value will aligned to the next multiple of 4k internally, so the actual allocation may be larger than the requested value</remarks>
		public PhysicalAddressPointer(uint physicalAddress, uint size)
		{
			m_addressPointer = MapPhysicalAddress(physicalAddress, size);
		}

		/// <summary>
		/// An accessor class to a physical memory address.
		/// </summary>
		/// <param name="physicalAddress">Physical Address to map</param>
		/// <param name="size">Minimum size of the desired allocation</param>
		/// <remarks>The physical address does not need to be aligned as the PhysicalAddressPointer will handle alignment
		/// The size value will aligned to the next multiple of 4k internally, so the actual allocation may be larger than the requested value</remarks>
		public PhysicalAddressPointer(uint physicalAddress, int size)
		{
			m_addressPointer = MapPhysicalAddress(physicalAddress, (uint)size);
		}

		~PhysicalAddressPointer()
		{
			this.Dispose();
		}

		/// <summary>
		/// Write an array of bytes to the mapped physical address
		/// </summary>
		/// <param name="bytes">data to write</param>
		public void WriteBytes(byte[] bytes)
		{
			Marshal.Copy(bytes, 0, m_addressPointer, bytes.Length);
		}

		/// <summary>
		/// Write a 32-bit value to the mapped address
		/// </summary>
		/// <param name="data">data to write</param>
		public void WriteInt32(int data)
		{
            WriteInt32(data, 0);
		}

		/// <summary>
		/// Write a 32-bit value to the mapped address
		/// </summary>
		/// <param name="data">data to write</param>
        /// <param name="offset">offset to start writing the data</param>
		public unsafe void WriteInt32(int data, int offset)
		{
            int baseAddr = (m_addressPointer.ToInt32() + offset);
            if (baseAddr % 4 == 0)
            {
                // dword aligned
                uint* pDest = (uint*)(baseAddr);
                *pDest = (uint)data;
            }
            else if(baseAddr % 2 == 0)
            {
                // word aligned
                ushort* pDest = (ushort*)(baseAddr);
                *pDest = (ushort)(data >> 0x10);
                pDest += 2;
                *pDest = (ushort)(data & 0xFFFF);
            }
            else
            {
                // byte aligned
                byte* pDest = (byte*)(baseAddr);
                foreach(byte b in BitConverter.GetBytes(data))
                {
                    *pDest = b;
                    pDest++;
                }
            }
        }

		/// <summary>
		/// Write a 16-bit value to the mapped address
		/// </summary>
		/// <param name="data">data to write</param>
		public void WriteInt16(short data)
		{
			Marshal.WriteInt16(m_addressPointer, data);
		}

		/// <summary>
		/// Write an 8-bit value to the mapped address
		/// </summary>
		/// <param name="data">data to write</param>
		public void WriteByte(byte data)
		{
			Marshal.WriteByte(m_addressPointer, data);
		}

		/// <summary>
		/// Read a series of bytes from the mapped address
		/// </summary>
		/// <param name="length">number of bytes to read</param>
		/// <returns>read data</returns>
		public byte[] ReadBytes(int length)
		{
			byte[] bytes = new byte[length];
			Marshal.Copy(m_addressPointer, bytes, 0, length);
			return bytes;
		}

		/// <summary>
		/// Read a 32-bit value from the mapped address
		/// </summary>
		/// <returns>read value</returns>
		public int ReadInt32()
		{
			return Marshal.ReadInt32(m_addressPointer);
		}

		/// <summary>
		/// Read a 32-bit value from the mapped address
		/// </summary>
		/// <returns>read value</returns>
		public int ReadInt32(int offset)
		{
			return Marshal.ReadInt32(m_addressPointer, offset);
		}

		/// <summary>
		/// Read a 16-bit value from the mapped address
		/// </summary>
		/// <returns>read value</returns>
		public short ReadInt16()
		{
			return Marshal.ReadInt16(m_addressPointer);
		}

		/// <summary>
		/// Read an 8-bit value from the mapped address
		/// </summary>
		/// <returns>read value</returns>
		public byte ReadByte()
		{
			return Marshal.ReadByte(m_addressPointer);
		}

		IntPtr MapPhysicalAddress(uint physicalAddress, uint size)
		{
			uint alignedAddress = 0;
			uint offset   = 0;
			uint alignedSize  = 0;
			IntPtr returnAddress = IntPtr.Zero;
   
   
			// get a page aligned address
			alignedAddress = PageAlignAddress(physicalAddress);
			offset = physicalAddress - alignedAddress;

			// get a page aligned size
			alignedSize = RoundSizeToNextPage(size + offset);

			// reserve some virtual memory
            m_virtualAddress = NativeMethods.VirtualAlloc(0, alignedSize, NativeMethods.MEM_RESERVE, NativeMethods.PAGE_NOACCESS);

			// sanity check
			if(m_virtualAddress == IntPtr.Zero)
			{
				// allocation failure!
				return IntPtr.Zero;
			}

   
			// Map physical memory to virtual memory
            if (NativeMethods.VirtualCopy(m_virtualAddress, (IntPtr)(alignedAddress >> 8), alignedSize,
                NativeMethods.PAGE_READWRITE | NativeMethods.PAGE_NOCACHE | NativeMethods.PAGE_PHYSICAL) == 0)
			{
				// copy failure!
                NativeMethods.VirtualFree(m_virtualAddress, 0, NativeMethods.MEM_RELEASE);

				return IntPtr.Zero;
			}

			// offset and return
			return new IntPtr(m_virtualAddress.ToInt32() + offset);
		}

		// simply aligns an address to a page boundary to prevent data aborts and fun stuff like that
		uint PageAlignAddress(uint addressToAlign)
		{
			return addressToAlign & ~(PAGE_SIZE -1);
		}

		// allocations must be made in page multiples.  
		// this method finds the next multiple given a desired size
		uint RoundSizeToNextPage(uint size)
		{
			return (size + PAGE_SIZE - 1) & ~(PAGE_SIZE - 1);
		}

		[CLSCompliant(false)]
		public unsafe void *GetUnsafePointer()
		{
			return m_virtualAddress.ToPointer();
		}

		public void Dispose()
		{
			if(m_virtualAddress != IntPtr.Zero)
			{
                NativeMethods.VirtualFree(m_virtualAddress, 0, NativeMethods.MEM_RELEASE);
			}
		}

		/// <summary>
		/// Gets an IntPtr for the base of the PhysicalAddressPointer
		/// </summary>
		/// <param name="pap">PhysicalAddressPointer to get the address of</param>
		/// <returns>IntPtr of the base virtual address</returns>
		public static explicit operator IntPtr(PhysicalAddressPointer pap)
		{
			return pap.m_virtualAddress;
		}
	}
}
