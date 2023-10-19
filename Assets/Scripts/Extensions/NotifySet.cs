using System;
using System.Collections;
using System.Collections.Generic;

public class NotifySet<T> : ISet<T>
{
    public event Action SetChanged;
    /// <summary>
    /// Determines whether the set should send the <see cref="SetChanged"/> event.
    /// </summary>
    public bool Notify = true;
    private readonly HashSet<T> set = new(0);

    public NotifySet() { }
    public NotifySet(IEnumerable<T> collection, IEqualityComparer<T>? comparer = null)
    {
        if (comparer is null) set = new(collection);
        else set = new(collection, comparer);
    }

    public int Count => ((ICollection<T>)set).Count;

    public bool IsReadOnly => ((ICollection<T>)set).IsReadOnly;

    public bool Add(T item)
    {
        var r = set.Add(item);
        if (r && Notify) SetChanged?.Invoke();
        return r;
    }

    public void ExceptWith(IEnumerable<T> other) => set.ExceptWith(other);

    public void IntersectWith(IEnumerable<T> other) => set.IntersectWith(other);

    public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => set.SetEquals(other);

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        set.SymmetricExceptWith(other);
        if (Notify) SetChanged?.Invoke();
    }

    public void UnionWith(IEnumerable<T> other)
    {
        set.UnionWith(other);
        if (Notify) SetChanged?.Invoke();
    }

    void ICollection<T>.Add(T item)
    {
        set.Add(item);
        if (Notify) SetChanged?.Invoke();
    }

    public void Clear()
    {
        set.Clear();
        if (Notify)
            SetChanged?.Invoke();
    }

    public bool Contains(T item) => set.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => set.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
        var r = set.Remove(item);
        if (r && Notify) SetChanged?.Invoke();
        return r;
    }

    public IEnumerator<T> GetEnumerator() => set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
}
