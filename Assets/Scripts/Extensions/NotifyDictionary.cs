using System.Collections;
using System;
using System.Collections.Generic;
public class NotifyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    public event Action DictionaryChanged;
    public bool Notify = true;
    private readonly Dictionary<TKey, TValue> dictionary = new(0);

    public NotifyDictionary() { }

    public NotifyDictionary(int capacity) => dictionary = new Dictionary<TKey, TValue>(capacity);

    public TValue this[TKey index] { 
        get => dictionary[index]; 
        set
        {
            dictionary[index] = value;
            if (Notify) DictionaryChanged?.Invoke();
        }
    }

    public int Count => dictionary.Count;

    public ICollection<TKey> Keys => dictionary.Keys;

    public ICollection<TValue> Values => dictionary.Values;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        if (Notify) DictionaryChanged?.Invoke();
    }

    public void Clear()
    {
        dictionary.Clear();
        if (Notify) DictionaryChanged?.Invoke();
    }

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
    public bool ContainsValue(TValue value) => dictionary.ContainsValue(value);


    public IEnumerator GetEnumerator() => dictionary.GetEnumerator();

    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
        if (Notify) DictionaryChanged?.Invoke();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => 
        ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var r = ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
        if (r && Notify) DictionaryChanged?.Invoke();
        return r;
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        dictionary.GetEnumerator();

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove(TKey key)
    {
        var r = dictionary.Remove(key);
        if (r && Notify) DictionaryChanged?.Invoke();
        return r;
    }
}
