using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Allows an application to determine whether a remote computer is accessible over the network.
	/// </summary>
	public class Ping : Component, IDisposable
	{
		private const int DefaultSendBufferSize = 64;
		private byte [] _defaultSendBuffer;
		private IntPtr _handlePingV4 = IntPtr.Zero;
		private IntPtr _replyBuffer;
		private IntPtr _requestBuffer;
		private bool _disposed;


		/// <summary>
		/// Initializes a new instance of the Ping class.
		/// </summary>
		public Ping()
		{
		}

		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (<c>ICMP</c>) echo message.
		/// </summary>
		public PingReply Send(string hostNameOrAddress)
		{
			return Send(hostNameOrAddress, SendBuffer, 5000, null);
		}

		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (<c>ICMP</c>) echo message.
		/// </summary>
		/// <param name="hostNameOrAddress"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public PingReply Send(string hostNameOrAddress, int timeout)
		{
			return Send(hostNameOrAddress, SendBuffer, timeout, (PingOptions) null);
		}

		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (<c>ICMP</c>) echo message.
		/// </summary>
		/// <param name="hostNameOrAddress"></param>
		/// <param name="buffer"></param>
		/// <param name="timeout"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public PingReply Send(string hostNameOrAddress, byte[] buffer, int timeout, PingOptions options)
		{
			if ("".CompareTo(hostNameOrAddress) >= 0)
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}

			IPAddress address;
			try
			{
				address = IPAddress.Parse(hostNameOrAddress);
			}
			catch
			{
				try
				{
                    IPHostEntry entry = Dns.GetHostEntry(hostNameOrAddress);
					address = entry.AddressList[0];
				}
				catch(Exception e)
				{
					throw new PingException("Impossible to resolve host or address.", e);
				}
			}

			return Send(address, buffer, timeout, options);
		}
 
		/// <summary>
		/// Attempts to send an Internet Control Message Protocol (<c>ICMP</c>) echo message.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="buffer"></param>
		/// <param name="timeout"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public PingReply Send(IPAddress address, byte[] buffer, int timeout, PingOptions options)
		{
			PingReply reply;
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}

			try
			{
				reply = InternalSend(address, buffer, timeout, options);
			}
			catch(Exception e)
			{
				throw new PingException("Impossible to send a packet.", e);
			}

			return reply;
		}

        private byte [] SendBuffer
        {
        	get
        	{
        		if (_defaultSendBuffer == null)
        		{
					_defaultSendBuffer = new byte[DefaultSendBufferSize];
                    for(int i = 1; i < DefaultSendBufferSize; i++)
                    {
						_defaultSendBuffer[i] = (byte)i;
                    }
        		}

				return _defaultSendBuffer;
        	}
        }


		private PingReply InternalSend(IPAddress address, byte [] buffer, int timeout, PingOptions options)
		{
			if (_handlePingV4 == IntPtr.Zero) _handlePingV4 = IcmpCreateFile();
			if (_replyBuffer == IntPtr.Zero)
			{
                _replyBuffer = Marshal.AllocHGlobal(0xffff);
                OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(_replyBuffer, 0, 0xffff, false);
            }

			IPOptions ipo = new IPOptions(options);
			InitStructure(buffer);
			IcmpSendEcho2(_handlePingV4, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, BitConverter.ToUInt32(address.GetAddressBytes(), 0), _requestBuffer, (ushort)buffer.Length, ref ipo, _replyBuffer, 0xffff, (uint)timeout);
			FreeStructure();

			IcmpEchoReply reply = (IcmpEchoReply)Marshal.PtrToStructure(this._replyBuffer, typeof(IcmpEchoReply));
			return new PingReply(reply);
		}

		private void InitStructure(byte[] buffer)
		{
            _requestBuffer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, _requestBuffer, buffer.Length);
		}

		private void FreeStructure()
		{
			if (_requestBuffer != IntPtr.Zero)
			{
                Marshal.FreeHGlobal(_requestBuffer);
				_requestBuffer = IntPtr.Zero;
			}
		}
 
		#region Disposing
        /// <summary>
        /// Disposes internally allocated resources
        /// </summary>
        /// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
            }
			this._disposed = true;

			if (this._replyBuffer != IntPtr.Zero)
			{
                Marshal.FreeHGlobal(_replyBuffer);
				_replyBuffer = IntPtr.Zero;
			}
			if (this._handlePingV4 != IntPtr.Zero)
			{
				IcmpCloseHandle(_handlePingV4);
				_handlePingV4 = IntPtr.Zero;
			}
		}
		
		void System.IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Ping()
		{
			Dispose(false);
		}
		#endregion

		[DllImport("iphlpapi.dll")]
		internal static extern uint IcmpSendEcho2(IntPtr icmpHandle, IntPtr Event, IntPtr apcRoutine, IntPtr apcContext, uint ipAddress, IntPtr data, ushort dataSize, ref IPOptions options, IntPtr replyBuffer, uint replySize, uint timeout);

		[DllImport("iphlpapi.dll")]
		internal static extern IntPtr IcmpCreateFile();

        [DllImport("iphlpapi.dll")]
		internal static extern bool IcmpCloseHandle(IntPtr handle);
	}
}
