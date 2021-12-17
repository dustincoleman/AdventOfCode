using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode.Common
{
    public class AutoDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TValue : new()
    {
        public Dictionary<TKey, TValue> Dictionary { get; } = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] 
        { 
            get
            {
                if (!Dictionary.ContainsKey(key))
                {
                    Dictionary[key] = new TValue();
                }

                return Dictionary[key];
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public ICollection<TKey> Keys => Dictionary.Keys;

        public ICollection<TValue> Values => Dictionary.Values;

        public int Count => Dictionary.Count;

        public void Add(TKey key, TValue value) => Dictionary.Add(key, value);

        public void Clear() => Dictionary.Clear();

        public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.GetEnumerator();

        public bool Remove(TKey key) => Dictionary.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).IsReadOnly;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Add(item);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Contains(item);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).CopyTo(array, arrayIndex);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dictionary).GetEnumerator();
    }
}
