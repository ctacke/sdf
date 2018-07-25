using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
    public class StaticMethods
    {
        /// <summary>
        /// Determins whether the control is in design mode or not
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static bool IsDesignMode(Control control)
        {
            return IsDesignTime;
        }

        public static bool IsDesignMode(System.ComponentModel.Component component)
        {
            return IsDesignTime;

        }

        /// <summary>
        /// Determine if this instance is running against .NET Framework
        /// </summary>
        public static bool IsDesignTime
        {
            get
            {
                // Determine if this instance is running against .NET Framework by using the MSCoreLib PublicKeyToken
                System.Reflection.Assembly mscorlibAssembly = typeof(int).Assembly;
                if ((mscorlibAssembly != null))
                {
                    if (mscorlibAssembly.FullName.ToUpper().EndsWith("B77A5C561934E089"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Determine if this instance is running against .NET Compact Framework
        /// </summary>
        public static bool IsRunTime
        {
            get
            {
                // Determine if this instance is running against .NET Compact Framework by using the MSCoreLib PublicKeyToken
                System.Reflection.Assembly mscorlibAssembly = typeof(int).Assembly;
                if ((mscorlibAssembly != null))
                {
                    if (mscorlibAssembly.FullName.ToUpper().EndsWith("969DB8053D3322AC"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Produces a number to use for scaling graphics
        /// </summary>
        /// <param name="g">Graphics object to use</param>
        /// <returns></returns>
        public static float Scale(Graphics g)
        {
            return g.DpiX / 96;
        }

    }
}
