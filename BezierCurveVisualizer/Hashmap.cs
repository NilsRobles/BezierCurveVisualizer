using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class Hashmap<K, V>
    {
        List<K> keys;
        List<V> values;
        public Hashmap() 
        {
            keys = new List<K>();
            values = new List<V>();
        }

        public void Add(K key, V value)
        {
            int oldIndex = keys.IndexOf(key);
            if (oldIndex == -1)
            {
                keys.Add(key);
                values.Add(value);
                return;
            }
            values[oldIndex] = value;
        }

        public void Remove(K key)
        {
            int index = keys.IndexOf(key);
            if (index != -1) return;
            keys.RemoveAt(index);
            values.RemoveAt(index);
        }

        public V Get(K key)
        {
            return values[keys.IndexOf(key)];
        }

        public bool Contains(K key)
        {
            return keys.Contains(key);
        }

        public void Clear() 
        { 
            keys.Clear();
        }
    }
}
