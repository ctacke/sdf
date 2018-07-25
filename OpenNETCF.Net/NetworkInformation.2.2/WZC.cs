using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    internal class WZC
    {
        public static int SetAdapter(INTF_ENTRY entry, INTF_FLAGS flags)
        {
            return NativeMethods.WZCSetInterface(null, flags, ref entry, null);
        }

        public static int QueryAdapter(string adapterName, out INTF_ENTRY entry)
        {
            // Attempt to get the status of the indicated
            // interface by calling WZCQueryInterface.  If
            // it works, we return true; if not, false.
            // Note that the first parameter, the WZC server,
            // is set to null, apparently indicating that the
            // local machine is the target.
            entry = new INTF_ENTRY();
            INTF_FLAGS flags = 0;
            entry.Guid = adapterName;
            int retVal = 0;

            try
            {
                retVal = NativeMethods.WZCQueryInterface(null, INTF_FLAGS.INTF_ALL, ref entry, out flags);
            }
            finally
            {
                NativeMethods.WZCDeleteIntfObj(ref entry);
            }

            return retVal;
        }

        #region ---- P/Invokes ----

        [DllImport("wzcsapi.dll")]
        private static extern int
            WZCQueryInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            out INTF_FLAGS pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        private static extern uint
            WZCEnumInterfaces(
            string pSrvAddr,
            ref INTFS_KEY_TABLE pIntfs);

        [DllImport("wzcsapi.dll")]
        private static extern int
            WZCSetInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            object pdwOutFlags);


        //---------------------------------------
        // WZCDeleteIntfObj: cleans an INTF_ENTRY object that is
        // allocated within any RPC call.
        // 
        // Parameters
        // pIntf
        //     [in] pointer to the INTF_ENTRY object to delete
        [DllImport("wzcsapi.dll")]
        private static extern void
            WZCDeleteIntfObj(
            ref INTF_ENTRY Intf);

        [DllImport("wzcsapi.dll")]
        private static extern void
            WZCDeleteIntfObj(
            IntPtr p);


        //---------------------------------------
        // WZCPassword2Key: Translates a user password (8 to 63 ascii chars)
        // into a 256 bit network key)  Note that the second parameter is the
        // key string, but unlike most strings, this one is using ASCII, not
        // Unicode.  We export a Unicode version and do the mapping inside
        // that.
        [DllImport("wzcsapi.dll", EntryPoint = "WZCPassword2Key")]
        private static extern void
            WZCPassword2KeyCE(
            byte[] pwzcConfig,
            byte[] cszPassword);

        #endregion

        public static void
            WZCPassword2Key(
            ref WLANConfiguration pwzcConfig,
            string cszPassword)
        {
            // Convert string from Unicode to Ascii.
            byte[] ascii = Encoding.ASCII.GetBytes(cszPassword + '\0');

            // Pass Ascii string and configuration structure
            // to the external call.
            WZCPassword2KeyCE(pwzcConfig.Data, ascii);
        }
    }
}
