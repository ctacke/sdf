using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace OpenNETCF.Net.NetworkInformation
{
    internal class WZC
    {
        public static int SetAdapter(INTF_ENTRY entry, INTF_FLAGS flags)
        {
            return WZC.WZCSetInterface(null, flags, ref entry, null);
        }

        public static void ResetAdapter(string adapterName)
        {
            INTF_ENTRY_EX intf = new INTF_ENTRY_EX();
            intf.Guid = adapterName;
            INTF_FLAGS flags = 0;

            try
            {
                WZCSetInterfaceEx(null, INTF_FLAGS.INTF_PREFLIST, ref intf, out flags);
            }
            catch (MissingMethodException)
            {
                throw new PlatformNotSupportedException("The required OS components for this method are not present.");
            }
        }

        internal static List<IAccessPoint> GetAPs(string adapterName)
        {
            var apList = new List<IAccessPoint>();

            INTF_ENTRY entry = new INTF_ENTRY();
            entry.Guid = adapterName;
            INTF_FLAGS flags = 0;

            int result = WZCQueryInterface(null, INTF_FLAGS.INTF_ALL, ref entry, out flags);

            if (result != 0)
            {
                entry.Dispose();
                throw new Exception("WZCQueryInterface failed for " + adapterName);
            }

            try
            {
                // Figure out how many SSIDs there are.
                if (entry.rdBSSIDList.cbData == 0)
                {
                    // list is empty
                    return apList;
                }

                NDIS_802_11_BSSID_LIST rawlist = new NDIS_802_11_BSSID_LIST(entry.rdBSSIDList.lpData, true);

                for (int i = 0; i < rawlist.NumberOfItems; i++)
                {
                    // Get the next raw item from the list.
                    BSSID bssid = rawlist.Item(i);

                    // Using the raw item, create a cooked 
                    // SSID item.
                    AccessPoint ssid = new AccessPoint(bssid);

                    // Add the new item to this.
                    apList.Add(ssid);
                }

                return apList;
            }
            finally
            {
                WZCDeleteIntfObj(ref entry);
            }
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
                retVal = WZC.WZCQueryInterface(null, INTF_FLAGS.INTF_ALL, ref entry, out flags);
            }
            catch(Exception ex)
            {
                // on a throw, the return value needs to get set to a non-zero
                try
                {
                    WZC.WZCDeleteIntfObj(ref entry);
                }
                catch { }
                return -1;
            }

            return retVal;
        }

        #region ---- P/Invokes ----

        [DllImport("wzcsapi.dll")]
        internal static extern int
            WZCQueryInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            out INTF_FLAGS pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        internal static extern uint
            WZCEnumInterfaces(
            string pSrvAddr,
            ref INTFS_KEY_TABLE pIntfs);

        [DllImport("wzcsapi.dll")]
        internal static extern int
            WZCSetInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            out INTF_FLAGS pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        public static extern int
            WZCSetInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            object pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        internal static extern int
            WZCSetInterfaceEx(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY_EX pIntf,
            out INTF_FLAGS pdwOutFlags);

        //---------------------------------------
        // WZCDeleteIntfObj: cleans an INTF_ENTRY object that is
        // allocated within any RPC call.
        // 
        // Parameters
        // pIntf
        //     [in] pointer to the INTF_ENTRY object to delete
        [DllImport("wzcsapi.dll")]
        internal static extern void
            WZCDeleteIntfObj(
            ref INTF_ENTRY Intf);

        [DllImport("wzcsapi.dll")]
        internal static extern void
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
            byte[] ascii = new byte[64];
            byte[] src = Encoding.ASCII.GetBytes(cszPassword);
            Buffer.BlockCopy(src, 0, ascii, 0, src.Length);

            // Pass Ascii string and configuration structure
            // to the external call.
            WZCPassword2KeyCE(pwzcConfig.Data, ascii);
        }
    }
}
