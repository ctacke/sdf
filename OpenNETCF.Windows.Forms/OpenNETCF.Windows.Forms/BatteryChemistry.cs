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

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Identifies the chemistry of the devices main battery.
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="PowerStatus.BatteryChemistry"/> property.</remarks>
    public enum BatteryChemistry : byte
    {
        /// <summary>
        /// Alkaline battery.
        /// </summary>
        Alkaline = 0x01,
        /// <summary>
        /// Nickel Cadmium battery.
        /// </summary>
        NiCad = 0x02,
        /// <summary>
        /// Nickel Metal Hydride battery.
        /// </summary>
        NiMH = 0x03,
        /// <summary>
        /// Lithium Ion battery.
        /// </summary>
        Lion = 0x04,
        /// <summary>
        /// Lithium Polymer battery.
        /// </summary>
        LiPoly = 0x05,
        /// <summary>
        /// Zinc Air battery.
        /// </summary>
        ZincAir = 0x06,
        /// <summary>
        /// Battery chemistry is unknown.
        /// </summary>
        Unknown = 0xFF,
    }
}
