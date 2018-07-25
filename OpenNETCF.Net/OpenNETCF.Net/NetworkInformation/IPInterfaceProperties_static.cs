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
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
    public partial class IPInterfaceProperties
    {
        private const AdapterAddressFlags ANYCAST_FLAGS =
            AdapterAddressFlags.SkipMulticast | AdapterAddressFlags.SkipUnicast |
            AdapterAddressFlags.SkipDnsServer | AdapterAddressFlags.SkipFriendlyName;

        private const AdapterAddressFlags MULTICAST_FLAGS =
            AdapterAddressFlags.SkipUnicast | AdapterAddressFlags.SkipAnycast |
            AdapterAddressFlags.SkipDnsServer | AdapterAddressFlags.SkipFriendlyName;

        private const AdapterAddressFlags UNICAST_FLAGS =
            AdapterAddressFlags.SkipMulticast | AdapterAddressFlags.SkipAnycast |
            AdapterAddressFlags.SkipDnsServer | AdapterAddressFlags.SkipFriendlyName |
            AdapterAddressFlags.IncludePrefix;

        private unsafe static void GetAddressesForAdapter(byte* pData, ref UnicastIPAddressInformationCollection addressList)
        {
            addressList.InternalClear();
            /*
            typedef struct _IP_ADAPTER_ADDRESSES {
              union {
                ULONGLONG Alignment;
                struct {
                  ULONG Length;
                  DWORD IfIndex;
                };
              };
              struct _IP_ADAPTER_ADDRESSES* Next;
              PCHAR AdapterName;
              PIP_ADAPTER_UNICAST_ADDRESS FirstUnicastAddress;
              PIP_ADAPTER_ANYCAST_ADDRESS FirstAnycastAddress;
              PIP_ADAPTER_MULTICAST_ADDRESS FirstMulticastAddress;
              PIP_ADAPTER_DNS_SERVER_ADDRESS FirstDnsServerAddress;
              PWCHAR DnsSuffix;
              PWCHAR Description;
              PWCHAR FriendlyName;
              BYTE PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
              DWORD PhysicalAddressLength;
              DWORD Flags;


                typedef struct _SOCKET_ADDRESS {
                    LPSOCKADDR lpSockaddr ;
                    INT iSockaddrLength ;
                } SOCKET_ADDRESS, *PSOCKET_ADDRESS, FAR * LPSOCKET_ADDRESS ;
             
             
             */
            int addressListOffset = 0;
            byte* p = pData;
            uint* pNext = null;

            addressListOffset = 16;
            p += addressListOffset;

            /*
            typedef struct _IP_ADAPTER_UNICAST_ADDRESS {
              union {
                ULONGLONG Alignment;
                struct {
                  ULONG Length;
                  DWORD Flags;
                }
              };
              struct _IP_ADAPTER_UNICAST_ADDRESS* Next;
              SOCKET_ADDRESS Address;
              IP_PREFIX_ORIGIN PrefixOrigin;
              IP_SUFFIX_ORIGIN SuffixOrigin;
              IP_DAD_STATE DadState;
              ULONG ValidLifetime;
              ULONG PreferredLifetime;
              ULONG LeaseLifetime;
            } IP_ADAPTER_UNICAST_ADDRESS*, PIP_ADAPTER_UNICAST_ADDRESS;
            */
            uint pAddress = (uint)*((uint*)p);
            p = (byte*)pAddress;

            if (p == null)
            {
                return;
            }

            UnicastIPAddressInformation info;
            do
            {
                info = new UnicastIPAddressInformation();

                // past length
                p += 4;
                // flags
                info.AdapterFlags = (PerAdapterFlags)((int)*((int*)p));
                p += 4;
                // next
                pNext = (uint*)*((uint*)p);
                p += 4;
                // socket address pointer
                info.Address = GetAddressFromSocketAddressPointer(p);
                p += 4;
                // prefix origin
                info.PrefixOrigin = (PrefixOrigin)((int)*((int*)p));
                p += 4;
                // suffix origin
                info.SuffixOrigin = (SuffixOrigin)((int)*((int*)p));
                p += 4;
                // dad state
                info.DuplicateAddressDetectionState = (DuplicateAddressDetectionState)((int)*((int*)p));
                p += 4;
                // valid lifetime
                info.AddressValidLifetime = (uint)*((uint*)p);
                p += 4;
                // preferred lifetime
                info.AddressPreferredLifetime = (uint)*((uint*)p);
                p += 4;
                // lease lifetime
                info.DhcpLeaseLifetime = (uint)*((uint*)p);
                p += 4;

                p = (byte*)pNext;

                addressList.InternalAdd(info);
            }
            while (pNext != null);
        }
        private unsafe static void GetAddressesForAdapter(byte* pData, ref IPAddressInformationCollection addressList)
        {
            addressList.InternalClear();
            /*
            typedef struct _IP_ADAPTER_ADDRESSES {
              union {
                ULONGLONG Alignment;
                struct {
                  ULONG Length;
                  DWORD IfIndex;
                };
              };
              struct _IP_ADAPTER_ADDRESSES* Next;
              PCHAR AdapterName;
              PIP_ADAPTER_UNICAST_ADDRESS FirstUnicastAddress;
              PIP_ADAPTER_ANYCAST_ADDRESS FirstAnycastAddress;
              PIP_ADAPTER_MULTICAST_ADDRESS FirstMulticastAddress;
              PIP_ADAPTER_DNS_SERVER_ADDRESS FirstDnsServerAddress;
              PWCHAR DnsSuffix;
              PWCHAR Description;
              PWCHAR FriendlyName;
              BYTE PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
              DWORD PhysicalAddressLength;
              DWORD Flags;


                typedef struct _SOCKET_ADDRESS {
                    LPSOCKADDR lpSockaddr ;
                    INT iSockaddrLength ;
                } SOCKET_ADDRESS, *PSOCKET_ADDRESS, FAR * LPSOCKET_ADDRESS ;
             
             
             */
            int addressListOffset = 0;
            byte* p = pData;
            uint* pNext = null;

            addressListOffset = 24;
            p += addressListOffset;

            /*
                typedef struct _IP_ADAPTER_ANYCAST_ADDRESS {
                  union {
                    ULONGLONG Alignment;
                    struct {
                    ULONG Length;
                    DWORD Flags;
                    }
                  };
                  struct _IP_ADAPTER_ANYCAST_ADDRESS* Next;
                  SOCKET_ADDRESS Address;
                } IP_ADAPTER_ANYCAST_ADDRESS, *PIP_ADAPTER_ANYCAST_ADDRESS;
            */
            uint pAddress = (uint)*((uint*)p);
            p = (byte*)pAddress;

            if (p == null)
            {
                return;
            }

            IPAddressInformation info;
            do
            {
                info = new IPAddressInformation();
                // past length
                p += 4;
                // flags
                info.AdapterFlags = (PerAdapterFlags)((int)*((int*)p));
                p += 4;
                // next
                pNext = (uint*)*((uint*)p);
                p += 4;
                // socket address pointer
                info.Address = GetAddressFromSocketAddressPointer(p);
                p += 4;

                p = (byte*)pNext;

                addressList.InternalAdd(info);
            }
            while (pNext != null);
        }
        private unsafe static void GetAddressesForAdapter(byte* pData, ref MulticastIPAddressInformationCollection addressList)
        {
            addressList.InternalClear();
            /*
            typedef struct _IP_ADAPTER_ADDRESSES {
              union {
                ULONGLONG Alignment;
                struct {
                  ULONG Length;
                  DWORD IfIndex;
                };
              };
              struct _IP_ADAPTER_ADDRESSES* Next;
              PCHAR AdapterName;
              PIP_ADAPTER_UNICAST_ADDRESS FirstUnicastAddress;
              PIP_ADAPTER_ANYCAST_ADDRESS FirstAnycastAddress;
              PIP_ADAPTER_MULTICAST_ADDRESS FirstMulticastAddress;
              PIP_ADAPTER_DNS_SERVER_ADDRESS FirstDnsServerAddress;
              PWCHAR DnsSuffix;
              PWCHAR Description;
              PWCHAR FriendlyName;
              BYTE PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
              DWORD PhysicalAddressLength;
              DWORD Flags;


                typedef struct _SOCKET_ADDRESS {
                    LPSOCKADDR lpSockaddr ;
                    INT iSockaddrLength ;
                } SOCKET_ADDRESS, *PSOCKET_ADDRESS, FAR * LPSOCKET_ADDRESS ;
             
             
             */
            int addressListOffset = 0;
            byte* p = pData;
            uint* pNext = null;

            addressListOffset = 28;
            p += addressListOffset;

            /*
                typedef struct _IP_ADAPTER_MULTICAST_ADDRESS {
                  union {
                    ULONGLONG Alignment;
                    struct {
                      ULONG Length;
                      DWORD Flags;
                    }
                  };
                  struct _IP_ADAPTER_MULTICAST_ADDRESS* Next;
                  SOCKET_ADDRESS Address;
                } IP_ADAPTER_MULTICAST_ADDRESS, *PIP_ADAPTER_MULTICAST_ADDRESS;
            */
            uint pAddress = (uint)*((uint*)p);
            p = (byte*)pAddress;

            if (p == null)
            {
                return;
            }

            MulticastIPAddressInformation info;
            do
            {
                info = new MulticastIPAddressInformation();
                // past length
                p += 4;
                // flags
                info.AdapterFlags = (PerAdapterFlags)((int)*((int*)p));
                p += 4;
                // next
                pNext = (uint*)*((uint*)p);
                p += 4;
                // socket address pointer
                info.Address = GetAddressFromSocketAddressPointer(p);
                p += 4;

                p = (byte*)pNext;

                addressList.InternalAdd(info);
            }
            while (pNext != null);
        }

        private static unsafe IPAddress GetAddressFromSocketAddressPointer(byte* pData)
        {
            byte* pSockAddr = pData;
            byte* p = pData;

            p += 4;
            uint length = (uint)*((uint*)p);

            p = (byte*)*((uint*)pSockAddr);

            AddressFamily family = (AddressFamily)((short)*((short*)p));
            p += 2; // past family
            if (family == AddressFamily.InterNetwork)
            {
                /*
                    struct sockaddr_in{
                      short sin_family;
                      unsigned short sin_port;
                      IN_ADDR sin_addr;
                      char sin_zero[8];
                    };
                */
                ushort port = (ushort)*((ushort*)p);
                p += 2; // past port
                byte[] addressBytes = new byte[4];
                Marshal.Copy(new IntPtr(p), addressBytes, 0, 4);
                p += 4; // past address
                p += 8; // past padding

                return new IPAddress(addressBytes);
            }
            else // IPv6
            {
                ushort port = (ushort)*((ushort*)p);
                p += 2; // past port
                uint flowInfo = (uint)*((uint*)p);
                p += 4;
                byte[] addressBytes = new byte[16];
                Marshal.Copy(new IntPtr(p), addressBytes, 0, 16);
                p += 16;
                uint scope_id = (uint)*((uint*)p);
                p += 4;

                return new IPAddress(addressBytes, scope_id);

                /*
                    struct sockaddr_in6 {
                      short sin6_family;
                      u_short sin6_port;
                      u_long sin6_flowinfo;
                      struct in6_addr sin6_addr;
                      u_long sin6_scope_id;
                    };

                    struct in6_addr {
                      union {
                        u_char Byte[16];
                        u_short Word[8];
                      } u;
                    };
                */
            }

        }

        private unsafe static void GetAddressesForAdapter(int index, ref UnicastIPAddressInformationCollection addressList)
        {
            int length = 0;

            int ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                UNICAST_FLAGS,
                IntPtr.Zero,
                null,
                ref length));

            byte[] data = new byte[length];

            fixed (byte* pData = data)
            {
                byte* p = pData;

                ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                    UNICAST_FLAGS,
                    IntPtr.Zero,
                    p,
                    ref length));

                if ((length == 0) || (ret != 0))
                {
                    // FAILED
                }

                // find this adapter by index
                int INDEX_OFFSET = 4;

                int currentIndex = 0;
                uint pNext = 0;
                do
                {
                    p += INDEX_OFFSET; // past length
                    currentIndex = (int)*((int*)p);
                    if (index == currentIndex)
                    {
                        p -= 4;
                        GetAddressesForAdapter(p, ref addressList);
                        return;
                    }
                    p += 4; // past index
                    pNext = (uint)*((uint*)p);
                    p = (byte*)pNext;
                } while (pNext != 0);

                // failed to find index
                throw new ArgumentException("Index not found");
            }
        }
        private unsafe static void GetAddressesForAdapter(int index, ref IPAddressInformationCollection addressList)
        {
            int length = 0;

            int ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                ANYCAST_FLAGS,
                IntPtr.Zero,
                null,
                ref length));

            byte[] data = new byte[length];

            fixed (byte* pData = data)
            {
                byte* p = pData;

                ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                    ANYCAST_FLAGS,
                    IntPtr.Zero,
                    p,
                    ref length));

                if ((length == 0) || (ret != 0))
                {
                    // FAILED
                }

                // find this adapter by index
                int INDEX_OFFSET = 4;

                int currentIndex = 0;
                uint pNext = 0;
                do
                {
                    p += INDEX_OFFSET; // past length
                    currentIndex = (int)*((int*)p);
                    if (index == currentIndex)
                    {
                        p -= 4;
                        GetAddressesForAdapter(p, ref addressList);
                        return;
                    }
                    p += 4; // past index
                    pNext = (uint)*((uint*)p);
                    p = (byte*)pNext;
                } while (pNext != 0);

                // failed to find index
                throw new ArgumentException("Index not found");
            }
        }
        private unsafe static void GetAddressesForAdapter(int index, ref MulticastIPAddressInformationCollection addressList)
        {
            int length = 0;

            int ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                MULTICAST_FLAGS,
                IntPtr.Zero,
                null,
                ref length));

            byte[] data = new byte[length];

            fixed (byte* pData = data)
            {
                byte* p = pData;

                ret = (NativeMethods.GetAdaptersAddresses(AddressFamily.Unspecified,
                    MULTICAST_FLAGS,
                    IntPtr.Zero,
                    p,
                    ref length));

                if ((length == 0) || (ret != 0))
                {
                    // FAILED
                }

                // find this adapter by index
                int INDEX_OFFSET = 4;

                int currentIndex = 0;
                uint pNext = 0;
                do
                {
                    p += INDEX_OFFSET; // past length
                    currentIndex = (int)*((int*)p);
                    if (index == currentIndex)
                    {
                        p -= 4;
                        GetAddressesForAdapter(p, ref addressList);
                        return;
                    }
                    p += 4; // past index
                    pNext = (uint)*((uint*)p);
                    p = (byte*)pNext;
                } while (pNext != 0);

                // failed to find index
                throw new ArgumentException("Index not found");
            }
        }
    }
}
