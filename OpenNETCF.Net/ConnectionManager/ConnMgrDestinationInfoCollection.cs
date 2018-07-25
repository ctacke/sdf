
using System.Collections;

namespace OpenNETCF.Net
{
    /// <summary>
    /// Summary description for DestinationInfoCollection.
    /// </summary>
    public class DestinationInfoCollection : CollectionBase
    {
        public DestinationInfoCollection()
        {
        }

        public DestinationInfoCollection(DestinationInfo[] items)
        {
            AddRange(items);
        }

        public DestinationInfoCollection(DestinationInfoCollection items)
        {
            AddRange(items);
        }

        public virtual void AddRange(DestinationInfo[] items)
        {
            foreach (DestinationInfo item in items)
            {
                List.Add(item);
            }
        }

        public virtual void AddRange(DestinationInfoCollection items)
        {
            foreach (DestinationInfo item in items)
            {
                List.Add(item);
            }
        }


        public virtual void Add(DestinationInfo value)
        {
            List.Add(value);
        }

        public virtual bool Contains(DestinationInfo value)
        {
            return List.Contains(value);
        }

        public virtual int IndexOf(DestinationInfo value)
        {
            return List.IndexOf(value);
        }

        public virtual void Insert(int index, DestinationInfo value)
        {
            List.Insert(index, value);
        }

        public virtual DestinationInfo this[int index]
        {
            get { return (DestinationInfo) List[index]; }
            set { List[index] = value; }
        }

        public virtual void Remove(DestinationInfo value)
        {
            List.Remove(value);
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(DestinationInfoCollection collection)
            {
                wrapped = ((CollectionBase) collection).GetEnumerator();
            }

            public DestinationInfo Current
            {
                get { return (DestinationInfo) (wrapped.Current); }
            }

            object IEnumerator.Current
            {
                get { return wrapped.Current; }
            }

            public bool MoveNext()
            {
                return wrapped.MoveNext();
            }

            public void Reset()
            {
                wrapped.Reset();
            }
        }

        public new virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}