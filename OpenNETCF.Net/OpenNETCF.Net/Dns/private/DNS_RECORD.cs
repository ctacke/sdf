using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace OpenNETCF.Net
{
    internal class DNS_RECORD
    {
        internal IntPtr m_address;

        internal DNS_RECORD(ARecord record)
        {
            pNext = IntPtr.Zero;
            pName = record.Name;
            wType = DNS_TYPE.IPv4Address;
            data = record.Address.GetAddressBytes();
            dwTtl = record.TTL;
            wDataLength = 4;
        }

        internal DNS_RECORD(IntPtr ptr)
        {
            m_address = ptr;
            pNext = Marshal.ReadIntPtr(m_address);
            pName = Marshal.PtrToStringUni(Marshal.ReadIntPtr(new IntPtr(m_address.ToInt32() + 4)));
            wType = (DNS_TYPE)Marshal.ReadInt16(m_address, 8);
            wDataLength = Marshal.ReadInt16(m_address, 10);
            Flags = (uint)Marshal.ReadInt32(m_address, 12);
            dwTtl = Marshal.ReadInt32(m_address, 16);

            data = new byte[wDataLength];
            Marshal.Copy(new IntPtr(m_address.ToInt32() + 24), data, 0, wDataLength);
        }

        ~DNS_RECORD()
        {
            Free();
        }

        private IntPtr pNext { get; set; }
        public string pName { get; private set; }
        internal DNS_TYPE wType { get; private set; }
        private short wDataLength { get; set; }
        public uint Flags { get; private set; }
        public int dwTtl { get; private set; }
        private uint dwReserved { get; set; }
        public byte[] data { get; private set; }

        private GCHandle? nameHandle = null;
        private GCHandle? dataHandle = null;

        internal IntPtr Pin()
        {
            Free();

            byte[] buffer = new byte[24 + data.Length];

            Buffer.BlockCopy(BitConverter.GetBytes(pNext.ToInt32()), 0, buffer, 0, 4);

            nameHandle = GCHandle.Alloc(pName, GCHandleType.Pinned);
            IntPtr ppName = nameHandle.Value.AddrOfPinnedObject();
            Buffer.BlockCopy(BitConverter.GetBytes(ppName.ToInt32()), 0, buffer, 4, 4);

            Buffer.BlockCopy(BitConverter.GetBytes((short)wType), 0, buffer, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(wDataLength), 0, buffer, 10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Flags), 0, buffer, 12, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(dwTtl), 0, buffer, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(dwReserved), 0, buffer, 20, 4);
            Buffer.BlockCopy(data, 0, buffer, 24, data.Length);

            dataHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            return dataHandle.Value.AddrOfPinnedObject();
        }

        internal void Free()
        {
            if (nameHandle != null)
            {
                nameHandle.Value.Free();
                nameHandle = null;
            }
            if (dataHandle != null)
            {
                dataHandle.Value.Free();
                dataHandle = null;
            }
        }

        //typedef struct _DnsRecord
        //{
        //    struct _DnsRecord * pNext;
        //    LPTSTR              pName;
        //    WORD                wType;
        //    WORD                wDataLength; // Not referenced for DNS record types
        //                                     // defined above.
        //    union
        //    {
        //        DWORD               DW;     // flags as DWORD
        //        DNS_RECORD_FLAGS    S;      // flags as structure

        //    } Flags;

        //    DWORD               dwTtl;
        //    DWORD               dwReserved;

        //    //  Record Data

        //    union
        //    {
        //        DNS_A_DATA      A;
        //        DNS_SOA_DATA    SOA, Soa;
        //        DNS_PTR_DATA    PTR, Ptr,
        //                        NS, Ns,
        //                        CNAME, Cname,
        //                        MB, Mb,
        //                        MD, Md,
        //                        MF, Mf,
        //                        MG, Mg,
        //                        MR, Mr;
        //        DNS_MINFO_DATA  MINFO, Minfo,
        //                        RP, Rp;
        //        DNS_MX_DATA     MX, Mx,
        //                        AFSDB, Afsdb,
        //                        RT, Rt;
        //        DNS_TXT_DATA    HINFO, Hinfo,
        //                        ISDN, Isdn,
        //                        TXT, Txt,
        //                        X25;
        //        DNS_NULL_DATA   Null;
        //        DNS_WKS_DATA    WKS, Wks;
        //        DNS_AAAA_DATA   AAAA;
        //        DNS_KEY_DATA    KEY, Key;
        //        DNS_SIG_DATA    SIG, Sig;
        //        DNS_ATMA_DATA   ATMA, Atma;
        //        DNS_NXT_DATA    NXT, Nxt;
        //        DNS_SRV_DATA    SRV, Srv;
        //        DNS_TKEY_DATA   TKEY, Tkey;
        //        DNS_TSIG_DATA   TSIG, Tsig;
        //        DNS_WINS_DATA   WINS, Wins;
        //        DNS_WINSR_DATA  WINSR, WinsR, NBSTAT, Nbstat;

        //    } Data;
        //}
        //DNS_RECORD, *PDNS_RECORD;
    }
}
