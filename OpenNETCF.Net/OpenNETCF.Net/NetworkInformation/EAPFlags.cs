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
