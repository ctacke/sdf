using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public class OwnerDrawListBox : BaseOwnerDrawControl
    {
        private ObjectCollection m_items;

        public IList<Object> Items
        {
            get { return m_items; }
        }

        public IntPtr ControlHandle { get { return m_hwndControl; } }

        public OwnerDrawListBox()
            : base("LISTBOX", (int)(/*LBS.HASSTRINGS |*/ LBS.OWNERDRAWVARIABLE | LBS.NOTIFY) | (int)WS.VSCROLL)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            m_items = new ObjectCollection(this);
            base.OnHandleCreated(e);
            BeginUpdate();
            foreach(object item in Items)
                Win32Window.SendMessage(m_hwndControl, (int)LB.ADDSTRING, 0, item.ToString());
            EndUpdate();
        }

        protected override IntPtr OnCommand(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam)
        {
            if ((wParam.ToInt32() >> 16) == 1) //sel change
            {
                OnSelectedIndexChanged(EventArgs.Empty);
            }
            return base.OnCommand(hWnd, msg, wParam, lParam);
        }
        //
        // Summary:
        //     Gets or sets the zero-based index of the currently selected item in a System.Windows.Forms.ListBox.
        //
        // Returns:
        //     A zero-based index of the currently selected item. A value of negative one
        //     (-1) is returned if no item is selected.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The System.Windows.Forms.ListBox.SelectionMode property is set to None.
        //
        //   System.ArgumentOutOfRangeException:
        //     The assigned value is less than -1 or greater than or equal to the item count.
        public int SelectedIndex
        {
            get
            {
                return (int)Win32Window.SendMessage(m_hwndControl, (int)LB.GETCURSEL, 0, 0);
            }
            set
            {
                Win32Window.SendMessage(m_hwndControl, (int)LB.SETCURSEL, value, 0);
            }
        }

        //
        // Summary:
        //     Gets or sets the currently selected item in the System.Windows.Forms.ListBox.
        //
        // Returns:
        //     An object that represents the current selection in the control.
        public object SelectedItem
        {
            get { return Items[SelectedIndex]; }
            set
            {
                int selIndex = Items.IndexOf(value);
                SelectedIndex = selIndex;
            }
        }
        
        //
        // Summary:
        //     Gets or searches for the text of the currently selected item in the System.Windows.Forms.ListBox.
        //
        // Returns:
        //     The text of the currently selected item in the control.
        public override string Text
        {
            get
            {
                int sel = SelectedIndex;
                if (sel == -1)
                    return "";
                else
                    return Marshal.PtrToStringUni((IntPtr)Win32Window.SendMessage(m_hwndControl, (int)LB.GETITEMDATA, sel, 0));
            }
            set { }
        }
        
        //
        // Summary:
        //     Gets or sets the index of the first visible item in the System.Windows.Forms.ListBox.
        //
        // Returns:
        //     The zero-based index of the first visible item in the control.
        public int TopIndex
        {
            get { return (int)Win32Window.SendMessage(m_hwndControl, (int)LB.GETTOPINDEX, 0, 0); }
            set { Win32Window.SendMessage(m_hwndControl, (int)LB.SETTOPINDEX, value, 0); }
        }

        // Summary:
        //     Occurs when the System.Windows.Forms.ListBox.SelectedIndex property has changed.
        public event EventHandler SelectedIndexChanged;

        // Summary:
        //     Maintains performance while items are added to the System.Windows.Forms.ListBox
        //     one at a time by preventing the control from drawing until the System.Windows.Forms.ListBox.EndUpdate()
        //     method is called.
        public void BeginUpdate()
        {
            Win32Window.SendMessage(m_hwndControl, (int)WM.SETREDRAW, 0, 0);
        }
        //
        // Summary:
        //     Resumes painting the System.Windows.Forms.ListBox control after painting
        //     is suspended by the System.Windows.Forms.ListBox.BeginUpdate() method.
        public void EndUpdate()
        {
            Win32Window.SendMessage(m_hwndControl, (int)WM.SETREDRAW, -1, 0);
        }
        //protected override void OnDataSourceChanged(EventArgs e);
        //protected override void OnDisplayMemberChanged(EventArgs e);
        //
        //
        // Parameters:
        //   e:
        //     Event object with the details
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, EventArgs.Empty);
        }
        
        //
        // Summary:
        //     Sets the object with the specified index in the derived class.
        //
        // Parameters:
        //   value:
        //     The object.
        //
        //   index:
        //     The array index of the object.
        //protected override void SetItemCore(int index, object value);
        //
        // Summary:
        //     Clears the contents of the System.Windows.Forms.ListBox and adds the specified
        //     items to the control.
        //
        // Parameters:
        //   value:
        //     An array of objects to insert into the control.
        //protected override void SetItemsCore(IList value);

        // Summary:
        //     Represents the collection of items in a System.Windows.Forms.ListBox.
        public class ObjectCollection : IList<object>, ICollection<object>, IEnumerable<object>
        {
            private OwnerDrawListBox m_parent;
            private List<object> m_list;

            internal ObjectCollection(OwnerDrawListBox parent)
            {
                m_list = new List<object>();
                this.m_parent = parent;
            }



            #region IList<object> Members

            public int IndexOf(object item)
            {
                return m_list.IndexOf(item);
            }

            public void Insert(int index, object item)
            {
                m_list.Insert(index, item);
                //string sItem = item.ToString();
                //byte[] bItem = Encoding.Unicode.GetBytes(sItem);
                //IntPtr str = Marshal.AllocHGlobal(bItem.Length + 2);
                //Marshal.Copy(bItem, 0, str, bItem.Length);
                Win32Window.SendMessage(m_parent.m_hwndControl, (int)LB.INSERTSTRING, index, item.ToString());
            }

            public void RemoveAt(int index)
            {
                m_list.RemoveAt(index);
                Marshal.FreeHGlobal((IntPtr)Win32Window.SendMessage(m_parent.m_hwndControl, (int)LB.GETITEMDATA, index, 0));
                Win32Window.SendMessage(m_parent.m_hwndControl, (int)LB.DELETESTRING, index, 0);
            }

            public object this[int index]
            {
                get
                {
                    return m_list[index];
                }
                set
                {
                    m_list[index] = value;
                }
            }

            #endregion

            #region ICollection<object> Members

            public void Add(object item)
            {
                Insert(Count, item);
            }

            public void Clear()
            {
                m_list.Clear();
                Win32Window.SendMessage(m_parent.m_hwndControl, (int)LB.RESETCONTENT, 0, 0);
            }

            public bool Contains(object item)
            {
                return m_list.Contains(item);
            }

            public void CopyTo(object[] array, int arrayIndex)
            {
                m_list.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return m_list.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(object item)
            {
                int i = IndexOf(item);
                if (i == -1)
                    return false;
                RemoveAt(i);
                return true;
            }

            #endregion

            #region IEnumerable<object> Members

            public IEnumerator<object> GetEnumerator()
            {
                return m_list.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            //{
            //    throw new Exception("The method or operation is not implemented.");
            //}

            #endregion


            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return m_list.GetEnumerator();
            }

            #endregion
        }

    }
}
