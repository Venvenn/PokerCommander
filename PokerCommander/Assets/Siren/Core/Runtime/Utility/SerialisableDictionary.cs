using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

/// <summary>
/// Generic Serializable Dictionary for Unity 2020.1 and above.
/// Simply declare your key/value types and you're good to go - zero boilerplate.
/// </summary>
[Serializable]
public class SerialisableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // Internal
    [FormerlySerializedAs("list")] [SerializeField]
    private List<KeyValuePair> m_list = new List<KeyValuePair>();
    [FormerlySerializedAs("indexByKey")] [SerializeField, HideInInspector]
    private Dictionary<TKey, int> m_indexByKey;
    [FormerlySerializedAs("dict")] [SerializeField, HideInInspector]
    private Dictionary<TKey, TValue> m_dict;

    #pragma warning disable 0414
    [SerializeField, HideInInspector]
    private bool keyCollision;
    #pragma warning restore 0414

    public SerialisableDictionary()
    {
        m_indexByKey = new Dictionary<TKey, int>();
        m_dict = new Dictionary<TKey, TValue>();
    }

    [Serializable]
    private struct KeyValuePair
    {
        public TKey Key;
        public TValue Value;
        public KeyValuePair(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }

    // Lists are serialized natively by Unity, no custom implementation needed.
    public void OnBeforeSerialize() { }

    // Populate dictionary with pairs from list and flag key-collisions.
    public void OnAfterDeserialize()
    {
        m_dict.Clear();
        m_indexByKey.Clear();
        keyCollision = false;
        for (int i = 0; i < m_list.Count; i++)
        {
            var key = m_list[i].Key;
            if (key != null && !ContainsKey(key))
            {
                m_dict.Add(key, m_list[i].Value);
                m_indexByKey.Add(key, i);
            }
            else
            {
                keyCollision = true;
            }
        }
    }

    // IDictionary
    public TValue this[TKey key]
    {
        get => m_dict[key];
        set
        {
            m_dict[key] = value;
            if (m_indexByKey.ContainsKey(key))
            {
                var index = m_indexByKey[key];
                m_list[index] = new KeyValuePair(key, value);
            }
            else
            {
                m_list.Add(new KeyValuePair(key, value));
                m_indexByKey.Add(key, m_list.Count - 1);
            }
        }
    }

    public ICollection<TKey> Keys => m_dict.Keys;
    public ICollection<TValue> Values => m_dict.Values;

    public void Add(TKey key, TValue value)
    {
        m_dict.Add(key, value);
        m_list.Add(new KeyValuePair(key, value));
        m_indexByKey.Add(key, m_list.Count - 1);
    }

    public bool ContainsKey(TKey key) => m_dict.ContainsKey(key);

    public bool Remove(TKey key) 
    {
        if (m_dict.Remove(key))
        {
            var index = m_indexByKey[key];
            m_list.RemoveAt(index);
            UpdateIndexLookup(index);
            m_indexByKey.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateIndexLookup(int removedIndex) {
        for (int i = removedIndex; i < m_list.Count; i++) {
            var key = m_list[i].Key;
            m_indexByKey[key]--;
        }
    }

    public bool TryGetValue(TKey key, out TValue value) => m_dict.TryGetValue(key, out value);

    // ICollection
    public int Count => m_dict.Count;
    public bool IsReadOnly { get; set; }

    public void Add(KeyValuePair<TKey, TValue> pair)
    {
        Add(pair.Key, pair.Value);
    }

    public void Clear()
    {
        m_dict.Clear();
        m_list.Clear();
        m_indexByKey.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> pair)
    {
        TValue value;
        if (m_dict.TryGetValue(pair.Key, out value))
        {
            return EqualityComparer<TValue>.Default.Equals(value, pair.Value);
        }
        else
        {
            return false;
        }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentException("The array cannot be null.");
        if (arrayIndex < 0)
           throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
        if (array.Length - arrayIndex < m_dict.Count)
            throw new ArgumentException("The destination array has fewer elements than the collection.");

        foreach (var pair in m_dict)
        {
            array[arrayIndex] = pair;
            arrayIndex++;
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> pair)
    {
        TValue value;
        if (m_dict.TryGetValue(pair.Key, out value))
        {
            bool valueMatch = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
            if (valueMatch)
            {
                return Remove(pair.Key);
            }
        }
        return false;
    }

    // IEnumerable
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => m_dict.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => m_dict.GetEnumerator();
}
