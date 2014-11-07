using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    internal struct SEC_WINNT_AUTH_IDENTITY
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string User;
        public int UserLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;
        public int DomainLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public int PasswordLength;
        public int Flags;

        //typedef struct _SEC_WINNT_AUTH_IDENTITY {
        //  unsigned short _RPC_FAR* User;
        //  unsigned long UserLength;
        //  unsigned short _RPC_FAR* Domain;
        //  unsigned long DomainLength;
        //  unsigned long _RPC_FAR* Password;
        //  unsigned long PasswordLength;
        //  unsigned long Flags;
        //} SEC_WINNT_AUTH_IDENTITY, *PSEC_WINNT_AUTH_IDENTITY;
    }
}
