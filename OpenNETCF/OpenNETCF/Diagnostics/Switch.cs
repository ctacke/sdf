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
using System.Collections;

namespace OpenNETCF.Diagnostics
{
    public abstract class Switch
    {
        private bool initialized;
        private string description;
        private string displayName;
        private int switchSetting;
        private static Hashtable switchSettings;
        private static int initCount;

        public string Description
        {
            get
            {
                if (this.description != null)
                {
                    return this.description;
                }
                return string.Empty;
            }
        }

        public string DisplayName
        {
            get { return this.displayName; }
        }

        protected int SwitchSetting
        {
            get
            {
                if (!this.initialized)
                {
                    bool isAlreadyInit = Switch.Initialize();
                    if ((switchSettings == null) && !isAlreadyInit)
                    {
                        return 0;
                    }
                    object _switch = switchSettings[this.displayName];
                    if (_switch != null)
                    {
                        this.switchSetting = (int)_switch;
                    }
                    else
                    {
                        this.switchSetting = 0;
                    }
                    this.initialized = true;
                    this.OnSwitchSettingChanged();
                }
                return this.switchSetting;
            }
            set
            {
                this.switchSetting = value;
                this.initialized = true;
                this.OnSwitchSettingChanged();
            }
        }
 
        protected Switch(string displayName, string description)
        {
            this.switchSetting = 0;
            this.initialized = false;
            if (displayName == null)
            {
                displayName = string.Empty;
            }
            this.displayName = displayName;
            this.description = description;
        }

        private static bool Initialize()
        {
            if (System.Threading.Interlocked.CompareExchange(ref initCount, 1, 0) != 0)
            {
                return false;
            }

            try
            {
                if (switchSettings != null)
                {
                    return true;
                }

                int retryCount = 0;
                while (DiagnosticsConfiguration.IsInitializing() && retryCount < 4)
                {
                    System.Threading.Thread.Sleep(50);
                    ++retryCount;
                }

                if (!DiagnosticsConfiguration.CanInitialize())
                {
                    return false;
                }

                Hashtable validSwitches = new Hashtable();
                IDictionary switches = DiagnosticsConfiguration.SwitchSettings;
                if (switches != null)
                {
                    validSwitches = new Hashtable(switches.Count);
                    foreach (DictionaryEntry traceSwitch in switches)
                    {
                        try
                        {
                            validSwitches[traceSwitch.Key] = int.Parse((string)traceSwitch.Value);
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    validSwitches = new Hashtable();
                }
                switchSettings = validSwitches;
            }
            finally
            {
                initCount = 0;
            }

            return true;
        }

        protected virtual void OnSwitchSettingChanged()
        {
        }
    }
}
