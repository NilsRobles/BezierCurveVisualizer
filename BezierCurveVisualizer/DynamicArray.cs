using System.Diagnostics;

namespace BezierCurveVisualizer
{
    internal class DynamicArray<T>
    {
        private T[] items;

        #region Constructors
        public DynamicArray()
        {
            items = Array.Empty<T>();
        }
        public DynamicArray(T[] items)
        {
            this.items = items;
        }

        #endregion

        #region Write items
        public void Add(T item)
        {
            T[] newItems = new T[items.Length + 1];
            Array.Copy(items, newItems, items.Length);
            newItems[^1] = item;
            items = newItems;
        }

        public void AddtAt(T item, int index)
        {
            T[] newItems = new T[items.Length + 1];
            Array.Copy(items, newItems, index);
            newItems[index] = item;
            Array.Copy(items, index, newItems, index + 1, items.Length - index);
            items = newItems;
        }

        public void Remove(T item)
        {
            if (items.Length == 0) return;
            int index = Array.IndexOf(items, item);
            T[] newItmes = new T[items.Length - 1];
            Array.Copy(items, newItmes, index);
            Array.Copy(items, index + 1, newItmes, index, newItmes.Length - index);
            items = newItmes;
        }

        public void RemoveAt(int index)
        {
            if (items.Length == 0) return;
            T[] newItems = new T[items.Length - 1];
            Array.Copy(items, newItems, index);
            Array.Copy(items, index + 1, newItems, index, newItems.Length - index);
            items = newItems;
        }
        
        public void Clear()
        {
            items = Array.Empty<T>();
        }

        public void Set(int index, T item)
        {
            items[index] = item;
        }

        public void SetArray(T[] items)
        {
            if (items == null) return;
            this.items = items;
        }

        #endregion

        #region Read items
        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public int Size() => items.Length;

        public T[] GetItems() => items;

        public T Get(int index) => items[index];

        #endregion

        public T this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
    }
}
