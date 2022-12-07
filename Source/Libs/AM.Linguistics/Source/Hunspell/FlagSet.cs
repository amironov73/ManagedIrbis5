// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* FlagSet.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class FlagSet
    : ArrayWrapper<FlagValue>, IEquatable<FlagSet>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly FlagSet Empty = new (Array.Empty<FlagValue>());

    /// <summary>
    ///
    /// </summary>
    public static readonly ArrayWrapperComparer<FlagValue, FlagSet> DefaultComparer = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="given"></param>
    /// <returns></returns>
    public static FlagSet Create
        (
            IEnumerable<FlagValue>? given
        )
    {
        return given is null ? Empty : TakeArray (given.Distinct().ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static FlagSet Union
        (
            FlagSet a,
            FlagSet b
        )
    {
        return Create (Enumerable.Concat (a, b));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool ContainsAny
        (
            FlagSet? a,
            FlagSet? b
        )
    {
        if (a is { IsEmpty: false } && b is { IsEmpty: false })
        {
            if (a.Count == 1)
            {
                return b.Contains (a[0]);
            }

            if (b.Count == 1)
            {
                return a.Contains (b[0]);
            }

            if (a.Count > b.Count)
            {
                ReferenceHelpers.Swap (ref a, ref b);
            }

            foreach (var item in a)
            {
                if (b.Contains (item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static FlagSet TakeArray
        (
            FlagValue[]? values
        )
    {
        if (values is null || values.Length == 0)
        {
            return Empty;
        }

        Array.Sort (values);
        return new FlagSet (values);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="set"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static FlagSet Union
        (
            FlagSet set,
            FlagValue value
        )
    {
        var valueIndex = Array.BinarySearch (set._items, value);
        if (valueIndex >= 0)
        {
            return set;
        }

        valueIndex = ~valueIndex; // locate the best insertion point

        var newItems = new FlagValue[set._items.Length + 1];
        if (valueIndex >= set._items.Length)
        {
            Array.Copy (set._items, newItems, set._items.Length);
            newItems[set._items.Length] = value;
        }
        else
        {
            Array.Copy (set._items, newItems, valueIndex);
            Array.Copy (set._items, valueIndex, newItems, valueIndex + 1, set._items.Length - valueIndex);
            newItems[valueIndex] = value;
        }

        return new FlagSet (newItems);
    }

    private FlagSet (FlagValue[] values)
        : base (values)
    {
        mask = default;
        for (var i = 0; i < values.Length; i++)
        {
            unchecked
            {
                mask |= values[i];
            }
        }
    }

    private readonly char mask;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains
        (
            FlagValue value
        )
    {
        if (!value.HasValue || IsEmpty)
        {
            return false;
        }

        if (_items.Length == 1)
        {
            return value.Equals (_items[0]);
        }

        return unchecked (value & mask) != default
               && value >= _items[0]
               && value <= _items[^1]
               && Array.BinarySearch (_items, value) >= 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public bool ContainsAny (FlagSet values) => ContainsAny (this, values);

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool ContainsAny (FlagValue a, FlagValue b)
    {
        return HasItems && (Contains (a) || Contains (b));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool ContainsAny (FlagValue a, FlagValue b, FlagValue c)
    {
        return HasItems && (Contains (a) || Contains (b) || Contains (c));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public bool ContainsAny (FlagValue a, FlagValue b, FlagValue c, FlagValue d)
    {
        return HasItems && (Contains (a) || Contains (b) || Contains (c) || Contains (d));
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals (FlagSet? other)
    {
        return !ReferenceEquals (other, null)
               &&
               (
                   ReferenceEquals (this, other)
                   || ArrayComparer<FlagValue>.Default.Equals (other._items, _items)
               );
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return Equals (obj as FlagSet);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return ArrayComparer<FlagValue>.Default.GetHashCode (_items);
    }
}
