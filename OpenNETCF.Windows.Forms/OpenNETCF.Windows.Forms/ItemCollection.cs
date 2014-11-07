using System;

using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Collections.Generic;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Represents the collection of items in a ListBox2. 
  /// </summary>
  public class ItemCollection : IList<ListItem>, IList
  {
    private ListBox2 m_parent = null;
    private SynchronizedCollection<ListItem> m_list = new SynchronizedCollection<ListItem>();

    public ItemCollection()
    {
    }

    internal ItemCollection(ListBox2 parent)
      : base()
    {
      this.m_parent = parent;

    }

    private delegate int ListItemAddHandler(ListBox2 parent, ListItem item);
    private delegate bool ListItemRemoveHandler(ListBox2 parent, ListItem item);
    private delegate void ListItemInsertHandler(ListBox2 parent, ListItem item, int index);
    private delegate void ListRefreshHandler(ListBox2 parent);

    private void RefreshParent(ListBox2 parent)
    {
      if (parent.InvokeRequired)
      {
        parent.Invoke(new ListRefreshHandler(RefreshParent), new object[] { parent });
        return;
      }
      parent.Refresh();
    }

    private int AddItemToList(ListBox2 parent, ListItem item)
    {
      if (parent.InvokeRequired)
      {
        return (int)parent.Invoke(new ListItemAddHandler(AddItemToList), new object[] { parent, item });
      }

      item.Font = m_parent.Font;
      item.ForeColor = m_parent.ForeColor;
      item.Parent = m_parent;

      int i = (int)m_list.Add((object)item);
      RefreshParent(parent);
      return i;
    }

    private bool RemoveItemFromList(ListBox2 parent, ListItem item)
    {
      bool b = (bool)m_list.Remove(item);
      RefreshParent(parent);
      return b;
    }

    private void InsertItemInList(ListBox2 parent, ListItem item, int index)
    {
      m_list.Insert(index, item);
      RefreshParent(parent);
    }

    /// <summary>
    /// Adds an item to the list of items for a ListBox2. 
    /// </summary>
    /// <param name="value">ListItem to add</param>
    /// <returns>Newly created ListItem</returns>
    public ListItem Add(ListItem value)
    {
      AddItemToList(m_parent, value);
      return value;
    }

    /// <summary>
    /// Adds an item to the list of items for a ListBox2 
    /// </summary>
    /// <param name="value">string for text property</param>
    /// <returns>Newly created ListItem</returns>
    public ListItem Add(string value)
    {
      // Use base class to process actual collection operation
      ListItem item = new ListItem(value);
      item.Parent = m_parent;
      item.Font = m_parent.Font;
      item.ForeColor = m_parent.ForeColor;
      this.Add(item);

      return item;
    }

    /// <summary>
    /// Removes the specified object from the collection.
    /// </summary>
    /// <param name="value">ListItem to remove</param>
    public void Remove(ListItem value)
    {
      RemoveItemFromList(m_parent, value);
    }

    /// <summary>
    /// Inserts an item into the list box at the specified index.  
    /// </summary>
    /// <param name="index">The zero-based index location where the item is inserted.</param>
    /// <param name="value">An object representing the item to insert.</param>
    public void Insert(int index, ListItem value)
    {
      InsertItemInList(m_parent, value, index);
    }

    /// <summary>
    /// Determines whether the specified item is located within the collection.  
    /// </summary>
    /// <param name="value">An object representing the item to locate in the collection.</param>
    /// <returns>true if the item is located within the collection; otherwise, false .</returns>
    public bool Contains(ListItem value)
    {
      return m_list.Contains(value);
    }

    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    public ListItem this[int index]
    {
      get { return m_list[index]; }
      set 
      { 
        m_list[index] = value;
        RefreshParent(m_parent);
      }
    }

    /// <summary>
    /// Removes all elements from the System.Collections.ArrayList.  
    /// </summary>
    public void Clear()
    {
      m_list.Clear();
      RefreshParent(m_parent);
    }

    /// <summary>
    /// Gets the item.
    /// </summary>
    public ListItem this[string text]
    {
      get
      {
        // Search for a Page with a matching title
        foreach (ListItem page in m_list)
        {
          if (page.Text == text)
          {
            return page;
          }
        }
        return null;
      }
    }

    /// <summary>
    /// Returns the index within the collection of the specified item
    /// </summary>
    /// <param name="value">An object representing the item to locate in the collection.</param>
    /// <returns>The zero-based index where the item is located within the collection; otherwise, negative one (-1). </returns>
    public int IndexOf(ListItem value)
    {
      // Find the 0 based index of the requested entry
      return m_list.IndexOf(value);
    }

    public int Count
    {
      get { return m_list.Count; }
    }

    public IEnumerator<ListItem> GetEnumerator()
    {
      return m_list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return m_list.GetEnumerator();
    }

    public void RemoveAt(int index)
    {
      m_list.RemoveAt(index);
      RefreshParent(m_parent);
    }

    void ICollection<ListItem>.Add(ListItem item)
    {
      AddItemToList(m_parent, item);
    }

    public void CopyTo(ListItem[] array, int arrayIndex)
    {
      m_list.CopyTo(array, arrayIndex);
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    bool ICollection<ListItem>.Remove(ListItem item)
    {
      return RemoveItemFromList(m_parent, item);
    }

    public int Add(object value)
    {
      return AddItemToList(m_parent, value as ListItem);
    }

    public bool Contains(object value)
    {
      return m_list.Contains(value);
    }

    public int IndexOf(object value)
    {
      return m_list.IndexOf(value);
    }

    public void Insert(int index, object value)
    {
      InsertItemInList(m_parent, value as ListItem, index);
    }

    public bool IsFixedSize
    {
      get { return false; }
    }

    public void Remove(object value)
    {
      RemoveItemFromList(m_parent, value as ListItem);
    }

    object IList.this[int index]
    {
      get { return m_list[index]; }
      set 
      { 
        m_list[index] = (ListItem)value;
        RefreshParent(m_parent);
      }
    }

    public void CopyTo(Array array, int index)
    {
      m_list.CopyTo(array, index);
    }

    public bool IsSynchronized
    {
      get { return true; }
    }

    public object SyncRoot
    {
      get { return m_list.SyncRoot; }
    }
  }
}
