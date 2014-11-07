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
    public class CustomDrawTreeView: BaseCustomDrawControl<TreeView, NMTVCUSTOMDRAW>
    {
        public event TreeViewQueryItemColorHandler QueryItemColor;
        public event TreeViewPaintBackgroundEventHandler PaintBackground;
        public event TreeViewPaintItemEventHandler PaintItem;

        protected TreeView treeView;

        public TreeView TreeView
        {
            get { return treeView; }
        }

        private List<Icon> m_icons;

        public List<Icon> Icons
        {
            get { return m_icons; }
        }

        public ImageList ImageList { get { return treeView.ImageList; } }

        public CustomDrawTreeView()
            : base()
        {
            m_icons = new List<Icon>();
        }

        protected override void SetControl(TreeView ctl)
        {
            treeView = (TreeView)ctl;
            ExtraInit(null, EventArgs.Empty);
            //NativeMethods.SendMessage(ctl.Handle, (int)TVM.SETBKCOLOR, 0, -1);
        }

        void ExtraInit(object sender, EventArgs e)
        {
            Win32Window.SendMessage(treeView.Handle, (int)TVM.SETINDENT, 10, 0);
            Win32Window.SendMessage(treeView.Handle, (int)TVM.SETITEMSPACING, 0, 6);
        }

        public TreeView ListView
        {
            get { return treeView; }
        }


        protected override bool OnPreerase(ref NMTVCUSTOMDRAW tvcd)
        {
            return true;
        }

        
        protected override bool OnPrepaintItem(ref NMTVCUSTOMDRAW tvcd)
        {
            if (!fullPaint)
            {
                if (QueryItemColor == null)
                    return false;

                TreeNode node = TreeNodeFromHandle(tvcd.lItemlParam);
                Color clrFore = ColorFromRGB(tvcd.clrText);
                Color clrBack = ColorFromRGB(tvcd.clrTextBk);
                Font font = Font;
                bool bRet = false;
                QueryItemColor(this, node, ref clrFore, ref clrBack, ref font);
                if (clrFore != ColorFromRGB(tvcd.clrText) || clrBack != ColorFromRGB(tvcd.clrTextBk))
                {
                    tvcd.clrText = ColorToRGB(clrFore);
                    tvcd.clrTextBk = ColorToRGB(clrBack);
                    bRet = true;
                }
                if (font != Font)
                {
                    NativeMethods.SelectObject(tvcd.hdc, font.ToHfont());
                    bRet = true;
                }
                return bRet;
            }

            if (PaintItem != null)
            {
                TreeViewPaintItemEventArgs args = new TreeViewPaintItemEventArgs();
                args.Hdc = tvcd.hdc;
                args.Item = TreeNodeFromHandle(tvcd.lItemlParam);
                args.Selected = (tvcd.uItemState & CDIS.SELECTED) != 0;
                args.Focused = (tvcd.uItemState & CDIS.FOCUS) != 0;

                RECT rect = new RECT();
                rect.left = (int)tvcd.dwItemSpec;
                Win32Window.SendMessage(treeView.Handle, (int)TVM.GETITEMRECT, -1, ref rect);

                args.RectPaint = (Rectangle)rect; //.FromLTRB(tvcd.l, tvcd.t, tvcd.r, tvcd.b);
                PaintItem(this, args);
                return true;
            }
            
            return false;
        }

        protected override bool OnPrepaint(ref NMTVCUSTOMDRAW tvcd)
        {
            RECT rcView = new RECT();
            Win32Window.SendMessage(treeView.Handle, (int)LVM.GETVIEWRECT, 0, ref rcView);
            IntPtr hdc = tvcd.hdc;
            return OnDrawBackground(hdc, rcView, tvcd.rc);
        }

        protected override bool OnDrawBackground(IntPtr hdc, RECT rcView, RECT rcPaint)
        {
            if (PaintBackground != null)
            {
                TreeViewPaintBackgroundEventArgs args = new TreeViewPaintBackgroundEventArgs(hdc, (Rectangle)rcView, (Rectangle)rcPaint);
                PaintBackground(this, args);
                return true;
            }
            return false;
        }

        private TreeNode TreeNodeFromHandle(IntPtr hNode)
        {
            Hashtable nodes = (Hashtable)typeof(TreeView).GetField("m_hashNodes", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(treeView);
            return (TreeNode)nodes[hNode];
        }

    }

    /// <summary>
    /// Arguments for custom-draw listbox background painting
    /// </summary>
    public class TreeViewPaintBackgroundEventArgs : EventArgs
    {
        /// <summary>
        /// Display context
        /// </summary>
        public IntPtr Hdc;
        /// <summary>
        /// Bounding rectangle for all items in the listview
        /// </summary>
        public Rectangle RectView;
        /// <summary>
        /// Paint rectangle
        /// </summary>
        public Rectangle RectPaint;

        public TreeViewPaintBackgroundEventArgs(IntPtr hdc, Rectangle rcView, Rectangle rcPaint)
        {
            Hdc = hdc;
            RectView = rcView;
            RectPaint = rcPaint;
        }
    }

    public class TreeViewPaintItemEventArgs : EventArgs
    {
        /// <summary>
        /// Display context
        /// </summary>
        public IntPtr Hdc;
        /// <summary>
        /// 
        /// </summary>
        public Rectangle RectPaint;
        /// <summary>
        /// Item being painted
        /// </summary>
        public TreeNode Item;
        /// <summary>
        /// Paint selected state
        /// </summary>
        public bool Selected;
        /// <summary>
        /// Paint focused state
        /// </summary>
        public bool Focused;
    }

    public delegate void TreeViewPaintBackgroundEventHandler(object sender, TreeViewPaintBackgroundEventArgs e);
    public delegate void TreeViewPaintItemEventHandler(object sender, TreeViewPaintItemEventArgs e);
    public delegate void TreeViewQueryItemColorHandler(object sender, TreeNode node, ref Color clrFore, ref Color clrBack, ref Font font);
}
