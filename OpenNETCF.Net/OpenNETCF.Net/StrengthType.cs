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

namespace OpenNETCF.Net
{
    /// <summary>
    /// The StrengthType enumeration provides a list of 
    /// relative RF Ethernet signal strength values that 
    /// correspond to the strengths displayed by Windows CE
    /// itself.
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.StrengthType instead", false)]
    public enum StrengthType
    {
        /// <summary>
        /// The adapter for which signal strength was requested is not
        /// a wireless network adapter or does not report its signal
        /// strength in the standard way
        /// </summary>
        NotAWirelessAdapter,

        /// <summary>
        /// The adapter is not receiving a network signal
        /// </summary>
        NoSignal,

        /// <summary>
        /// The network signal has very low strength
        /// </summary>
        VeryLow,

        /// <summary>
        /// The network signal has low strength
        /// </summary>
        Low,

        /// <summary>
        /// The network signal is good
        /// </summary>
        Good,

        /// <summary>
        /// The network signal is very good
        /// </summary>
        VeryGood,

        /// <summary>
        /// The network signal is excellent
        /// </summary>
        Excellent
    }
}
