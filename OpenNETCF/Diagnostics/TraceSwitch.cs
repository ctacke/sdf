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
