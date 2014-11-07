using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Flags used for controlling how EAP is used
    /// </summary>
    [FlagsAttribute]
    public enum EAPFlags : int
    {
        /// <summary>
        /// No EAP
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// EAP is on
        /// </summary>
        Enabled = -2147483648,	// 0x80000000,

        /// <summary>
        /// EAP authentication by user
        /// </summary>
        MachineAuthenticationDisabled = 0,

        /// <summary>
        /// EAP authentication by machine (as opposed to user,
        /// presumably)
        /// </summary>
        MachineAuthenticationEnabled = 0x40000000,

        /// <summary>
        /// EAP guest login not allowed
        /// </summary>
        GuestAuthenticationDisabled = 0,

        /// <summary>
        /// EAP gues login allowed
        /// </summary>
        GuestAuthenticationEnabled = 0x20000000,

        /// <summary>
        /// This is the default configuration for all connections
        /// in WZC
        /// </summary>
        DefaultState = Enabled,

        /// <summary>
        /// This is the default state of the machine authentication
        /// flag in WZC
        /// </summary>
        DefaultMachineAuthentication = MachineAuthenticationEnabled,

        /// <summary>
        /// This is the default state of the guest authentication
        /// flag in WZC
        /// </summary>
        DefaultGuestAuthentication = GuestAuthenticationDisabled,

        /// <summary>
        /// This is the default set of flags set by WZC for 
        /// connections
        /// </summary>
        DefaultWZCFlags = (DefaultState | DefaultMachineAuthentication | DefaultGuestAuthentication),
    }
}
