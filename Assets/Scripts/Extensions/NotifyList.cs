using System.Collections;
using System;
using System.Collections.Generic;
public class NotifyList<T> : IList<T>
{
    public event Action ListChanged;
    /// <summary>
    /// Determines whether the set should send the <see cref="ListChanged"/> event.
    /// </summary>
    public bool Notify = true;
    private readonly List<T> list = new();

    public NotifyList() { }

    public NotifyList(IEnumerable<T> list) => this.list = new List<T>(list);

    public NotifyList(int capacity) => list = new List<T>(capacity);

    public T this[int index] { get => list[index]; 
        set
        {
            list[index] = value;
            if (Notify) ListChanged?.Invoke();
        }
    }

    public int Count => ((ICollection<T>)list).Count;

    public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

    public void Add(T item)
    {
        list.Add(item);
        if (Notify) ListChanged?.Invoke();
    }

    public void Clear()
    {
        list.Clear();
        if (Notify) ListChanged?.Invoke();
    }

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T item) => list.IndexOf(item);

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        if (Notify) ListChanged?.Invoke();
    }

    public bool Remove(T item)
    {
        var r = list.Remove(item);
        if (!r) return r;
        if (Notify) ListChanged?.Invoke();
        return r;
    }

    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
        if (Notify) ListChanged?.Invoke();
    }

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

    public bool IndexInRange(int index) => index >= 0 && index < list.Count;
    // Use externally. Not internally.
    public void EmitListChangedEvent()
    {
        var t = Notify;
        Notify = true;
        ListChanged?.Invoke();
        Notify = t;
    }
}
