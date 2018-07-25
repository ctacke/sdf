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

namespace OpenNETCF.Diagnostics
{
    public class TraceSwitch : Switch
    {
        public TraceLevel Level
        {
            get { return (TraceLevel)base.SwitchSetting; }

            set
            {
                if ((value < TraceLevel.Off) || (value > TraceLevel.Verbose))
                {
                    throw new ArgumentException("Invalid TraceSwitch level");
                }
                base.SwitchSetting = (int)value;
            }

        }

        public bool TraceError
        {
            get { return (this.Level >= TraceLevel.Error); }
        }

        public bool TraceInfo
        {
            get { return (this.Level >= TraceLevel.Info); }
        }

        public bool TraceWarning
        {
            get { return (this.Level >= TraceLevel.Warning); }
        }

        public bool TraceVerbose
        {
            get { return (this.Level >= TraceLevel.Verbose); }
        }
 
        public TraceSwitch(string displayName, string description)
            : base(displayName, description)
        {
        }

        protected override void OnSwitchSettingChanged()
        {
            int switchValue = base.SwitchSetting;
            if (switchValue < 0)
            {
                Trace2.WriteLine("TraceSwitch Level too low");
                base.SwitchSetting = 0;
            }
            else if (switchValue > 4)
            {
                Trace2.WriteLine("TraceSwitch level too high");
                base.SwitchSetting = 4;
            }
        }

 


    }
}
