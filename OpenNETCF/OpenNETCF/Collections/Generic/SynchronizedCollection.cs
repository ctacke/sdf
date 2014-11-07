using System;

using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OpenNETCF.Collections.Generic
{
  /// <summary>
  /// Provides a thread-safe collection that contains objects of a type specified by the generic parameter as elements.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class SynchronizedCollection<T> : IList<T>,
    ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
  {
    private List<T> m_list;

    /// <summary>
    /// Initializes a new instance of the SynchronizedCollection(T) class. 
    /// </summary>
    public SynchronizedCollection()
    {
      m_list = new List<T>();
      SyncRoot = new object();
    }

    public object SyncRoot { get; private set; }

    public int IndexOf(T item)
    {
      lock (SyncRoot)
      {
        return m_list.IndexOf(item);
      }
    }

    public void Insert(int index, T item)
    {
      lock (SyncRoot)
      {
        m_list.Insert(index, item);
      }
    }

    public void RemoveAt(int index)
    {
      lock (SyncRoot)
      {
        m_list.RemoveAt(index);
      }
    }

    public T this[int index]
    {
      get
      {
        lock (SyncRoot)
        {
          return m_list[index];
        }
      }
      set
      {
        lock (SyncRoot)
        {
          m_list[index] = value;
        }
      }
    }

    public void Add(T item)
    {
      lock (SyncRoot)
      {
        m_list.Add(item);
      }
    }

    public void Clear()
    {
      lock (SyncRoot)
      {
        m_list.Clear();
      }
    }

    public bool Contains(T item)
    {
      lock (SyncRoot)
      {
        return m_list.Contains(item);
      }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      lock (SyncRoot)
      {
        m_list.CopyTo(array, arrayIndex);
      }
    }

    public int Count
    {
      get 
      {
        lock (SyncRoot)
        {
          return m_list.Count;
        }
      }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(T item)
    {
      lock (SyncRoot)
      {
        return m_list.Remove(item);
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      lock (SyncRoot)
      {
        return new SynchronizedEnumerator<T>(SyncRoot, m_list.GetEnumerator());
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      lock (SyncRoot)
      {
        return new SynchronizedEnumerator<T>(SyncRoot, m_list.GetEnumerator());
      }
    }

    public virtual bool IsSynchronized
    {
      get { return true; }
    }

    int IList<T>.IndexOf(T item)
    {
      lock (SyncRoot)
      {
        return m_list.IndexOf(item);
      }
    }

    void IList<T>.Insert(int index, T item)
    {
      lock (SyncRoot)
      {
         m_list.Insert(index, item);
      }
    }

    void IList<T>.RemoveAt(int index)
    {
      lock (SyncRoot)
      {
        m_list.RemoveAt(index);
      }
    }

    T IList<T>.this[int index]
    {
      get
      {
        lock (SyncRoot)
        {
          return m_list[index];
        }
      }
      set
      {
        lock (SyncRoot)
        {
          m_list[index] = value;
        }
      }
    }

    void ICollection<T>.Add(T item)
    {
      lock (SyncRoot)
      {
        m_list.Add(item);
      }
    }

    void ICollection<T>.Clear()
    {
      lock (SyncRoot)
      {
        m_list.Clear();
      }
    }

    bool ICollection<T>.Contains(T item)
    {
      lock (SyncRoot)
      {
        return m_list.Contains(item);
      }
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      lock (SyncRoot)
      {
        m_list.CopyTo(array, arrayIndex);
      }
    }

    int ICollection<T>.Count
    {
      get 
      {
        lock (SyncRoot)
        {
          return m_list.Count;
        }
      }
    }

    bool ICollection<T>.IsReadOnly
    {
      get { return false; }
    }

    bool ICollection<T>.Remove(T item)
    {
      lock (SyncRoot)
      {
        return m_list.Remove(item);
      }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      lock (SyncRoot)
      {
        return new SynchronizedEnumerator<T>(SyncRoot, m_list.GetEnumerator());
      }
    }

    public int Add(object value)
    {
      if(!(value is T))
      {
        throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
      }

      lock (SyncRoot)
      {
        m_list.Add((T)value);
        return m_list.Count - 1;
      }
    }

    public bool Contains(object value)
    {
      if (!(value is T))
      {
        throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
      }

      lock (SyncRoot)
      {
        return m_list.Contains((T)value);
      }
    }

    public int IndexOf(object value)
    {
      if (!(value is T))
      {
        throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
      }

      lock (SyncRoot)
      {
        return m_list.IndexOf((T)value);
      }
    }

    public void Insert(int index, object value)
    {
      if (!(value is T))
      {
        throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
      }

      lock (SyncRoot)
      {
        m_list.Insert(index, (T)value);
      }
    }

    public bool IsFixedSize
    {
      get { return false; }
    }

    public void Remove(object value)
    {
      if (!(value is T))
      {
        throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
      }

      lock (SyncRoot)
      {
        m_list.Remove((T)value);
      }
    }

    object IList.this[int index]
    {
      get
      {
        lock (SyncRoot)
        {
          return m_list[index];
        }
      }
      set
      {
        if (!(value is T))
        {
          throw new ArgumentException(string.Format("value must be of type {0}", typeof(T).Name));
        }

        lock (SyncRoot)
        {
          m_list[index] = (T)value;
        }
      }
    }

    public void CopyTo(Array array, int index)
    {
      lock (SyncRoot)
      {
        m_list.CopyTo((T[])array, index);
      }
    }
  }

  internal sealed class SynchronizedEnumerator<T> : IEnumerator<T>
  {
    private object m_syncRoot;
    private IEnumerator<T> m_enumerator;

    public SynchronizedEnumerator(object syncRoot, IEnumerator<T> enumerator)
    {
      m_syncRoot = syncRoot;
      m_enumerator = enumerator;
    }

    public T Current
    {
      get 
      {
        lock (m_syncRoot)
        {
          return m_enumerator.Current;
        }
      }
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      lock (m_syncRoot)
      {
        return m_enumerator.MoveNext();
      }
    }

    public void Reset()
    {
      lock (m_syncRoot)
      {
        m_enumerator.Reset();
      }
    }

    object IEnumerator.Current
    {
      get
      {
        lock (m_syncRoot)
        {
          return m_enumerator.Current;
        }
      }
    }
  }
}
