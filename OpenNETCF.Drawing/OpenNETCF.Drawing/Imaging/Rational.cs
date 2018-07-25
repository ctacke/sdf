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

namespace OpenNETCF.Drawing.Imaging
{
    /// <summary>
    /// Holds a Rational number used by imaging properties
    /// </summary>
    [CLSCompliant(false)]
    public struct Rational
    {
        [CLSCompliant(false)]
        public Rational(uint num1, uint num2)
        {
            Numerator = num1;
            Denominator = num2;
        }

        public Rational(uint val)
        {
            Numerator = val;
            Denominator = 1;
        }

        public uint Numerator;
        public uint Denominator;

        public override int GetHashCode()
        {
            return (int)(Numerator ^ Denominator);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator);
        }
    }
    /// <summary>
    /// Holds a Signed Rational number used by imaging properties
    /// </summary>
    [CLSCompliant(false)]
    public struct SRational
    {
        public SRational(int num1, uint num2)
        {
            Numerator = num1;
            Denominator = num2;
        }

        public SRational(int val)
        {
            Numerator = val;
            Denominator = 1;
        }

        public int Numerator;
        public uint Denominator;

        public override int GetHashCode()
        {
            return (int)(Numerator ^ Denominator);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator, Denominator);
        }
    }
}
