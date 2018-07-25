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
using System.Diagnostics;
using System.Reflection;

namespace OpenNETCF.Net.NetworkInformation
{
    internal static class Internal
    {
        private static DateTime BETA_END = new DateTime(2007, 1, 31, 0, 0, 0);

        [Conditional("BETA")]
        public static void ShowBetaInfo()
        {
            //if (DateTime.Now.CompareTo(BETA_END) > 0)
            //{
            //    System.Windows.Forms.MessageBox.Show("The beta period for this product has expired.  Visit www.opennetcf.com for a new version.",
            //        "OpenNETCF.Net.Networkinformation",
            //        System.Windows.Forms.MessageBoxButtons.OK,
            //        System.Windows.Forms.MessageBoxIcon.Exclamation,
            //        System.Windows.Forms.MessageBoxDefaultButton.Button1);
            //}
        }

        [Conditional("EVAL")]
        public static void ShowEvalInfo()
        {
            //System.Windows.Forms.MessageBox.Show("This is an evaluation version.  Visit www.opennetcf.com for a licensed release version.",
            //        "OpenNETCF.Net.Networkinformation",
            //        System.Windows.Forms.MessageBoxButtons.OK,
            //        System.Windows.Forms.MessageBoxIcon.Exclamation,
            //        System.Windows.Forms.MessageBoxDefaultButton.Button1);

        }
    }
}
