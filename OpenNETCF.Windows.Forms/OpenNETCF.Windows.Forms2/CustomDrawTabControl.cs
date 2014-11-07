using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public class CustomDrawTabControl: BaseCustomDrawControl<TabControl, NMCUSTOMDRAW>
    {
        protected TabControl tabControl;

        public TabControl TabControl
        {
            get { return tabControl; }
        }

        private List<Icon> m_icons;

        public event TreeViewPaintBackgroundEventHandler PaintBackground;
        public event TreeViewPaintItemEventHandler PaintItem;


        public List<Icon> Icons
        {
            get { return m_icons; }
        }

        public CustomDrawTabControl()
            : base()
        {
            m_icons = new List<Icon>();
        }

        protected override void SetControl(System.Windows.Forms.TabControl ctl)
        {
            tabControl = ctl as TabControl;
        }
    }
}
