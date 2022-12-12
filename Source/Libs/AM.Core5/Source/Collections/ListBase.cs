// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable NonReadonlyMemberInGetHashCode

/* ListBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// ListBase is an abstract class that can be used as a base class for a read-write collection that needs
/// to implement the generic IList&lt;T&gt; and non-generic IList collections. The derived class needs
/// to override the following methods: Count, Clear, Insert, RemoveAt, and the indexer. The implementation
/// of all the other methods in IList&lt;T&gt; and IList are handled by ListBase.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public abstract class ListBase<T>
    : CollectionBase<T>, IList<T>, IList
{
    #region IList members

    /// <inheritdoc cref="CollectionBase{T}.Count"/>
    public abstract override int Count { get; }

    /// <inheritdoc cref="CollectionBase{T}.Clear"/>
    public abstract override void Clear();

    /// <inheritdoc cref="IList{T}.this"/>
    public abstract T this [int index] { get; set; }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public abstract void Insert (int index, T item);

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    public abstract void RemoveAt (int index);

    /// <inheritdoc cref="CollectionBase{T}.GetEnumerator"/>
    public override IEnumerator<T> GetEnumerator()
    {
        var count = Count;
        for (var i = 0; i < count; ++i)
        {
            yield return this[i];
        }
    }

    /// <inheritdoc cref="CollectionBase{T}.Contains"/>
    public override bool Contains (T item)
    {
        return (IndexOf (item) >= 0);
    }

    /// <inheritdoc cref="CollectionBase{T}.Add"/>
    public override void Add (T item)
    {
        Insert (Count, item);
    }

    /// <inheritdoc cref="CollectionBase{T}.Remove"/>
    public override bool Remove (T item)
    {
        var index = IndexOf (item);
        if (index >= 0)
        {
            RemoveAt (index);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public virtual void CopyTo (T[] array)
    {
        CopyTo (array, 0);
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public virtual void CopyTo
        (
            int index,
            T[] array,
            int arrayIndex,
            int count
        )
    {
        Range (index, count).CopyTo (array, arrayIndex);
    }

    /// <summary>
    /// Provides a read-only view of this list. The returned IList&lt;T&gt; provides
    /// a view of the list that prevents modifications to the list. Use the method to provide
    /// access to the list without allowing changes. Since the returned object is just a view,
    /// changes to the list will be reflected in the view.
    /// </summary>
    /// <returns>An IList&lt;T&gt; that provides read-only access to the list.</returns>
    public new virtual IList<T> AsReadOnly()
    {
        return Algorithms.ReadOnly (this)!;
    }

    /// <summary>
    /// Finds the first item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, than
    /// the default value for T (null or all-zero) is returned.
    /// </summary>
    /// <remarks>If the default value for T (null or all-zero) matches the condition defined by <paramref name="predicate"/>,
    /// and the list might contain the default value, then it is impossible to distinguish the different between finding
    /// the default value and not finding any item. To distinguish these cases, use <see cref="TryFind"/>.</remarks>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <returns>The first item that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, the default value for T is returned.</returns>
    /// <seealso cref="TryFind"/>
    public virtual T Find
        (
            Predicate<T> predicate
        )
    {
        Sure.NotNull (predicate);

        return Algorithms.FindFirstWhere (this, predicate);
    }

    /// <summary>
    /// Finds the first item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A delegate that defines the condition to check for.</param>
    /// <param name="foundItem">If true is returned, this parameter receives the first item in the list
    /// that satifies the condition defined by <paramref name="predicate"/>.</param>
    /// <returns>True if an item that  satisfies the condition <paramref name="predicate"/> was found. False
    /// if no item in the list satisfies that condition.</returns>
    public virtual bool TryFind
        (
            Predicate<T> predicate,
            out T foundItem
        )
    {
        Sure.NotNull (predicate);

        return Algorithms.TryFindFirstWhere (this, predicate, out foundItem);
    }

    /// <summary>
    /// Finds the last item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, than
    /// the default value for T (null or all-zero) is returned.
    /// </summary>
    /// <remarks>If the default value for T (null or all-zero) matches the condition defined by <paramref name="predicate"/>,
    /// and the list might contain the default value, then it is impossible to distinguish the different between finding
    /// the default value and not finding any item. To distinguish these cases, use <see cref="TryFindLast"/>.</remarks>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <returns>The last item that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, the default value for T is returned.</returns>
    /// <seealso cref="TryFindLast"/>
    public virtual T FindLast (Predicate<T> predicate)
    {
        return Algorithms.FindLastWhere (this, predicate);
    }

    /// <summary>
    /// Finds the last item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A delegate that defines the condition to check for.</param>
    /// <param name="foundItem">If true is returned, this parameter receives the last item in the list
    /// that satifies the condition defined by <paramref name="predicate"/>.</param>
    /// <returns>True if an item that  satisfies the condition <paramref name="predicate"/> was found. False
    /// if no item in the list satisfies that condition.</returns>
    public virtual bool TryFindLast (Predicate<T> predicate, out T foundItem)
    {
        return Algorithms.TryFindLastWhere (this, predicate, out foundItem);
    }

    /// <summary>
    /// Finds the index of the first item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <returns>The index of the first item that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindIndex (Predicate<T> predicate)
    {
        return Algorithms.FindFirstIndexWhere (this, predicate);
    }

    /// <summary>
    /// Finds the index of the first item, in the range of items extending from <paramref name="index"/> to the end, that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <param name="index">The starting index of the range to check.</param>
    /// <returns>The index of the first item in the given range that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindIndex (int index, Predicate<T> predicate)
    {
        int foundIndex = Algorithms.FindFirstIndexWhere (Range (index, Count - index), predicate);
        if (foundIndex < 0)
        {
            return -1;
        }
        else
        {
            return foundIndex + index;
        }
    }

    /// <summary>
    /// Finds the index of the first item, in the range of <paramref name="count"/> items starting from <paramref name="index"/>, that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <param name="index">The starting index of the range to check.</param>
    /// <param name="count">The number of items in range to check.</param>
    /// <returns>The index of the first item in the given range that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindIndex (int index, int count, Predicate<T> predicate)
    {
        int foundIndex = Algorithms.FindFirstIndexWhere (Range (index, count), predicate);
        if (foundIndex < 0)
        {
            return -1;
        }
        else
        {
            return foundIndex + index;
        }
    }

    /// <summary>
    /// Finds the index of the last item in the list that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <returns>The index of the last item that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindLastIndex (Predicate<T> predicate)
    {
        return Algorithms.FindLastIndexWhere (this, predicate);
    }

    /// <summary>
    /// Finds the index of the last item, in the range of items extending from the beginning
    /// of the list to <paramref name="index"/>, that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <param name="index">The ending index of the range to check.</param>
    /// <returns>The index of the last item in the given range that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindLastIndex (int index, Predicate<T> predicate)
    {
        return Algorithms.FindLastIndexWhere (Range (0, index + 1), predicate);
    }

    /// <summary>
    /// Finds the index of the last item, in the range of <paramref name="count"/> items ending at <paramref name="index"/>, that satisfies the condition
    /// defined by <paramref name="predicate"/>. If no item matches the condition, -1 is returned.
    /// </summary>
    /// <param name="predicate">A delegate that defined the condition to check for.</param>
    /// <param name="index">The ending index of the range to check.</param>
    /// <param name="count">The number of items in range to check.</param>
    /// <returns>The index of the last item in the given range that satisfies the condition <paramref name="predicate"/>. If no item satisfies that
    /// condition, -1 is returned.</returns>
    public virtual int FindLastIndex (int index, int count, Predicate<T> predicate)
    {
        int foundIndex = Algorithms.FindLastIndexWhere (Range (index - count + 1, count), predicate);

        if (foundIndex >= 0)
        {
            return foundIndex + index - count + 1;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Finds the index of the first item in the list that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search fror.</param>
    /// <returns>The index of the first item in the list that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int IndexOf (T item)
    {
        return Algorithms.FirstIndexOf (this, item, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Finds the index of the first item, in the range of items extending from <paramref name="index"/> to the end,
    /// that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search fror.</param>
    /// <param name="index">The starting index of the range to check.</param>
    /// <returns>The index of the first item in the given range that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int IndexOf (T item, int index)
    {
        int foundIndex = Algorithms.FirstIndexOf (Range (index, Count - index), item, EqualityComparer<T>.Default);

        if (foundIndex >= 0)
        {
            return foundIndex + index;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Finds the index of the first item, in the range of <paramref name="count"/> items starting from <paramref name="index"/>,
    /// that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search fror.</param>
    /// <param name="index">The starting index of the range to check.</param>
    /// <param name="count">The number of items in range to check.</param>
    /// <returns>The index of the first item in the given range that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int IndexOf (T item, int index, int count)
    {
        int foundIndex = Algorithms.FirstIndexOf (Range (index, count), item, EqualityComparer<T>.Default);

        if (foundIndex >= 0)
        {
            return foundIndex + index;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Finds the index of the last item in the list that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search fror.</param>
    /// <returns>The index of the last item in the list that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int LastIndexOf (T item)
    {
        return Algorithms.LastIndexOf (this, item, EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Finds the index of the last item, in the range of items extending from the beginning
    /// of the list to <paramref name="index"/>, that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search fror.</param>
    /// <param name="index">The ending index of the range to check.</param>
    /// <returns>The index of the last item in the given range that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int LastIndexOf (T item, int index)
    {
        int foundIndex = Algorithms.LastIndexOf (Range (0, index + 1), item, EqualityComparer<T>.Default);

        return foundIndex;
    }

    /// <summary>
    /// Finds the index of the last item, in the range of <paramref name="count"/> items ending at <paramref name="index"/>,
    /// that is equal to <paramref name="item"/>.
    /// </summary>
    /// <remarks>The default implementation of equality for type T is used in the search. This is the
    /// equality defined by IComparable&lt;T&gt; or object.Equals.</remarks>
    /// <param name="item">The item to search for.</param>
    /// <param name="index">The ending index of the range to check.</param>
    /// <param name="count">The number of items in range to check.</param>
    /// <returns>The index of the last item in the given range that that is equal to <paramref name="item"/>.  If no item is equal
    /// to <paramref name="item"/>, -1 is returned.</returns>
    public virtual int LastIndexOf (T item, int index, int count)
    {
        int foundIndex =
            Algorithms.LastIndexOf (Range (index - count + 1, count), item, EqualityComparer<T>.Default);

        if (foundIndex >= 0)
        {
            return foundIndex + index - count + 1;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Returns a view onto a sub-range of this list. Items are not copied; the
    /// returned IList&lt;T&gt; is simply a different view onto the same underlying items. Changes to this list
    /// are reflected in the view, and vice versa. Insertions and deletions in the view change the size of the
    /// view, but insertions and deletions in the underlying list do not.
    /// </summary>
    /// <remarks>
    /// <para>This method can be used to apply an algorithm to a portion of a list. For example:</para>
    /// <code>Algorithms.ReverseInPlace(deque.Range(3, 6))</code>
    /// will reverse the 6 items beginning at index 3.</remarks>
    /// <param name="start">The starting index of the view.</param>
    /// <param name="count">The number of items in the view.</param>
    /// <returns>A list that is a view onto the given sub-part of this list. </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> or <paramref name="count"/> is negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> + <paramref name="count"/> is greater than the
    /// size of the list.</exception>
    public virtual IList<T> Range (int start, int count)
    {
        return Algorithms.Range (this, start, count);
    }

    /// <summary>
    /// Convert the given parameter to T. Throw an ArgumentException
    /// if it isn't.
    /// </summary>
    /// <param name="name">parameter name</param>
    /// <param name="value">parameter value</param>
    private static T ConvertToItemType (string name, object? value)
    {
        try
        {
            return (T) value!;
        }
        catch (InvalidCastException)
        {
            throw new ArgumentException ("Value has wrong type", name);
        }
    }

    /// <summary>
    /// Adds an item to the end of the list. This method is equivalent to calling:
    /// <code>Insert(Count, item)</code>
    /// </summary>
    /// <param name="value">The item to add to the list.</param>
    /// <exception cref="ArgumentException"><paramref name="value"/> cannot be converted to T.</exception>
    int IList.Add
        (
            object? value
        )
    {
        var count = Count;
        Insert (count, ConvertToItemType ("value", value));
        return count;
    }

    /// <summary>
    /// Removes all the items from the list, resulting in an empty list.
    /// </summary>
    void IList.Clear()
    {
        Clear();
    }

    /// <summary>
    /// Determines if the list contains any item that compares equal to <paramref name="value"/>.
    /// </summary>
    /// <remarks>Equality in the list is determined by the default sense of
    /// equality for T. If T implements IComparable&lt;T&gt;, the
    /// Equals method of that interface is used to determine equality. Otherwise,
    /// Object.Equals is used to determine equality.</remarks>
    /// <param name="value">The item to search for.</param>
    bool IList.Contains (object? value)
    {
        return value is T or null && Contains ((T) value!);
    }

    /// <summary>
    /// Find the first occurrence of an item equal to <paramref name="value"/>
    /// in the list, and returns the index of that item.
    /// </summary>
    /// <remarks>Equality in the list is determined by the default sense of
    /// equality for T. If T implements IComparable&lt;T&gt;, the
    /// Equals method of that interface is used to determine equality. Otherwise,
    /// Object.Equals is used to determine equality.</remarks>
    /// <param name="value">The item to search for.</param>
    /// <returns>The index of <paramref name="value"/>, or -1 if no item in the
    /// list compares equal to <paramref name="value"/>.</returns>
    int IList.IndexOf
        (
            object? value
        )
    {
        return value is T or null ? IndexOf ((T)value!) : -1;
    }

    /// <summary>
    /// Insert a new
    /// item at the given index.
    /// </summary>
    /// <param name="index">The index in the list to insert the item at. After the
    /// insertion, the inserted item is located at this index. The
    /// first item in the list has index 0.</param>
    /// <param name="value">The item to insert at the given index.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is
    /// less than zero or greater than Count.</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> cannot be converted to T.</exception>
    void IList.Insert
        (
            int index,
            object? value
        )
    {
        Insert (index, ConvertToItemType ("value", value));
    }

    /// <inheritdoc cref="IList.IsFixedSize"/>
    bool IList.IsFixedSize => false;

    /// <inheritdoc cref="IList.IsReadOnly"/>
    bool IList.IsReadOnly => ((ICollection<T>) this).IsReadOnly;

    /// <summary>
    /// Searches the list for the first item that compares equal to <paramref name="value"/>.
    /// If one is found, it is removed. Otherwise, the list is unchanged.
    /// </summary>
    /// <remarks>Equality in the list is determined by the default sense of
    /// equality for T. If T implements IComparable&lt;T&gt;, the
    /// Equals method of that interface is used to determine equality. Otherwise,
    /// Object.Equals is used to determine equality.</remarks>
    /// <param name="value">The item to remove from the list.</param>
    /// <exception cref="ArgumentException"><paramref name="value"/> cannot be converted to T.</exception>
    void IList.Remove (object? value)
    {
        if (value is T or null)
        {
            Remove ((T) value!);
        }
    }

    /// <summary>
    /// Removes the
    /// item at the given index.
    /// </summary>
    /// <param name="index">The index in the list to remove the item at. The
    /// first item in the list has index 0.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is
    /// less than zero or greater than or equal to Count.</exception>
    void IList.RemoveAt
        (
            int index
        )
    {
        RemoveAt (index);
    }

    /// <summary>
    /// Gets or sets the
    /// value at a particular index in the list.
    /// </summary>
    /// <param name="index">The index in the list to get or set an item at. The
    /// first item in the list has index 0, and the last has index Count-1.</param>
    /// <value>The item at the given index.</value>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is
    /// less than zero or greater than or equal to Count.</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> cannot be converted to T.</exception>
    object? IList.this [int index]
    {
        get => this[index];

        set => this[index] = ConvertToItemType ("value", value);
    }

    #endregion
}
