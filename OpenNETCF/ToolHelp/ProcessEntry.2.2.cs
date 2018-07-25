using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ToolHelp
{
	partial class ProcessEntry
	{
        public ThreadEntry[] GetThreads()
        {
            return ThreadEntry.GetThreads(this.ProcessID);
        }

        public ModuleEntry[] GetModules()
        {
            return ModuleEntry.GetModules(this.ProcessID);
        }
	}
}
