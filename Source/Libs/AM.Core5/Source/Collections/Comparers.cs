// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* Comparers.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// A collection of methods to create IComparer and IEqualityComparer instances in various ways.
/// </summary>
internal static class Comparers
{
    /// <summary>
    /// Class to change an IEqualityComparer&lt;TKey&gt; to an IEqualityComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
    /// Only the keys are compared.
    /// </summary>
    [Serializable]
    class KeyValueEqualityComparer<TKey, TValue>
        : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IEqualityComparer<TKey> keyEqualityComparer;

        public KeyValueEqualityComparer (IEqualityComparer<TKey> keyEqualityComparer)
        {
            this.keyEqualityComparer = keyEqualityComparer;
        }

        public bool Equals (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return keyEqualityComparer.Equals (x.Key, y.Key);
        }

        public int GetHashCode (KeyValuePair<TKey, TValue> obj)
        {
            return Util.GetHashCode (obj.Key, keyEqualityComparer);
        }

        public override bool Equals (object? obj)
        {
            return obj is KeyValueEqualityComparer<TKey, TValue> comparer
                   && Equals (keyEqualityComparer, comparer.keyEqualityComparer);
        }

        public override int GetHashCode()
        {
            return keyEqualityComparer.GetHashCode();
        }
    }

    /// <summary>
    /// Class to change an IComparer&lt;TKey&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
    /// Only the keys are compared.
    /// </summary>
    [Serializable]
    class KeyValueComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IComparer<TKey> keyComparer;

        public KeyValueComparer (IComparer<TKey> keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        public int Compare (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return keyComparer.Compare (x.Key, y.Key);
        }

        public override bool Equals
            (
                object? obj
            )
        {
            Sure.NotNull (obj);

            return obj is KeyValueComparer<TKey, TValue> comparer
                   && Equals (keyComparer, comparer.keyComparer);
        }

        public override int GetHashCode()
        {
            return keyComparer.GetHashCode();
        }
    }

    /// <summary>
    /// Class to change an IComparer&lt;TKey&gt; and IComparer&lt;TValue&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
    /// Keys are compared, followed by values.
    /// </summary>
    [Serializable]
    class PairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IComparer<TKey> keyComparer;
        private readonly IComparer<TValue> valueComparer;

        public PairComparer (IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            this.keyComparer = keyComparer;
            this.valueComparer = valueComparer;
        }

        public int Compare (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            var keyCompare = keyComparer.Compare (x.Key, y.Key);

            return keyCompare == 0 ? valueComparer.Compare (x.Value, y.Value) : keyCompare;
        }

        public override bool Equals (object? obj)
        {
            if (obj is PairComparer<TKey, TValue> comparer)
            {
                return Equals (keyComparer, comparer.keyComparer) &&
                       Equals (valueComparer, comparer.valueComparer);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return keyComparer.GetHashCode() ^ valueComparer.GetHashCode();
        }
    }

    /// <summary>
    /// Class to change an Comparison&lt;T&gt; to an IComparer&lt;T&gt;.
    /// </summary>
    [Serializable]
    class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> comparison;

        public ComparisonComparer (Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        public int Compare (T? x, T? y)
        {
            return comparison (x!, y!);
        }

        public override bool Equals (object? obj)
        {
            return obj is ComparisonComparer<T> comparer && comparison.Equals (comparer.comparison);
        }

        public override int GetHashCode()
        {
            return comparison.GetHashCode();
        }
    }

    /// <summary>
    /// Class to change an Comparison&lt;TKey&gt; to an IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;.
    /// GetHashCode cannot be used on this class.
    /// </summary>
    [Serializable]
    class ComparisonKeyValueComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly Comparison<TKey> comparison;

        public ComparisonKeyValueComparer (Comparison<TKey> comparison)
        {
            this.comparison = comparison;
        }

        public int Compare (KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return comparison (x.Key, y.Key);
        }

        public override bool Equals (object? obj)
        {
            return obj is ComparisonKeyValueComparer<TKey, TValue> comparer
                   && comparison.Equals (comparer.comparison);
        }

        public override int GetHashCode()
        {
            return comparison.GetHashCode();
        }
    }


    /// <summary>
    /// Given an Comparison on a type, returns an IComparer on that type.
    /// </summary>
    /// <typeparam name="T">T to compare.</typeparam>
    /// <param name="comparison">Comparison delegate on T</param>
    /// <returns>IComparer that uses the comparison.</returns>
    public static IComparer<T> ComparerFromComparison<T>
        (
            Comparison<T> comparison
        )
    {
        Sure.NotNull (comparison);

        return new ComparisonComparer<T> (comparison);
    }

    /// <summary>
    /// Given an IComparer on TKey, returns an IComparer on
    /// key-value Pairs.
    /// </summary>
    /// <typeparam name="TKey">TKey of the pairs</typeparam>
    /// <typeparam name="TValue">TValue of the apris</typeparam>
    /// <param name="keyComparer">IComparer on TKey</param>
    /// <returns>IComparer for comparing key-value pairs.</returns>
    public static IComparer<KeyValuePair<TKey, TValue>> ComparerKeyValueFromComparerKey<TKey, TValue>
        (
            IComparer<TKey> keyComparer
        )
    {
        Sure.NotNull (keyComparer);

        return new KeyValueComparer<TKey, TValue> (keyComparer);
    }

    /// <summary>
    /// Given an IEqualityComparer on TKey, returns an IEqualityComparer on
    /// key-value Pairs.
    /// </summary>
    /// <typeparam name="TKey">TKey of the pairs</typeparam>
    /// <typeparam name="TValue">TValue of the apris</typeparam>
    /// <param name="keyEqualityComparer">IComparer on TKey</param>
    /// <returns>IEqualityComparer for comparing key-value pairs.</returns>
    public static IEqualityComparer<KeyValuePair<TKey, TValue>> EqualityComparerKeyValueFromComparerKey<TKey, TValue>
        (
            IEqualityComparer<TKey> keyEqualityComparer
        )
    {
        Sure.NotNull (keyEqualityComparer);

        return new KeyValueEqualityComparer<TKey, TValue> (keyEqualityComparer);
    }

    /// <summary>
    /// Given an IComparer on TKey and TValue, returns an IComparer on
    /// key-value Pairs of TKey and TValue, comparing first keys, then values.
    /// </summary>
    /// <typeparam name="TKey">TKey of the pairs</typeparam>
    /// <typeparam name="TValue">TValue of the apris</typeparam>
    /// <param name="keyComparer">IComparer on TKey</param>
    /// <param name="valueComparer">IComparer on TValue</param>
    /// <returns>IComparer for comparing key-value pairs.</returns>
    public static IComparer<KeyValuePair<TKey, TValue>> ComparerPairFromKeyValueComparers<TKey, TValue>
        (
            IComparer<TKey> keyComparer,
            IComparer<TValue> valueComparer
        )
    {
        Sure.NotNull (keyComparer);
        Sure.NotNull (valueComparer);

        return new PairComparer<TKey, TValue> (keyComparer, valueComparer);
    }

    /// <summary>
    /// Given an Comparison on TKey, returns an IComparer on
    /// key-value Pairs.
    /// </summary>
    /// <typeparam name="TKey">TKey of the pairs</typeparam>
    /// <typeparam name="TValue">TValue of the apris</typeparam>
    /// <param name="keyComparison">Comparison delegate on TKey</param>
    /// <returns>IComparer for comparing key-value pairs.</returns>
    public static IComparer<KeyValuePair<TKey, TValue>> ComparerKeyValueFromComparisonKey<TKey, TValue>
        (
            Comparison<TKey> keyComparison
        )
    {
        Sure.NotNull (keyComparison);

        return new ComparisonKeyValueComparer<TKey, TValue> (keyComparison);
    }

    /// <summary>
    /// Given an element type, check that it implements IComparable&lt;T&gt; or IComparable, then returns
    /// a IComparer that can be used to compare elements of that type.
    /// </summary>
    /// <returns>The IComparer&lt;T&gt; instance.</returns>
    /// <exception cref="InvalidOperationException">T does not implement IComparable&lt;T&gt;.</exception>
    public static IComparer<T> DefaultComparer<T>()
    {
        if (typeof (IComparable<T>).IsAssignableFrom (typeof (T)) ||
            typeof (IComparable).IsAssignableFrom (typeof (T)))
        {
            return Comparer<T>.Default;
        }

        throw new InvalidOperationException ("Type does not implement IComparable<T> or IComparable");
    }

    /// <summary>
    /// Given an key and value type, check that TKey implements IComparable&lt;T&gt; or IComparable, then returns
    /// a IComparer that can be used to compare KeyValuePairs of those types.
    /// </summary>
    /// <returns>The IComparer&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; instance.</returns>
    /// <exception cref="InvalidOperationException">TKey does not implement IComparable&lt;T&gt;.</exception>
    public static IComparer<KeyValuePair<TKey, TValue>> DefaultKeyValueComparer<TKey, TValue>()
    {
        var keyComparer = DefaultComparer<TKey>();
        return ComparerKeyValueFromComparerKey<TKey, TValue> (keyComparer);
    }
}
