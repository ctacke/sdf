using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Encapsulates a Wireless Zero Config (WZC) network interface
    /// </summary>
    public class WirelessZeroConfigNetworkInterface : WirelessNetworkInterface
    {
        internal WirelessZeroConfigNetworkInterface(int index, string name)
            : base(index, name)
        {
        }

        /// <summary>
        /// Gets the WEP status for the current adapter
        /// </summary>
        public override WEPStatus WEPStatus
        {
            get
            {
                INTF_ENTRY entry;
                if (WZC.QueryAdapter(this.Name, out entry) == 0)
                {
                    WEPStatus status = entry.nWepStatus;
                    entry.Dispose();
                    return status;
                }
                return WEPStatus.Unknown;
            }
        }

        /// <summary>
        /// Gets the AuthenticationMode for the current adapter
        /// </summary>
        public override AuthenticationMode AuthenticationMode
        {
            get
            {
                INTF_ENTRY entry;
                if (WZC.QueryAdapter(this.Name, out entry) == 0)
                {
                    AuthenticationMode mode = entry.nAuthMode;
                    entry.Dispose();
                    return mode;
                }

                return AuthenticationMode.Unknown;
            }
        }

        /// <summary>
        /// Enables or disables WZC Fallback for the current adapter
        /// </summary>
        /// <returns>
        /// Returns true/false if WZC Fallback is enabled for the current adapter
        /// </returns>
        public bool FallbackEnabled
        {
            set
            {
                if (NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    return;


                // Create the entry that will be passed to WZCSetInterface.
                INTF_ENTRY entry = new INTF_ENTRY();

                if (value)
                {
                    entry.dwCtlFlags |= (uint)INTF_FLAGS.INTF_FALLBACK;
                }
                else
                {
                    entry.dwCtlFlags &= ~(uint)INTF_FLAGS.INTF_FALLBACK;
                }

                int retVal = WZC.SetAdapter(entry, INTF_FLAGS.INTF_FALLBACK);
                if(retVal > 0)
                {
                    throw new System.ComponentModel.Win32Exception(retVal, "Unable to set WZC interface");
                }

                entry.Dispose();
            }
            get
            {
                if (NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    return false;

                INTF_ENTRY entry;
                if (WZC.QueryAdapter(this.Name, out entry) == 0)
                {
                    return true;
                }
                entry.Dispose();
                return false;
            }
        }

        /// <summary>
        /// Returns a list of the SSID values which the 
        /// adapter can currently 'hear'.
        /// </summary>
        /// <returns>
        /// SSIDList instance containing the SSIDs.
        /// </returns>
        public AccessPointCollection NearbyAccessPoints
        {
            get { return (new AccessPointCollection(this)); }
        }

        /// <summary>
        /// Returns the list of preferred SSID values which the 
        /// adapter is currently assigned.  Note that none of
        /// these might be within range, however.
        /// </summary>
        /// <returns>
        /// SSIDList instance containing the preferred SSIDs.
        /// </returns>
        public AccessPointCollection PreferredAccessPoints
        {
            get { return (new AccessPointCollection(this, false)); }
        }

        /// <summary>
        /// Returns the list of preferred SSID values which the 
        /// adapter is currently assigned, but also updates the
        /// signal strengths to their current values.  Otherwise,
        /// the signal strengths are not really valid.
        /// </summary>
        /// <returns>
        /// SSIDList instance containing the preferred SSIDs.
        /// </returns>
        public AccessPointCollection NearbyPreferredAccessPoints
        {
            get { return (new AccessPointCollection(this, true)); }
        }

        #region --- SetWirelessSettings calls ---
        #region internal use calls
        /// <summary>
        /// This routine is used, internally, to make entries
        /// for the preferred SSID list.  It centralizes the
        /// mapping of key data into the structure, etc.
        /// </summary>
        /// <param name="SSID">
        /// The SSID string for the new entry
        /// </param>
        /// <param name="bInfrastructure">
        /// Set to true for infrastucture mode; false for
        /// ad hoc mode
        /// </param>
        /// <param name="Key">
        /// WEP key material
        /// </param>
        /// <param name="keyIndex">
        /// Key index.  Valid values are 1-4
        /// </param>
        /// <param name="authMode">
        /// Authentication mode for the connection
        /// </param>
        /// <param name="privacyMode">
        /// Privacy (encryption) mode for the connection
        /// </param>
        /// <returns>
        /// New WZC_WLAN_CONFIG object or null on failure
        /// </returns>
        private WLANConfiguration MakeSSIDEntry(string SSID, bool bInfrastructure,
            byte[] Key, int keyIndex,
            AuthenticationMode authMode, WEPStatus privacyMode)
        {
            WLANConfiguration thisConfig = new WLANConfiguration();

            // Set the length.
            thisConfig.Length = thisConfig.Data.Length;

            // Set the MAC address.
            thisConfig.MACAddress = this.GetPhysicalAddress().GetAddressBytes();

            // Set the SSID.
            thisConfig.SSID = SSID;

            // Save the privacy mode.
            thisConfig.Privacy = privacyMode;

            // Set the key index.  Note that, since the 'first'
            // key is key #1 in all of the wireless specifications,
            // we have to decrement the value for WZC, which expects
            // it to be 0.
            thisConfig.KeyIndex = keyIndex - 1;

            // Proceed with configuration.
            byte[] arrKey = null;
            if (Key != null)
            {
                // Key size has already been checked (this
                // is an entry invariant).

                arrKey = Key.Clone() as byte[];
                thisConfig.KeyLength = arrKey.Length;
                thisConfig.CtlFlags |= WZCControl.WEPKPresent | WZCControl.WEPKXFormat;
                if (arrKey.Length == 10)
                    thisConfig.CtlFlags |= WZCControl.WEPK40Bit;
                byte[] chFakeKeyMaterial = new byte[] { 0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66 };
                for (int i = 0; i < arrKey.Length; i++)
                    arrKey[i] ^= chFakeKeyMaterial[(7 * i) % 13];
                thisConfig.KeyMaterial = arrKey;
            }
            else
            {
                // Clear the key material, as well as setting
                // the length to zero.
                byte[] key = new byte[] 
					{ 0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 
					  0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0 };

                thisConfig.KeyMaterial = key;
                thisConfig.KeyLength = 0;
            }
            thisConfig.AuthenticationMode = authMode;

            // ???? do the right thing, based on the mode.

            // If we have no key, we should probably set this to WEP Off.
            thisConfig.InfrastructureMode = bInfrastructure ? InfrastructureMode.Infrastructure : InfrastructureMode.AdHoc;

            return thisConfig;
        }

        /// <summary>
        /// The KeyType enumeration tells the Adapter what
        /// sort of key material is being passed.  There are
        /// several types for various forms of WEP and WPA
        /// keys.  External callers of Adapter methods should
        /// set it to None, however.
        /// </summary>
        internal enum KeyType
        {
            None,
            WPAPassphrase,
            WPABinary,
            WEP
        }

        /// <summary>
        /// The ProcessKey routine makes necessary modifications
        /// to the key material of a WPA key before it is passed
        /// to WZC routines.  The processing done to it depends
        /// on how it was generated.
        /// </summary>
        /// <param name="kt">
        /// The key type, indicating how the key material in
        /// the structure was originally generated
        /// </param>
        /// <param name="config">
        /// The configuration being changed
        /// </param>
        /// <param name="passphrase">
        /// For WPA-PSK passphrase type, the passphrase.
        /// </param>
        internal void ProcessKey(KeyType kt, ref WLANConfiguration config,
            string passphrase)
        {
            // Define fake key material for 'encrypting' the
            // keys.
            byte[] chFakeKeyMaterial = new byte[] { 0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66 };
            byte[] key;
            uint i;

            switch (kt)
            {
                case KeyType.WPAPassphrase:
                    // We set this explicitly here.  It was set
                    // out of line in the NetUI code.
                    config.Privacy = WEPStatus.TKIPEnabled;

                    config.CtlFlags |= WZCControl.WEPKXFormat | WZCControl.ONEXEnabled;

                    NativeMethods.WZCPassword2Key(
                        ref config, passphrase);

                    // Tell it to just encrypt and no conversion.
                    config.KeyLength = WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL;
                    config.CtlFlags |= WZCControl.WEPKPresent;

                    // Note that, since the config structure doesn't
                    // actually have a byte[] for key material, we
                    // can't modify bytes of that 'array' in-place.
                    key = config.KeyMaterial;
                    for (i = 0; i < WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL; i++)
                        key[i] ^= chFakeKeyMaterial[(7 * i) % 13];
                    config.KeyMaterial = key;
                    break;
                case KeyType.WPABinary:
                    // We set this explicitly here.  It was set
                    // out of line in the NetUI code.
                    config.Privacy = WEPStatus.TKIPEnabled;

                    config.KeyLength = WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL;
                    config.CtlFlags |= WZCControl.WEPKPresent;

                    // Note that, since the config structure doesn't
                    // actually have a byte[] for key material, we
                    // can't modify bytes of that 'array' in-place.
                    key = config.KeyMaterial;
                    for (i = 0; i < WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL; i++)
                        config.KeyMaterial[i] ^= chFakeKeyMaterial[(7 * i) % 13];
                    config.KeyMaterial = key;
                    break;
            }
        }

        /// <summary>
        /// The CheckKeySize routine checks the incoming 
        /// WEP or WPA key from the user and throws an 
        /// exception of an appropriate type, if the key
        /// size is wrong or if it contains invalid
        /// characters.  In this case, it also maps the
        /// string key into a byte array of the binary
        /// values corresponding to the string, which should
        /// represent the hex values in the key.
        /// </summary>
        /// <param name="sKey">
        /// The string key to be used.  Must consist of a
        /// string of hexadecimal digits, for WEP keys and
        /// WPA binary keys.  May be a passphrase for WPA-PSK,
        /// though, which can be 8-63 characters long.
        /// </param>
        /// <param name="authMode">
        /// Can be any of the authentication mode types, 
        /// including WEP and WPA-PSK.
        /// </param>
        /// <param name="Key">
        /// Reference parameter into which the returned
        /// binary key value is written.  Will be set to
        /// null if the input string key is empty.
        /// </param>
        /// <returns>
        /// KeyType value indicating the type of the key. Note:
        /// for WPAPassphrase type, the caller will still have
        /// to encode the password before using it (Key is not
        /// set).  Note: for WPABinary type, the caller will
        /// still have to encrypt the key, although it is
        /// converted from hex string to binary by this routine.
        /// </returns>
        internal KeyType CheckKeySize(string sKey, AuthenticationMode authMode,
            ref byte[] Key)
        {
            Key = null;

            // If the key is empty, just return.
            if ((sKey == null) || (sKey.Length == 0))
                return KeyType.None;

            // Handle two cases: WEP key and WPA-PSK
            // password/binary key.
            if ((authMode == AuthenticationMode.WPAPSK) ||
                (authMode == AuthenticationMode.WPAAdHoc))
            {
                //  User can only enter either 64 hex entries, or 8/63 any ASCII entries which is always
                //  converted to XFORMAT.
                if ((sKey.Length >= 8) && (sKey.Length <= 63))
                {
                    return KeyType.WPAPassphrase;
                }
                else if (sKey.Length == 64)
                {
                    Key = new byte[sKey.Length >> 1];
                    try
                    {
                        for (int i = 0; i < (sKey.Length >> 1); i++)
                            Key[i] = byte.Parse(sKey.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        throw new ArgumentException("Key may contain hexadecimal digits only");
                    }

                    return KeyType.WPABinary;
                }
                else
                {
                    // Throw exception: Invalid WPA key
                    // material.
                    throw new ArgumentException("WPA key must contain more than 16 and less than 128 hex digits or exactly 64 bytes of binary key data", "sKey");
                }
            }
            else	// WEP
            {
                if (sKey.Length != 10 && sKey.Length != 26)
                    throw new ArgumentException("Key must contain 10 or 26 hexadecimal digits", "sKey");

                Key = new byte[sKey.Length >> 1];
                try
                {
                    for (int i = 0; i < (sKey.Length >> 1); i++)
                        Key[i] = byte.Parse(sKey.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    throw new ArgumentException("Key may contain hexadecimal digits only");
                }

                return KeyType.WEP;
            }
        }

        /// <summary>
        /// The CheckKeySize routine checks the incoming 
        /// WEP or WPA key from the user and throws an 
        /// exception of an appropriate type, if the key
        /// size is wrong.
        /// </summary>
        /// <param name="Key">
        /// The binary key to be used.
        /// </param>
        /// <param name="authMode">
        /// Can be any of the authentication mode types, 
        /// including WEP and WPA-PSK.
        /// </param>
        internal KeyType CheckKeySize(byte[] Key, AuthenticationMode authMode)
        {
            if ((Key == null) || (Key.Length == 0))
                return KeyType.None;

            // Handle two cases: WEP key and WPA-PSK
            // password/binary key.
            if ((authMode == AuthenticationMode.WPAPSK) ||
                (authMode == AuthenticationMode.WPAAdHoc))
            {
                // The only binary key material that you can send here is
                // 32 bytes long.
                if (Key.Length != 32)
                {
                    throw new ArgumentException("Key must contain 32 bytes of key data", "Key");
                }

                return KeyType.WPABinary;
            }
            else
            {
                // Length must be 5 or 13.
                if ((Key.Length != 5) && (Key.Length != 13))
                {
                    throw new ArgumentException("Key must contain 5 or 13 bytes of key data", "Key");
                }

                return KeyType.WEP;
            }
        }
        #endregion

        /// <summary>
        /// Connects to an already-configured wireless network by SSID
        /// </summary>
        /// <param name="SSID"></param>
        /// <returns></returns>
        public bool ConnectToPreferredNetwork(string SSID)
        {
            INTF_ENTRY entry;
            int uret = WZC.QueryAdapter(this.Name, out entry);

            try
            {
                if (uret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(uret, "Unable to Query WZC Interface");
                }

                // We need to push the indicated item to the top of the
                // preferred list.  Once we do that and call WZCSetInterface
                // the connection will be established to that SSID.
                // The preferred list is in the rdStSSIDList field.
                RAW_DATA rdold = entry.rdStSSIDList;
                WLANConfigurationList prefl = new WLANConfigurationList(rdold);

                // Start at the bottom of the list.  If the current item
                // is the one we want to copy, save it and start copying
                // items down in the list.  
                WLANConfiguration targetItem = null;
                int i;
                for (i = (int)prefl.NumberOfItems - 1; i >= 0; i--)
                {
                    targetItem = prefl.Item(i);
                    if (targetItem.SSID == SSID)
                    {
                        break;
                    }
                }

                // If we get no match for our SSID value, the item is *not*
                // in the preferred list.  Return false.
                if (targetItem == null)
                {
                    return false;
                }

                // If the SSID is already first in the list, we're done.
                if (i > 0)
                {
                    // Now, copy the rest of the items one place down in the
                    // list.
                    for (int j = i; j >= 1; j--)
                    {
                        // Copy old list item j-1 to new list item j.
                        prefl.SetItem(j, prefl.Item(j - 1));
                    }

                    // Put the saved target item in index 0 in the new list.
                    prefl.SetItem(0, targetItem);
                }

                uret = WZC.SetAdapter(entry, INTF_FLAGS.INTF_ALL_FLAGS | INTF_FLAGS.INTF_PREFLIST);
                if (uret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(uret, "Unable to Set WZC Interface");
                }
            }
            finally
            {
                entry.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Sets wireless settings associated with a given 
        /// interface and AP, adding to, rather than replacing
        /// the preferred list of APs.  This version of the 
        /// method is designed for the case where *all* of 
        /// the options are going to be set, where no initial 
        /// configuration exists at all and where existing
        /// items in the preferred list should be maintained.
        /// After this method executes, if it is successful,
        /// the specified SSID will be at the top, highest-
        /// priority, end of the preferred list.
        /// </summary>
        /// <param name="SSID">
        /// Target SSID to connect
        /// </param>
        /// <param name="bInfrastructure">
        /// Is infrastructure
        /// </param>
        /// <param name="sKey">
        /// WEP key string representing hex string (each 
        /// two characters are converted to a single byte)
        /// </param>
        /// <param name="keyIndex">
        /// Index of the WEP key.  Valid values are 1-4
        /// </param>
        /// <param name="authMode">
        /// Authentication mode for the connection
        /// </param>
        /// <param name="privacyMode">
        /// Privacy (encryption) mode for the connection
        /// </param>
        /// <param name="eapParams">
        /// Parameters describing how the connection should use EAP
        /// to authenticate the user to the network
        /// </param>
        /// <returns>true if succeeded</returns>
        public bool AddPreferredNetwork(string SSID, bool bInfrastructure,
            string sKey, int keyIndex,
            AuthenticationMode authMode, WEPStatus privacyMode,
            EAPParameters eapParams)
        {
            // Check key length and fire exception if 
            // out-of-range.
            byte[] arrKey = null;
            KeyType kt = CheckKeySize(sKey, authMode, ref arrKey);

            return AddPreferredNetwork(SSID, bInfrastructure, arrKey, keyIndex, authMode, privacyMode, kt, sKey, eapParams);
        }

        /// <summary>
        /// Sets wireless settings associated with a given 
        /// interface and AP, adding the new SSID to the
        /// list of preferred SSID values, rather than 
        /// replacing the whole list.  This version of the 
        /// method is designed for the case where *all* of 
        /// the options are going to be set, where no 
        /// initial configuration exists at all and where the
        /// current SSID values in the preferred list should
        /// be retained.  After this call completes, if it
        /// is successful, the new SSID is at the top, highest-
        /// priority, end of the preferred list.
        /// </summary>
        /// <param name="SSID">
        /// Target SSID to connect
        /// </param>
        /// <param name="bInfrastructure">
        /// Is infrastructure
        /// </param>
        /// <param name="keyMaterial">
        /// WEP or WPA key.  Key is in binary format. 
        /// For WEP, it should contain 5 or 13 bytes of data.
        /// The number of bytes for WPA depends on whether a
        /// passphrase was used to generate it or whether it
        /// is the actual 32-byte binary key
        /// </param>
        /// <param name="keyIndex">
        /// Index of the key.  Valid values are 1-4
        /// </param>
        /// <param name="authMode">
        /// Authentication mode for the connection
        /// </param>
        /// <param name="privacyMode">
        /// Privacy (encryption) mode for the connection
        /// </param>
        /// <param name="eapParams">
        /// Parameters describing how the connection should use EAP
        /// to authenticate the user to the network
        /// </param>
        /// <returns>true if succeeded</returns>
        public bool AddPreferredNetwork(string SSID, bool bInfrastructure,
            byte[] keyMaterial, int keyIndex,
            AuthenticationMode authMode, WEPStatus privacyMode,
            EAPParameters eapParams)
        {
            // Verify that key size is valid for authentication
            // mode.  This is only done if this routine was
            // called from an external source.  If it was called
            // from another overload of SetWirelessSettingsAddEx(),
            // it should have a value other than none, which we
            // use.
            KeyType kt = CheckKeySize(keyMaterial, authMode);

            return AddPreferredNetwork(SSID, bInfrastructure,
                keyMaterial, keyIndex, authMode, privacyMode, kt, null, eapParams);
        }

        internal bool AddPreferredNetwork(string SSID,
            bool infrastructureMode,
            byte[] keyMaterial,
            int keyIndex,
            AuthenticationMode authMode,
            WEPStatus privacyMode,
            KeyType kt,
            string wpaPassphrase,
            EAPParameters eapParams)
        {
            if ((keyIndex < 0) || (keyIndex > 4))
            {
                throw new ArgumentException("Invalid keyIndex");
            }
            if ((SSID == null) || (SSID == string.Empty))
            {
                throw new ArgumentException("Invalid SSID");
            }

            // We may yet need to do some processing on the key,
            // if it is a WPA key.  We need a WZC_WLAN_CONFIG 
            // structure to pass to the WZC routine that does 
            // this processing, however, so that is done below.

            // Get the current preferred list of SSID values.
            // First, we need to get an INTF_ENTRY for this adapter.
            INTF_ENTRY entry;
            int uret = WZC.QueryAdapter(this.Name, out entry);

            try
            {
                if (uret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(uret, "No preferred list found");
                }

                // We need to push the indicated item to the top of the
                // preferred list.  Once we do that and call WZCSetInterface
                // the connection will be established to that SSID.
                // The preferred list is in the rdStSSIDList field.
                RAW_DATA rdold = entry.rdStSSIDList;
                WLANConfigurationList prefl = new WLANConfigurationList(rdold);

                // Start at the bottom of the list.  If the current item
                // is the one we want to copy, save it and start copying
                // items down in the list.  
                WLANConfiguration targetItem = null;
                int i;
                for (i = (int)prefl.NumberOfItems - 1; i >= 0; i--)
                {
                    targetItem = prefl.Item(i);
                    if (targetItem.SSID == SSID)
                    {
                        break;
                    }
                }

                // If we get no match for our SSID value, the item 
                // is *not* in the preferred list, so we can
                // skip removing it.
                if (i >= 0)
                {
                    // Now, copy the items before i on the
                    // list down to cover i.  This leaves
                    // position 0 in the list as a copy of
                    // position 1.  We'll fill in position 0
                    // with the new most-preferred SSID.
                    for (int j = i; j >= 1; j--)
                    {
                        // Copy old list item j-1 to new list item j.
                        prefl.SetItem(j, prefl.Item(j - 1));
                    }
                }
                else
                {
                    // The item was not in the list.  We have
                    // to expand the list and move all of
                    // the original items down one spot.
                    WLANConfigurationList prefl2 = new WLANConfigurationList(prefl.NumberOfItems + 1);
                    for (int j = 0; j < (int)prefl.NumberOfItems; j++)
                    {
                        // Copy from old list to new list.
                        prefl2.SetItem(j + 1, prefl.Item(j));
                    }

                    // Replace the old list with the new one
                    // for the rest of the code.  Entry #0
                    // is unset.
                    prefl = prefl2;
                }

                // Create a new item and put that in the list
                // at item #0, which presently exists but
                // doesn't mean anything (it's either a 
                // totally blank item, if the SSID was not
                // in the list before the call, or it's the
                // old first item in the list).

                // Unlike the other SetWirelessSettings versions,
                // we *don't* get the current configuration here;
                // our parameters will set that.
                WLANConfiguration thisConfig = MakeSSIDEntry(SSID, infrastructureMode, keyMaterial, keyIndex, authMode, privacyMode);

                // Turn on the 802.1x enable flag, if indicated
                // by the caller.
                if ((eapParams != null) && eapParams.Enable8021x)
                {
                    thisConfig.CtlFlags |= WZCControl.ONEXEnabled;

                    // Copy the EAP parameters to the new configuration.
                    thisConfig.EapolParams = eapParams;
                }

                // Process the key for the target item, based
                // on the key type.  Note that we have to do this
                // before the item gets copied to the list.  We
                // can't get a real reference to any of the data
                // items in the list, only copies of thier data.
                ProcessKey(kt, ref thisConfig, wpaPassphrase);

                // OK, finally, set the item in the preferred
                // list according to the parameters to this
                // call.
                prefl.SetItem(0, thisConfig);

                // Must now copy the new preferred list to the entry that
                // we will sent with WZCSetInterface.
                entry.rdStSSIDList = prefl.rawData;

                uret = WZC.SetAdapter(entry, INTF_FLAGS.INTF_PREFLIST);

                // Finally, we are ready to select the new SSID as our 
                // primary preferred connection.
                uret = NativeMethods.WZCSetInterface(null, INTF_FLAGS.INTF_PREFLIST, ref entry, null);
                if (uret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(uret, "Unable to Set WZC Interface");
                }
            }
            finally
            {
                entry.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Removes a network from the adapter's preferred list
        /// </summary>
        /// <param name="SSID">The SSID of the network to remove</param>
        public bool RemovePreferredNetwork(string SSID)
        {
            if ((SSID == null) || (SSID == string.Empty))
            {
                throw new ArgumentException("Invalid SSID");
            }

            // Get the current preferred list of SSID values.
            // First, we need to get an INTF_ENTRY for this adapter.
            INTF_ENTRY entry;
            int uret = WZC.QueryAdapter(this.Name, out entry);

            try
            {
                if (uret > 0)
                {
                    // There is no list.  Return false.
                    return false;
                }

                // Find the indicated item and remove it from
                // the list by creating a new list and setting
                // that as the preferred list.
                RAW_DATA rdold = entry.rdStSSIDList;
                WLANConfigurationList prefl = new WLANConfigurationList(rdold);

                // If there are no items in the list, return false.
                if (prefl.NumberOfItems == 0)
                {
                    return false;
                }

                // Build the new list.
                WLANConfigurationList prefnew = new WLANConfigurationList(prefl.NumberOfItems - 1);

                // Start at the top of the old list and copy items
                // from old to new, until we find the item to be
                // removed.
                int j = 0;
                int i;
                for (i = 0; i < prefl.NumberOfItems; i++)
                {
                    WLANConfiguration item = prefl.Item(i);

                    if (item.SSID == SSID)
                    {
                        // Skip to next item without incrementing
                        // j.
                    }
                    else
                    {
                        prefnew.SetItem(j, item);
                        j++;
                    }
                }

                // Check for whether the item was found in the
                // list or not.  If not, we don't reset the
                // wireless settings and instead return false.
                if (j == i)
                {
                    return false;
                }

                // Replace the old list with the new one
                // for the rest of the code.  Entry #0
                // is unset.
                prefl = prefnew;

                // Must now copy the new preferred list to the entry that
                // we will sent with WZCSetInterface.
                entry.rdStSSIDList = prefl.rawData;

                // Finally, we are ready to select the new SSID as our 
                // primary preferred connection.
                uret = WZC.SetAdapter(entry, INTF_FLAGS.INTF_ALL_FLAGS | INTF_FLAGS.INTF_PREFLIST);

                if (uret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(uret, "Unable to Set WZC Interface");
                }
            }
            finally
            {
                entry.Dispose();
            }
            return (uret <= 0);
        }
        #endregion
    }
}