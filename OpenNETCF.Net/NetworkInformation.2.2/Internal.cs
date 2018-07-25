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
