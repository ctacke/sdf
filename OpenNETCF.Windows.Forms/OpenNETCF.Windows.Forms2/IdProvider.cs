using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Windows.Forms
{
    internal static class IdProvider
    {
        static Dictionary<IntPtr, int> idList;
        static IdProvider()
        {
            idList = new Dictionary<IntPtr, int>();
        }

        public static int NewId(IntPtr parent)
        {
            lock (idList)
            {
                if (!idList.ContainsKey(parent))
                    idList[parent] = 1000;
                int id = idList[parent]++;
                idList[parent] = id;
                return id;
            }
        }
    }
}
